// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using Microsoft.Win32;
using TcxHRViewer.Annotations;

namespace TcxHRViewer
{
    public sealed class Vm : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<Activity> Activities => _activities;
        private readonly List<Activity> _activities = new();
        public ICommand Add { get; }

        private void _add()
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Activities|*.tcx",
                FilterIndex = 0,
                RestoreDirectory = true
            };

            if (dialog.ShowDialog(Application.Current.MainWindow) == true)
            {
                foreach (var t in dialog.FileNames)
                {
                    _activities.Add(ParseTcx(t));
                }

                OnPropertyChanged(nameof(Activities));
            }
        }

        public ICommand Clear { get; }

        private void _clear()
        {
            _activities.Clear();
            OnPropertyChanged(nameof(Activities));
        }

        public Vm()
        {
            Add = new RelayCommand(_add);
            Clear = new RelayCommand(_clear);
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static Activity ParseTcx(string path)
        {
            var activity = new Activity(Path.GetFileNameWithoutExtension(path));

            var doc = new XmlDocument();
            doc.Load(path);

            foreach (var t in doc.DocumentElement["Activities"]["Activity"].Cast<XmlNode>().Where(t => t.Name == "Lap"))
            {
                var lap = new Lap(DateTime.Parse(t.Attributes["StartTime"].Value));
                foreach (XmlNode u in t["Track"])
                {
                    if (int.TryParse(u["HeartRateBpm"]?.InnerText, out int rate))
                    {
                        lap.Add(new Point(DateTime.Parse(u["Time"].InnerText), rate));
                    }
                }

                activity.Add(lap);
            }

            return activity;
        }
    }
}