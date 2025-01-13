using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DocumentRequirements.Pages
{
    public class DeleteDocumentModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteDocumentModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Document Document { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                Document = await _context.Documents.FirstOrDefaultAsync(d => d.Id == id);

                if (Document == null)
                {
                    return NotFound();
                }

                return Page();
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Ошибка при загрузке документа для удаления: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Произошла ошибка при загрузке документа. Пожалуйста, попробуйте позже.");
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

                Document = await _context.Documents.FindAsync(id);

                if (Document != null)
                {
                    _context.Documents.Remove(Document);
                    await _context.SaveChangesAsync();
                    Console.WriteLine($"Документ удален: {Document.Name}");
                }

                return RedirectToPage("./Index");
            }
            catch (DbUpdateException dbEx)
            {
                
                Console.WriteLine($"Ошибка базы данных при удалении документа: {dbEx.Message}");
                ModelState.AddModelError(string.Empty, "Произошла ошибка при удалении документа. Пожалуйста, попробуйте позже.");
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