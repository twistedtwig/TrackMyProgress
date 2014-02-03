using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Behaviours.Enums;
using Behaviours.GoalStrategies;
using GoalInterfaces;
using GoalManagementLibrary.Models;
using System;
using Mappings;
using Models;
using Repository.Models;

namespace GoalManagementLibrary
{
    public class GoalManager
    {
        private readonly IGoalRepository _goalRepository;

        public GoalManager(IGoalRepository goalRepository)
        {
            _goalRepository = goalRepository;
        }



        private CreateGoalResult ValidateGoal(CreateGoalRequest request)
        {
            var result = new CreateGoalResult();
            result.Request = request;

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                result.Success = false;
                result.Messages.Add("Goals requires a name.");
            }

            if (!IsGoalNameUnique<GoalEntity>(request.Id, request.Name, g => g.Name))
            {
                result.Success = false;
                result.Messages.Add("Goals Name must be unique.");
            }
            
            if (string.IsNullOrWhiteSpace(request.ShortName) || request.ShortName.Length > Goal.MaxShortNameLength)
            {
                result.Success = false;
                result.Messages.Add("Goals shortname is required and can't be more than " + Goal.MaxShortNameLength + " characters.");
            }

            if (!IsGoalNameUnique<GoalEntity>(request.Id, request.ShortName, g => g.ShortName))
            {
                result.Success = false;
                result.Messages.Add("Goals short Name must be unique.");
            }

           
            if (!request.StartDate.HasValue)
            {
                result.Success = false;
                result.Messages.Add("Goals requires a start date.");
            }
            else if (request.StartDate.Value.Equals(DateTime.MinValue))
            {
                result.Success = false;
                result.Messages.Add("Goals requires a start date.");
            }

            if (request.CategoryId < 1)
            {
                result.Success = false;
                result.Messages.Add("Goals requires a category.");
            }


            if (request.GoalTypeId < 1)
            {
                result.Success = false;
                result.Messages.Add("Goals requires a Goal Type.");
            }

            if (request.GoalDurationTypeId < 1)
            {
                result.Success = false;
                result.Messages.Add("Goals requires a Goal Duration Length.");
            }

            if (request.GoalBehaviourTypeId < 1)
            {
                result.Success = false;
                result.Messages.Add("Goals requires a Goal Behaviour Type.");
            }

            if (!((GoalBehaviourType)request.GoalBehaviourTypeId == GoalBehaviourType.None)
                && request.ChangeValue == 0)
            {
                result.Success = false;
                result.Messages.Add("When a Goals behaviour is not NONE the change value can not be zero.");
            }

            //only want to check this if all else is good, otherwise get silly errors on the client.
            if (result.Success)
            {
                var cat = _goalRepository.Get<CategoryEntity>(request.CategoryId);

                if (cat == null)
                {
                    result.Success = false;
                    result.Messages.Add("Invalid category ID: " + request.CategoryId);
                }
            }

            return result;
        }

        public bool IsGoalNameUnique<T>(int id, string name, Expression<Func<T, string>> action)
        {
            var expression = (MemberExpression)action.Body;
     
            var item = Expression.Parameter(typeof(T), "item");
            var prop = Expression.Property(item, expression.Member.Name);
            var propName = Expression.Constant(name);
            var equal = Expression.Equal(prop, propName);
            var lambda = Expression.Lambda<Func<T, bool>>(equal, item);
            var allGoalsWithName = _goalRepository.All<T>().AsQueryable().Where(lambda);

            if (id == 0)
            {
                return !allGoalsWithName.Any();
            }

            if (!allGoalsWithName.Any())
            {
                return true;
            }

            //if 1 then need to check if id == id param //i.e. it is yours and the name hasnt changed.
            if (allGoalsWithName.Count() == 1)
            {
                var itemId = Expression.Parameter(typeof(T), "item");
                var propId = Expression.Property(itemId, "Id");
                var propValue = Expression.Constant(id);
                var equalId = Expression.Equal(propId, propValue);
                var lambdaId = Expression.Lambda<Func<T, bool>>(equalId, itemId);
                var allGoalsWithNameId = allGoalsWithName.Where(lambdaId);

                return allGoalsWithNameId.Any();
            }


            //if more than one then its a dup what ever.
            if (allGoalsWithName.Count() > 1) return false;

            return false;
        }
        
        public CreateGoalResult CreateGoal(CreateGoalRequest request)
        {
            if (request == null) throw new ArgumentNullException("request");

            var result = ValidateGoal(request);

            if (!result.Success)
            {
                return result;
            }

            var goalEntity = new GoalEntity();
            goalEntity.Category = _goalRepository.Get<CategoryEntity>(request.CategoryId);


            if (!result.Success)
            {
                return result;
            }

            goalEntity.Name = request.Name;
            goalEntity.ShortName = request.ShortName;
            goalEntity.HexColour = request.HexColour;
            goalEntity.IntervalDurationId = request.GoalDurationTypeId;
            goalEntity.StartDate = request.StartDate.Value;
            goalEntity.EnumGoalBehaviourId = request.GoalBehaviourTypeId;
            goalEntity.ChangeValue = request.ChangeValue;
            goalEntity.UnitDescription = request.UnitDescription;
            goalEntity.EnumGoalTypeId = request.GoalTypeId;

            Goal tempGoal = GoalMapper.Map(goalEntity);
            EnsureGoalHasAllIterations(tempGoal, DateTime.Now, request.FirstIterationTarget);
            goalEntity.Intervals = tempGoal.Intervals.Select(GoalIterationMapper.Map).ToList();
           
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

            var validation = ValidateGoal(new CreateGoalRequest(goal));
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

            EnsureGoalHasAllIterations(goal, DateTime.Now);

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

            return GetCurrentIteration(goal, currentDate);
        }

        public GoalIteration GetCurrentIteration(Goal goal, DateTime currentDate)
        {
            if (goal == null) return null;            
            return goal.Intervals.FirstOrDefault(i => i.StartDate <= currentDate && i.EndDate >= currentDate);
        }


        public Goal EnsureGoalHasAllIterations(Goal goal, DateTime currentDate, double? initialTarget = null)
        {
            if (goal == null) return null;
            if (goal.StartDate == DateTime.MinValue) return goal;

            var behaviourStrategy = GoalBehaviourFactory.Create(goal.BehaviourType);


            if (goal.Intervals == null) goal.Intervals = new List<GoalIteration>();

            var currentStartDate = DateHelper.GetStartOfDuration(goal.IntervalDuration, goal.StartDate);
            var currentEndDate = DateHelper.GetEndOfDuration(goal.IntervalDuration, currentStartDate);
            var target = initialTarget.HasValue ? initialTarget.Value : 0;

            var goals = goal.Intervals.OrderBy(x => x.StartDate).ToList();
            if (target == 0 && goals.Any())
            {
                target = goals.First().Target;
            }

            bool stillCreating = true;

            while (stillCreating)
            {
                if (currentStartDate.Date < currentDate.Date)
                {
                    var g = goals.FirstOrDefault(i => i.StartDate.Date.Equals(currentStartDate.Date) && i.EndDate.Date.Equals(currentEndDate.Date));
                    if (g != null)
                    {
                        if (g.Target == 0)
                        {
                            g.Target = target;
                        }
                    }
                    else
                    {
                        var iteration = new GoalIteration
                        {
                            StartDate = currentStartDate,
                            EndDate = currentEndDate,
                            Target = target
                        };
                        goals.Add(iteration);
                    }
                }
                else
                {
                    stillCreating = false;
                }

                target = behaviourStrategy.Execute(target, goal.ChangeValue);
                currentStartDate = DateHelper.AddDuration(goal.IntervalDuration, currentStartDate);
                currentEndDate = DateHelper.GetEndOfDuration(goal.IntervalDuration, currentStartDate);
            }

            goal.Intervals = goals.OrderBy(x => x.StartDate).ToList();
            return goal;
        }


        public bool AddEntry(int goalId, int iterationId, DateTime currentDate, double value)
        {
            using (var uow = _goalRepository.CreateUnitOfWork())
            {

                var goalEntity = uow.Get<GoalEntity>(goalId);
                if (goalEntity == null) return false;

                var goal = GoalMapper.Map(goalEntity);
                goal = EnsureGoalHasAllIterations(goal, currentDate);

                var iteration = GetCurrentIteration(goal, currentDate);
                if (iteration == null) return false;

                iteration.Entries.Add(new GoalRecord { Amount = value, Date = currentDate });

                //update the iteration values so that the totals are correct.
                UpdateIterationValues(iteration);

                uow.Update(GoalMapper.Map(goal));
                return true;
            }
        }
     
        private void UpdateIterationValues(GoalIterationEntity iteration)
        {
            iteration.Achieved = CalculateAchieved(iteration.Entries.Select(e => e.Amount));
            iteration.Percentage = CalculatePercentage(iteration.Achieved, iteration.Target);
        }

        private void UpdateIterationValues(GoalIteration iteration)
        {
            iteration.Achieved = CalculateAchieved(iteration.Entries.Select(e => e.Amount));
            iteration.Percentage = CalculatePercentage(iteration.Achieved, iteration.Target);
        }

        private double CalculateAchieved(IEnumerable<double> list)
        {
            return list.Sum();
        }

        private double CalculatePercentage(double achieved, double target)
        {
            return Math.Round((achieved / target) * 100, 2);
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
                UpdateIterationValues(iteration);
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
            CalcuateIterations(goal.Intervals, goalSummary);
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

            var iterationList = GetGoalIterationEntities(iterationIds, goal);

            CalcuateIterations(iterationList, goalSummary);
            return goalSummary;
        }


        public TwoLineReportViewModel GetIterationsReport(int goalId, int[] iterationIds)
        {
            var goal = _goalRepository.First<GoalEntity>(g => g.Id == goalId);
            if (goal== null || iterationIds == null || iterationIds.Length == 0) return null;
            
            var iterationList = GetGoalIterationEntities(iterationIds, goal);
            if (!iterationList.Any()) return null;

            var reportModel = new TwoLineReportViewModel();
            reportModel.Title = string.Format("{0} summary: {1} to {2}", goal.Name, iterationList.First().StartDate.Date.ToShortDateString(), iterationList.Last().EndDate.Date.ToShortDateString());

            foreach (var entity in iterationList)
            {
                reportModel.ReportItems.Add(new TwoLineReportItem
                    {
                        Achieved = entity.Achieved,
                        Target = entity.Target,
                        XaxisValue = entity.StartDate.Date.ToShortDateString()
                    });
            }

            return reportModel;
        }

        private void CalcuateIterations(IList<GoalIterationEntity> iterations, GoalSummary goalSummary)
        {
            goalSummary.NumberOfIterations = iterations.Count;
            goalSummary.AvgEntriesPerIteration = iterations.Sum(x => x.Entries.Count) / iterations.Count;
            goalSummary.AvgPercentageToTarget = iterations.Sum(x => x.Percentage) / iterations.Count;
        }

        private List<GoalIterationEntity> GetGoalIterationEntities(int[] iterationIds, GoalEntity goal)
        {
            var iterationList = new List<GoalIterationEntity>();
            foreach (var id in iterationIds)
            {
                var iter = goal.Intervals.FirstOrDefault(x => x.Id == id);
                if (iter != null)
                {
                    if (iterationList.All(x => x.Id != id))
                    {
                        iterationList.Add(iter);
                    }
                }
            }
            return iterationList;
        }
    }
}
