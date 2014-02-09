using System.Collections.Generic;
using System.Linq;

namespace Models
{
    public class IterationDetailReportViewModel
    {
        public IterationDetailReportViewModel(int days, IList<string> keys)
        {
            Items = new List<IterationDetailReportDay>(days);
            for (int i = 0; i <= days; i++)
            {
                var item = new IterationDetailReportDay {Day = i};
                Items.Add(item);

                if (keys != null && keys.Any())
                {
                    foreach (var key in keys)
                    {
                        item.Values.Add(new IterationDetailReportItem { Key = key });
                    }
                }
            }
        }

        public string Title { get; set; }
        public string UnitDesciption { get; set; }
        public IList<IterationDetailReportDay> Items { get; set; }
       
        public void Add(int day, string key, double value)
        {
            var item = Items.First(i => i.Day == day);

            var v = item.Values.First(k => k.Key == key);
            v.Value = value;
        }

        public void Organize()
        {
            Items = Items.OrderBy(x => x.Day).ToList();

            IterationDetailReportDay previousDay = null;

            foreach (var item in Items)
            {
                if (previousDay == null)
                {
                    previousDay = item;
                    continue;
                }
                
                foreach (var dayValue in item.Values)
                {
                    var previousDayItem = previousDay.Values.First(x => x.Key == dayValue.Key);
                    dayValue.Value += previousDayItem.Value;
                }

                item.Average = item.Values.Sum(x => x.Value)/item.Values.Count;

                previousDay = item;
            }
        }
    }

    public class IterationDetailReportDay
    {
        public IterationDetailReportDay()
        {
            Values = new List<IterationDetailReportItem>();
        }

        public int Day { get; set; }
        public IList<IterationDetailReportItem> Values { get; set; }
        public double Average { get; set; }
    }

    public class IterationDetailReportItem
    {
        public IterationDetailReportItem()
        {
            
        }

        public IterationDetailReportItem(string key, double value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }
        public double Value { get; set; }
    }
}
