using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Models;

namespace GoalManagementLibrary.Models
{
    public class CreateGoalFromTarget
    {
        public CreateGoalFromTarget()
        {
            
        }

        public CreateGoalFromTarget(IList<Category> categories)
        {
            Categories = categories;
        }

        public string Name { get; set; }
        public int CategoryId { get; set; }
        
        [DisplayName("Short Name")]
        public string ShortName { get; set; }

        [DisplayName("Display Colour")]
        public string HexColour { get; set; }

        [DisplayName("Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [DisplayName("Target Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? TargetDate { get; set; }

        [DisplayName("Target Value")]        
        public double? TargetValue { get; set; }

        [DisplayName("Current Value")]
        public double? CurrentValue { get; set; }

        [DisplayName("Unit Desctiption")]
        public string UnitDescription { get; set; }



        
        public int GoalDurationTypeId { get; set; }

        public int GoalTypeId { get; set; }
        public int GoalBehaviourTypeId { get; set; }


        public IList<Category> Categories { get; set; }
    }
}
