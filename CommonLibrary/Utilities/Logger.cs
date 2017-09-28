using CommonLibrary.Patterns;
using System;

namespace CommonLibrary.Utilities
{
    public class Logger
    {
        private Action<string> LogAction;

        public static Logger GetLogger
        {
            get
            {
                return Singleton<Logger>.GetInstance();
            }
        }

        public bool IsInitialized
        {
            get;
            private set;
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