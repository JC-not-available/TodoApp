using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.Core.Entities;
using TodoApp.Core.Interfaces;

namespace TodoApp.Application.Services
{
    public class TodoService
    {
        private readonly ITodoRepository _todoRepository;

        public TodoService(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<IEnumerable<TodoItem>> GetAllTodos()
        {
            return await _todoRepository.GetAllItem();
        }

        public async Task<TodoItem?> GetTodoById(int id)
        {
            return await _todoRepository.GetItemById(id);
        }

        public async Task AddTodo(TodoItem todoItem)
        {
            await _todoRepository.AddItem(todoItem);
        }

        public async Task UpdateTodo(TodoItem todoItem)
        {
            await _todoRepository.UpdateItem(todoItem);
        }

        public async Task DeleteTodo(int id)
        {
            await _todoRepository.DeleteItem(id);
        }

        public async Task DeleteTodosAndReset()
        {
            await _todoRepository.DeleteAndReset();
        }
    }
}
