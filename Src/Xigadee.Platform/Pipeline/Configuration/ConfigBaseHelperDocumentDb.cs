﻿#region Copyright
// Copyright Hitachi Consulting
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//    http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xigadee
{

    public static class ConfigBaseHelperDocumentDb
    {
        [ConfigSettingKey("DocumentDb")]
        public const string KeyDocDBAccountName = "DocDBAccountName";

        [ConfigSettingKey("DocumentDb")]
        public const string KeyDocDBAccountAccessKey = "DocDBAccountAccessKey";

        [ConfigSettingKey("DocumentDb")]
        public const string KeyDocDBDatabaseName = "DocDBDatabaseName";

        [ConfigSettingKey("DocumentDb")]
        public const string KeyDocDBCollectionName = "DocDBCollectionName";


        [ConfigSetting("DocumentDb")]
        public static DocumentDbConnection DocDBConnection(this IEnvironmentConfiguration config, bool throwExceptionIfNotFound = false) 
            => DocumentDbConnection.ToConnection(config.DocDBAccountName(throwExceptionIfNotFound), config.DocDBAccountAccessKey(throwExceptionIfNotFound));

        [ConfigSetting("DocumentDb")]
        public static string DocDBAccountName(this IEnvironmentConfiguration config, bool throwExceptionIfNotFound = false) 
            => config.PlatformOrConfigCache(KeyDocDBAccountName, throwExceptionIfNotFound: throwExceptionIfNotFound);

        [ConfigSetting("DocumentDb")]
        public static string DocDBAccountAccessKey(this IEnvironmentConfiguration config, bool throwExceptionIfNotFound = false) 
            => config.PlatformOrConfigCache(KeyDocDBAccountAccessKey, throwExceptionIfNotFound: throwExceptionIfNotFound);

        [ConfigSetting("DocumentDb")]
        public static string DocDBDatabaseName(this IEnvironmentConfiguration config, bool throwExceptionIfNotFound = false) 
            => config.PlatformOrConfigCache(KeyDocDBDatabaseName, throwExceptionIfNotFound: throwExceptionIfNotFound);

        [ConfigSetting("DocumentDb")]
        public static string DocDBCollectionName(this IEnvironmentConfiguration config, bool throwExceptionIfNotFound = false) 
            => config.PlatformOrConfigCache(KeyDocDBCollectionName, throwExceptionIfNotFound: throwExceptionIfNotFound);
    }
}