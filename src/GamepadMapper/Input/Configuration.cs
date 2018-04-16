using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamepadMapper.Input
{
    public class Configuration
    {
        public double LtDeadZone { get; set; } = 0.2d;

        public double RtDeadZone { get; set; } = 0.2d;

        public double LsDeadZone { get; set; } = 0.1d;

        public double RsDeadZone { get; set; } = 0.1d;
    }
}
