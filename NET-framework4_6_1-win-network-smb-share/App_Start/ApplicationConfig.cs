/*
Copyright (C) 2019-Present Pivotal Software, Inc. All rights reserved. 

This program and the accompanying materials are made available under the terms of the 
under the Apache License, Version 2.0 (the "License”); you may not use this file except 
in compliance with the License. You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under the 
License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
either express or implied. See the License for the specific language governing permissions 
and limitations under the License.
*/

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using System;
using System.IO;

namespace NET_framework4_6_1_win_network_smb_share
{
    public class ApplicationConfig
    {
        public static CloudFoundryApplicationOptions CloudFoundryApplication {

            get {
                var opts = new CloudFoundryApplicationOptions();
                var appSection = Configuration.GetSection(CloudFoundryApplicationOptions.CONFIGURATION_PREFIX);
                appSection.Bind(opts);
                return opts;
            }
        }
        public static CloudFoundryServicesOptions CloudFoundryServices {
            get {
                var opts = new CloudFoundryServicesOptions();
                var serviceSection = Configuration.GetSection(CloudFoundryServicesOptions.CONFIGURATION_PREFIX);
                serviceSection.Bind(opts);
                return opts;
            }
        }
        public static IConfigurationRoot Configuration { get; set; }

        public static void Configure(string environment)
        {

            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(GetContentRoot())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .AddCloudFoundry();

            Configuration = builder.Build();
        }
        
        public static string GetContentRoot()
        {
            var basePath = (string)AppDomain.CurrentDomain.GetData("APP_CONTEXT_BASE_DIRECTORY") ??
               AppDomain.CurrentDomain.BaseDirectory;
            return Path.GetFullPath(basePath);
        }
    }
}