using Competiciones.Data;
using Competiciones.Models.Entities;
using Competiciones.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Competiciones.Controllers
{
    public class PartidoController : Controller
    {
        AppDbContext _contexto;

        public PartidoController(AppDbContext context)
        {
            _contexto = context;
        }

        public IActionResult Index()
        {
            List<Partido> listaPartidos = _contexto.Partidos.Include(p => p.EquipoLocal).Include(p => p.EquipoVisitante).Include(p => p.Competicion).ToList();
            return View(listaPartidos);
        }
        [HttpGet]
        public IActionResult Create()
        {
            CrearPartidoViewModel vm = new CrearPartidoViewModel
            {
                Competiciones = _contexto.Competiciones.Include(c => c.Equipos).Select(c => new Competicion
                {
                    Id = c.Id,
                    Name = c.Name,
                    Equipos = c.Equipos.Select(e => new Equipo
                    {
                        Id = e.Id,
                        Name = e.Name,
                        RutaEscudo = e.RutaEscudo
                    }).ToList()
                }).ToList()
            };
            return View(vm);
        }
        [HttpPost]
        public IActionResult Create(CrearPartidoViewModel p)
        {
            if (ModelState.IsValid)
            {
                Partido nuevoPartido = new() { Horario = p.Horario, CompeticionId = p.CompeticionId, EquipoLocalId = p.EquipoLocalId, EquipoVisitanteId = p.EquipoVisitanteId };
                _contexto.Partidos.Add(nuevoPartido);
                _contexto.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(p);
        }

        public JsonResult GetAll(DateTime start, DateTime end)
        {
            List<Partido> partidos = _contexto.Partidos.Include(p => p.EquipoLocal).Include(p => p.EquipoVisitante).Where(p => p.Horario >= start && p.Horario <= end).ToList();

            var eventos = partidos.Select(p => new
            {
                id = p.Id,
                title = p.EquipoLocal.Name + " VS " + p.EquipoVisitante.Name,
                start = p.Horario,
                end = p.Horario.AddHours(2)
            });
            return Json(eventos);
        }
        public IActionResult Details(int id)
        {
            Partido partido = _contexto.Partidos.Include(p => p.Competicion).Include(p => p.EquipoLocal).Include(p => p.EquipoVisitante).FirstOrDefault(p => p.Id == id);

            return View(partido);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Partido partido = _contexto.Partidos.Find(id);
            EditarPartidoViewModel vm = new()
            {
                Id = partido.Id,
                Horario = partido.Horario,
                EquipoLocalId = partido.EquipoLocalId,
                EquipoVisitanteId = partido.EquipoVisitanteId,
                CompeticionId = partido.CompeticionId,
                Competiciones = _contexto.Competiciones.Include(c => c.Equipos).Select(c => new Competicion
                {
                    Id = c.Id,
                    Name = c.Name,
                    Equipos = c.Equipos.Select(e => new Equipo
                    {
                        Id = e.Id,
                        Name = e.Name,
                        RutaEscudo = e.RutaEscudo
                    }).ToList()
                }).ToList(),
                Resultado = partido.Resultado
            };
            return View(vm);
        }
        [HttpPost]
        public IActionResult Edit(EditarPartidoViewModel p)
        {
            if (ModelState.IsValid)
            {

                Partido nuevoPartido = new()
                {
                    Id = p.Id,
                    Horario = p.Horario,
                    Resultado = p.Resultado,
                    CompeticionId = p.CompeticionId,
                    EquipoLocalId = p.EquipoLocalId,
                    EquipoVisitanteId = p.EquipoVisitanteId
                };
                EntityEntry employe = _contexto.Partidos.Attach(nuevoPartido);
                employe.State = EntityState.Modified;
                _contexto.SaveChanges();

                return RedirectToAction("Index");
            }
            p.Competiciones = _contexto.Competiciones.Include(c => c.Equipos).Select(c => new Competicion
            {
                Id = c.Id,
                Name = c.Name,
                Equipos = c.Equipos.Select(e => new Equipo
                {
                    Id = e.Id,
                    Name = e.Name,
                    RutaEscudo = e.RutaEscudo
                }).ToList()
            }).ToList();
            return View(p);
        }

        public IActionResult Delete(int id)
        {
            Partido partido = _contexto.Partidos.Find(id);

            _contexto.Partidos.Remove(partido);
            _contexto.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
