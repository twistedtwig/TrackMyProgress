using System.Collections.Generic;

namespace Models
{
    public class TwoLineReportViewModel
    {
        public TwoLineReportViewModel()
        {
            ReportItems = new List<TwoLineReportItem>();
        }

        public string Title { get; set; }
        public IList<TwoLineReportItem> ReportItems { get; set; }
    }

    public class TwoLineReportItem
    {
        public double Achieved { get; set; }
        public double Target { get; set; }
        public string XaxisValue { get; set; }
    }
}
