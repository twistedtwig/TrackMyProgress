using System.Collections.Generic;

namespace Models
{
    public class IterationSummaryReportViewModel
    {
        public IterationSummaryReportViewModel()
        {
            ReportItems = new List<IterationSummaryItem>();
            Trend = new List<Trend>();
        }

        public string Title { get; set; }
        public string UnitDescription { get; set; }
        public IList<IterationSummaryItem> ReportItems { get; set; }
        public IList<Trend> Trend { get; set; }
    }

    public class IterationSummaryItem
    {
        public double Achieved { get; set; }
        public double Target { get; set; }
        public string XaxisValue { get; set; }
    }

    public class Trend
    {
        public Trend()
        {
            
        }

        public Trend(double value, string axis)
        {
            Value = value;
            XaxisValue = axis;
        }

        public double Value { get; set; }
        public string XaxisValue { get; set; }
    }
}
