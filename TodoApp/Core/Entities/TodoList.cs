using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TodoApp.Core.Entities
{
    public class TodoItem
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        public required string Title { get; set; }

        [DefaultValue(false)]
        public bool IsCompleted { get; set; }
    }
}
