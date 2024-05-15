using Microsoft.AspNetCore.Mvc;

namespace MenuSystem.Models
{
    public class UrunYonetimiModel : Controller
    {
        public Category Category { get; set; }
        public List<Menu> Menus { get; set; }
    }
}
