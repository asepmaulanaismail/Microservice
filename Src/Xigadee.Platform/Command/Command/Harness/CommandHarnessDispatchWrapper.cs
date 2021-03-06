﻿using System;

namespace Xigadee
{
    /// <summary>
    /// This command provides additional method for the command harness dispatcher.
    /// </summary>
    /// <seealso cref="Xigadee.DispatchWrapper" />
    /// <seealso cref="Xigadee.ICommandHarnessDispath" />
    public class CommandHarnessDispatchWrapper : DispatchWrapper, ICommandHarnessDispath
    {
        internal CommandHarnessDispatchWrapper(CommandPolicy policy
            , IPayloadSerializationContainer serializer
            , Action<TransmissionPayload, string> executeOrEnqueue
            , Func<ServiceStatus> getStatus
            , bool traceEnabled
            , string originatorServiceId = null) 
            : base(serializer, executeOrEnqueue, getStatus, traceEnabled)
        {
            DefaultOriginatorServiceId = originatorServiceId;
            Policy = policy;
        }
        /// <summary>
        /// Gets the command policy.
        /// </summary>
        protected CommandPolicy Policy { get; }

        /// <summary>
        /// Gets the default originator service identifier. If this is not null, it will be appended to generated service messages.
        /// </summary>
        public string DefaultOriginatorServiceId { get; }

        /// <summary>
        /// This method creates a service message and injects it in to the execution path and bypasses the listener infrastructure.
        /// </summary>
        /// <param name="header">The message header fragment to identify the recipient. The channel will be inserted by the command harness.</param>
        /// <param name="package">The object package to process.</param>
        /// <param name="ChannelPriority">The priority that the message should be processed. The default is 1. If this message is not a valid value, it will be matched to the nearest valid value.</param>
        /// <param name="options">The process options.</param>
        /// <param name="release">The release action which is called when the payload has been executed by the receiving commands.</param>
        /// <param name="responseHeader">This is the optional response header fragment. The channel will be inserted by the harness</param>
        /// <param name="ResponseChannelPriority">This is the response channel priority. This will be set if the response header is not null. The default priority is 1.</param>
        /// <param name="originatorServiceId">This optional parameter allows you to set the originator serviceId</param>
        public void Process(ServiceMessageHeaderFragment header
            , object package = null
            , int ChannelPriority = 1
            , ProcessOptions options = ProcessOptions.RouteExternal | ProcessOptions.RouteInternal
            , Action<bool, Guid> release = null
            , ServiceMessageHeaderFragment responseHeader = null
            , int ResponseChannelPriority = 1
            , string originatorServiceId = null
            )
        {
            this.Process((Policy.ChannelId, header)
            , package
            , ChannelPriority
            , options
            , release
            , responseHeader == null? (ServiceMessageHeader)null: (Policy.ResponseChannelId, responseHeader)
            , ResponseChannelPriority
            , originatorServiceId
            );
        }

        /// <summary>
        /// This method creates a service message and injects it in to the execution path and bypasses the listener infrastructure.
        /// </summary>
        /// <param name="header">The message header fragment to identify the recipient. The channel will be inserted by the command harness.</param>
        /// <param name="package">The object package to process.</param>
        /// <param name="ChannelPriority">The priority that the message should be processed. The default is 1. If this message is not a valid value, it will be matched to the nearest valid value.</param>
        /// <param name="options">The process options.</param>
        /// <param name="release">The release action which is called when the payload has been executed by the receiving commands.</param>
        /// <param name="responseHeader">This is the optional response header</param>
        /// <param name="ResponseChannelPriority">This is the response channel priority. This will be set if the response header is not null. The default priority is 1.</param>
        /// <param name="originatorServiceId">This optional parameter allows you to set the originator serviceId</param>
        public void Process(ServiceMessageHeaderFragment header
            , object package = null
            , int ChannelPriority = 1
            , ProcessOptions options = ProcessOptions.RouteExternal | ProcessOptions.RouteInternal
            , Action<bool, Guid> release = null
            , ServiceMessageHeader responseHeader = null
            , int ResponseChannelPriority = 1
            , string originatorServiceId = null
            )
        {
            this.Process((Policy.ChannelId, header)
            , package
            , ChannelPriority
            , options
            , release
            , responseHeader
            , ResponseChannelPriority
            , originatorServiceId
            );
        }

        /// <summary>
        /// This method injects a payload in to the execution path and bypasses the listener infrastructure.
        /// </summary>
        /// <param name="payload">The transmission payload to execute.</param>
        public override void Process(TransmissionPayload payload)
        {
            ValidateServiceStarted();

            if (payload.Message.OriginatorServiceId == null && DefaultOriginatorServiceId != null)
                payload.Message.OriginatorServiceId = DefaultOriginatorServiceId;

            if (PayloadTraceEnabled)
            {
                payload.TraceEnabled = true;
                payload.TraceWrite($"CommandHarness/{Name} received.");
            }

            ExecuteOrEnqueue(payload, $"CommandHarness/{Name} method request");
        }
    }
}
