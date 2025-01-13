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
                // �������� ���������� �� ���� ������
                var documents = _context.Documents.ToList();
                Console.WriteLine($"��������� ����������: {documents.Count}");

                // �������� ����������� ������
                DocumentOptions = new SelectList(documents, "Id", "Name");
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"������ ��� �������� ����������: {ex.Message}");
                // ������������� ������ ������, ����� �������� ������ �� ��������
                DocumentOptions = new SelectList(Enumerable.Empty<SelectListItem>());
                // ��������� ��������� �� ������ � ModelState
                ModelState.AddModelError(string.Empty, "��������� ������ ��� �������� ����������. ����������, ���������� �����.");
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
                        Console.WriteLine($"������ ���������: {error.ErrorMessage}");
                    }

                    // ������������ ������ ���������� ��� ����������� ������
                    DocumentOptions = new SelectList(_context.Documents.ToList(), "Id", "Name");
                    return Page();
                }

                // ��������, ��� DocumentId ������������� ������������� ���������
                var documentExists = await _context.Documents.AnyAsync(d => d.Id == Requirement.DocumentId);
                if (!documentExists)
                {
                    ModelState.AddModelError("Requirement.DocumentId", "��������� �������� �� ����������.");
                    DocumentOptions = new SelectList(_context.Documents.ToList(), "Id", "Name");
                    return Page();
                }

                
                Console.WriteLine($"DocumentId: {Requirement.DocumentId}");
                Console.WriteLine($"RequirementDesignation: {Requirement.RequirementDesignation}");
                Console.WriteLine($"Formulation: {Requirement.Formulation}");

                
                Requirement.Id = Guid.NewGuid();
                _context.Requirements.Add(Requirement);
                await _context.SaveChangesAsync();

                Console.WriteLine($"���������� ���������: {Requirement.RequirementDesignation}");

                return RedirectToPage("./Index");
            }
            catch (DbUpdateException dbEx)
            {
                
                Console.WriteLine($"������ ���� ������: {dbEx.Message}");
                ModelState.AddModelError(string.Empty, "��������� ������ ��� ���������� ������. ����������, ���������� �����.");
                DocumentOptions = new SelectList(_context.Documents.ToList(), "Id", "Name");
                return Page();
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"��������� ������: {ex.Message}");
                ModelState.AddModelError(string.Empty, "��������� �������������� ������. ����������, ���������� �����.");
                DocumentOptions = new SelectList(_context.Documents.ToList(), "Id", "Name");
                return Page();
            }
        }
    }
}