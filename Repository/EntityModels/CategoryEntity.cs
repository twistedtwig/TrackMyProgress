using System;

namespace EntityModels
{
    public class CategoryEntity
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public String Name { get; set; }
        public string HexColour { get; set; }
    }
}
