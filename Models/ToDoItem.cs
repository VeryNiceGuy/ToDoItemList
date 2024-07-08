namespace ToDoList.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateOnly? DueDate { get; set; }
        public virtual Priority? Priority { get; set; }
        public virtual User? User { get; set; }
    }
}
