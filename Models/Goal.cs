using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Behaviours.Enums;
using Behaviours.GoalBehaviours;
using Behaviours.GoalStrategies;

namespace Models
{
    public class Goal
    {
        public const int MaxShortNameLength = 10;

        public Goal()
        {
            Intervals = new List<GoalIteration>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        [Required, StringLength(MaxShortNameLength)]
        [DisplayName("Short Name")]
        public string ShortName { get; set; }
        public string HexColour { get; set; }
        public Category Category { get; set; }

        [DisplayName("Change Value")]
        public double ChangeValue { get; set; }
        
        [DisplayName("Unit Desctiption")]
        public string UnitDescription { get; set; }

        [DisplayName("Start Date")]
//        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        public GoalDurationType IntervalDuration { get; set; }

        public GoalDurationType UsualEntryFrequency { get; set; }

        public GoalType GoalType { get; set; }
        public IGoalTypeStrategy Strategy { get; set; }

        public GoalBehaviourType BehaviourType { get; set; }
        public IGoalBehaviourStrategy Behaviour { get; set; }

        public IList<GoalIteration> Intervals { get; set; }
    }
}
