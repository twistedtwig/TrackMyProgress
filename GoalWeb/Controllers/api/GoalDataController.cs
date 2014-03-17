using System;
using System.Collections.Generic;
using System.Web.Http;
using Goals.Models;
using Goals.Models.RequestResponse;
using GoalManagement;
using RepositoryInterfaces;

namespace GoalWeb.Controllers.api
{
    [Authorize]
    public class GoalDataController : GoalBaseApiController
    {
        private readonly GoalManager _goalManager;

        public GoalDataController(GoalManager goalManager, IRepo repo) : base(repo)
        {
            _goalManager = goalManager;
        }

        public IEnumerable<TrackingSummary> GetTrackingSummmary([FromUri] DateTime startDate, DateTime endDate)
        {
            var x = _goalManager.GetTrackingInfoSummary(new GetTrackingSummaryRequest { StartDate = startDate, EndDate = endDate, UserId = UserId});
            return x;
        }

        public IterationDetailInformationModel GetIterationDetailInfo(int goalId)
        {
            if (goalId <= 0) return null;
            return _goalManager.GetIterationDetailInfo(UserId, goalId);
        }

        public IterationSummaryReportViewModel GetIterationReportModel([FromUri] int[] iterationIds, [FromUri] int goalId)
        {
            return _goalManager.GetIterationsReport(UserId, goalId, iterationIds);
        }

        public IterationDetailReportViewModel GetIterationDetailsReport([FromUri] int goalId, [FromUri] int[] iterationIds)
        {
            return _goalManager.GetIterationDetailsReport(UserId, goalId, iterationIds);
        }

    }

}
