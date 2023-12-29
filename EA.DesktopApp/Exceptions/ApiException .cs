using System;
using System.Net;

namespace EA.DesktopApp.Exceptions
{
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode;

        public ApiException(string message) : base(message)
        {
        }
    }
}