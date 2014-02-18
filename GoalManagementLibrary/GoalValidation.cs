﻿using Behaviours.Enums;
using GoalInterfaces;
using GoalManagementLibrary.Models;
using Models;
using Repository.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace GoalManagementLibrary
{
    internal class GoalValidation
    {
        private readonly IGoalRepository _goalRepository;

        public GoalValidation(IGoalRepository goalRepository)
        {
            _goalRepository = goalRepository;
        }

        internal CreateGoalResult ValidateGoal(CreateGoalRequest request)
        {
            var result = new CreateGoalResult();
            result.Request = request;

            ValidateNames(result, request.Name, request.ShortName, request.Id);


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

            if ((GoalBehaviourType)request.GoalBehaviourTypeId != GoalBehaviourType.None && request.ChangeValue == 0)
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

        internal void ValidateNames(CreationResult result, string name, string shortname, int id = 0)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                result.Success = false;
                result.Messages.Add("Goals requires a name.");
            }

            if (!IsGoalNameUnique<GoalEntity>(id, name, g => g.Name))
            {
                result.Success = false;
                result.Messages.Add("Goals Name must be unique.");
            }

            if (string.IsNullOrWhiteSpace(shortname) || shortname.Length > Goal.MaxShortNameLength)
            {
                result.Success = false;
                result.Messages.Add("Goals shortname is required and can't be more than " + Goal.MaxShortNameLength +
                                    " characters.");
            }

            if (!IsGoalNameUnique<GoalEntity>(id, shortname, g => g.ShortName))
            {
                result.Success = false;
                result.Messages.Add("Goals short Name must be unique.");
            }
        }

        internal bool IsGoalNameUnique<T>(int id, string name, Expression<Func<T, string>> action)
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

        internal void ValidateGoalAndBehaviourType(CreationResult result, int goalTypeId, int goalBehaviourTypeId)
        {
            if (goalTypeId == (int)GoalType.ReachSomething)
            {
                if (goalBehaviourTypeId != (int)GoalBehaviourType.IncrementPercentage &&
                    goalBehaviourTypeId != (int)GoalBehaviourType.IncrementValue)
                {
                    result.Success = false;
                    result.Messages.Add("Invalid goal behaviour type.");
                    result.Messages.Add("Goal Type Reach something is designed to increment values.");
                }
            }

            if (goalTypeId == (int)GoalType.ReduceSomething)
            {
                if (goalBehaviourTypeId != (int)GoalBehaviourType.None)
                {
                    result.Success = false;
                    result.Messages.Add("Invalid goal behaviour type.");
                    result.Messages.Add("Goal Type Reach something is designed change a value.");
                }
            }

            if (goalTypeId == (int)GoalType.ReduceSomething)
            {
                if (goalBehaviourTypeId != (int)GoalBehaviourType.ReducePercentage &&
                    goalBehaviourTypeId != (int)GoalBehaviourType.ReduceValue)
                {
                    result.Success = false;
                    result.Messages.Add("Invalid goal behaviour type.");
                    result.Messages.Add("Goal Type Reduce something is designed reduce a value.");
                }
            }
        }

        internal void ValidateCreateGoalFromTarget(CreateGoalFromTarget request, CreateGoalFromTargetResult result)
        {
            if (!request.TargetDate.HasValue)
            {
                result.Success = false;
                result.Messages.Add("Requires a target Date");
            }
            else if (request.TargetDate.Value <= DateTime.Now)
            {
                result.Success = false;
                result.Messages.Add("Target Date must be in the future");
            }

            if (!request.TargetValue.HasValue)
            {
                result.Success = false;
                result.Messages.Add("Requires a target value");
            }

            if (!request.CurrentValue.HasValue)
            {
                result.Success = false;
                result.Messages.Add("Requires a current value");
            }

            if (string.IsNullOrWhiteSpace(request.UnitDescription))
            {
                result.Success = false;
                result.Messages.Add("Requires a unit description");
            }

            if (request.GoalDurationTypeId <= 0)
            {
                result.Success = false;
                result.Messages.Add("Requires a valid Goal Duration");
            }

            if (request.GoalTypeId <= 0)
            {
                result.Success = false;
                result.Messages.Add("Requires a valid Goal Type");
            }

            if (request.GoalBehaviourTypeId <= 0)
            {
                result.Success = false;
                result.Messages.Add("Requires a valid Goal Behaviour");
            }

            if (request.CategoryId <= 0)
            {
                result.Success = false;
                result.Messages.Add("Requires a Category");
            }

            ValidateNames(result, request.Name, request.ShortName);
            ValidateGoalAndBehaviourType(result, request.GoalTypeId, request.GoalBehaviourTypeId);
        }
    }
}