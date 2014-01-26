using System.Collections.Generic;
using Models;

namespace GoalManagementLibrary.Models
{
    public class CreateGoalResult
    {
        public CreateGoalResult()
        {
            Success = true;
            Messages = new List<string>();
        }

        public Goal Goal { get; set; }

        public List<string> Messages { get; set; }
        public bool Success { get; set; }

        public CreateGoalRequest Request { get; set; }
    }
}