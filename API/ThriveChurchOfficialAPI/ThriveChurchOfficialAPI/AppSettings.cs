﻿namespace ThriveChurchOfficialAPI
{
    /// <summary>
    /// Settings read from the appsettings.json config file
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Gets the API Key from ESV Api within the Appsettings.json
        /// </summary>
        public string EsvApiKey { get; set; }

        /// <summary>
        /// Use this within your AppSettings.json to set the path to your mongoDB instance
        /// </summary>
        public string MongoConnectionString { get; set; }

        /// <summary>
        /// If a contributor has no EsvApiKey, they can omit the check for one. However,
        /// this may cause issues with using the Passage Controller
        /// </summary>
        public string OverrideEsvApiKey { get; set; }

        /// <summary>
        /// Email PW used to send alert emails in the event an exception occurred
        /// </summary>
        public string EmailPW { get; set; }
    }
}