namespace Competiciones.Models.Entities
{
    public class Jugador
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? LastNames { get; set; }

        public int? Altura { get; set; }
        public float? Peso { get; set; }
        public int? Edad { get; set; }

        // FK hacia Pais
        public int PaisId { get; set; }
        public Pais Pais { get; set; }

        // FK hacia Equipo
        public int EquipoId { get; set; }
        public Equipo Equipo { get; set; }
    }
}
