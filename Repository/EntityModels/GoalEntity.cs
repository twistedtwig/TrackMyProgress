using System;
using System.Collections.Generic;

namespace EntityModels
{
    public class GoalEntity
    {
        public GoalEntity()
        {
            Intervals = new List<GoalIterationEntity>();
        }

        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string HexColour { get; set; }
        public CategoryEntity Category { get; set; }
        public double ChangeValue { get; set; }
        public string UnitDescription { get; set; }

        public DateTime StartDate { get; set; }
        public int IntervalDurationId { get; set; }

        public int EnumGoalTypeId { get; set; }
        public int EnumGoalBehaviourId { get; set; }

        public IList<GoalIterationEntity> Intervals { get; set; }
    }
}
