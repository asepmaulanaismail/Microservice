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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion
namespace Xigadee
{
    /// <summary>
    /// The MessageFilterWrapper class allows for extended filter functionality for the messaging bus.
    /// </summary>
    [DebuggerDisplay("{Header.ToKey()}|{ClientId} DL={IsDeadLetter}")]
    public class MessageFilterWrapper: IEquatable<MessageFilterWrapper>
    {
        public MessageFilterWrapper(ServiceMessageHeader header, bool isDeadLetter = false)
        {
            Header = header;
            IsDeadLetter = isDeadLetter;
        }

        public readonly ServiceMessageHeader Header;

        public readonly bool IsDeadLetter;

        public string ClientId { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is MessageFilterWrapper))
                return false;

            return Equals((MessageFilterWrapper)obj);
        }


        #region GetHashCode()
        /// <summary>
        /// This is the Hashcode override.
        /// </summary>
        /// <returns>This is the calculated hashcode.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var result = 0;

                result = (result * 397) ^ SafeHashCode(IsDeadLetter);
                result = (result * 397) ^ SafeHashCode(ClientId);

                result = (result * 397) ^ SafeHashCode(Header.ChannelId);
                result = (result * 397) ^ SafeHashCode(Header.MessageType);
                result = (result * 397) ^ SafeHashCode(Header.ActionType);

                return result;
            }
        }
        #endregion
        #region SafeHashCode(object item)
        /// <summary>
        /// This helper method gets the Hashcode for the item or returns 0 if the object is null.
        /// </summary>
        /// <param name="item">The item to get the hashcode for.</param>
        /// <returns>The hashcode or 0.</returns>
        private int SafeHashCode(object item)
        {
            if (item == null)
                return 0;

            return item.GetHashCode();
        }
        #endregion


        public bool Equals(MessageFilterWrapper other)
        {
            if (other == null)
                return false;

            return IsDeadLetter == other.IsDeadLetter 
                && ClientId == other.ClientId
                && Header.Equals(other.Header);
        }

    }
}
