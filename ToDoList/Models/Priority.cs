namespace ToDoList.Models
{
    public class Priority
    {
        public int Level { get; set; }
        public virtual ICollection<ToDoItem> ToDoItems { get; set; } = new List<ToDoItem>();
    }
}
