using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TodoApp.Application.Services;
using TodoApp.Core.Entities;

namespace TodoApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TodoController : ControllerBase
    {
        private readonly TodoService _todoService;

        public TodoController(TodoService todoService)
        {
            _todoService = todoService;
        }

        /// <summary>
        /// Get all Todo items.
        /// </summary>
        /// <returns>A list of all Todo items.</returns>
        /// <response code="200">Returns the list of Todo items</response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var todos = await _todoService.GetAllTodos();
            return Ok(todos);
        }

        /// <summary>
        /// Returns a specific Todo item.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return the Todo item with matching ID </returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/Todo/1
        ///
        /// </remarks>
        /// <response code="200">Returns the specific item</response>
        /// <response code="404">If the item doesn't exist</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var todo = await _todoService.GetTodoById(id);
            if (todo == null)
            {
                return NotFound();
            }
            return Ok(todo);
        }

        /// <summary>
        /// Creates a Todo Item.
        /// </summary>
        /// <param name="todoItem"></param>
        /// <returns>A newly created Todo Item</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/Todo
        ///     {
        ///        "title": "Sweep the floor",
        ///        "isCompleted": true
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        public async Task<IActionResult> Create(TodoItem todoItem)
        {
            await _todoService.AddTodo(todoItem);
            return CreatedAtAction(nameof(GetById), new { id = todoItem.Id }, todoItem);
        }

        /// <summary>
        /// Updates a Todo Item.
        /// </summary>
        /// <param name="id">The ID of the Todo item to update.</param>
        /// <param name="todoItem">The Updated Todo item</param>
        /// <returns>No content</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/Todo/1
        ///     {
        ///        "id": "1",
        ///        "title": "Mop the floor",
        ///        "isCompleted": true
        ///     }
        ///
        /// </remarks>
        /// <response code="204">Item updated</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }
            await _todoService.UpdateTodo(todoItem);
            return NoContent();
        }
        
        /// <summary>
        /// Deletes a Todo item.
        /// </summary>
        /// <param name="id">The ID of the Todo item to delete.</param>
        /// <returns>No content</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE api/Todo/1
        ///
        /// </remarks>
        /// <response code="204">If the deletion is successful</response>
        /// <response code="404">If the item is not found</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _todoService.DeleteTodo(id);
            return NoContent();
        }

        /// <summary>
        /// Delete all Todo items and reset the ID sequence.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">If the deletion is successful</response>
        [HttpDelete("reset")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Reset()
        {
            await _todoService.DeleteTodosAndReset();
            return NoContent();
        }
    }
}

