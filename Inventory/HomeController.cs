using Inventory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Inventory.Pages.Relations;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Inventory
{
    public class HomeController : Controller
    {
        private readonly Inventory.Data.InventoryContext _context;

        public HomeController(Inventory.Data.InventoryContext context)
        {
            _context = context;
        }
        public IList<Relation> Relation { get; set; } = default!;
        // POST: Relations/ScaricaCsv
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ScaricaCsv()
        {
            try
            {
                Console.WriteLine("1");
                var lista = _context.Relations
                    .Include(o => o.Device.Model.Brand)
                    .Include(o => o.Device.Model)
                    .Include(o => o.Device)
                    .Include(o => o.Device.Account)
                    .Include(o => o.User)
                    .Include(o => o.User.CostCenter);
                Console.WriteLine("2");
                var data = lista.ToList(); // Replace with your DbSet
                Console.WriteLine("3");
                // Generate the CSV content
                var csvContent = GenerateCsvContent(data);

                // Log for debugging
                Console.WriteLine("CSV content generated successfully!");

                // Return the CSV file as a response
                return File(csvContent, "text/csv", "data.csv");
            }
            catch (Exception exception)
            {
                // Handle exceptions (log or display an error message)
                Console.WriteLine($"Error, exporting data to CSV: {exception.Message}");
                return Content("Error exporting data to CSV.");
            }
        }

        private byte[] GenerateCsvContent(IEnumerable<Relation> data)
        {
            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
            {
                // Write the CSV header (column names)
                streamWriter.WriteLine("Id, Numero di serie, Modello, Descrizione, Marca, Codice cespite ZHE, Stato, Data, Note, Utente, Di account, Centro di costo, Codice centro di costo, Ordine d'acquisto, Richiesta d'acquisto, CAR"); // Replace with actual column names

                // Write each data record
                foreach (var record in data)
                {
                    var device = record.Device;
                    var model = device?.Model;
                    var brand = model?.Brand;
                    var account = device?.Account;
                    var user = record.User;
                    var costCenter = user?.CostCenter;

                    streamWriter.WriteLine($"{record.Id}, " +
                        $"{device?.Sn ?? ""}, " +
                        $"{model?.Name ?? ""}, " +
                        $"{model?.Description ?? ""}, " +
                        $"{brand?.Name ?? ""}, " +
                        $"{device?.Zhe ?? ""}, " +
                        $"{device?.Stato ?? ""}, " +
                        $"{device?.Data?.ToString("yyyy-MM-dd") ?? ""}, " +
                        $"{device?.Note ?? ""}, " +
                        $"{user?.Name ?? ""}, " +
                        $"{user?.Di ?? ""}, " +
                        $"{costCenter?.Description ?? ""}, " +
                        $"{costCenter?.Name ?? ""}, " +
                        $"{account?.Oa ?? ""}, " +
                        $"{account?.Rda ?? ""}, " +
                        $"{account?.Car ?? ""}");
                    //streamWriter.WriteLine($"{record.Id}, {record.Device.Sn}, {record.Device.Model.Name}, {record.Device.Model.Description}, {record.Device.Model.Brand.Name}, {record.Device.Zhe}, {record.Device.Stato}, {record.Device.Data}, {record.Device.Note}, {record.User.Name}, {record.User.Di}, {record.User.CostCenter.Description}, {record.User.CostCenter.Name}, {record.Device.Account.Oa}, {record.Device.Account.Rda}, {record.Device.Account.Car}");
                }

                // Flush and return the content as bytes
                streamWriter.Flush();

                var csvBytes = memoryStream.ToArray();

                // Log the length of the CSV bytes for debugging
                Console.WriteLine($"CSV content length: {csvBytes.Length} bytes!");

                return csvBytes;
            }
        }
    }
}
