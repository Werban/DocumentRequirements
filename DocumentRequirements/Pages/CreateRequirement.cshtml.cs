using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DocumentRequirements.Pages
{
    public class CreateRequirementModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateRequirementModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Requirement Requirement { get; set; }

        public SelectList DocumentOptions { get; set; }

        public void OnGet()
        {
            try
            {
                // Загрузка документов из базы данных
                var documents = _context.Documents.ToList();
                Console.WriteLine($"Загружено документов: {documents.Count}");

                // Создание выпадающего списка
                DocumentOptions = new SelectList(documents, "Id", "Name");
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Ошибка при загрузке документов: {ex.Message}");
                // Устанавливаем пустой список, чтобы избежать ошибок на странице
                DocumentOptions = new SelectList(Enumerable.Empty<SelectListItem>());
                // Добавляем сообщение об ошибке в ModelState
                ModelState.AddModelError(string.Empty, "Произошла ошибка при загрузке документов. Пожалуйста, попробуйте позже.");
            }
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

                    // Перезагрузка списка документов для выпадающего списка
                    DocumentOptions = new SelectList(_context.Documents.ToList(), "Id", "Name");
                    return Page();
                }

                // Проверка, что DocumentId соответствует существующему документу
                var documentExists = await _context.Documents.AnyAsync(d => d.Id == Requirement.DocumentId);
                if (!documentExists)
                {
                    ModelState.AddModelError("Requirement.DocumentId", "Выбранный документ не существует.");
                    DocumentOptions = new SelectList(_context.Documents.ToList(), "Id", "Name");
                    return Page();
                }

                
                Console.WriteLine($"DocumentId: {Requirement.DocumentId}");
                Console.WriteLine($"RequirementDesignation: {Requirement.RequirementDesignation}");
                Console.WriteLine($"Formulation: {Requirement.Formulation}");

                
                Requirement.Id = Guid.NewGuid();
                _context.Requirements.Add(Requirement);
                await _context.SaveChangesAsync();

                Console.WriteLine($"Требование добавлено: {Requirement.RequirementDesignation}");

                return RedirectToPage("./Index");
            }
            catch (DbUpdateException dbEx)
            {
                
                Console.WriteLine($"Ошибка базы данных: {dbEx.Message}");
                ModelState.AddModelError(string.Empty, "Произошла ошибка при сохранении данных. Пожалуйста, попробуйте позже.");
                DocumentOptions = new SelectList(_context.Documents.ToList(), "Id", "Name");
                return Page();
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Произошла непредвиденная ошибка. Пожалуйста, попробуйте позже.");
                DocumentOptions = new SelectList(_context.Documents.ToList(), "Id", "Name");
                return Page();
            }
        }
    }
}