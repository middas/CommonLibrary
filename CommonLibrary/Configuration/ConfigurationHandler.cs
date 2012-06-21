using System;
using System.Collections.Specialized;
using System.Configuration;
using System.ServiceModel.Configuration;
using System.Web.Configuration;
using CommonLibrary.Native;

namespace Certiport.Common.Utility
{
    /// <summary>
    /// Handles all configuration calls
    /// </summary>
    /// <example>
    /// Add <section name="Machine" type="System.Configuration.NameValueFileSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" /> to the configuration file
    /// </example>
    public class ConfigurationHandler
    {
        private static string _Environment;

        private static ConfigurationHandler _ConfigurationSettings;
        /// <summary>
        /// The singleton <see cref="ConfigurationHandler"/>
        /// </summary>
        public static ConfigurationHandler Current
        {
            get
            {
                if (_ConfigurationSettings == null)
                {
                    _ConfigurationSettings = new ConfigurationHandler();
                }

                return _ConfigurationSettings;
            }
        }

        /// <summary>
        /// The constructor - private to ensure calls only from <see cref="Current"/>
        /// </summary>
        private ConfigurationHandler()
        {
        }

        /// <summary>
        /// Initializes the configuration to a specific environment
        /// </summary>
        /// <param name="environment">The environment</param>
        public void Initialize(string environment)
        {
            if (_Environment.IsNullOrEmptyTrim() && _Environment != environment)
            {
                _Environment = environment;

                UpdateConfigValues();
            }
        }

        /// <summary>
        /// Initializes the configuration to a specific environment by the current Computer Name
        /// </summary>
        public void Initialize()
        {
            string environment;

            // load all machine specific configurations
            NameValueCollection machines = ConfigurationManager.GetSection("Machine") as NameValueCollection;

            if (machines != null && machines.Count > 0)
            {
                // attempt to set the environment by the machine configuration
                environment = machines[System.Environment.MachineName.ToUpper()];

                // if no environment is found, default to null
                if (environment.IsNullOrEmptyTrim())
                {
                    environment = "TEST";
                }
            }
            else
            {
                // default to null
                environment = "TEST";
            }

            Initialize(environment);
        }

        /// <summary>
        /// Updates the dynamic configuration values based on the initialized environment
        /// </summary>
        private static void UpdateConfigValues()
        {
            // update appsetting dynamic values
            foreach (string key in ConfigurationManager.AppSettings.AllKeys)
            {
                if (ConfigurationManager.AppSettings[key].IsNullOrEmptyTrim() && !ConfigurationHandler.Current.GetConfig(key).IsNullOrEmptyTrim())
                {
                    ConfigurationManager.AppSettings[key] = ConfigurationHandler.Current.GetConfig(key);
                }
            }

            // update endpoint dynamic values
            Configuration config;

            try
            {
                config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }
            catch
            {
                config = WebConfigurationManager.OpenWebConfiguration("~");
            }

            bool requiresSave = false;
            var clients = config.GetSection("system.serviceModel/client") as ClientSection;

            ChannelEndpointElementCollection endPointCollection = clients.ElementInformation.Properties[string.Empty].Value as ChannelEndpointElementCollection;

            foreach (ChannelEndpointElement endpoint in endPointCollection)
            {
                if (!ConfigurationHandler.Current.GetConfig(endpoint.Name).IsNullOrEmptyTrim())
                {
                    if (endpoint.Address.OriginalString != ConfigurationHandler.Current.GetConfig(endpoint.Name))
                    {
                        endpoint.Address = new Uri(ConfigurationHandler.Current.GetConfig(endpoint.Name));

                        requiresSave = true;
                    }
                }
            }

            if (requiresSave)
            {
                config.Save();

                ConfigurationManager.RefreshSection("system.serviceModel/client");
            }
        }

        /// <summary>
        /// Gets the configuration value based on the initialized environment
        /// </summary>
        /// <param name="key">The configuration Key value</param>
        /// <returns>The configuration Value from the Key</returns>
        public string GetConfig(string key)
        {
            if (!_Environment.IsNullOrEmptyTrim())
            {
                string value;

                var keys = ConfigurationManager.GetSection(_Environment) as NameValueCollection;

                value = keys[key];

                if (value.IsNullOrEmptyTrim())
                {
                    value = ConfigurationManager.AppSettings[key];
                }

                return value;
            }
            else
            {
                throw new NullReferenceException("The environment has not been initialized.  Call the Initialize method.");
            }
        }
    }
}
