using System;

namespace Puya.Api
{
    public class PuyaGatewayAttribute : Attribute
    {
        public string? RoutePattern { get; set; }
        public int Priority { get; set; } = 0;
    }
}
