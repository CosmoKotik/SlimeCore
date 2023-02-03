using SlimeCore.Core.Networking.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Version = SlimeCore.Core.Networking.Response.Version;

namespace SlimeCore.Core.Networking
{
    public class StatusResponse
    {
        public Version version { get; set; }
        public Players players { get; set; }
        public Description description { get; set; }
        public bool previewChat { get; set; }
        public bool enforcesSecureChat { get; set; }
    }
}
