using System.Text.Json.Serialization;
using ToDoList.Converters;
using ToDoList.Models;

namespace ToDoList.Dto
{
    public class ToDoItemRead
    {
        public int Id { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Title { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly? DueDate { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public PriorityRead? Priority { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public UserRead? User { get; set; }
        public static ToDoItemRead FromToDoItem(ToDoItem toDoItem) => new ()
            {
                Id = toDoItem.Id,
                Title = toDoItem.Title,
                Description = toDoItem.Description,
                IsCompleted = toDoItem.IsCompleted,
                DueDate = toDoItem.DueDate,
                Priority = toDoItem.Priority != null ? PriorityRead.FromPriority(toDoItem.Priority) : null,
                User = toDoItem.User != null ? UserRead.FromUser(toDoItem.User) : null
            };
    }
}
