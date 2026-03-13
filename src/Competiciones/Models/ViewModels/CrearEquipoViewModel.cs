using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Competiciones.Models.ViewModels
{
    public class CrearEquipoViewModel
    {
        [Required(ErrorMessage = "Debes selecionar un pais")]
        public int PaisId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Debes selecionar un escudo")]
        public IFormFile Escudo { get; set; }
        public List<SelectListItem>? Paises { get; set; }
    }
}