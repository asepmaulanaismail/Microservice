﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Xigadee
{
    public interface ICommand: IService, IRequireScheduler, IPayloadSerializerConsumer, IServiceEventSource, IServiceLogger, IServiceOriginator, IRequireSharedServices
    {
        /// <summary>
        /// This is the default listening channel id for incoming requests.
        /// </summary>
        string ChannelId { get; set; }
        /// <summary>
        /// Specifies whether the channel can be autoset during configuration
        /// </summary>
        bool ChannelIdAutoSet { get; }
        /// <summary>
        /// This is the channel used for the response to outgoing messages.
        /// </summary>
        string ResponseChannelId { get; set; }
        /// <summary>
        /// Specifies whether the response channel can be set during configuration.
        /// </summary>
        bool ResponseChannelIdAutoSet { get; }
        /// <summary>
        /// This is the channel used to negotiate control for a master job.
        /// </summary>
        string MasterJobNegotiationChannelIdIncoming { get; set; }
        /// <summary>
        /// This is the channel used to negotiate control for a master job.
        /// </summary>
        string MasterJobNegotiationChannelIdOutgoing { get; set; }
        /// <summary>
        /// Specifies whether the master job negotiation channel can be set during configuration.
        /// </summary>
        bool MasterJobNegotiationChannelIdAutoSet { get; }

        /// <summary>
        /// This flag specifies whether the command should be informed when a submitted process has timed out.
        /// </summary>
        bool TaskManagerTimeoutSupported { get; }
        /// <summary>
        /// This is the reference to the task manager that can be called to submit new jobs for processing.
        /// </summary>
        Action<IService, TransmissionPayload> TaskManager { get; set; }
        /// <summary>
        /// This method is called for processes that support direct notification from the Task Manager that a process has been
        /// cancelled.
        /// </summary>
        /// <param name="originatorKey">The is the originator tracking key.</param>
        void TaskManagerTimeoutNotification(string originatorKey);

        /// <summary>
        /// This method should return true if the handler support this specific message.
        /// </summary>
        /// <param name="messageHeader">The message header.</param>
        /// <returns>Returns true if the message channelId is supported.</returns>
        bool SupportsMessage(ServiceMessageHeader messageHeader);
        /// <summary>
        /// This method processes the supported message.
        /// </summary>
        /// <param name="request">The request payload.</param>
        /// <param name="responseMessages">The response collection.</param>
        Task ProcessMessage(TransmissionPayload request, List<TransmissionPayload> responseMessages);
        /// <summary>
        /// Returns a list of message header types.
        /// </summary>
        /// <returns>Returns ServiceMessageHeader definition.</returns>
        List<MessageFilterWrapper> SupportedMessageTypes();
        /// <summary>
        /// This is the handler priority used when starting and stopping services.
        /// The higher the value, the sooner the command will be started relative to the other commands with lower priorities.
        /// </summary>
        int StartupPriority { get; set; }
        /// <summary>
        /// This event is used to signal a change of registered message types for the command.
        /// </summary>
        event EventHandler<CommandChange> OnCommandChange;
        /// <summary>
        /// This event is fired when the command's masterjob state is changed.
        /// </summary>
        event EventHandler<MasterJobStateChange> OnMasterJobStateChange;
    }

}
