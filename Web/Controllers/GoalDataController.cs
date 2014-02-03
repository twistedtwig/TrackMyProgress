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

        public IterationDetailInformationModel GetIterationDetailInfo(int goalId)
        {
            if (goalId <= 0) return null;
            return _goalManager.GetIterationDetailInfo(goalId);
        }

        public TwoLineReportViewModel GetIterationReportModel([FromUri] int goalId, [FromUri] int[] iterationIds)
        {
            var ints = new int[3];
            return _goalManager.GetIterationsReport(goalId, ints);
        }

        [HttpGet]
        public TwoLineReportViewModel Test([FromUri] int[] iterationIds, int goalId)
        {
            return null;
        }
    }

}
