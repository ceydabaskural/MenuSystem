using System.ComponentModel;

namespace MenuSystem.Models
{
    public class MenuPropModel : Menu
    {
        [DisplayName("Kategori Adı")]
        public string CategoryName { get; set; }
    }
}
