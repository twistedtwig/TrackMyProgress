using EntityModels;
using Goals.Mappings;
using Goals.Models;
using Goals.Models.RequestResponse;
using Goals.Shared.Enums;
using System;
using System.Linq;
using RepositoryInterfaces;

namespace GoalManagement
{
    internal class GoalCreation
    {
        private readonly IRepo _repository;
        private readonly GoalValidation _goalValidation;

        public GoalCreation(IRepo goalRepository, GoalValidation goalValidation)
        {
            _repository = goalRepository;
            _goalValidation = goalValidation;
        }


        internal CreateGoalResult ValidateCreateGoal(CreateGoalRequest request)
        {
            if (request == null) throw new ArgumentNullException("request");

            var result = _goalValidation.ValidateGoal(request);

            if (!result.Success)
            {
                return result;
            }

            if (!result.Success)
            {
                return result;
            }

            return result;
        }

        internal GoalEntity CreateGoalObject(CreateGoalRequest request)
        {
            if (!ValidateCreateGoal(request).Success) return null;

            var goalEntity = new GoalEntity();
            goalEntity.Category = _repository.Get<CategoryEntity>(request.CategoryId);

            goalEntity.UserId = request.UserId;
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
            GoalUtilities.EnsureGoalHasAllIterations(tempGoal, DateTime.Now, request.FirstIterationTarget);
            goalEntity.Intervals = tempGoal.Intervals.Select(GoalIterationMapper.Map).ToList();

            return goalEntity;
        }


        public GoalEntity CreateFromTarget(CreateGoalFromTarget request)
        {
            var goalType = (GoalType) request.GoalTypeId;
            switch (goalType)
            {
                case GoalType.ChangeSomething:
                    break;
                case GoalType.ReachSomething:
                    return CreateReachSomethingGoal(request);
                    
                case GoalType.TrackSomething:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            throw new ArgumentOutOfRangeException();
        }

        private GoalEntity CreateReachSomethingGoal(CreateGoalFromTarget request)
        {
            //generally assume validation has been done before hand but this is a last ditch attempt at stopping rubbish data.
            var result = new CreateGoalFromTargetResult();
            _goalValidation.ValidateCreateGoalFromTarget(request, result);
            if (!result.Success) return null;


            var goal = new GoalEntity();
            goal.UserId = request.UserId;
            goal.IntervalDurationId = request.GoalDurationTypeId;
            goal.HexColour = request.HexColour;
            goal.Name = request.Name;
            goal.ShortName = request.ShortName;
            goal.Category = _repository.Get<CategoryEntity>(request.CategoryId);
            goal.EnumGoalBehaviourId = request.GoalBehaviourTypeId;
            goal.EnumGoalTypeId = request.GoalTypeId;
            goal.StartDate = DateHelper.GetStartOfDuration((GoalDurationType) request.GoalDurationTypeId, request.StartDate.HasValue ? request.StartDate.Value : DateTime.Now);
            goal.UnitDescription = request.UnitDescription;


            //to be calculated

            var numberOfIteratoins = CalculateNumberOfIterations(goal.StartDate, request.TargetDate.Value, (GoalDurationType)request.GoalDurationTypeId);
            double changeValue = 0;

            if (request.GoalBehaviourTypeId == (int) GoalBehaviourType.IncrementValue)
            {
                changeValue = CalculateReachSomethingIterationChangeValue(request.CurrentValue.Value, request.TargetValue.Value, numberOfIteratoins);
            }
            else if (request.GoalBehaviourTypeId == (int) GoalBehaviourType.IncrementPercentage)
            {
                changeValue = CalculateReachSomethingIterationPercentageChange(request.CurrentValue.Value, request.TargetValue.Value, numberOfIteratoins);                
            }
            else
            {
                throw new ArgumentOutOfRangeException("GoalBehaviourTypeId");
            }
            //goal.ChangeValue
            goal.ChangeValue = changeValue;
            //setup all the intervals

            Goal mappedGoal = GoalMapper.Map(goal);
            GoalUtilities.EnsureGoalHasAllIterations(mappedGoal, request.TargetDate.Value, request.CurrentValue.Value);

            return GoalMapper.Map(mappedGoal);
        }

        private int CalculateNumberOfIterations(DateTime startDate, DateTime endDate, GoalDurationType duration)
        {
            var currentStartDate = DateHelper.GetStartOfDuration(duration, startDate);
            bool stillCreating = true;
            int numberOfIterations = 0;
            while (stillCreating)
            {
                if (currentStartDate.Date < endDate.Date)
                {
                    currentStartDate = DateHelper.AddDuration(duration, currentStartDate);
                    numberOfIterations++;
                }
                else
                {
                    stillCreating = false;
                }
            }

            return numberOfIterations;
        }

        private void EnsureReachSomethingValuesAreGood(double initial, double target, int numberIterations)
        {
            if (initial > target)
            {
                throw new ArgumentOutOfRangeException("target", "Initial value is greater than target");
            }

            if (initial == 0 || target == 0 || numberIterations < 2)
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        private double CalculateReachSomethingIterationChangeValue(double initial, double target, int numberIterations)
        {
            EnsureReachSomethingValuesAreGood(initial, target, numberIterations);

            return (target - initial) / numberIterations;
        }

        private double CalculateReachSomethingIterationPercentageChange(double initial, double target, int numberIterations)
        {
            double iterationCount = numberIterations -1;
            EnsureReachSomethingValuesAreGood(initial, target, numberIterations);

            double d = target/initial;
            double pow = Math.Pow(d, 1 / iterationCount);
            double dminOne = (pow - 1);
            return dminOne * 100;
        }
    }
}
