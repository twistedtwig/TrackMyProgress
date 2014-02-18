using Behaviours.Enums;
using GoalInterfaces;
using GoalManagementLibrary.Models;
using Mappings;
using Models;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoalManagementLibrary
{
    public class GoalManager
    {
        private readonly IGoalRepository _goalRepository;
        private readonly GoalValidation _goalValidation;

        public GoalManager(IGoalRepository goalRepository)
        {
            _goalRepository = goalRepository;
            _goalValidation = new GoalValidation(_goalRepository);
        }
        
        public CreateGoalResult CreateGoal(CreateGoalRequest request)
        {
            var goalCreator = new GoalCreation(_goalRepository, _goalValidation);
            var result = goalCreator.ValidateCreateGoal(request);
            if (!result.Success) return result;

            var goalEntity = goalCreator.CreateGoalObject(request);

            using (var uow = _goalRepository.CreateUnitOfWork())
            {
                uow.Add(goalEntity);
            }

            result.Goal = GoalMapper.Map(goalEntity);
            result.Success = true;
            result.Messages.Add("The Goal was created successfully");
            return result;
        }

        public Goal Get(int id)
        {
            return GoalMapper.Map(_goalRepository.Get<GoalEntity>(id));
        }

        public List<Goal> Goals()
        {
            return _goalRepository.GetGoals().Select(GoalMapper.Map).ToList();
        }

        public IList<Goal> Goals(DateTime currentDate)
        {
            return _goalRepository.Get<GoalEntity>(g => g.StartDate <= currentDate).Select(GoalMapper.Map).ToList();
        }

        public SaveResult<Goal> Save(Goal goal)
        {
            var result = new SaveResult<Goal>();
            result.Model = goal;

            if (goal == null)
            {
                result.Success = false;
                result.Messages.Add("No goal model provided.");
            }

            var validation = _goalValidation.ValidateGoal(new CreateGoalRequest(goal));
            if (!validation.Success)
            {
                result.Success = false;
                result.Messages.AddRange(validation.Messages);
            }
            else
            {
                result.Success = validation.Success;
            }

            if (!result.Success)
            {
                return result;
            }

            GoalUtilities.EnsureGoalHasAllIterations(goal, DateTime.Now);

            using (var uow = _goalRepository.CreateUnitOfWork())
            {
                uow.Update(GoalMapper.Map(goal));
            }

            result.Success = true;
            return result;
        }


        public void Delete(Goal goal)
        {
            if (goal == null) return;

            using (var uow = _goalRepository.CreateUnitOfWork())
            {
                var goalEntity = uow.Get<GoalEntity>(goal.Id);
                uow.Remove(goalEntity);
            }
        }


        public GoalIteration GetCurrentIteration(int goalId, DateTime currentDate)
        {
            var goal = GoalMapper.Map(_goalRepository.Get<GoalEntity>(goalId));
            if (goal == null) return null;

            return GoalUtilities.GetCurrentIteration(goal, currentDate);
        }

        

        public bool AddEntry(int goalId, int iterationId, DateTime currentDate, double value)
        {
            using (var uow = _goalRepository.CreateUnitOfWork())
            {

                var goalEntity = uow.Get<GoalEntity>(goalId);
                if (goalEntity == null) return false;

                var goal = GoalMapper.Map(goalEntity);
                goal = GoalUtilities.EnsureGoalHasAllIterations(goal, currentDate);

                var iteration = GoalUtilities.GetCurrentIteration(goal, currentDate);
                if (iteration == null) return false;

                iteration.Entries.Add(new GoalRecord { Amount = value, Date = currentDate });

                //update the iteration values so that the totals are correct.
                GoalUtilities.UpdateIterationValues(iteration);

                uow.Update(GoalMapper.Map(goal));
                return true;
            }
        }

        public IList<TrackingSummary> GetTrackingInfoSummary(GetTrackingSummaryRequest request)
        {
            var goals = _goalRepository.All<GoalEntity>();

            return (from goal in goals
                    from interval in goal.Intervals
                    from entry in interval.Entries
                    where entry.Date >= request.StartDate && entry.Date <= request.EndDate
                    select new TrackingSummary
                        {
                            Id = entry.Id, 
                            Date = entry.Date, 
                            Name = goal.ShortName,
                            HexColour = "#" + goal.HexColour
                        }
                    ).ToList();
        }

        public IterationDetail GetIterationDetail(int IterationEntryId)
        {
            var goals = _goalRepository.All<GoalEntity>();

            return (from goal in goals
                    from interval in goal.Intervals
                    from entry in interval.Entries
                    where entry.Id == IterationEntryId
                    select new IterationDetail
                        {
                            EntryId = entry.Id, 
                            IterationId = interval.Id, 
                            GoalId = goal.Id, 
                            Date = entry.Date, 
                            GoalName = goal.Name, 
                            Units = goal.UnitDescription, 
                            Value = entry.Amount
                        }).FirstOrDefault();
        }

        public bool Delete(IterationDetail detail)
        {
            if (detail == null) return false;

            using (var uow = _goalRepository.CreateUnitOfWork())
            {
                var goalEntity = uow.Get<GoalEntity>(detail.GoalId);
                if (goalEntity == null) return false;

                var iteration = goalEntity.Intervals.FirstOrDefault(i => i.Id == detail.IterationId);
                if (iteration == null) return false;

                var entry = iteration.Entries.FirstOrDefault(e => e.Id == detail.EntryId);
                if (entry == null) return false;

                iteration.Entries.Remove(entry);
                GoalUtilities.UpdateIterationValues(iteration);
                uow.Update(goalEntity);
                return true;
            }
        }

        public GoalSummary GetGoalSummary(int goalId)
        {
            var goal = _goalRepository.First<GoalEntity>(g => g.Id == goalId);
            var goalSummary = new GoalSummary();

            if (goal == null)
            {
                goalSummary.GoalId = -1;
                return goalSummary;
            }

            goalSummary.Title = goal.Name;
            goalSummary.GoalId = goal.Id;
            GoalUtilities.CalcuateIterations(goal.Intervals, goalSummary);
            return goalSummary;
        }

        public IterationDetailInformationModel GetIterationDetailInfo(int goalId)
        {
            var goal = _goalRepository.First<GoalEntity>(x => x.Id == goalId);
            if (goal == null) return null;

            var detail = new IterationDetailInformationModel();
            detail.GoalId = goal.Id;

            foreach (var interval in goal.Intervals)
            {
                detail.Iterations.Add(new IterationSummary
                    {
                        IterationId = interval.Id,
                        StartDate = interval.StartDate.Date.ToShortDateString(),
                        EndDate = interval.EndDate.Date.ToShortDateString()
                    });
            }

            int index = goal.Intervals.Count - 1;
            var intervalLast = goal.Intervals[index];
            while (intervalLast.EndDate > DateTime.Now && index > 0)
            {
                index--;
                intervalLast = goal.Intervals[index];
            }

            detail.InitialEnd = index;
            detail.InitialStart = index >= 6 ? index - 6 : 0;

            return detail;
        }

        public GoalSummary GetIterationSummaries(int goalId, int[] iterationIds)
        {
            var goal = _goalRepository.First<GoalEntity>(g => g.Id == goalId);
            var goalSummary = new GoalSummary();

            if (iterationIds == null || iterationIds.Length == 0) return goalSummary;

            if (goal == null)
            {
                goalSummary.GoalId = -1;
                return goalSummary;
            }

            var iterationList = GoalUtilities.GetGoalIterationEntities(iterationIds, goal);

            GoalUtilities.CalcuateIterations(iterationList, goalSummary);
            return goalSummary;
        }


        public IterationDetailReportViewModel GetIterationDetailsReport(int goalId, int[] iterationIds)
        {
            var goal = _goalRepository.First<GoalEntity>(g => g.Id == goalId);
            if (goal == null || iterationIds == null || iterationIds.Length == 0) return null;

            var titles = goal.Intervals.Where(i => iterationIds.Contains(i.Id)).Select(interval => interval.StartDate.ToShortDateString() + " - " + interval.EndDate.ToShortDateString()).ToList();

            var model = new IterationDetailReportViewModel(goal.IntervalDurationId, titles);
            model.Title = "Iteration Details Report";
            model.UnitDesciption = goal.UnitDescription;
            
            foreach (var interval in goal.Intervals)
            {
                if(!iterationIds.Contains(interval.Id))
                {
                    continue;
                }

                var startDate = interval.StartDate;
                foreach (var entry in interval.Entries)
                {
                    TimeSpan span = entry.Date - startDate;
                    var dayInt = span.Days;

                    model.Add(dayInt, interval.StartDate.ToShortDateString() + " - " + interval.EndDate.ToShortDateString(), entry.Amount);
                }
            }

            model.Organize();
            return model;
        }

        public IterationSummaryReportViewModel GetIterationsReport(int goalId, int[] iterationIds)
        {
            var goal = _goalRepository.First<GoalEntity>(g => g.Id == goalId);
            if (goal== null || iterationIds == null || iterationIds.Length == 0) return null;
            
            var iterationList = GoalUtilities.GetGoalIterationEntities(iterationIds, goal);
            if (!iterationList.Any()) return null;

            var reportModel = new IterationSummaryReportViewModel();
            reportModel.UnitDescription = goal.UnitDescription;
            reportModel.Title = string.Format("{0} Iteration Summary: {1} to {2}", goal.Name, iterationList.First().StartDate.Date.ToShortDateString(), iterationList.Last().EndDate.Date.ToShortDateString());

            foreach (var entity in iterationList)
            {
                reportModel.ReportItems.Add(new IterationSummaryItem
                    {
                        Achieved = entity.Achieved,
                        Target = entity.Target,
                        XaxisValue = entity.StartDate.Date.ToShortDateString()
                    });
            }

            //find average increase per iteration
            IList<double> variance = new List<double>();

            double? previousValue = null;
            foreach (var iteration in iterationList)
            {
                if (!previousValue.HasValue)
                {
                    previousValue = iteration.Achieved;
                    continue;
                }

                variance.Add(((iteration.Achieved - previousValue.Value) / previousValue.Value) * 100);
                previousValue = iteration.Achieved;
            }

            var avgVariance = variance.Any() ? variance.Sum() / variance.Count : 0;

            //find last iteration total
            double lastTotal = iterationList.Aggregate<GoalIterationEntity, double>(0, (current, it) => it.Achieved > 0 ? it.Achieved : current);

            //create 6 more iterations
            int numberOfTrendIterations = 6;
            var currentStartDate = iterationList.Last().StartDate;
            var iterationDates = new List<DateTime>();
            for (int i = 0; i < numberOfTrendIterations; i++)
            {
                var endDate = DateHelper.GetEndOfDuration((GoalDurationType) goal.IntervalDurationId, currentStartDate);
                currentStartDate = DateHelper.GetStartOfDuration((GoalDurationType)goal.IntervalDurationId, endDate.AddDays(1));
                
                iterationDates.Add(currentStartDate);                
            }

            //for each new iteration increase value by total * avg increment
            double runningTotal = lastTotal;

            foreach (var date in iterationDates)
            {
                runningTotal = runningTotal * ((avgVariance / 100) +1);
                reportModel.Trend.Add(new Trend(runningTotal, date.ToShortDateString()));
            }

            return reportModel;
        }

        

        public CreateGoalFromTargetResult CreateGoalFromTarget(CreateGoalFromTarget request)
        {
            if (request == null) throw new ArgumentNullException("request");

            var result = new CreateGoalFromTargetResult();

            //validate input
            _goalValidation.ValidateCreateGoalFromTarget(request, result);

            if (!result.Success)
            {
                return result;
            }

            var goalCreator = new GoalCreation(_goalRepository, _goalValidation);
            var goalEntity = goalCreator.CreateFromTarget(request);

            using (var uow = _goalRepository.CreateUnitOfWork())
            {
                uow.Add(goalEntity);
            }

            result.Success = true;
            result.Messages.Add("The Goal was created successfully");
            return result;
        }

        
    }
}
