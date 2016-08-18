﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Xigadee
{
    /// <summary>
    /// This class manages the Uri mapping for a particular key.
    /// </summary>
    /// <typeparam name="K">The key type.</typeparam>
    public class TransportUriMapper<K>
    {
        #region Declarations
        /// <summary>
        /// This method is used to convert the key in to a string.
        /// </summary>
        protected IKeyMapper<K> mKeyMapper;
        /// <summary>
        /// THis dictionary contains the individual templates for each HTTP method.
        /// </summary>
        Dictionary<HttpMethod, string> mUriTemplates;
        #endregion
        #region Constructor
        /// <summary>
        /// This is the default constructor.
        /// </summary>
        /// <param name="keyMapper">The key mapper.</param>
        /// <param name="rootUri">This is the root Uri.</param>
        public TransportUriMapper(IKeyMapper<K> keyMapper = null, Uri rootUri = null, string pathEntity = null)
        {
            mUriTemplates = new Dictionary<HttpMethod, string>();

            if (keyMapper != null)
                mKeyMapper = keyMapper;
            else
                mKeyMapper = (KeyMapper<K>)KeyMapper.Resolve<K>();

            if (rootUri != null)
                Server = rootUri;
            else
                UseHttps = true;

            PathEntity = pathEntity;
        } 
        #endregion

        public Uri Server
        {
            get
            {
                var builder = UriRoot();
                return builder.Uri;
            }
            set
            {
                UseHttps = string.Equals(value.Scheme, "https", StringComparison.InvariantCultureIgnoreCase);
                Host = value.Host;
                Path = value.AbsolutePath;
                Port = value.Port;
            }
        }
        /// <summary>
        /// This property determines whether to use https to call the Api.
        /// </summary>
        public bool UseHttps { get; set; }

        /// <summary>
        /// This is the scheme, i.e. http or https
        /// </summary>
        public virtual string Scheme { get { return UseHttps ? "https" : "http"; } }
        /// <summary>
        /// This is the Api host server
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// This is the default path for the Api - usually something like "/v1"
        /// </summary>
        public virtual string Path { get; set; }

        /// <summary>
        /// This is the port that the api is listening. If null the default port will be used, i.e. 80, 443.
        /// </summary>
        public int? Port { get; set; }

        /// <summary>
        /// This is the default port adjusted for the http or https default port if not set.
        /// </summary>
        protected int PortAdjusted
        {
            get
            {
                return Port.HasValue?Port.Value:UseHttps?443:80;
            }
        }

        /// <summary>
        /// This is the entity path which is added to the end of uri
        /// </summary>
        public string PathEntity { get; set; }

        protected virtual UriBuilder UriRoot()
        {
            return new UriBuilder(Scheme, Host, PortAdjusted, Path);
        }
        /// <summary>
        /// This method returns a UriBuilder for the request.
        /// </summary>
        /// <param name="method">The current method.</param>
        /// <returns>Returns the builder.</returns>
        protected virtual UriBuilder UriParts(HttpMethod method)
        {
            var builder = UriRoot();

            if (!string.IsNullOrWhiteSpace(PathEntity))
            {
                if (builder.Path.EndsWith("/"))
                    builder.Path = $"{builder.Path}{PathEntity}";
                else
                    builder.Path = $"{builder.Path}/{PathEntity}";
            }

            return builder;
        }

        public virtual KeyValuePair<HttpMethod,Uri> MakeUri(HttpMethod method)
        {
            var builder = UriParts(method);

            return new KeyValuePair<HttpMethod, Uri>(method, builder.Uri);
        }

        public virtual KeyValuePair<HttpMethod, Uri> MakeUri(HttpMethod method, K key)
        {
            var builder = UriParts(method);
            
            builder.Path = string.Format("{0}/{1}", builder.Path, Uri.EscapeDataString(mKeyMapper.ToString(key)));
            return new KeyValuePair<HttpMethod, Uri>(method, builder.Uri);
        }

        public virtual KeyValuePair<HttpMethod, Uri> MakeUri(HttpMethod method, string refKey, string refValue)
        {
            var builder = UriParts(method);
            builder.Query = string.Format("reftype={0}&refvalue={1}", Uri.EscapeDataString(refKey), Uri.EscapeDataString(refValue));
            return new KeyValuePair<HttpMethod, Uri>(method, builder.Uri);
        }
    }
}
