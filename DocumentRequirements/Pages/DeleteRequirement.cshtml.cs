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
                
                Console.WriteLine($"������ ��� �������� ���������� ��� ��������: {ex.Message}");
                ModelState.AddModelError(string.Empty, "��������� ������ ��� �������� ����������. ����������, ���������� �����.");
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
                    Console.WriteLine($"���������� �������: {Requirement.RequirementDesignation}");
                }

                return RedirectToPage("./Index");
            }
            catch (DbUpdateException dbEx)
            {
                
                Console.WriteLine($"������ ���� ������ ��� �������� ����������: {dbEx.Message}");
                ModelState.AddModelError(string.Empty, "��������� ������ ��� �������� ����������. ����������, ���������� �����.");
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