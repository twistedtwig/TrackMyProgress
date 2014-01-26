using System;
using Behaviours.Enums;

namespace GoalManagementLibrary
{
    public static class DateHelper
    {
        public static bool IsDateInRange(int durationLength, DateTime dateToCheck, DateTime rangeDate, DayOfWeek dayOfWeek = DayOfWeek.Monday)
        {
            if (durationLength <= 0) return false;

            var startOfRange = GetStartOfDuration(durationLength, rangeDate);
            var endOfRange = AddDuration(durationLength, startOfRange);
            return dateToCheck >= startOfRange && dateToCheck < endOfRange;
        }


        public static DateTime GetStartOfDuration(int durationLength, DateTime currentDate = new DateTime(), DayOfWeek dayOfWeek = DayOfWeek.Monday)
        {
            return GetStartOfDuration(GetGoalDurationType(durationLength), currentDate, dayOfWeek);
        }

       
        public static DateTime GetStartOfDuration(GoalDurationType duration, DateTime currentDate = new DateTime(), DayOfWeek dayOfWeek = DayOfWeek.Monday)
        {
            if (duration == GoalDurationType.Day)
            {
                return currentDate;
            }

            if (duration == GoalDurationType.Week)
            {
                int delta = dayOfWeek - currentDate.DayOfWeek;
                return currentDate.AddDays(delta);
            }

            if (duration == GoalDurationType.Month)
            {
                return new DateTime(currentDate.Year, currentDate.Month, 1);
            }

            if (duration == GoalDurationType.Year)
            {
                return new DateTime(currentDate.Year, 1, 1);
            }

            throw new ArgumentOutOfRangeException("durationLength");
        }

        public static DateTime GetEndOfDuration(int durationLength, DateTime startDate)
        {
            return GetEndOfDuration(GetGoalDurationType(durationLength), startDate);
        }

        public static DateTime GetEndOfDuration(GoalDurationType duration, DateTime startDate)
        {
            return AddDuration(duration, startDate).AddSeconds(-1);
        }

        public static DateTime AddDuration(int durationLength, DateTime startOfIteration)
        {
            return AddDuration(GetGoalDurationType(durationLength), startOfIteration);
        }

        public static DateTime AddDuration(GoalDurationType duration, DateTime startOfIteration)
        {
            switch (duration)
            {
                case GoalDurationType.Day:
                    return startOfIteration.AddDays(1);

                case GoalDurationType.Week:
                    return startOfIteration.AddDays(7);

                case GoalDurationType.Month:
                    return startOfIteration.AddMonths(1);

                case GoalDurationType.Year:
                    return startOfIteration.AddYears(1);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static GoalDurationType GetGoalDurationType(int durationLength)
        {
            GoalDurationType d;

            if (durationLength == (int)GoalDurationType.Day)
            {
                d = GoalDurationType.Day;
            }
            else if (durationLength == (int)GoalDurationType.Week)
            {
                d = GoalDurationType.Week;
            }
            else if (durationLength == (int)GoalDurationType.Month)
            {
                d = GoalDurationType.Month;
            }
            else if (durationLength == (int)GoalDurationType.Year)
            {
                d = GoalDurationType.Year;
            }
            else
            {
                throw new ArgumentOutOfRangeException("durationLength");
            }
            return d;
        }

    }
}