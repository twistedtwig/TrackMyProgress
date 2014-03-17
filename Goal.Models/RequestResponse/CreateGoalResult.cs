using System.Collections.Generic;

namespace Goals.Models.RequestResponse
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
        public Models.Goal Goal { get; set; }
        public CreateGoalRequest Request { get; set; }
    }
}