using System;
using CommonLibrary.Patterns;

namespace CommonLibrary.Utilities
{
    public class Logger
    {
        public bool IsInitialized
        {
            get;
            private set;
        }

        private Action<string> LogAction;

        public static Logger GetLogger
        {
            get
            {
                return Singleton<Logger>.GetInstance();
            }
        }

        public void Initialize(Action<string> logAction)
        {
            if (!IsInitialized)
            {
                LogAction = logAction;
            }
        }

        public void Log(string data)
        {
            LogAction(data);
        }
    }
}