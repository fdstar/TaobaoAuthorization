using System;
using System.Collections.Generic;
using System.Text;

namespace TaobaoAuthorization.Configuration
{
    public class AppSettings
    {
        public string TaobaoOAuthUrl { get; set; }
        public string RedirectHost { get; set; }
        public int SignTimeRange { get; set; } = 5;
    }
}
