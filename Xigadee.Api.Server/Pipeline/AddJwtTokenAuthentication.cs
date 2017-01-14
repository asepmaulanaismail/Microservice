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
using System.Web.Http.Filters;

namespace Xigadee
{
    public static partial class WebApiExtensionMethods
    {
        /// <summary>
        /// This method adds basic Jwt authentication to the web app
        /// </summary>
        /// <typeparam name="P">The web pipe type</typeparam>
        /// <param name="webpipe">The pipe.</param>
        /// <param name="algo">The supported HMAC algorithm</param>
        /// <param name="base64Secret">The base64secret</param>
        /// <param name="audience">The audience value to check.</param>
        /// <param name="action">The action to be called on the filter creation.</param>
        /// <returns>Returns the web pipe.</returns>
        public static P AddWebApiJwtTokenAuthentication<P>(this P webpipe
            , JwtHashAlgorithm algo
            , string base64Secret
            , string audience = "api"
            , Action<IAuthenticationFilter> action = null)
            where P : IPipelineWebApi
        {
            var policy = new JwtTokenVerificationPolicy
            {
                  Algorithm = algo
                , Audience = audience
                , Secret = Convert.FromBase64String(base64Secret)
            };

            return webpipe.AddWebApiJwtTokenAuthentication(policy, action);
        }

        /// <summary>
        /// This method adds basic Jwt authentication to the web app
        /// </summary>
        /// <typeparam name="P">The web pipe type</typeparam>
        /// <param name="webpipe">The pipe.</param>
        /// <param name="algo">The supported HMAC algorithm</param>
        /// <param name="secret">The secret</param>
        /// <param name="audience">The audience value to check.</param>
        /// <param name="action">The action to be called on the filter creation.</param>
        /// <returns>Returns the web pipe.</returns>
        public static P AddWebApiJwtTokenAuthentication<P>(this P webpipe
            , JwtHashAlgorithm algo
            , byte[] secret
            , string audience = "api"
            , Action<IAuthenticationFilter> action = null)
            where P : IPipelineWebApi
        {
            var policy = new JwtTokenVerificationPolicy
            {
                  Algorithm = algo
                , Audience = audience
                , Secret = secret
            };

            return webpipe.AddWebApiJwtTokenAuthentication(policy, action);
        }
        /// <summary>
        /// This method adds basic Jwt authentication to the web app
        /// </summary>
        /// <typeparam name="P">The web pipe type</typeparam>
        /// <param name="webpipe">The pipe.</param>
        /// <param name="creator">This method can be used to create the token policy from configuration.</param>
        /// <param name="action">The action to be called on the filter creation.</param>
        /// <returns>Returns the web pipe.</returns>
        public static P AddWebApiJwtTokenAuthentication<P>(this P webpipe
           , Func<IEnvironmentConfiguration, JwtTokenVerificationPolicy> creator
           , Action<IAuthenticationFilter> action = null)
           where P : IPipelineWebApi
        {
            var policy = creator(webpipe.Configuration);

            return webpipe.AddWebApiJwtTokenAuthentication(policy, action);
        }
        /// <summary>
        /// This method adds basic Jwt authentication to the web app
        /// </summary>
        /// <typeparam name="P">The web pipe type</typeparam>
        /// <param name="webpipe">The pipe.</param>
        /// <param name="policy">The token policy.</param>
        /// <param name="action">The action to be called on the filter creation.</param>
        /// <returns>Returns the web pipe.</returns>
        public static P AddWebApiJwtTokenAuthentication<P>(this P webpipe
           , JwtTokenVerificationPolicy policy
           , Action<IAuthenticationFilter> action = null)
           where P : IPipelineWebApi
        {
            var filter = new JwtAuthenticationFilter(policy);

            action?.Invoke(filter);

            webpipe.HttpConfig.Filters.Add(filter);

            return webpipe;
        }

    }

}
