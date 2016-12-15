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

#region using
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#endregion
namespace Xigadee
{
    public partial class CommunicationContainer
    {
        #region Security
        /// <summary>
        /// This interface contains a referece to the security container and is used to provide extensible
        /// security support.
        /// </summary>
        public ISecurityService Security { get; set; } 
        #endregion

        #region PayloadIncomingSecurityCheck(TransmissionPayload payload)
        /// <summary>
        /// This method validates the payload with the security container.
        /// </summary>
        /// <param name="payload">The incoming payload.</param>
        protected virtual void PayloadIncomingSecurity(TransmissionPayload payload)
        { 
            //Try and resolve the channel.
            Channel channel = null;
            TryGet(payload.Message.ChannelId, ChannelDirection.Incoming, out channel);

            //Decrypt and verify the incoming message.
            Security.Verify(channel, payload);
        }
        #endregion
        #region PayloadOutgoingSecurity(TransmissionPayload payload)
        /// <summary>
        /// This method validates the payload with the security container.
        /// </summary>
        /// <param name="payload">The incoming payload.</param>
        protected virtual void PayloadOutgoingSecurity(TransmissionPayload payload)
        {
            //Try and resolve the channel.
            Channel channel = null;
            TryGet(payload.Message.ChannelId, ChannelDirection.Outgoing, out channel);

            //Secure the outgoing payload.
            Security.Secure(channel, payload);
        }
        #endregion
    }
}
