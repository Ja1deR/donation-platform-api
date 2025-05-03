using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Software_2.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            // Puedes inicializar datos aquí si es necesario
            ViewData["Title"] = "Inicio - Rescate Alimentario";
        }

        // Opcional: Método para manejar POST
        public IActionResult OnPost()
        {
            // Lógica para manejar formularios
            return Page();
        }
    }
}