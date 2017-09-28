using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CommonLibrary.Utilities
{
    /// <summary>
    /// Attempts to inject a dll into the memory of a process
    /// </summary>
    public class DllInjector
    {
        /// <summary>
        /// Attempts to inject the DLL into the remote process and start a thread
        /// </summary>
        /// <param name="remoteProcess"></param>
        /// <param name="dllName"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool InjectDll(string remoteProcess, string dllName, out string error)
        {
            bool success = false;
            error = null;

            int procID = GetProcessID(remoteProcess);

            if (procID > 0)
            {
                IntPtr hProcess = (IntPtr)UnsafeNativeFunctions.OpenProcess((uint)UnsafeNativeFunctions.ProcessAccessFlags.All, 1, procID);

                if (hProcess != null)
                {
                    success = InjectDll(hProcess, dllName, out error);
                }
                else
                {
                    success = false;
                    error = "Could not open target process";
                }
            }
            else
            {
                success = false;
                error = "Could not find remote process";
            }

            return success;
        }

        /// <summary>
        /// Attempts to return the process ID
        /// A -1 result is a failure to find the process
        /// </summary>
        /// <param name="proc"></param>
        /// <returns></returns>
        private int GetProcessID(string proc)
        {
            int id = -1;

            Process[] procList = Process.GetProcessesByName(proc);

            if (procList != null && procList.Length > 0)
            {
                id = procList[0].Id;
            }

            return id;
        }

        /// <summary>
        /// Attempts to inject the dll into the process handle
        /// </summary>
        /// <param name="hProcess"></param>
        /// <param name="dllName"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        private bool InjectDll(IntPtr hProcess, string dllName, out string error)
        {
            bool success = false;
            error = null;

            IntPtr bytesOut;

            // Length of the string containing  the DLL file name +1 byte padding
            int lenWrite = dllName.Length + 1;

            // Allocate memory within the virtual address space of the target process
            IntPtr allocMem = (IntPtr)UnsafeNativeFunctions.VirtualAllocEx(hProcess, (IntPtr)null, (uint)lenWrite, (uint)UnsafeNativeFunctions.AllocationType.Commit, (uint)UnsafeNativeFunctions.MemoryProtection.ExecuteReadWrite);

            // Write DLL file name to allocated memory in target process
            UnsafeNativeFunctions.WriteProcessMemory(hProcess, allocMem, dllName, (UIntPtr)lenWrite, out bytesOut);

            UIntPtr injector = (UIntPtr)UnsafeNativeFunctions.GetProcAddress(UnsafeNativeFunctions.GetModuleHandle("kernel32.dll"), "LoadLibraryA");

            if (injector != null && injector != UIntPtr.Zero)
            {
                // Create thread in target process and store handle in hThread
                IntPtr hThread = (IntPtr)UnsafeNativeFunctions.CreateRemoteThread(hProcess, (IntPtr)null, 0, injector, allocMem, 0, out bytesOut);

                if (hThread != null && hThread != IntPtr.Zero)
                {
                    // wait 10 seconds for timeout
                    int result = UnsafeNativeFunctions.WaitForSingleObject(hThread, 10000);

                    if (result == 0x80L || result == 0x102L || result == -1)
                    {
                        success = false;
                        error = "Thread timed out";

                        if (hThread != null)
                        {
                            UnsafeNativeFunctions.CloseHandle(hThread);
                        }
                    }
                    else
                    {
                        // The DLL has been successfully injected and remote thread started
                        success = true;

                        if (hThread != null)
                        {
                            // Close the handle to the thread to prevent memory leaks
                            UnsafeNativeFunctions.CloseHandle(hThread);
                        }

                        if (hProcess != null)
                        {
                            // Close the handle to the process to prevent memory leaks
                            UnsafeNativeFunctions.CloseHandle(hProcess);
                        }
                    }
                }
                else
                {
                    UnsafeNativeFunctions.CloseHandle(hProcess);

                    success = false;

                    error = "Could not create thread in remote process";
                }
            }
            else
            {
                UnsafeNativeFunctions.CloseHandle(hProcess);

                success = false;

                error = "Could not load Kernel32 LoadLibraryA";
            }

            return success;
        }

        /// <summary>
        /// Native functions
        /// </summary>
        private class UnsafeNativeFunctions
        {
            [Flags]
            public enum AllocationType : uint
            {
                Commit = 0x1000,
                Reserve = 0x2000,
                Decommit = 0x4000,
                Release = 0x8000,
                Reset = 0x80000,
                Physical = 0x400000,
                TopDown = 0x100000,
                WriteWatch = 0x200000,
                LargePages = 0x20000000
            }

            [Flags]
            public enum FreeType : uint
            {
                Decommit = 0x4000,
                Release = 0x8000
            }

            [Flags]
            public enum MemoryProtection : uint
            {
                Execute = 0x10,
                ExecuteRead = 0x20,
                ExecuteReadWrite = 0x40,
                ExecuteWriteCopy = 0x80,
                NoAccess = 0x01,
                ReadOnly = 0x02,
                ReadWrite = 0x04,
                WriteCopy = 0x08,
                GuardModifierflag = 0x100,
                NoCahceModifierflag = 0x200,
                WriteCombineModifierflag = 0x400
            }

            [Flags]
            public enum ProcessAccessFlags : uint
            {
                All = 0x001F0FFF,
                Terminate = 0x00000001,
                CreateThread = 0x00000002,
                VMOperation = 0x00000008,
                VMRead = 0x00000010,
                VMWrite = 0x00000020,
                DupHandle = 0x00000040,
                SetInformation = 0x00000200,
                QueryInformation = 0x00000400,
                Synchronize = 0x00100000
            }

            /// <summary>
            /// Closes the process handle
            /// </summary>
            /// <param name="hObject"></param>
            /// <returns></returns>
            [DllImport("kernel32.dll")]
            public static extern Int32 CloseHandle(IntPtr hObject);

            /// <summary>
            /// Put information from the remote process into a new thread in the target process
            /// </summary>
            /// <param name="hProcess"></param>
            /// <param name="lpThreadAttributes"></param>
            /// <param name="dwStackSize"></param>
            /// <param name="lpStartAddress"></param>
            /// <param name="lpParameter"></param>
            /// <param name="dwCreationFlags"></param>
            /// <param name="lpThreadID"></param>
            /// <returns></returns>
            [DllImport("kernel32.dll")]
            public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, UIntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out IntPtr lpThreadID);

            /// <summary>
            /// Gets the handle of a module
            /// </summary>
            /// <param name="lpModuleName"></param>
            /// <returns></returns>
            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr GetModuleHandle(string lpModuleName);

            /// <summary>
            /// Gets the address of a process
            /// </summary>
            /// <param name="hModule"></param>
            /// <param name="procName"></param>
            /// <returns></returns>
            [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true)]
            public static extern UIntPtr GetProcAddress(IntPtr hModule, string procName);

            /// <summary>
            /// Get the handle of a process
            /// </summary>
            /// <param name="dwDesiredAccess"></param>
            /// <param name="bInheritHandle"></param>
            /// <param name="dwProcessId"></param>
            /// <returns></returns>
            [DllImport("kernel32.dll")]
            public static extern IntPtr OpenProcess(UInt32 dwDesiredAccess, Int32 bInheritHandle, Int32 dwProcessId);

            /// <summary>
            /// Sets aside memory to be written to
            /// </summary>
            /// <param name="hProcess"></param>
            /// <param name="lpAddress"></param>
            /// <param name="dwSize"></param>
            /// <param name="flAllocationType"></param>
            /// <param name="flProtect"></param>
            /// <returns></returns>
            [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
            public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

            /// <summary>
            /// Clears memory
            /// </summary>
            /// <param name="hProcess"></param>
            /// <param name="lpAddress"></param>
            /// <param name="dwSize"></param>
            /// <param name="dwFreeType"></param>
            /// <returns></returns>
            [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
            public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, UIntPtr dwSize, uint dwFreeType);

            /// <summary>
            /// Waits for thread to finish
            /// </summary>
            /// <param name="handle"></param>
            /// <param name="milliseconds"></param>
            /// <returns></returns>
            [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
            public static extern Int32 WaitForSingleObject(IntPtr handle, Int32 milliseconds);

            /// <summary>
            /// Writes data to memory
            /// </summary>
            /// <param name="hProcess"></param>
            /// <param name="lpBaseAddress"></param>
            /// <param name="lpBuffer"></param>
            /// <param name="nSize"></param>
            /// <param name="lpNumberOfBytesWritten"></param>
            /// <returns></returns>
            [DllImport("kernel32.dll")]
            public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, string lpBuffer, UIntPtr nSize, out IntPtr lpNumberOfBytesWritten);
        }
    }
}