using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DocumentRequirements.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Document> Documents { get; set; } = new List<Document>();
        public List<Requirement> Requirements { get; set; } = new List<Requirement>();

        [BindProperty(SupportsGet = true)]
        public Guid? SelectedDocumentId { get; set; } // Для хранения выбранного DocumentId

        public async Task OnGetAsync()
        {
            // Загрузка всех документов
            Documents = await _context.Documents.ToListAsync();

            // Фильтрация требований по выбранному документу
            if (SelectedDocumentId.HasValue)
            {
                Requirements = await _context.Requirements
                    .Include(r => r.Document) // Включаем связанные документы
                    .Where(r => r.DocumentId == SelectedDocumentId.Value) // Фильтруем по DocumentId
                    .ToListAsync();
            }
            else
            {
                // Если документ не выбран, загружаем все требования
                Requirements = await _context.Requirements
                    .Include(r => r.Document)
                    .ToListAsync();
            }

            // Логирование для отладки
            Console.WriteLine($"Загружено документов: {Documents.Count}");
            Console.WriteLine($"Загружено требований: {Requirements.Count}");
        }
    }
}