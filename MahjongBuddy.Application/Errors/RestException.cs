using System;
using System.Net;

namespace MahjongBuddy.Application.Errors
{
    public class RestException : Exception
    {
        public HttpStatusCode Code { get; }
        public Object Errors { get; }
        public RestException(HttpStatusCode code, object errors = null)
        {
            Code = code;
            Errors = errors;
        }
    }
}
