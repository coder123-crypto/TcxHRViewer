// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using LinearAxis = OxyPlot.Axes.LinearAxis;
using TimeSpanAxis = OxyPlot.Axes.TimeSpanAxis;

namespace TcxHRViewer
{
    public class ActivitiesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable<Activity> activities)
            {
                var model = new PlotModel
                {
                    LegendTitle = "Activities",
                    LegendPosition = LegendPosition.LeftTop
                };

                model.Axes.Add(new TimeSpanAxis
                {
                    Position = AxisPosition.Bottom,
                    Title = "Time",
                    MajorGridlineStyle = LineStyle.Solid
                });
                model.Axes.Add(new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Title = "HeartRate",
                    MajorGridlineStyle = LineStyle.Solid
                });

                int colorIndex = 0;
                foreach (var activity in activities)
                {
                    bool solid = true;
                    for (int i = 0; i < activity.Count; i++)
                    {
                        var lap = activity[i];

                        var series = new LineSeries
                        {
                            Color = Colors[colorIndex % Colors.Count],
                            Selectable = false,
                            LineStyle = solid ? LineStyle.Solid : LineStyle.Dot,
                            Title = i == 0 ? activity.Title : string.Empty,
                            ItemsSource = lap.Select(t => new
                            {
                                X = t.Time - activity[0].StartTime,
                                Y = t.HeartRate,
                                Z = $"Lap #{activity.IndexOf(lap) + 1}\n" +
                                    $"Lap's length {(lap.Count > 1 ? lap[^1].Time - lap[0].Time : TimeSpan.Zero)}\n" +
                                    $"Time of activity: {t.Time - activity[0].StartTime}\n" +
                                    $"Time of lap: {t.Time - lap.StartTime}\n" +
                                    $"Heart rate: {t.HeartRate}"
                            }),
                            DataFieldX = "X",
                            DataFieldY = "Y",
                            TrackerFormatString = "{Z}"
                        };

                        model.Series.Add(series);
                        solid = !solid;
                    }

                    colorIndex++;
                }

                return model;
            }

            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static readonly IReadOnlyList<OxyColor> Colors = new List<OxyColor>
        {
            OxyColors.Red,
            OxyColors.Blue,
            OxyColors.Green,
            OxyColors.Magenta,
            OxyColors.Goldenrod,
            OxyColors.Salmon,
            OxyColors.Orchid,
            OxyColors.Olive,
            OxyColors.Khaki
        };
    }
}