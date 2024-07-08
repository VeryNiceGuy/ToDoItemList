using System.Text.Json.Serialization;
using ToDoList.Converters;

namespace ToDoList.Dto
{
    public class ToDoItemWrite
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly? DueDate { get; set; }
        public int? Level { get; set; }
        public int? UserId { get; set; }
    }
}
