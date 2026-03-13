using Competiciones.Data;
using Competiciones.Models.Entities;
using Competiciones.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Competiciones.Controllers
{
    public class JugadorController : Controller
    {
        private readonly AppDbContext _contexto;

        public JugadorController(AppDbContext context)
        {
            _contexto = context;
        }

        [HttpGet]
        public ViewResult Create()
        {
            CrearJugadorViewModel vm = new CrearJugadorViewModel();
            vm.Paises = _contexto.Paises.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();
            vm.Equipos = _contexto.Equipos.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Name }).ToList();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(CrearJugadorViewModel j)
        {
            if (ModelState.IsValid)
            {
                Jugador nuevoJugador = new() { Name = j.Name, LastNames = j.LastNames, PaisId = j.PaisId, EquipoId = j.EquipoId, Altura = j.Altura, Peso = j.Peso, Edad = j.Edad };
                _contexto.Jugadores.Add(nuevoJugador);
                _contexto.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Create");
        }

        public IActionResult Details(int id)
        {
            Jugador jugador = _contexto.Jugadores.Include(j => j.Equipo).Include(j => j.Pais).FirstOrDefault(j => j.Id == id);
            if (jugador == null)
            {
                return NotFound();
            }

            DetallesJugadorViewModel vm = new DetallesJugadorViewModel
            {
                Titulo = "Ficha Juagador",
                Jugador = jugador
            };

            return View(vm);
        }

        public IActionResult Index()
        {
            List<Jugador> listaJugadores = _contexto.Jugadores.Include(j => j.Equipo).Include(j => j.Pais).ToList();
            return View(listaJugadores);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Jugador jugador = _contexto.Jugadores.Find(id);
            EditarJugadorViewModel jugadorEditar = new EditarJugadorViewModel
            {
                Id = jugador.Id,
                Name = jugador.Name,
                LastNames = jugador.LastNames,
                Altura = jugador.Altura,
                Peso = jugador.Peso,
                Edad = jugador.Edad,
                PaisId = jugador.PaisId,
                EquipoId = jugador.EquipoId,
                Equipos = _contexto.Equipos.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Name }).ToList(),
                Paises = _contexto.Paises.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList()
            };

            return View(jugadorEditar);
        }
        [HttpPost]
        public IActionResult Edit(EditarJugadorViewModel model)
        {
            if (ModelState.IsValid)
            {
                Jugador jugador = _contexto.Jugadores.Find(model.Id);

                jugador.Name = model.Name;
                jugador.LastNames = model.LastNames;
                jugador.PaisId = model.PaisId;
                jugador.EquipoId = model.EquipoId;
                jugador.Altura = model.Altura;
                jugador.Peso = model.Peso;
                jugador.Edad = model.Edad;

                EntityEntry employe = _contexto.Jugadores.Attach(jugador);
                employe.State = EntityState.Modified;
                _contexto.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            Jugador jugador = _contexto.Jugadores.Find(id);

            _contexto.Jugadores.Remove(jugador);
            _contexto.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}