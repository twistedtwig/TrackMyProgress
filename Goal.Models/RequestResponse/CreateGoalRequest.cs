using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Goals.Models.RequestResponse
{
    public class CreateGoalRequest
    {
        public CreateGoalRequest()
        {
            Id = 0;
        }

        public CreateGoalRequest(IList<Category> categories)
        {
            Categories = categories;
        }

        public CreateGoalRequest(Goal goal)
        {
            Id = goal.Id;
            UserId = goal.UserId;
            Name = goal.Name;
            ShortName = goal.ShortName;
            HexColour = goal.HexColour;
            CategoryId = goal.Category.Id;
            ChangeValue = goal.ChangeValue;
            UnitDescription = goal.UnitDescription;

            StartDate = goal.StartDate;
            GoalDurationTypeId = (int) goal.IntervalDuration;
            GoalTypeId = (int) goal.GoalType;
            GoalBehaviourTypeId = (int) goal.BehaviourType;
            
            if (goal.Intervals.Any())
            {
                FirstIterationTarget = goal.Intervals.First().Target;
            }
        }

        public int Id { get; set; }

        public string Name { get; set; }
        public int CategoryId { get; set; }

        public Guid UserId { get; set; }

        [DisplayName("Short Name")]
        public string ShortName { get; set; }

        [DisplayName("Display Colour")]
        public string HexColour { get; set; }
        
        [DisplayName("Change Value")]
        public double ChangeValue { get; set; }

        [DisplayName("Unit Desctiption")]
        public string UnitDescription { get; set; }

        [DisplayName("Start Date")]
//        [DataType(DataType.Date)]   
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }
        public int GoalDurationTypeId { get; set; }

        public int GoalTypeId { get; set; }
        public int GoalBehaviourTypeId { get; set; }

        [DisplayName("First Target")]
        public double FirstIterationTarget { get; set; }

        public IList<Category> Categories { get; set; }
    }
}
