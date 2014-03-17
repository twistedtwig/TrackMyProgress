using System.Collections.Generic;
using Goals.Models;

namespace GoalWeb.Models
{
    public class GoalSelectorModel
    {
        public int GoalId { get; set; }
        public IList<Goal> Goals { get; set; }
    }
}