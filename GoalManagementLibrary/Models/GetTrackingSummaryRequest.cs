using System;

namespace GoalManagementLibrary.Models
{
    public class GetTrackingSummaryRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
