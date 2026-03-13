namespace Competiciones.Models.ViewModels
{
    public class EditarPaisViewModel : CrearPaisViewModel
    {
        public int Id { get; set; }

        public string rutaBanderaExistente { get; set; }
    }
}