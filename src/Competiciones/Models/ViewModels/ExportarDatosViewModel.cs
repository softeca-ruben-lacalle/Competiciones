using Competiciones.Models.Entities;

namespace Competiciones.Models.ViewModels
{
    public class ExportarDatosViewModel
    {
        public List<Jugador> listaJugadores { get; set; } = [];
        public List<Competicion> listaCompeticiones { get; set; } = [];
        public List<Equipo> listaEquipos { get; set; } = [];
        public List<Pais> listaPaises { get; set; } = [];
        public List<Partido> listaPartidos { get; set; } = [];
    }
}
