using System;

namespace Goals.Models.RequestResponse
{
    public class CreateCategoryRequest
    {
        public string Name { get; set; }
        public string HexColour { get; set; }
        public Guid UserId { get; set; }
    }
}