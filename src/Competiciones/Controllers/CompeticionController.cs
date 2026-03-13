using Competiciones.Data;
using Competiciones.Models.Entities;
using Competiciones.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Competiciones.Controllers
{
    public class CompeticionController : Controller
    {
        private readonly AppDbContext _contexto;

        public CompeticionController(AppDbContext context)
        {
            _contexto = context;
        }

        [HttpGet]
        public ViewResult Create()
        {
            CrearCompeticionViewModel vm = new CrearCompeticionViewModel();
            vm.Paises = _contexto.Paises.ToList().Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();
            vm.Equipos = _contexto.Equipos.ToList();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(CrearCompeticionViewModel p, int[] equiposSeleccionados)
        {
            if (ModelState.IsValid)
            {
                foreach (int id in equiposSeleccionados)
                {
                    Equipo equipo = _contexto.Equipos.Find(id);
                    p.Equipos.Add(equipo);
                }

                Competicion nuevaCompeticion = new()
                {
                    Name = p.Name,
                    PaisId = p.PaisId,
                    Equipos = p.Equipos
                };
                _contexto.Competiciones.Add(nuevaCompeticion);
                _contexto.SaveChanges();

                return RedirectToAction("Index");
            }
            p.Equipos = _contexto.Equipos.ToList();
            p.Paises = _contexto.Paises.ToList().Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();
            return View(p);
        }

        public IActionResult Details(int id)
        {
            Competicion competicion = _contexto.Competiciones.Include(c => c.Equipos).FirstOrDefault(c => c.Id == id);
            if (competicion == null) return NotFound();

            DetallesCompeticionView vm = new DetallesCompeticionView
            {
                Titulo = competicion.Name,
                Competicion = competicion,
            };
            return View(vm);
        }

        public IActionResult Index()
        {
            List<Competicion> listaCompeticiones = _contexto.Competiciones.Include(c => c.Equipos).Include(c => c.Pais).ToList();


            return View(listaCompeticiones);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Competicion competicion = _contexto.Competiciones.Include(c => c.Equipos).FirstOrDefault(c => c.Id == id);
            EditarCompeticionViewModel competicionEditar = new EditarCompeticionViewModel
            {
                Id = competicion.Id,
                Name = competicion.Name,
                PaisId = competicion.PaisId,
                Paises = _contexto.Paises.ToList().Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList(),
                Equipos = competicion.Equipos,
                listaEquipos = _contexto.Equipos.ToList()
            };

            return View(competicionEditar);
        }
        [HttpPost]
        public IActionResult Edit(EditarCompeticionViewModel model, int[] equiposSeleccionados)
        {
            if (ModelState.IsValid)
            {
                foreach (int id in equiposSeleccionados)
                {
                    Equipo equipo = _contexto.Equipos.Find(id);
                    model.Equipos.Add(equipo);
                }

                Competicion competicion = _contexto.Competiciones.Include(c => c.Equipos).First(c => c.Id == model.Id);

                competicion.Name = model.Name;
                competicion.PaisId = model.PaisId;
                competicion.Equipos.Clear();
                competicion.Equipos = model.Equipos;

                EntityEntry employe = _contexto.Competiciones.Attach(competicion);
                employe.State = EntityState.Modified;
                _contexto.SaveChanges();


                return RedirectToAction("Index");
            }
            model.Equipos = _contexto.Equipos.ToList();
            model.Paises = _contexto.Paises.ToList().Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();
            return View(model);
        }
        public IActionResult Delete(int id)
        {
            Competicion competicion = _contexto.Competiciones.Find(id);

            _contexto.Competiciones.Remove(competicion);
            _contexto.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}