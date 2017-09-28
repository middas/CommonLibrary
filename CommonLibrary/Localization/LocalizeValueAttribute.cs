using System;

namespace CommonLibrary.Localization
{
    public class LocalizeValueAttribute : Attribute
    {
        public LocalizeValueAttribute(string key)
        {
            Key = key;
        }

        public string Key { get; private set; }
    }
}