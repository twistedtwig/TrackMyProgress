using GoalManagementLibrary;
using GoalManagementLibrary.Models;
using Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Web.Controllers
{
    public class GoalDataController : ApiController
    {
         private GoalManager _goalManager;

         public GoalDataController(GoalManager goalManager)
        {
            _goalManager = goalManager;
        }

        public IEnumerable<TrackingSummary> GetTrackingSummmary([FromUri] DateTime startDate, DateTime endDate)
        {
            var x = _goalManager.GetTrackingInfoSummary(new GetTrackingSummaryRequest { StartDate = startDate, EndDate = endDate });
            return x;
        }
    }

}
