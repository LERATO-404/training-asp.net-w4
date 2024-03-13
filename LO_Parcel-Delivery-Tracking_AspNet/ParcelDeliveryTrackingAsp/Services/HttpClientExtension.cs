﻿using System.Net.Http.Headers;

namespace ParcelDeliveryTrackingAsp.Services
{
    public static class HttpClientExtension
    {
        public static HttpClient AddBearerToken(this HttpClient cl, string token)
        {
            //int timeoutSec = 90;
            //cl.Timeout = new TimeSpan(0, 0, timeoutSec);

            string contentType = "application/json";
            cl.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
            cl.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", token));
            var userAgent = "d-fens HttpClient";
            cl.DefaultRequestHeaders.Add("User-Agent", userAgent);

            return cl;
        }
    }
}
