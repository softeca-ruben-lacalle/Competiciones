using System.ComponentModel.DataAnnotations;

namespace Competiciones.Models.ViewModels
{
    public class EditarPartidoViewModel : CrearPartidoViewModel
    {
        public int Id { get; set; }

        [RegularExpression("^\\d+-\\d+$", ErrorMessage = "Debe cumplir con el formato X-X")]
        public string Resultado { get; set; }

    }
}
