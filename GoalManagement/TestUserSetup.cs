using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoalManagement;

namespace GoalManagement
{
    public class TestUserSetup
    {
        private GoalManager _goalManager;

        public TestUserSetup(GoalManager goalManager)
        {
            _goalManager = goalManager;
        }

        public void RemoveAllGoals(Guid userId)
        {
            //TODO make this work for this user only.


            var goals = _goalManager.Goals(userId);
            foreach (var goal in goals)
            {
                //check userId

                _goalManager.Delete(userId, goal);
            }
        }

        public void SetupNewGoals(Guid userId)
        {
            //TODO make this work for this user only.


            //TODO make it do stuff
            SetupYearsWorthOfRunning(userId);
            Setup6MonthsOfWeightLose(userId);
        }

        private void SetupYearsWorthOfRunning(Guid userId)
        {
            throw new NotImplementedException();
        }

        private void Setup6MonthsOfWeightLose(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
