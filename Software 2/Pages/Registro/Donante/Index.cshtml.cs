using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Software_2.Pages.Registro.Donante
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // Lógica de registro aquí
            return RedirectToPage("/Index");
        }
    }
}