﻿
namespace GoalWeb.Models
{
    public class GoalIterationFailedToLoadSummary
    {
        public GoalIterationFailedToLoadSummary(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}