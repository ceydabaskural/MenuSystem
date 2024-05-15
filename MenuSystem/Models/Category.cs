using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MenuSystem.Models
{
    public class Category
    {
        [DisplayName("Kategori No")]
        public int CategoryId { get; set; }


        [DisplayName("Kategori Adı")]
        [Required(ErrorMessage ="Kategori Adı alanı boş geçilemez")]
        public string CategoryName { get; set; }                 
    }
}
