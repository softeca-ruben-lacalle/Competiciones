using System.ComponentModel.DataAnnotations;
using Competiciones.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

public class CrearCompeticionViewModel
{
    [Required(ErrorMessage = "Obligatorio")]
    public int PaisId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Name { get; set; }

    public List<SelectListItem>? Paises { get; set; }

    public List<Equipo>? Equipos { get; set; } = [];
}