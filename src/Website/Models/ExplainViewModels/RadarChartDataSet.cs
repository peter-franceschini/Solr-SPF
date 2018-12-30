using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Models.ExplainViewModels
{
    public class RadarChartDataSet
    {
        public string Label { get; set; }
        public double?[] RadarChartDataPoints { get; set; }
        public string ColorOne { get; set; }
        public string ColorTwo { get; set; }
    }
}
