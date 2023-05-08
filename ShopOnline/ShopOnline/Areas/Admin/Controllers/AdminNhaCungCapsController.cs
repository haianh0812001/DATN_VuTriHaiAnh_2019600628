using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using ShopOnline.Models;

namespace ShopOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminNhaCungCapsController : Controller
    {
        private readonly dbMarketsContext _context;
        public INotyfService _notyfService { get; }
        public AdminNhaCungCapsController(dbMarketsContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminNhaCungCaps
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            var lsNhacungcaps = _context.NhaCungCaps
                .AsNoTracking()
                .OrderBy(x => x.Nccid);
            PagedList<NhaCungCap> models = new PagedList<NhaCungCap>(lsNhacungcaps, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        // GET: Admin/AdminNhaCungCaps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NhaCungCaps == null)
            {
                return NotFound();
            }

            var nhaCungCap = await _context.NhaCungCaps
                .FirstOrDefaultAsync(m => m.Nccid == id);
            if (nhaCungCap == null)
            {
                return NotFound();
            }

            return View(nhaCungCap);
        }

        // GET: Admin/AdminNhaCungCaps/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminNhaCungCaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nccid,Nccname,Phone,Address,CreateDate")] NhaCungCap nhaCungCap)
        {
            if (ModelState.IsValid)
            {
                nhaCungCap.CreateDate = DateTime.Now;
                _context.Add(nhaCungCap);
                await _context.SaveChangesAsync();
                _notyfService.Success("Tạo mới thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(nhaCungCap);
        }

        // GET: Admin/AdminNhaCungCaps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NhaCungCaps == null)
            {
                return NotFound();
            }

            var nhaCungCap = await _context.NhaCungCaps.FindAsync(id);
            if (nhaCungCap == null)
            {
                return NotFound();
            }
            return View(nhaCungCap);
        }

        // POST: Admin/AdminNhaCungCaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Nccid,Nccname,Phone,Address,CreateDate")] NhaCungCap nhaCungCap)
        {
            if (id != nhaCungCap.Nccid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhaCungCap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhaCungCapExists(nhaCungCap.Nccid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(nhaCungCap);
        }

        // GET: Admin/AdminNhaCungCaps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NhaCungCaps == null)
            {
                return NotFound();
            }

            var nhaCungCap = await _context.NhaCungCaps
                .FirstOrDefaultAsync(m => m.Nccid == id);
            if (nhaCungCap == null)
            {
                return NotFound();
            }

            return View(nhaCungCap);
        }

        // POST: Admin/AdminNhaCungCaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ncc = await _context.NhaCungCaps.FindAsync(id);
            _context.NhaCungCaps.Remove(ncc);
            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool NhaCungCapExists(int id)
        {
          return (_context.NhaCungCaps?.Any(e => e.Nccid == id)).GetValueOrDefault();
        }
    }
}
