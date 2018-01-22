using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gibbed.RedFaction2.FileFormats.Level.Data
{
    public enum EntityAIMode : byte
    {
        Catatonic = 0,
        Waiting = 1,
        Waypoints = 2,
        Collecting = 3,
        MotionDetection = 4,
    }
}
