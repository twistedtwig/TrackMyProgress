using System.Collections.Generic;
using Goals.Shared.Exceptions;

namespace Goals.Models.RequestResponse
{
    public abstract class ProcessingResult
    {
        protected ProcessingResult()
        {
            Success = true;
            Messages = new List<string>();
            ProcessingException = new ModelProcessingException();
        }

        public List<string> Messages { get; set; }
        public bool Success { get; set; }

        public ModelProcessingException ProcessingException { get; set; }

        public void AddMessage(string message, string propName = "")
        {
            Messages.Add(message);
            if (string.IsNullOrWhiteSpace(propName))
            {
                ProcessingException.GeneralErrors.Add(message);
            }
            else
            {
                ProcessingException.AddPropertyError(propName, message);
            }
        }
    }

    public class CreateGoalResult : ProcessingResult
    {        
        public Goal Goal { get; set; }
        public CreateGoalRequest Request { get; set; }
    }
}