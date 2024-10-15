using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Inventory.Pages.Devices
{
    public class IndexModel : PageModel
    {
        private readonly Inventory.Data.InventoryContext _context;

        public IndexModel(Inventory.Data.InventoryContext context)
        {
            _context = context;
        }

        public IList<Inventory.Models.Device> Device { get; set; } = default!;
        public IList<Inventory.Models.Relation> Relation { get; set; } = default!;
        public async Task OnGetAsync()
        {
            if (_context.Relations != null)
            {
                Relation = await _context.Relations
                    .Include(x => x.Device)
                    .ThenInclude(x => x.Model)
                    .ThenInclude(x => x.Brand)
                    .Include(m  => m.Device)
                    .ThenInclude(m => m.Account)
                    .Include(n => n.User)
                    .ThenInclude(n => n.CostCenter)
                    .ToListAsync();
            }
            /*if (_context.Devices != null)
            {
                Device = await _context.Devices
                .Include(m => m.Model)
                .ThenInclude(m => m.Brand)
                .Include(m => m.Account)
                .ToListAsync();
            }*/
        }
    }
}
