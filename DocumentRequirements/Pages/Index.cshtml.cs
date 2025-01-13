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

        public async Task OnGetAsync()
        {
            // �������� ����������
            Documents = await _context.Documents.ToListAsync();

            // �������� ���������� � ��������� � ����������
            Requirements = await _context.Requirements
                .Include(r => r.Document) // �������� ��������� ���������
                .ToListAsync();

            
            Console.WriteLine($"��������� ����������: {Documents.Count}");
            Console.WriteLine($"��������� ����������: {Requirements.Count}");
        }
    }
}