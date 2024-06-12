using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MenuSystem.Models
{
    public class Menu
    {
        [DisplayName("Menü No")]
        public int MenuId { get; set; }


        [DisplayName("Adı")]
        [Required(ErrorMessage = "Menü Adı zorunlu alandır.")]
        public string MenuName { get; set; }


        [DisplayName("Detay")]
        public string? Details { get; set; }


        [DisplayName("Fiyat")]
        [Required(ErrorMessage ="Fiyat zorunlu alandır")]
        public decimal Price { get; set; }


        [DisplayName("Görsel")]
        public string? ImageUrl { get; set; }


        [DisplayName("Kategori Adı")]
        public int CategoryId { get; set; }
    }
}
