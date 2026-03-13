using Competiciones.Data;
using Competiciones.Models.Entities;
using Competiciones.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Competiciones.Controllers
{
    public class PaisController : Controller
    {
        private readonly AppDbContext _contexto;
        private readonly IWebHostEnvironment hosting;

        public PaisController(AppDbContext contexto, IWebHostEnvironment h)
        {
            _contexto = contexto;
            hosting = h;
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CrearPaisViewModel p)
        {
            if (ModelState.IsValid)
            {
                string guidImagen = null;
                if (p.Bandera != null)
                {
                    string ficherosImagenes = Path.Combine(hosting.WebRootPath, "images");
                    guidImagen = Guid.NewGuid().ToString() + p.Bandera.FileName;
                    string rutaDefinitaba = Path.Combine(ficherosImagenes, guidImagen);
                    using (FileStream stream = new FileStream(rutaDefinitaba, FileMode.Create))
                    {
                        p.Bandera.CopyTo(stream);
                    }
                }

                Pais nuevoPais = new()
                {
                    Name = p.Name,
                    rutaBandera = guidImagen ?? string.Empty
                };

                _contexto.Paises.Add(nuevoPais);
                _contexto.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Details(int id)
        {
            Pais pais = _contexto.Paises.Find(id);
            if (pais == null)
            {
                return NotFound();
            }

            DetallesPaisViewModel vm = new DetallesPaisViewModel
            {
                Titulo = pais.Name,
                Pais = pais,
                Competiciones = _contexto.Competiciones.Where(c => c.PaisId == id).ToList()
            };
            return View(vm);
        }

        public IActionResult Index()
        {
            List<Pais> listaPaises = _contexto.Paises.ToList();
            return View(listaPaises);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Pais pais = _contexto.Paises.Find(id);
            EditarPaisViewModel paisEditar = new EditarPaisViewModel
            {
                Id = pais.Id,
                Name = pais.Name,
                rutaBanderaExistente = pais.rutaBandera
            };
            return View(paisEditar);
        }

        public IActionResult Delete(int id)
        {
            Pais pais = _contexto.Paises.Find(id);

            _contexto.Paises.Remove(pais);
            _contexto.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}