using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DocumentRequirements.Pages
{
    public class DeleteRequirementModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteRequirementModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Requirement Requirement { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                Requirement = await _context.Requirements
                    .Include(r => r.Document)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (Requirement == null)
                {
                    return NotFound();
                }

                return Page();
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Ошибка при загрузке требования для удаления: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Произошла ошибка при загрузке требования. Пожалуйста, попробуйте позже.");
                return RedirectToPage("./Index");
            }
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                Requirement = await _context.Requirements.FindAsync(id);

                if (Requirement != null)
                {
                    _context.Requirements.Remove(Requirement);
                    await _context.SaveChangesAsync();
                    Console.WriteLine($"Требование удалено: {Requirement.RequirementDesignation}");
                }

                return RedirectToPage("./Index");
            }
            catch (DbUpdateException dbEx)
            {
                
                Console.WriteLine($"Ошибка базы данных при удалении требования: {dbEx.Message}");
                ModelState.AddModelError(string.Empty, "Произошла ошибка при удалении требования. Пожалуйста, попробуйте позже.");
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Произошла непредвиденная ошибка. Пожалуйста, попробуйте позже.");
                return RedirectToPage("./Index");
            }
        }
    }
}