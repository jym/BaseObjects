using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Base.Objects
{
    public class AuthInfo
    {
        HttpContext _httpContext;

        public AuthInfo(IHttpContextAccessor contextAccessor)
        {
            _httpContext = contextAccessor.HttpContext;
        }
    }
}
