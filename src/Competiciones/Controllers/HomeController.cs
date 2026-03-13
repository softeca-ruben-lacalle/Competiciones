using System.Data;
using System.Diagnostics;
using ClosedXML.Excel;
using Competiciones.Data;
using Competiciones.Models.Entities;
using Competiciones.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Competiciones.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment hosting;
        public readonly AppDbContext _contexto;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment h, AppDbContext context)
        {
            _logger = logger;
            hosting = h;
            _contexto = context;
        }

        public IActionResult Index()
        {
            ViewBag.ListaCompeticiones = _contexto.Competiciones.Include(c => c.Pais).Include(c => c.Equipos).ToList();
            ViewBag.ListaJugadores = _contexto.Jugadores.Include(j => j.Equipo).Include(j => j.Pais).ToList();
            ViewBag.ListaEquipos = _contexto.Equipos.Include(e => e.Pais).ToList();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public FileResult Export()
        {
            ExportarDatosViewModel datos = new ExportarDatosViewModel()
            {
                listaCompeticiones = _contexto.Competiciones.Include(c => c.Equipos).ToList(),
                listaEquipos = _contexto.Equipos.ToList(),
                listaJugadores = _contexto.Jugadores.ToList(),
                listaPaises = _contexto.Paises.ToList(),
                listaPartidos = _contexto.Partidos.ToList()
            };
            string nombreArchivo = $"Competiones.xlsx";
            return GenerarExcel(nombreArchivo, datos);
        }

        private FileResult GenerarExcel(string nombreArchivo, ExportarDatosViewModel listasDatos)
        {
            // Hacemos la Tabla de Paises
            List<Pais> listaPaises = listasDatos.listaPaises;
            DataTable paisesTable = new("Paises");
            paisesTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Id"),
                new DataColumn("Name"),
                new DataColumn("rutaBandera")
            });

            foreach (Pais pais in listaPaises)
            {
                paisesTable.Rows.Add(pais.Id, pais.Name, pais.rutaBandera);
            }

            // Hacemos la Tabla de Competiciones
            List<Competicion> listaCompeticiones = listasDatos.listaCompeticiones;
            DataTable competicionesTable = new("Competiciones");
            competicionesTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Id"),
                new DataColumn("Name"),
                new DataColumn("PaisId")
            });

            foreach (Competicion competcion in listaCompeticiones)
            {
                competicionesTable.Rows.Add(competcion.Id, competcion.Name, competcion.PaisId);
            }

            // Hacemos la Tabla de EquipoCompeticion
            DataTable equipocompeticionesTable = new("EquipoCompeticiones");
            equipocompeticionesTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("IdEquipo"),
                new DataColumn("IdCompeticion")
            });

            foreach (Competicion competicion in listaCompeticiones)
            {
                foreach (Equipo equipo in competicion.Equipos)
                {
                    equipocompeticionesTable.Rows.Add(equipo.Id, competicion.Id);
                }
            }

            // Hacemos la Tabla de Equipos
            List<Equipo> listaEquipos = listasDatos.listaEquipos;
            DataTable equiposTable = new("Equipos");
            equiposTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Id"),
                new DataColumn("Name"),
                new DataColumn("PaisId"),
                new DataColumn("RutaEscudo")
            });

            foreach (Equipo equipo in listaEquipos)
            {
                equiposTable.Rows.Add(equipo.Id, equipo.Name, equipo.PaisId, equipo.RutaEscudo);
            }

            // Hacemos la Tabla de Jugadores
            List<Jugador> listaJugadores = listasDatos.listaJugadores;
            DataTable jugadoresTable = new("Jugadores");
            jugadoresTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Id"),
                new DataColumn("Name"),
                new DataColumn("LastNames"),
                new DataColumn("Altura"),
                new DataColumn("Peso"),
                new DataColumn("Edad"),
                new DataColumn("PaisId"),
                new DataColumn("EquipoId")
            });

            foreach (Jugador jugador in listaJugadores)
            {
                jugadoresTable.Rows.Add(jugador.Id, jugador.Name, jugador.LastNames, jugador.Altura, jugador.Peso, jugador.Edad, jugador.PaisId, jugador.EquipoId);
            }

            // Hacemos la Tabla de Partidos
            List<Partido> listaPartidos = listasDatos.listaPartidos;
            DataTable partidosTable = new("Partidos");
            partidosTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Id"),
                new DataColumn("Horario"),
                new DataColumn("Resultado"),
                new DataColumn("CompeticionId"),
                new DataColumn("EquipoLocalId"),
                new DataColumn("EquipoVisitanteId")
            });

            foreach (Partido partido in listaPartidos)
            {
                partidosTable.Rows.Add(partido.Id, partido.Horario, partido.Resultado, partido.CompeticionId, partido.EquipoLocalId, partido.EquipoVisitanteId);
            }


            // Agregamos todas las tablas y devulvemos el archivo
            using (XLWorkbook wb = new())
            {
                wb.Worksheets.Add(paisesTable);
                wb.Worksheets.Add(competicionesTable);
                wb.Worksheets.Add(equiposTable);
                wb.Worksheets.Add(jugadoresTable);
                wb.Worksheets.Add(partidosTable);
                wb.Worksheets.Add(equipocompeticionesTable);

                using (MemoryStream stream = new())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        nombreArchivo);
                }
            }

        }
        [HttpGet]
        public IActionResult Import()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Import(ImportarDatosViewModel vm)
        {
            if (ModelState.IsValid)
            {
                borrarDb();
                Stream stream = vm.Datos.OpenReadStream();

                XLWorkbook wb = new(stream);

                // Insertamos paises
                IXLWorksheet hojaPaises = wb.Worksheet("Paises");
                List<Pais> listaPaises = [];

                foreach (IXLRow fila in hojaPaises.Rows().Skip(1))
                {
                    listaPaises.Add(new()
                    {
                        Id = int.Parse(fila.Cell(1).GetString()),
                        Name = fila.Cell(2).GetString(),
                        rutaBandera = fila.Cell(3).GetString()
                    });
                }

                HashSet<int> idsExistentes = _contexto.Paises.Select(p => p.Id).ToHashSet();

                foreach (Pais pais in listaPaises)
                {
                    if (!idsExistentes.Contains(pais.Id))
                        _contexto.Paises.Add(pais);
                }

                _contexto.SaveChanges();


                // Insertamos equipos
                IXLWorksheet hojaEquipos = wb.Worksheet("Equipos");
                List<Equipo> listaEquipos = [];

                foreach (IXLRow fila in hojaEquipos.Rows().Skip(1))
                {
                    listaEquipos.Add(new()
                    {
                        Id = int.Parse(fila.Cell(1).GetString()),
                        Name = fila.Cell(2).GetString(),
                        PaisId = int.Parse(fila.Cell(3).GetString()),
                        RutaEscudo = fila.Cell(4).GetString()
                    });
                }

                idsExistentes = _contexto.Equipos.Select(e => e.Id).ToHashSet();

                foreach (Equipo equipo in listaEquipos)
                {
                    if (!idsExistentes.Contains(equipo.Id))
                        _contexto.Equipos.Add(equipo);
                }

                _contexto.SaveChanges();


                // Insertamos competiciones
                IXLWorksheet hojaCompeticiones = wb.Worksheet("Competiciones");
                List<Competicion> listaCompeticiones = [];

                foreach (IXLRow fila in hojaCompeticiones.Rows().Skip(1))
                {
                    listaCompeticiones.Add(new()
                    {
                        Id = int.Parse(fila.Cell(1).GetString()),
                        Name = fila.Cell(2).GetString(),
                        PaisId = int.Parse(fila.Cell(3).GetString())
                    });
                }

                idsExistentes = _contexto.Competiciones.Select(c => c.Id).ToHashSet();

                foreach (Competicion competicion in listaCompeticiones)
                {
                    if (!idsExistentes.Contains(competicion.Id))
                        _contexto.Competiciones.Add(competicion);
                }

                _contexto.SaveChanges();


                // Insertamos relaciones Equipo - Competición
                IXLWorksheet hojaEquipoCompeticion = wb.Worksheet("EquipoCompeticiones");

                Dictionary<int, Competicion> competiciones = _contexto.Competiciones
                    .Include(c => c.Equipos)
                    .ToDictionary(c => c.Id);

                Dictionary<int, Equipo> equipos = _contexto.Equipos.ToDictionary(e => e.Id);

                foreach (IXLRow fila in hojaEquipoCompeticion.Rows().Skip(1))
                {
                    int competicionId = int.Parse(fila.Cell(2).GetString());
                    int equipoId = int.Parse(fila.Cell(1).GetString());

                    if (!competiciones.TryGetValue(competicionId, out var competicion)) continue;
                    if (!equipos.TryGetValue(equipoId, out var equipo)) continue;

                    if (!competicion.Equipos.Any(e => e.Id == equipoId))
                        competicion.Equipos.Add(equipo);
                }

                _contexto.SaveChanges();


                // Insertamos jugadores
                IXLWorksheet hojaJugadores = wb.Worksheet("Jugadores");
                List<Jugador> listaJugadores = [];

                foreach (IXLRow fila in hojaJugadores.Rows().Skip(1))
                {
                    listaJugadores.Add(new()
                    {
                        Id = int.Parse(fila.Cell(1).GetString()),
                        Name = fila.Cell(2).GetString(),
                        LastNames = fila.Cell(3).GetString(),
                        Altura = int.Parse(fila.Cell(4).GetString()),
                        Peso = int.Parse(fila.Cell(5).GetString()),
                        Edad = int.Parse(fila.Cell(6).GetString()),
                        PaisId = int.Parse(fila.Cell(7).GetString()),
                        EquipoId = int.Parse(fila.Cell(8).GetString())
                    });
                }

                idsExistentes = _contexto.Jugadores.Select(j => j.Id).ToHashSet();

                foreach (Jugador jugador in listaJugadores)
                {
                    if (!idsExistentes.Contains(jugador.Id))
                        _contexto.Jugadores.Add(jugador);
                }

                _contexto.SaveChanges();


                // Insertamos partidos
                IXLWorksheet hojaPartidos = wb.Worksheet("Partidos");
                List<Partido> listaPartidos = [];

                foreach (IXLRow fila in hojaPartidos.Rows().Skip(1))
                {
                    listaPartidos.Add(new()
                    {
                        Id = int.Parse(fila.Cell(1).GetString()),
                        Horario = DateTime.Parse(fila.Cell(2).GetString()),
                        Resultado = fila.Cell(3).GetString(),
                        CompeticionId = int.Parse(fila.Cell(4).GetString()),
                        EquipoLocalId = int.Parse(fila.Cell(5).GetString()),
                        EquipoVisitanteId = int.Parse(fila.Cell(6).GetString())
                    });
                }

                idsExistentes = _contexto.Partidos.Select(p => p.Id).ToHashSet();

                foreach (Partido partido in listaPartidos)
                {
                    if (!idsExistentes.Contains(partido.Id))
                        _contexto.Partidos.Add(partido);
                }

                _contexto.SaveChanges();


                return RedirectToAction("Index");
            }

            return View(vm);
        }

        private void borrarDb()
        {
            _contexto.Paises.ExecuteDelete();
        }


    }
}
