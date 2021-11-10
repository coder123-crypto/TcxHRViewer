// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System.Collections.Generic;

namespace TcxHRViewer
{
    public class Activity : List<Lap>
    {
        public string Title { get; }

        public Activity(string title)
        {
            Title = title;
        }
    }
}