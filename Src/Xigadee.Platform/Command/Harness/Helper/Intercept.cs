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

namespace Xigadee
{
    /// <summary>
    /// This class is used to provide helper methods to the harness.
    /// </summary>
    public static partial class CommandHarnessHelper
    {
        /// <summary>
        /// Outgoings the intercept.
        /// </summary>
        /// <typeparam name="H">The harness type.</typeparam>
        /// <param name="harness">The harness.</param>
        /// <param name="action">The action.</param>
        /// <returns>Returns the harness to continue the pipeline.</returns>
        /// <exception cref="ArgumentNullException">action - action cannot be null</exception>
        public static H OutgoingIntercept<H>(this H harness, Action<CommandHarnessRequestContext> action)
            where H : ICommandHarness
        {
            if (action == null)
                throw new ArgumentNullException("action", "action cannot be null");

            Action<ICommandHarness, CommandHarnessEventArgs> actionInternal = (h, args) => 
            {
                var payload = args.Event.Tracker.ToTransmissionPayload();

                var context = new CommandHarnessRequestContext(h, args, payload);

                action(context);

                context.Responses.ForEach((r) => harness.Dispatcher.Process(r));
            };

            harness.ConfigureIntercept(actionInternal, CommandHarnessTrafficDirection.Outgoing);
            harness.ConfigureIntercept(actionInternal, CommandHarnessTrafficDirection.Response);

            return harness;
        }
        /// <summary>
        /// Intercepts the specified event arguments.
        /// </summary>
        /// <typeparam name="H">The harness type.</typeparam>
        /// <param name="harness">The harness.</param>
        /// <param name="action">The event arguments action.</param>
        /// <param name="direction">The optional direction filter.</param>
        /// <param name="header">The optional header.</param>
        /// <returns>The harness</returns>
        public static H ConfigureIntercept<H>(this H harness
            , Action<ICommandHarness, CommandHarnessEventArgs> action
            , CommandHarnessTrafficDirection? direction = null
            , ServiceMessageHeader header = null)
            where H : ICommandHarness
        {
            if (action == null)
                throw new ArgumentNullException("action", "action cannot be null");

            harness.OnEvent += (object sender, CommandHarnessEventArgs e) =>
            {
                //Is this a payload event, no then do not bubble up.
                if (!e.Event.Tracker.HasTransmissionPayload())
                    return;

                if (direction.HasValue && e.Event.Direction != direction.Value)
                    return;

                var payload = e.Event.Tracker.ToTransmissionPayload();
                ServiceMessageHeader headerFind = payload.Message.ToServiceMessageHeader();

                if (header == null || headerFind == header)
                    action(harness, e);
            };

            return harness;
        }
    }
}