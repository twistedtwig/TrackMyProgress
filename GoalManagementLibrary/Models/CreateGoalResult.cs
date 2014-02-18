using System.Collections.Generic;
using Models;

namespace GoalManagementLibrary.Models
{
    public abstract class CreationResult
    {
        protected CreationResult()
        {
            Success = true;
            Messages = new List<string>();
        }

        public List<string> Messages { get; set; }
        public bool Success { get; set; }
    }

    public class CreateGoalResult : CreationResult
    {        
        public Goal Goal { get; set; }
        public CreateGoalRequest Request { get; set; }
    }
}