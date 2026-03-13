using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Competiciones.Models.ViewModels
{
    public class CrearJugadorViewModel
    {
        [Required(ErrorMessage = "Obligatorio")]
        public int PaisId { get; set; }

        [Required(ErrorMessage = "Obligatorio")]
        public int EquipoId { get; set; }

        [Required(ErrorMessage = "Obligatorio"), MaxLength(25, ErrorMessage = "Máximo 25 caracteres")]
        public string? Name { get; set; }

        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string? LastNames { get; set; }

        public int? Altura { get; set; }
        public float? Peso { get; set; }
        public int? Edad { get; set; }

        public List<SelectListItem>? Paises { get; set; }
        public List<SelectListItem>? Equipos { get; set; }
    }
}