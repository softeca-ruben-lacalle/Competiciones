using Competiciones.Models.Entities;

namespace Competiciones.Models.ViewModels
{
    public class DetallesPaisViewModel
    {
        public string Titulo { get; set; }
        public Pais Pais { get; set; }

        public List<Competicion> Competiciones { get; set; }
    }
}