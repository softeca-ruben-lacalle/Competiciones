using System.ComponentModel.DataAnnotations;

namespace Competiciones.Models.ViewModels
{
    public class CrearPaisViewModel
    {
        [Required(ErrorMessage = "Obligatorio"), MaxLength(100, ErrorMessage = "Demasiado largo")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Obligatorio")]
        public IFormFile Bandera { get; set; }
    }
}