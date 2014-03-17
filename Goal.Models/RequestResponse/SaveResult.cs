using System;
using System.Collections.Generic;

namespace Goals.Models.RequestResponse
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
