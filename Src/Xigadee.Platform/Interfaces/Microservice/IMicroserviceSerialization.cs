﻿using System;

namespace Xigadee
{
    /// <summary>
    /// This interface is responsible for the Microservice security.
    /// </summary>
    public interface IMicroserviceSerialization
    {
        /// <summary>
        /// Registers the payload serializer.
        /// </summary>
        /// <param name="fnSerializer">The serializer function.</param>
        /// <returns>The serializer.</returns>
        IPayloadSerializer RegisterPayloadSerializer(Func<IPayloadSerializer> fnSerializer);
        /// <summary>
        /// Registers the payload serializer.
        /// </summary>
        /// <param name="serializer">The serializer.</param>
        /// <returns>The serializer.</returns>
        IPayloadSerializer RegisterPayloadSerializer(IPayloadSerializer serializer);
        /// <summary>
        /// Clears the payload serializers.
        /// </summary>
        void ClearPayloadSerializers();
        /// <summary>
        /// Gets the payload serializer count.
        /// </summary>
        int PayloadSerializerCount { get; }
    }
}
