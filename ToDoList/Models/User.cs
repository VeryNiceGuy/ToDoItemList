namespace ToDoList.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public virtual ICollection<ToDoItem> ToDoItems { get; set; } = new List<ToDoItem>();
    }
}
