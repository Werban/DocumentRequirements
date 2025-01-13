using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DocumentRequirements.Pages
{
    public class CreateDocumentModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateDocumentModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Document Document { get; set; } = new Document();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                
                if (!ModelState.IsValid)
                {
                    
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        Console.WriteLine($"Ошибка валидации: {error.ErrorMessage}");
                    }

                    return Page();
                }

                
                Document.EffectiveDate = Document.EffectiveDate.ToUniversalTime();

                
                Document.Id = Guid.NewGuid();
                _context.Documents.Add(Document);
                await _context.SaveChangesAsync();

                Console.WriteLine($"Документ добавлен: {Document.Name}");

                return RedirectToPage("./Index");
            }
            catch (DbUpdateException dbEx)
            {
                
                Console.WriteLine($"Ошибка базы данных при сохранении документа: {dbEx.Message}");
                ModelState.AddModelError(string.Empty, "Произошла ошибка при сохранении документа. Пожалуйста, попробуйте позже.");
                return Page();
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Произошла непредвиденная ошибка. Пожалуйста, попробуйте позже.");
                return Page();
            }
        }
    }
}