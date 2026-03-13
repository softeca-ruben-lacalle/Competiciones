using System.ComponentModel.DataAnnotations;

namespace Competiciones.Models.Entities
{
    public class Competicion
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Obligatorio"), MaxLength(100, ErrorMessage = "Demasiado largo")]
        public required string Name { get; set; }

        // FK hacia Pais: almacena el Id en la BD
        [Required(ErrorMessage = "Obligatorio")]
        public int PaisId { get; set; }
        public Pais Pais { get; set; }

        // FK hacia EquipoCompeticion
        public List<Equipo> Equipos { get; set; } = [];
        // FK hacia Partidos
        public List<Partido> Partidos { get; set; } = [];
    }
}
