using CommonLibrary.Native;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.Configuration;

namespace CommonLibrary.Utilities
{
    /// <summary>
    ///
    /// </summary>
    /// <example>
    /// <section name="Machine" type="System.Configuration.NameValueFileSectionHandler,System, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
    /// <sectionGroup name="Environment">
    ///  <section name="Test" type="System.Configuration.NameValueFileSectionHandler,System, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
    ///  <section name="Stage" type="System.Configuration.NameValueFileSectionHandler,System, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
    ///  <section name="Local" type="System.Configuration.NameValueFileSectionHandler,System, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
    ///  <section name="Prod" type="System.Configuration.NameValueFileSectionHandler,System, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
    /// </sectionGroup>
    /// </example>
    public class MachineConfiguration
    {
        private static string _EnvironmentValue;

        public static string EnvironmentValue
        {
            get
            {
                if (_EnvironmentValue.IsNullOrEmptyTrim())
                {
                    // get machine name
                    string machine = Environment.MachineName;

                    // set environment from machine name
                    try
                    {
                        _EnvironmentValue = ((NameValueCollection)ConfigurationManager.GetSection("Machine"))[machine] ?? "Test";
                    }
                    catch
                    {
                        _EnvironmentValue = ((NameValueCollection)WebConfigurationManager.GetSection("Machine"))[machine] ?? "Test";
                    }
                }

                return _EnvironmentValue;
            }
        }

        public static string GetConfig(string key)
        {
            string value = null;

            try
            {
                value = ((NameValueCollection)ConfigurationManager.GetSection(string.Format("Environment/{0}", EnvironmentValue)))[key];
            }
            catch
            {
                value = ((NameValueCollection)WebConfigurationManager.GetSection(string.Format("Environment/{0}", EnvironmentValue)))[key];
            }

            return value;
        }
    }
}