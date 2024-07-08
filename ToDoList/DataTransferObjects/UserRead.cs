using ToDoList.Models;

namespace ToDoList.Dto
{
    public class UserRead
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public static UserRead FromUser(User user) =>
            new() { Id = user.Id, Name = user.Name };
    }
}
