using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Inventory.Models;
using System.Text;
//using OfficeOpenXml;

namespace Inventory.Pages.Relations
{
    public class IndexModel : PageModel
    {
        private readonly Inventory.Data.InventoryContext _context;

        public IndexModel(Inventory.Data.InventoryContext context)
        {
            _context = context;
        }

        public IList<Inventory.Models.Relation> Relation { get; set; } = default!;

        [BindProperty]
        public string? SearchSn { get; set; }

        [BindProperty]
        public string? SearchModel { get; set; }

        [BindProperty]
        public string? SearchDescription { get; set; }

        [BindProperty]
        public string? SearchBrand { get; set; }

        [BindProperty]
        public string? SearchZhe { get; set; }

        [BindProperty]
        public string? SearchState { get; set; }

        [BindProperty]
        public DateOnly? SearchDate { get; set; }

        [BindProperty]
        public string? SearchNote { get; set; }

        [BindProperty]
        public string? SearchUser { get; set; }

        [BindProperty]
        public string? SearchDi { get; set; }

        [BindProperty]
        public string? SearchCostCenter { get; set; }

        [BindProperty]
        public string? SearchCostCenterCode { get; set; }

        [BindProperty]
        public string? SearchPurchaseOrder { get; set; }

        [BindProperty]
        public string? SearchPurchaseRequest { get; set; }

        [BindProperty]
        public string? SearchCAR { get; set; }

        [BindProperty]
        public string? SubmitSource { get; set; }

        [BindProperty]
        public string? ButtonSource { get; set; }

        [BindProperty]
        public int Pagina { get; set; }

        [BindProperty]
        public string? DirPagina { get; set; }

        public async Task OnGetAsync()
        {
            if (_context.Relations != null)
            {
                Pagina = 1;
                Relation = await _context.Relations
                .Include(m => m.Device)
                .ThenInclude(m => m.Model)
                .ThenInclude(m => m.Brand)
                .Include(m => m.Device)
                .ThenInclude(m => m.Account)
                .Include(m => m.User)
                .ThenInclude(m => m.CostCenter)
                .OrderBy(m => m.Device.Sn)
                .Take(Pagina*50)
                .ToListAsync();
                ButtonSource = "buttonSnA";
            }
        }

        public JsonResult OnGetSearch(string term, string column)
        {
            if (string.IsNullOrEmpty(term) || string.IsNullOrEmpty(column))
            {
                return new JsonResult(new List<object>());
            }

            IEnumerable<object> results;

            switch (column)
            {
                case "Sn":
                    results = _context.Relations
                        .Include(m => m.Device)
                        .AsEnumerable()
                        .Where(i => i.Device != null && i.Device.Sn != null && i.Device.Sn.Contains(term, StringComparison.OrdinalIgnoreCase))
                        .Select(i => new { i.Id, Sn = i.Device.Sn })
                        .OrderBy(i => i.Sn);
                    break;
                case "Model":
                    results = _context.Relations
                        .Include(m => m.Device)
                        .ThenInclude(m => m.Model)
                        .AsEnumerable()
                        .Where(i => i.Device != null && i.Device.Model != null && i.Device.Model.Name != null && i.Device.Model.Name.Contains(term, StringComparison.OrdinalIgnoreCase))
                        .Select(i => new { i.Id, Model = i.Device.Model.Name })
                        .OrderBy(i => i.Model);
                    break;
                case "Description":
                    results = _context.Relations
                        .Include(m => m.Device)
                        .ThenInclude(m => m.Model)
                        .AsEnumerable()
                        .Where(i => i.Device != null && i.Device.Model != null && i.Device.Model.Description != null && i.Device.Model.Description.Contains(term, StringComparison.OrdinalIgnoreCase))
                        .Select(i => new { i.Id, Description = i.Device.Model.Description })
                        .OrderBy(i => i.Description);
                    break;
                case "Brand":
                    results = _context.Relations
                        .Include(m => m.Device)
                        .ThenInclude(m => m.Model)
                        .ThenInclude(m => m.Brand)
                        .AsEnumerable()
                        .Where(i => i.Device != null && i.Device.Model != null && i.Device.Model.Brand != null && i.Device.Model.Brand.Name != null && i.Device.Model.Brand.Name.Contains(term, StringComparison.OrdinalIgnoreCase))
                        .Select(i => new { i.Id, Brand = i.Device.Model.Brand.Name })
                        .OrderBy(i => i.Brand);
                    break;
                case "Zhe":
                    results = _context.Relations
                        .Include(m => m.Device)
                        .AsEnumerable()
                        .Where(i => i.Device != null && i.Device.Zhe != null && i.Device.Zhe.Contains(term, StringComparison.OrdinalIgnoreCase))
                        .Select(i => new { i.Id, Zhe = i.Device.Zhe })
                        .OrderBy(i => i.Zhe);
                    break;
                case "State":
                    results = _context.Relations
                        .Include(m => m.Device)
                        .AsEnumerable()
                        .Where(i => i.Device != null && i.Device.Stato != null && i.Device.Stato.Contains(term, StringComparison.OrdinalIgnoreCase))
                        .Select(i => new { i.Id, State = i.Device.Stato })
                        .OrderBy(i => i.State);
                    break;
                case "Note":
                    results = _context.Relations
                        .Include(m => m.Device)
                        .AsEnumerable()
                        .Where(i => i.Device != null && i.Device.Note != null && i.Device.Note.Contains(term, StringComparison.OrdinalIgnoreCase))
                        .Select(i => new { i.Id, Note = i.Device.Note })
                        .OrderBy(i => i.Note);
                    break;
                case "User":
                    results = _context.Relations
                        .Include(m => m.User)
                        .AsEnumerable()
                        .Where(i => i.User != null && i.User.Name != null && i.User.Name.Contains(term, StringComparison.OrdinalIgnoreCase))
                        .Select(i => new { i.Id, User = i.User.Name })
                        .OrderBy(i => i.User);
                    break;
                case "Di":
                    results = _context.Relations
                        .Include(m => m.User)
                        .AsEnumerable()
                        .Where(i => i.User != null && i.User.Di != null && i.User.Di.Contains(term, StringComparison.OrdinalIgnoreCase))
                        .Select(i => new { i.Id, Di = i.User.Di })
                        .OrderBy(i => i.Di);
                    break;
                case "CostCenter":
                    results = _context.Relations
                        .Include(m => m.User)
                        .ThenInclude(m => m.CostCenter)
                        .AsEnumerable()
                        .Where(i => i.User != null && i.User.CostCenter != null && i.User.CostCenter.Description != null && i.User.CostCenter.Description.Contains(term, StringComparison.OrdinalIgnoreCase))
                        .Select(i => new { i.Id, CostCenter = i.User.CostCenter.Description })
                        .OrderBy(i => i.CostCenter);
                    break;
                case "CostCenterCode":
                    results = _context.Relations
                        .Include(m => m.User)
                        .ThenInclude(m => m.CostCenter)
                        .AsEnumerable()
                        .Where(i => i.User != null && i.User.CostCenter != null && i.User.CostCenter.Name != null && i.User.CostCenter.Name.Contains(term, StringComparison.OrdinalIgnoreCase))
                        .Select(i => new { i.Id, CostCenterCode = i.User.CostCenter.Name })
                        .OrderBy(i => i.CostCenterCode);
                    break;
                case "PurchaseOrder":
                    results = _context.Relations
                        .Include(m => m.Device)
                        .ThenInclude(m => m.Account)
                        .AsEnumerable()
                        .Where(i => i.Device != null && i.Device.Account != null && i.Device.Account.Oa != null && i.Device.Account.Oa.Contains(term, StringComparison.OrdinalIgnoreCase))
                        .Select(i => new { i.Id, PurchaseOrder = i.Device.Account.Oa })
                        .OrderBy(i => i.PurchaseOrder);
                    break;
                case "PurchaseRequest":
                    results = _context.Relations
                        .Include(m => m.Device)
                        .ThenInclude(m => m.Account)
                        .AsEnumerable()
                        .Where(i => i.Device != null && i.Device.Account != null && i.Device.Account.Rda != null && i.Device.Account.Rda.Contains(term, StringComparison.OrdinalIgnoreCase))
                        .Select(i => new { i.Id, PurchaseRequest = i.Device.Account.Rda })
                        .OrderBy(i => i.PurchaseRequest);
                    break;
                case "CAR":
                    results = _context.Relations
                        .Include(m => m.Device)
                        .ThenInclude(m => m.Account)
                        .AsEnumerable()
                        .Where(i => i.Device != null && i.Device.Account != null && i.Device.Account.Car != null && i.Device.Account.Car.Contains(term, StringComparison.OrdinalIgnoreCase))
                        .Select(i => new { i.Id, CAR = i.Device.Account.Car })
                        .OrderBy(i => i.CAR);
                    break;
                default:
                    results = new List<object>();
                    break;
            }

            return new JsonResult(results);
        }

        public async Task<IActionResult> OnPostAsync()
        {

            var query = _context.Relations
                .Include(m => m.Device)
                .ThenInclude(m => m.Model)
                .ThenInclude(m => m.Brand)
                .Include(m => m.Device)
                .ThenInclude(m => m.Account)
                .Include(m => m.User)
                .ThenInclude(m => m.CostCenter)
                .AsQueryable();

           /* if (string.IsNullOrEmpty(SubmitSource))
            {
                Relation = await query.ToListAsync();
                return Page();
            }*/

            if (!string.IsNullOrEmpty(SearchSn))
            {
                query = query.Where(m => m.Device.Sn != null && EF.Functions.Like(m.Device.Sn, $"%{SearchSn}%"));
            }

            if (!string.IsNullOrEmpty(SearchModel))
            {
                query = query.Where(m => m.Device.Model.Name != null && EF.Functions.Like(m.Device.Model.Name, $"%{SearchModel}%"));
            }

            if (!string.IsNullOrEmpty(SearchDescription))
            {
                query = query.Where(m => m.Device.Model.Description != null && EF.Functions.Like(m.Device.Model.Description, $"%{SearchDescription}%"));
            }

            if (!string.IsNullOrEmpty(SearchBrand))
            {
                query = query.Where(m => m.Device.Model.Brand.Name != null && EF.Functions.Like(m.Device.Model.Brand.Name, $"%{SearchBrand}%"));
            }

            if (!string.IsNullOrEmpty(SearchZhe))
            {
                query = query.Where(m => m.Device.Zhe != null && EF.Functions.Like(m.Device.Zhe, $"%{SearchZhe}%"));
            }

            if (!string.IsNullOrEmpty(SearchState))
            {
                query = query.Where(m => m.Device.Stato != null && EF.Functions.Like(m.Device.Stato, $"%{SearchState}%"));
            }

            if (SearchDate.HasValue)
            {
                query = query.Where(m => m.Device.Data != null && m.Device.Data == SearchDate);
            }

            if (!string.IsNullOrEmpty(SearchNote))
            {
                query = query.Where(m => m.Device.Note != null && EF.Functions.Like(m.Device.Note, $"%{SearchNote}%"));
            }

            if (!string.IsNullOrEmpty(SearchUser))
            {
                query = query.Where(m => m.User.Name != null && EF.Functions.Like(m.User.Name, $"%{SearchUser}%"));
            }

            if (!string.IsNullOrEmpty(SearchDi))
            {
                query = query.Where(m => m.User.Di != null && EF.Functions.Like(m.User.Di, $"%{SearchDi}%"));
            }

            if (!string.IsNullOrEmpty(SearchCostCenter))
            {
                query = query.Where(m => m.User.CostCenter.Description != null && EF.Functions.Like(m.User.CostCenter.Description, $"%{SearchCostCenter}%"));
            }

            if (!string.IsNullOrEmpty(SearchCostCenterCode))
            {
                query = query.Where(m => m.User.CostCenter.Name != null && EF.Functions.Like(m.User.CostCenter.Name, $"%{SearchCostCenterCode}%"));
            }

            if (!string.IsNullOrEmpty(SearchPurchaseOrder))
            {
                query = query.Where(m => m.Device.Account.Oa != null && EF.Functions.Like(m.Device.Account.Oa, $"%{SearchPurchaseOrder}%"));
            }

            if (!string.IsNullOrEmpty(SearchPurchaseRequest))
            {
                query = query.Where(m => m.Device.Account.Rda != null && EF.Functions.Like(m.Device.Account.Rda, $"%{SearchPurchaseRequest}%"));
            }

            if (!string.IsNullOrEmpty(SearchCAR))
            {
                query = query.Where(m => m.Device.Account.Car != null && EF.Functions.Like(m.Device.Account.Car, $"%{SearchCAR}%"));
            }

            switch (ButtonSource)
            {
                case "buttonSnA":
                    query = query.OrderBy(m => m.Device.Sn);
                    break;
                case "buttonSnZ":
                    query = query.OrderByDescending(m => m.Device.Sn);
                    break;
                case "buttonModelA":
                    query = query.OrderBy(m => m.Device.Model.Name);
                    break;
                case "buttonModelZ":
                    query = query.OrderByDescending(m => m.Device.Model.Name);
                    break;
                case "buttonDescriptionA":
                    query = query.OrderBy(m => m.Device.Model.Description);
                    break;
                case "buttonDescriptionZ":
                    query = query.OrderByDescending(m => m.Device.Model.Description);
                    break;
                case "buttonBrandA":
                    query = query.OrderBy(m => m.Device.Model.Brand.Name);
                    break;
                case "buttonBrandZ":
                    query = query.OrderByDescending(m => m.Device.Model.Brand.Name);
                    break;
                case "buttonZheA":
                    query = query.OrderBy(m => m.Device.Zhe);
                    break;
                case "buttonZheZ":
                    query = query.OrderByDescending(m => m.Device.Zhe);
                    break;
                case "buttonStateA":
                    query = query.OrderBy(m => m.Device.Stato);
                    break;
                case "buttonStateZ":
                    query = query.OrderByDescending(m => m.Device.Stato);
                    break;
                case "buttonDateA":
                    query = query.OrderBy(m => m.Device.Data);
                    break;
                case "buttonDateZ":
                    query = query.OrderByDescending(m => m.Device.Data);
                    break;
                case "buttonNoteA":
                    query = query.OrderBy(m => m.Device.Note);
                    break;
                case "buttonNoteZ":
                    query = query.OrderByDescending(m => m.Device.Note);
                    break;
                case "buttonUserA":
                    query = query.OrderBy(m => m.User.Name);
                    break;
                case "buttonUserZ":
                    query = query.OrderByDescending(m => m.User.Name);
                    break;
                case "buttonDiA":
                    query = query.OrderBy(m => m.User.Di);
                    break;
                case "buttonDiZ":
                    query = query.OrderByDescending(m => m.User.Di);
                    break;
                case "buttonCostCenterA":
                    query = query.OrderBy(m => m.User.CostCenter.Description);
                    break;
                case "buttonCostCenterZ":
                    query = query.OrderByDescending(m => m.User.CostCenter.Description);
                    break;
                case "buttonCostCenterCodeA":
                    query = query.OrderBy(m => m.User.CostCenter.Name);
                    break;
                case "buttonCostCenterCodeZ":
                    query = query.OrderByDescending(m => m.User.CostCenter.Name);
                    break;
                case "buttonPurchaseOrderA":
                    query = query.OrderBy(m => m.Device.Account.Oa);
                    break;
                case "buttonPurchaseOrderZ":
                    query = query.OrderByDescending(m => m.Device.Account.Oa);
                    break;
                case "buttonPurchaseRequestA":
                    query = query.OrderBy(m => m.Device.Account.Rda);
                    break;
                case "buttonPurchaseRequestZ":
                    query = query.OrderByDescending(m => m.Device.Account.Rda);
                    break;
                case "buttonCARA":
                    query = query.OrderBy(m => m.Device.Account.Car);
                    break;
                case "buttonCARZ":
                    query = query.OrderByDescending(m => m.Device.Account.Car);
                    break;
                default:
                    query = query.OrderBy(m => m.Device.Sn);
                    break;
            }

            if (DirPagina == null)
                query = query.Take(50);

            if (DirPagina == "prevPage")
            {
                if (Pagina > 1)
                {
                    Pagina--;
                    query = query
                        .Skip((Pagina - 1) * 50)
                        .Take(50);
                }
                else
                {
                    query = query.Take(50);
                }
            }

            if (DirPagina == "succPage")
            {
                if ((query.Count() / 50) >= Pagina)
                {
                    Pagina++;
                    query = query
                        .Skip((Pagina - 1) * 50)
                        .Take(50);
                }
                else
                {
                    query = query
                        .Skip((Pagina - 1) * 50)
                        .Take(50);
                }
            }

            Relation = await query.ToListAsync();

            return Page();
        }
    }
}
