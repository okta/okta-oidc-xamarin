// <copyright file="IdentityResponse.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace Okta.Xamarin.Widget.Pipeline.Identity
{
    public class IdentityResponse : IIdentityResponse
    {
        private Exception exception;

        public IdentityResponse() { }

        internal IdentityResponse(HttpResponseMessage responseMessage)
        {
            try
            {
                this.HttpResponseMessage = responseMessage.EnsureSuccessStatusCode();
                this.Raw = this.HttpResponseMessage?.Content?.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                this.Exception = ex;
            }
        }

        [JsonProperty("error")]
        public string ApiError { get; set; }

        [JsonProperty("error_description")]
        public string ApiErrorDescription { get; set; }

        [JsonProperty("interaction_handle")]
        public string InteractionHandle { get; set; }

        internal HttpResponseMessage HttpResponseMessage { get; }

        internal Exception Exception
        {
            get
            {
                if (this.exception == null && !string.IsNullOrEmpty(this.ApiError))
                {
                    this.exception = new IdentityApiException(this);
                }

                return this.exception;
            }
            set => this.exception = value;
        }

        [JsonIgnore]
        public string Raw { get; set; }

        [JsonIgnore]
        public bool HasException => this.Exception != null;

        public void EnsureSuccess()
        {
            if (this.HasException)
            {
                throw this.Exception;
            }
        }

        public T As<T>()
        {
            return JsonConvert.DeserializeObject<T>(this.Raw);
        }
    }
}
