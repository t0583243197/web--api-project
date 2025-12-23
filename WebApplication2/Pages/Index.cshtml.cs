using Microsoft.AspNetCore.Mvc; // מייבא MVC attributes
using Microsoft.AspNetCore.Mvc.RazorPages; // מייבא PageModel

namespace WebApplication2.Pages // מרחב שמות של Pages
{ // פתיחת namespace
    public class IndexModel : PageModel // מודל העמוד Index
    { // פתיחת מחלקה
        private readonly ILogger<IndexModel> _logger; // לוגר לעמוד

        public IndexModel(ILogger<IndexModel> logger) // בנאי מקבל לוגר
        { // פתיחת בנאי
            _logger = logger; // שמירת הלוגר בשדה
        } // סגירת בנאי

        public void OnGet() // מטודה שמטפלת בבקשת GET
        { // פתיחת מטודה
        } // סגירת מטודה
    } // סגירת מחלקה
} // סגירת namespace
