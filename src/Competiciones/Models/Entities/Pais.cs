using System.ComponentModel.DataAnnotations;

namespace Competiciones.Models.Entities
{
    public class Pais
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Obligatorio"), MaxLength(100, ErrorMessage = "Demasiado largo")]
        public required string Name { get; set; }
        public required string rutaBandera { get; set; }

        public List<Competicion> Competiciones { get; set; } = new();
        public List<Equipo> Equipos { get; set; }
        public List<Jugador> Jugadores { get; set; }
    }
}
