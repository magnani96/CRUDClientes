using Azure;
using CRUDPractico.Data;
using CRUDPractico.Models;
using CRUDPractico.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;

namespace CRUDPractico.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        static readonly HttpClient httpClient = new HttpClient();

        public ClientesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        [HttpGet]
        public IActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(AgregarClienteViewModel viewModel)
        {
            using HttpResponseMessage respuesta = await httpClient.GetAsync($"https://sistemaintegracomex.com.ar/Account/GetNombreByCuit?cuit={viewModel.Cuil}");
            string razonSocial;

            if (respuesta.IsSuccessStatusCode)
            {
                razonSocial = await respuesta.Content.ReadAsStringAsync();
            }
            else
            {
                return StatusCode((int)respuesta.StatusCode);
            }

            if (razonSocial == "@nombre")
            {
                return RedirectToAction("AgregarErrorCuil", "Clientes");
            }
            else
            {   
                if (dbContext.Clientes.Any(x => x.Cuil == viewModel.Cuil)) {
                    return RedirectToAction("AgregarErrorRepetido", "Clientes");
                }
                else
                {
                    var cliente = new Cliente
                    {
                        Cuil = viewModel.Cuil,
                        RazonSocial = razonSocial,
                        Telefono = viewModel.Telefono,
                        Direccion = viewModel.Direccion,
                        Activo = viewModel.Activo
                    };

                    await dbContext.Clientes.AddAsync(cliente);
                    await dbContext.SaveChangesAsync();
                    return RedirectToAction("Listar", "Clientes");
                }
                
            }
            
        }

        [HttpGet]
        public IActionResult AgregarErrorCuil()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AgregarErrorRepetido()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var clientes = await dbContext.Clientes.ToListAsync();
            return View(clientes);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(long cuil)
        {
            var cliente = await dbContext.Clientes.FindAsync(cuil);
            return View(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Cliente viewModel)
        {
            var cliente = await dbContext.Clientes.FindAsync(viewModel.Cuil);

            if (cliente is not null)
            {
                cliente.Telefono = viewModel.Telefono;
                cliente.Direccion = viewModel.Direccion;
                cliente.Activo = viewModel.Activo;
            }

            await dbContext.SaveChangesAsync();

            return RedirectToAction("Listar", "Clientes");
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(Cliente viewModel)
        {
            var cliente = await dbContext.Clientes.AsNoTracking().FirstOrDefaultAsync(x => x.Cuil == viewModel.Cuil);

            if (cliente is not null)
            {
                dbContext.Clientes.Remove(viewModel);
                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("Listar", "Clientes");
        }

    }
}
