namespace MenuSystem.Models
{
    public class MenuCreateModel
    {
        public Menu Menu { get; set; }
        public List<Category>? Categories { get; set; }
    }
}
