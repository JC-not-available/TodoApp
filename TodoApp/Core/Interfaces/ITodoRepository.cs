using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.Core.Entities;

namespace TodoApp.Core.Interfaces
{
    public interface ITodoRepository
    {
        Task<IEnumerable<TodoItem>> GetAllItem();
        Task<TodoItem?> GetItemById(int id);
        Task AddItem(TodoItem todoItem);
        Task UpdateItem(TodoItem todoItem);
        Task DeleteItem(int id);
        Task DeleteAndReset();
    }
}
