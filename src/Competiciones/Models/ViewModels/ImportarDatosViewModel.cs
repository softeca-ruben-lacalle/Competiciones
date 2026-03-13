using System.ComponentModel.DataAnnotations;

namespace Competiciones.Models.ViewModels
{
    public class ImportarDatosViewModel
    {
        [Required(ErrorMessage = "Debes de elegir un archivo")]
        public IFormFile Datos { get; set; }
    }
}
