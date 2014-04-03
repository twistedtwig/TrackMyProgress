using EntityModels;
using Goals.Mappings;
using Goals.Models;
using Goals.Models.RequestResponse;
using Goals.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Goals.Shared.Extensions;
using RepositoryInterfaces;

namespace GoalManagement
{
    public class GoalManager
    {
        private readonly IRepo _repository;
        private readonly GoalValidation _goalValidation;

        public GoalManager(IRepo goalRepository)
        {
            _repository = goalRepository;
            _goalValidation = new GoalValidation(_repository);
        }
        
        public CreateGoalResult CreateGoal(CreateGoalRequest request)
        {
            var goalCreator = new GoalCreation(_repository, _goalValidation);
            var result = goalCreator.ValidateCreateGoal(request);
            if (!result.Success) return result;

            var goalEntity = goalCreator.CreateGoalObject(request);

            using (var uow = _repository.CreateUnitOfWork())
            {
                uow.Add(goalEntity);
            }

            result.Goal = GoalMapper.Map(goalEntity);
            result.Success = true;
            result.AddMessage("The Goal was created successfully");
            return result;
        }

        public Goal Get(Guid userId, int id)
        {
            var goalEntity = _repository.Get<GoalEntity>(id);
            if (goalEntity.UserId.Equals(userId))
            {
                return GoalMapper.Map(goalEntity);
            }

            return null;
        }

        public List<Goal> Goals(Guid userId)
        {
            return _repository.All<GoalEntity>().Where(g => g.UserId.Equals(userId)).Select(GoalMapper.Map).ToList();
        }

        public IList<Goal> Goals(Guid userId, DateTime currentDate)
        {
            return _repository.Get<GoalEntity>(g => g.StartDate <= currentDate && g.UserId.Equals(userId)).Select(GoalMapper.Map).ToList();
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
                foreach (var message in validation.Messages)
                {
                    result.AddMessage(message);
                }
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

            using (var uow = _repository.CreateUnitOfWork())
            {
                uow.Update(GoalMapper.Map(goal));
            }

            result.Success = true;
            return result;
        }


        public void Delete(Guid userId, Goal goal)
        {
            if (goal == null) return;

            using (var uow = _repository.CreateUnitOfWork())
            {
                var goalEntity = uow.Get<GoalEntity>(goal.Id);
                if (goalEntity.UserId.Equals(userId))
                {
                    uow.Remove(goalEntity);
                }
            }
        }


        public GoalIteration GetCurrentIteration(Guid userId, int goalId, DateTime currentDate)
        {
            var localCurrentDate = currentDate.ConvertToLocal();
            var goal = GoalMapper.Map(_repository.Get<GoalEntity>(goalId));
            if (goal == null || !goal.UserId.Equals(userId)) return null;

            return GoalUtilities.GetCurrentIteration(goal, localCurrentDate);
        }



        public bool AddEntry(Guid userId, int goalId, int iterationId, DateTime currentDate, double value)
        {
            var localCurrentDate = currentDate.ConvertToLocal();

            using (var uow = _repository.CreateUnitOfWork())
            {
                var goalEntity = uow.Get<GoalEntity>(goalId);
                if (goalEntity == null || !goalEntity.UserId.Equals(userId)) return false;

                var goal = GoalMapper.Map(goalEntity);
                goal = GoalUtilities.EnsureGoalHasAllIterations(goal, localCurrentDate);

                var iteration = GoalUtilities.GetCurrentIteration(goal, localCurrentDate);
                if (iteration == null) return false;

                iteration.Entries.Add(new GoalRecord { Amount = value, Date = localCurrentDate });

                //update the iteration values so that the totals are correct.
                GoalUtilities.UpdateIterationValues(iteration);

                uow.Update(GoalMapper.Map(goal));
                return true;
            }
        }

        public IList<TrackingSummary> GetTrackingInfoSummary(GetTrackingSummaryRequest request)
        {
            var goals = _repository.Get<GoalEntity>(x => x.UserId.Equals(request.UserId));

            //ensure dates are in local format
            GoalMapper.ConvertToLocalDate(goals);
            
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

        public IterationDetail GetIterationDetail(Guid userId, int iterationEntryId)
        {
            var goals = _repository.Get<GoalEntity>(x => x.UserId.Equals(userId));

            //ensure dates are in local format
            GoalMapper.ConvertToLocalDate(goals);

            return (from goal in goals
                    from interval in goal.Intervals
                    from entry in interval.Entries
                    where entry.Id == iterationEntryId
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

        public bool Delete(Guid userId, IterationDetail detail)
        {
            if (detail == null) return false;

            using (var uow = _repository.CreateUnitOfWork())
            {
                var goalEntity = uow.Get<GoalEntity>(detail.GoalId);
                if (goalEntity == null || !goalEntity.UserId.Equals(userId)) return false;

                var iteration = goalEntity.Intervals.FirstOrDefault(i => i.Id == detail.IterationId);
                if (iteration == null) return false;

                var entry = iteration.Entries.FirstOrDefault(e => e.Id == detail.EntryId);
                if (entry == null) return false;

                iteration.Entries.Remove(entry);
                GoalUtilities.UpdateIterationValues(iteration);
                uow.Remove(entry);
                uow.Update(goalEntity);
                return true;
            }
        }

        public GoalSummary GetGoalSummary(Guid userId, int goalId)
        {
            var goal = _repository.First<GoalEntity>(g => g.Id == goalId);
            var goalSummary = new GoalSummary();

            if (goal == null || !goal.UserId.Equals(userId))
            {
                goalSummary.GoalId = -1;
                return goalSummary;
            }

            //ensure dates are in local format
            GoalMapper.ConvertToLocalDate(goal);

            goalSummary.Title = goal.Name;
            goalSummary.GoalId = goal.Id;
            GoalUtilities.CalcuateIterations(goal.Intervals, goalSummary);
            return goalSummary;
        }

        public IterationDetailInformationModel GetIterationDetailInfo(Guid userId, int goalId)
        {
            var goal = _repository.First<GoalEntity>(x => x.Id == goalId);
            if (goal == null || !goal.UserId.Equals(userId)) return null;

            //ensure dates are in local format
            GoalMapper.ConvertToLocalDate(goal);

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

        public GoalSummary GetIterationSummaries(Guid userId, int goalId, int[] iterationIds)
        {
            var goal = _repository.First<GoalEntity>(g => g.Id == goalId && g.UserId.Equals(userId));
            var goalSummary = new GoalSummary();

            if (iterationIds == null || iterationIds.Length == 0) return goalSummary;

            if (goal == null)
            {
                goalSummary.GoalId = -1;
                return goalSummary;
            }

            //ensure dates are in local format
            GoalMapper.ConvertToLocalDate(goal);

            var iterationList = GoalUtilities.GetGoalIterationEntities(iterationIds, goal);

            GoalUtilities.CalcuateIterations(iterationList, goalSummary);
            return goalSummary;
        }


        public IterationDetailReportViewModel GetIterationDetailsReport(Guid userId, int goalId, int[] iterationIds)
        {
            var goal = _repository.First<GoalEntity>(g => g.Id == goalId && g.UserId.Equals(userId));
            if (goal == null || iterationIds == null || iterationIds.Length == 0) return null;

            //ensure dates are in local format
            GoalMapper.ConvertToLocalDate(goal);

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

        public IterationSummaryReportViewModel GetIterationsReport(Guid userId, int goalId, int[] iterationIds)
        {
            var goal = _repository.First<GoalEntity>(g => g.Id == goalId && g.UserId.Equals(userId));
            //ensure dates are in local format
            GoalMapper.ConvertToLocalDate(goal);

            if (goal== null || iterationIds == null || iterationIds.Length == 0) return null;
            
            var iterationList = GoalUtilities.GetGoalIterationEntities(iterationIds, goal);
            if (!iterationList.Any()) return null;

            var reportModel = new IterationSummaryReportViewModel();
            reportModel.UnitDescription = goal.UnitDescription;
            reportModel.Title = string.Format("{0} Iteration Summary: {1} to {2}", goal.Name, iterationList.First().StartDate.ToShortDateString(), iterationList.Last().EndDate.Date.ToShortDateString());

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

            var goalCreator = new GoalCreation(_repository, _goalValidation);
            var goalEntity = goalCreator.CreateFromTarget(request);

            using (var uow = _repository.CreateUnitOfWork())
            {
                uow.Add(goalEntity);
            }

            result.Success = true;
            result.AddMessage("The Goal was created successfully");
            return result;
        }

        
    }
}
