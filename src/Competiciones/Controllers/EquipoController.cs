using Competiciones.Data;
using Competiciones.Models.Entities;
using Competiciones.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Competiciones.Controllers
{
    public class EquipoController : Controller
    {
        public readonly AppDbContext _contexto;
        public readonly IWebHostEnvironment hosting;

        public EquipoController(AppDbContext context, IWebHostEnvironment h)
        {
            _contexto = context;
            hosting = h;
        }

        [HttpGet]
        public ViewResult Create()
        {
            CrearEquipoViewModel vm = new CrearEquipoViewModel();
            vm.Paises = _contexto.Paises.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Name }).ToList();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(CrearEquipoViewModel e)
        {
            if (ModelState.IsValid)
            {
                string guidImage = null;
                if (e.Escudo != null)
                {
                    string ficherosImagenes = Path.Combine(hosting.WebRootPath, "images");
                    guidImage = Guid.NewGuid().ToString() + e.Escudo.FileName;
                    string rutaDefinitiva = Path.Combine(ficherosImagenes, guidImage);
                    using (FileStream stream = new FileStream(rutaDefinitiva, FileMode.Create))
                    {
                        e.Escudo.CopyTo(stream);
                    }
                }
                Equipo nuevoEquipo = new() { Name = e.Name, PaisId = e.PaisId, RutaEscudo = guidImage ?? string.Empty };
                _contexto.Equipos.Add(nuevoEquipo);
                _contexto.SaveChanges();
                return RedirectToAction("Index");
            }
            e.Paises = _contexto.Paises.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Name }).ToList();
            return View(e);
        }

        public IActionResult Details(int id)
        {
            Equipo equipo = _contexto.Equipos.Include(e => e.Jugadores).FirstOrDefault(e => e.Id == id);
            if (equipo == null) return NotFound();

            DetallesEquipoViewModel vm = new DetallesEquipoViewModel
            {
                Titulo = equipo.Name,
                Equipo = equipo
            };
            return View(vm);
        }

        public IActionResult Index()
        {
            List<Equipo> listaEquipos = _contexto.Equipos.Include(e => e.Pais
            ).ToList();
            return View(listaEquipos);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Equipo equipo = _contexto.Equipos.Find(id);
            EditarEquipoViewModel equipoEditar = new EditarEquipoViewModel
            {
                Id = equipo.Id,
                Name = equipo.Name,
                PaisId = equipo.PaisId,
                Paises = _contexto.Paises.ToList().Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList(),
                rutaEscudoExistente = equipo.RutaEscudo
            };

            return View(equipoEditar);
        }
        [HttpPost]
        public IActionResult Edit(EditarEquipoViewModel model)
        {
            if (ModelState.IsValid)
            {
                Equipo equipo = _contexto.Equipos.Find(model.Id);

                equipo.Name = model.Name;
                equipo.PaisId = model.PaisId;
                equipo.Competiciones = model.Competiones;
                equipo.Jugadores = model.Jugadores;

                if (model.Escudo != null)
                {
                    if (model.rutaEscudoExistente != null)
                    {
                        string ruta = Path.Combine(hosting.WebRootPath, "images", model.rutaEscudoExistente);
                        System.IO.File.Delete(ruta);

                    }

                    equipo.RutaEscudo = SubirImagen(model);

                }

                EntityEntry employe = _contexto.Equipos.Attach(equipo);
                employe.State = EntityState.Modified;
                _contexto.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            Equipo equipo = _contexto.Equipos.Find(id);

            _contexto.Equipos.Remove(equipo);
            _contexto.SaveChanges();

            return RedirectToAction("Index");
        }

        private string SubirImagen(EditarEquipoViewModel model)
        {
            string nombreFichero = null;

            if (model.Escudo != null)
            {
                string carpetaSubida = Path.Combine(hosting.WebRootPath, "images");
                nombreFichero = Guid.NewGuid().ToString() + "_" + model.Escudo.FileName;
                string ruta = Path.Combine(carpetaSubida, nombreFichero);
                using (FileStream fileStream = new FileStream(ruta, FileMode.Create))
                {
                    model.Escudo.CopyTo(fileStream);
                }
            }
            return nombreFichero;
        }
    }
}