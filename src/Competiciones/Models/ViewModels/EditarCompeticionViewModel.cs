using Competiciones.Models.Entities;

namespace Competiciones.Models.ViewModels
{
    public class EditarCompeticionViewModel : CrearCompeticionViewModel
    {
        public int Id { get; set; }

        public List<Equipo> listaEquipos;
    }
}