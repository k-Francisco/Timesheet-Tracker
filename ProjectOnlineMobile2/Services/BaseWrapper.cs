using ProjectOnlineMobile2.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace ProjectOnlineMobile2.Services
{
    public class BaseWrapper
    {
        protected HttpClientHandler handler { get; private set; }
        protected MediaTypeWithQualityHeaderValue mediaType { get; private set; }

        public BaseWrapper()
        {
            if(mediaType == null)
            {
                mediaType = new MediaTypeWithQualityHeaderValue("application/json");
                mediaType.Parameters.Add(new NameValueHeaderValue("odata", "verbose"));
            }

        }

    }
}
