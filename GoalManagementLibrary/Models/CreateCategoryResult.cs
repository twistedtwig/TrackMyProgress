using System.Collections.Generic;
using Models;

namespace GoalManagementLibrary.Models
{
    public class CreateCategoryResult
    {
        public CreateCategoryResult()
        {
            Success = true;
            Messages = new List<string>();
        }

        public CreateCategoryRequest Request { get; set; }

        public Category Category { get; set; }

        public List<string> Messages { get; set; }
        public bool Success { get; set; }
    }
}