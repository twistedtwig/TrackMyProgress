
namespace Goals.Models.RequestResponse
{
    public class SaveResult<T> : ProcessingResult where T : class 
    {
        public T Model { get; set; }
    }
}
