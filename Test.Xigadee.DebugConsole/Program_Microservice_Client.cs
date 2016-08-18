﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using Owin;
using Unity.WebApi;
using Xigadee;

namespace Test.Xigadee
{
    static partial class Program
    {
        static Dictionary<string, string> sClientSettings = new Dictionary<string, string>();

        static void MicroserviceClientStart()
        {
            sContext.Client.StatusChanged += StatusChanged;

            sContext.Client.Populate(ResolveClientSetting, true);
            sContext.Client.Start();
        }

        static void MicroserviceClientStop()
        {
            sContext.Client.Stop();
            sContext.Client.StatusChanged -= StatusChanged;
        }


        static string ResolveClientSetting(string key, string value)
        {
            if (sClientSettings.ContainsKey(key))
                return sClientSettings[key];

            return null;
        }
    }
}