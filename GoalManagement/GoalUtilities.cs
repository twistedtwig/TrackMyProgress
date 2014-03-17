using System;
using System.Collections.Generic;
using System.Linq;
using Behaviours.GoalStrategies;
using EntityModels;
using Goals.Models;

namespace GoalManagement
{
    public static class GoalUtilities
    {

        public static Goal EnsureGoalHasAllIterations(Goal goal, DateTime currentDate, double? initialTarget = null)
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

        public static GoalIteration GetCurrentIteration(Goal goal, DateTime currentDate)
        {
            if (goal == null) return null;
            return goal.Intervals.FirstOrDefault(i => i.StartDate <= currentDate && i.EndDate >= currentDate);
        }


        public static void UpdateIterationValues(GoalIterationEntity iteration)
        {
            iteration.Achieved = CalculateAchieved(iteration.Entries.Select(e => e.Amount));
            iteration.Percentage = CalculatePercentage(iteration.Achieved, iteration.Target);
        }

        public static void UpdateIterationValues(GoalIteration iteration)
        {
            iteration.Achieved = CalculateAchieved(iteration.Entries.Select(e => e.Amount));
            iteration.Percentage = CalculatePercentage(iteration.Achieved, iteration.Target);
        }

        private static double CalculateAchieved(IEnumerable<double> list)
        {
            return list.Sum();
        }

        private static double CalculatePercentage(double achieved, double target)
        {
            return Math.Round((achieved / target) * 100, 2);
        }

        public static void CalcuateIterations(IList<GoalIterationEntity> iterations, GoalSummary goalSummary)
        {
            goalSummary.NumberOfIterations = iterations.Count;
            goalSummary.AvgEntriesPerIteration = iterations.Sum(x => x.Entries.Count) / iterations.Count;
            goalSummary.AvgPercentageToTarget = Math.Round(iterations.Sum(x => x.Percentage) / iterations.Count);
        }

        public static List<GoalIterationEntity> GetGoalIterationEntities(int[] iterationIds, GoalEntity goal)
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
