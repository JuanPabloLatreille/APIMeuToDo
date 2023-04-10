using System.ComponentModel.DataAnnotations;

namespace MeuToDo.ViewModels
{
    public class CreateToDoViewModel
    {
        [Required]
        public string Title { get; set; }
    }
}
