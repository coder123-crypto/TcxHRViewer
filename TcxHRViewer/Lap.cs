// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.Collections.Generic;

namespace TcxHRViewer
{
    public class Lap : List<Point>
    {
        public DateTime StartTime { get; }

        public Lap(DateTime startTime)
        {
            StartTime = startTime;
        }
    }
}