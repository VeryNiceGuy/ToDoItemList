using ToDoList.Models;

namespace ToDoList.Dto
{
    public class PriorityRead
    {
        public int Level { get; set; }
        public static PriorityRead FromPriority(Priority priority) =>
            new() { Level = priority.Level };
    }
}
