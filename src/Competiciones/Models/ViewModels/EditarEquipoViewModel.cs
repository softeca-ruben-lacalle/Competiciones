using Competiciones.Models.Entities;

namespace Competiciones.Models.ViewModels
{
    public class EditarEquipoViewModel : CrearEquipoViewModel
    {
        public int Id { get; set; }
        public string rutaEscudoExistente { get; set; }
        public List<Competicion> Competiones { get; set; } = [];
        public List<Jugador> Jugadores { get; set; } = [];

    }
}