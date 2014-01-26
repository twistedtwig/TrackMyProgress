using Models;
using System.Collections.Generic;

namespace Web.Models
{
    public class GoalSelectorModel
    {
        public int GoalId { get; set; }
        public IList<Goal> Goals { get; set; }
    }
}