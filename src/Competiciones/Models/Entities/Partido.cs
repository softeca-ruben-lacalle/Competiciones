namespace Competiciones.Models.Entities
{
    public class Partido
    {
        public int Id { get; set; }
        public DateTime Horario { get; set; }
        public string? Resultado { get; set; }

        // FK hacia Competicion
        public int CompeticionId { get; set; }
        public Competicion Competicion { get; set; }
        // FK hacia Equipo Local
        public int EquipoLocalId { get; set; }
        public Equipo EquipoLocal { get; set; }
        // FK hacie Equipo Visitante
        public int EquipoVisitanteId { get; set; }
        public Equipo EquipoVisitante { get; set; }
    }
}
