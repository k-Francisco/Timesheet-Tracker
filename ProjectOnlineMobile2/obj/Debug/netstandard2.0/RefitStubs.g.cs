﻿// <auto-generated />
using System;
using System.Net.Http;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ProjectOnlineMobile2.Models;
using Refit;
using System.Text;
using System.Threading.Tasks;

/* ******** Hey You! *********
 *
 * This is a generated file, and gets rewritten every time you build the
 * project. If you want to edit it, you need to edit the mustache template
 * in the Refit package */

#pragma warning disable
namespace RefitInternalGenerated
{
    [AttributeUsage (AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate)]
    sealed class PreserveAttribute : Attribute
    {

        //
        // Fields
        //
        public bool AllMembers;

        public bool Conditional;
    }
}
#pragma warning restore

namespace ProjectOnlineMobile2.Services
{
    using RefitInternalGenerated;

    /// <inheritdoc />
    [Preserve]
    public partial class AutoGeneratedISharepointApi : ISharepointApi
    {
        /// <inheritdoc />
        public HttpClient Client { get; protected set; }
        readonly ConcurrentDictionary<string, Func<HttpClient, object[], object>> methodImpls = new ConcurrentDictionary<string, Func<HttpClient, object[], object>>();
        readonly IRequestBuilder requestBuilder;

        public AutoGeneratedISharepointApi(HttpClient client, IRequestBuilder requestBuilder)
        {
            Client = client;
            this.requestBuilder = requestBuilder;
        }

        /// <inheritdoc />
        public virtual Task<UserModel> GetCurrentUser()
        {
            var arguments = new object[] {  };
            var func = methodImpls.GetOrAdd("GetCurrentUser()", _ => requestBuilder.BuildRestResultFuncForMethod("GetCurrentUser", new Type[] {  }));
            return (Task<UserModel>)func(Client, arguments);
        }

        /// <inheritdoc />
        public virtual Task<FormDigestModel> GetFormDigest()
        {
            var arguments = new object[] {  };
            var func = methodImpls.GetOrAdd("GetFormDigest()", _ => requestBuilder.BuildRestResultFuncForMethod("GetFormDigest", new Type[] {  }));
            return (Task<FormDigestModel>)func(Client, arguments);
        }

    }
}
