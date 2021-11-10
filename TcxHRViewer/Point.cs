// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;

namespace TcxHRViewer
{
    public class Point
    {
        public DateTime Time { get; }

        public int HeartRate { get; }

        public Point(DateTime time, int heartRate)
        {
            Time = time;
            HeartRate = heartRate;
        }
    }
}