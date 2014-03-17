using System;

namespace Goals.Models.RequestResponse
{
    public class GetTrackingSummaryRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid UserId { get; set; }
    }
}
