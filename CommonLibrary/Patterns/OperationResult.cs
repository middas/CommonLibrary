using System;

namespace CommonLibrary.Patterns
{
    public class OperationResult
    {
        public OperationResult()
        {
            Succeeded = false;
        }

        public Exception Exception { get; private set; }

        public string Message { get; private set; }

        public object ReturnVal { get; private set; }

        public bool Succeeded { get; private set; }

        public static bool IsNullOrFailed(OperationResult result)
        {
            return result == null || !result.Succeeded;
        }

        public static bool IsResultNotEmptyAndSucceeded(OperationResult result)
        {
            return result.Succeeded && result.ReturnVal != null;
        }

        public void SetFailed(string message)
        {
            Succeeded = false;
            Message = message;
        }

        public void SetFailed(string message, Exception ex)
        {
            Succeeded = false;
            Message = message;
            Exception = ex;
        }

        public void SetSuccess()
        {
            Succeeded = true;
        }

        public void SetSuccess(object returnVal)
        {
            Succeeded = true;
            ReturnVal = returnVal;
        }
    }
}