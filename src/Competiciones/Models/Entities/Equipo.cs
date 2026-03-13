using System.ComponentModel.DataAnnotations;

namespace Competiciones.Models.Entities
{
    public class Equipo
    {
        public int Id { get; set; }

        public required string Name { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        public int PaisId { get; set; }
        public Pais Pais { get; set; }
        public required string RutaEscudo { get; set; }

        //FK hacia EquipoCompeticion
        public List<Competicion> Competiciones { get; set; } = [];
        //Fk hacia Jugador
        public List<Jugador> Jugadores { get; set; } = [];
        // FK hacia partidos
        public List<Partido> PartidosComoLocal { get; set; }
        public List<Partido> PartidosComoVisitante { get; set; }


    }
}
