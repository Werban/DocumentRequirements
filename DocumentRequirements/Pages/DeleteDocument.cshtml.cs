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
                
                Console.WriteLine($"������ ��� �������� ��������� ��� ��������: {ex.Message}");
                ModelState.AddModelError(string.Empty, "��������� ������ ��� �������� ���������. ����������, ���������� �����.");
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
                    Console.WriteLine($"�������� ������: {Document.Name}");
                }

                return RedirectToPage("./Index");
            }
            catch (DbUpdateException dbEx)
            {
                
                Console.WriteLine($"������ ���� ������ ��� �������� ���������: {dbEx.Message}");
                ModelState.AddModelError(string.Empty, "��������� ������ ��� �������� ���������. ����������, ���������� �����.");
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"��������� ������: {ex.Message}");
                ModelState.AddModelError(string.Empty, "��������� �������������� ������. ����������, ���������� �����.");
                return RedirectToPage("./Index");
            }
        }
    }
}