using System;

namespace CommonLibrary.Patterns
{
    public class Singleton<T> where T : class
    {
        private static T _Holder;

        public static T GetInstance()
        {
            if (_Holder == null)
            {
                _Holder = default(T);
            }

            return _Holder;
        }

        public static T GetInstance(Func<T> constructor)
        {
            if (_Holder == null)
            {
                _Holder = constructor();
            }

            return _Holder;
        }
    }
}