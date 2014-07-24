﻿using System;
using CookComputing.XmlRpc;
using System.Reflection;

namespace Terradue.OpenNebula {
    public partial class OneClient {

        /// <summary>
        /// Gets or sets the proxy URL.
        /// </summary>
        /// <value>The proxy URL.</value>
        private string ProxyUrl { get; set; }

        /// <summary>
        /// Gets or sets the admin username.
        /// </summary>
        /// <value>The admin username.</value>
        private string AdminUsername { get; set; }

        /// <summary>
        /// Gets or sets the admin password.
        /// </summary>
        /// <value>The admin password.</value>
        private string AdminPassword { get; set; }

        /// <summary>
        /// Gets or sets the target username.
        /// </summary>
        /// <value>The target username.</value>
        public string TargetUsername { get; set; }


        /// <summary>
        /// Gets the session SHA.
        /// </summary>
        /// <value>The session SHA.</value>
        protected string SessionSHA { 
            get { 
                return "portal:portaltest";
                return "serveradmin:f4b887a18de059129df8a265176f80bc479439a6";
                return this.AdminUsername + (this.TargetUsername != null ? ":" + this.TargetUsername + ":" : ":") + this.AdminPassword; 
            } 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Terradue.OpenNebula.OneUser"/> class.
        /// </summary>
        /// <param name="adminUsername">Admin username.</param>
        /// <param name="adminPassword">Admin password.</param>
        public OneClient(string adminUsername, string adminPassword) {
            this.ProxyUrl = Configuration.XMLRPC_SERVER;
            this.AdminUsername = adminUsername;
            this.AdminPassword = adminPassword;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Terradue.OpenNebula.OneClient"/> class.
        /// </summary>
        /// <param name="proxy">Proxy url of the XML RPC server</param>
        /// <param name="adminUsername">Admin username.</param>
        /// <param name="adminPassword">Admin password.</param>
        public OneClient(string proxy, string adminUsername, string adminPassword) {
            if (proxy == null) throw new Exception("ONe XML RPC proxy url cannot be null");
            if (adminUsername == null) throw new Exception("ONe XML RPC user cannot be null");

            this.ProxyUrl = proxy;
            this.AdminUsername = adminUsername;
            this.AdminPassword = adminPassword;
        }
            
        /// <summary>
        /// Creates the proxy management object
        /// </summary>
        /// <returns>The proxy.</returns>
        /// <param name="type">Type.</param>
        public IXmlRpcProxy GetProxy(Type type){
            MethodInfo mi = typeof(XmlRpcProxyGen).GetMethod("Create", new Type[]{});
            MethodInfo gmi = mi.MakeGenericMethod(type);
            IXmlRpcProxy result = (IXmlRpcProxy)gmi.Invoke(null,null);
            result.Url = this.ProxyUrl;
            return result;
        }

        /// <summary>
        /// Deserializes the response.
        /// </summary>
        /// <returns>The response.</returns>
        /// <param name="type">Type.</param>
        /// <param name="response">Response.</param>
        private object Deserialize(Type type, string response){
            object result = null;

            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(type);
            using (System.IO.MemoryStream s = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(response ?? ""))) {
                result = ser.Deserialize(s);
            }

            return result;
        }

    }

}

