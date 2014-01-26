using System;
using System.Collections.Generic;

namespace GoalManagementLibrary.Models
{
    public class SaveResult<T> where T : class
    {
        public SaveResult()
        {
            Messages = new List<string>();
            Success = false;
        }

        public T Model { get; set; }

        public bool Success { get; set; }
        public List<String> Messages { get; set; }
    }
}
