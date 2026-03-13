using System.ComponentModel.DataAnnotations;
using Competiciones.Models.Entities;

namespace Competiciones.Models.ViewModels
{
    public class CrearPartidoViewModel
    {
        [Required(ErrorMessage = "La competicion es Obligatoria")]
        public int CompeticionId { get; set; }
        [Required(ErrorMessage = "El equipo local es Obligatorio")]
        public int EquipoLocalId { get; set; }
        [Required(ErrorMessage = "El equipo visitante es Obligatorio")]
        public int EquipoVisitanteId { get; set; }
        [Required(ErrorMessage = "El horario es Obligatorio")]
        public virtual DateTime Horario { get; set; }
        public List<Competicion>? Competiciones { get; set; }
    }
}
