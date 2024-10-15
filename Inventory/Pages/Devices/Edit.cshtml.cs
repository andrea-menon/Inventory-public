using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventory.Models;

namespace Inventory.Pages.Devices
{
    [CookieAuth("AllowedAccessCookieValue")]
    public class EditModel : PageModel
    {
        private readonly Inventory.Data.InventoryContext _context;

        public EditModel(Inventory.Data.InventoryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Device Device { get; set; } = default!;
        [BindProperty]
        public Relation Relation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Devices == null || _context.Relations == null)
            {
                return NotFound();
            }

            Relation = await _context.Relations
                .Include(r => r.Device)
                .Include(r => r.Device.Model)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (Relation == null)
            {
                return NotFound();
            }

            Device = Relation.Device;

            ViewData["ModelID"] = new SelectList(_context.Models, "Id", "Name");
            ViewData["AccountID"] = new SelectList(_context.Accounts, "Id", "Oa");
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Name");

            return Page();
        }

        /*
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Devices == null || _context.Relations == null)
            {
                return NotFound();
            }

            var relation = await _context.Relations.FirstOrDefaultAsync(relation => relation.Id == id);
            var device = await _context.Devices.FirstOrDefaultAsync(m => m.Id == relation.DeviceId);
            
            if (device == null)
            {
                return NotFound();
            }
            Device = device;
            Relation = relation;
            ViewData["ModelID"] = new SelectList(_context.Models, "Id", "Name");
            ViewData["AccountID"] = new SelectList(_context.Accounts, "Id", "Oa");
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Name");
            return Page();
        }
        */

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Relation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RelationExists(Relation.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            
            // Reattach Device and update state
            var existingDevice = await _context.Devices.FindAsync(Device.Id);
            if (existingDevice != null)
            {
                existingDevice.Sn = Device.Sn;
                existingDevice.ModelId = Device.ModelId;
                existingDevice.Zhe = Device.Zhe;
                existingDevice.Stato = Device.Stato;
                existingDevice.Data = Device.Data;
                existingDevice.Note = Device.Note;
                existingDevice.AccountId = Device.AccountId;

                _context.Attach(existingDevice).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeviceExists(Device.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                return NotFound();
            }
            
            /*            
            _context.Attach(Device).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceExists(Device.Id))
                {
                    return NotFound();
                }
                else
                {
                    //throw;
                }
            }       
            */

            return RedirectToPage("./Index");
        }

        private bool RelationExists(int id)
        {
            return (_context.Relations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private bool DeviceExists(int id)
        {
            return (_context.Devices?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
