using MenuSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace MenuSystem.Controllers
{
    public class UrunYonetimiController : Controller
    {
        SqlConnection connection = new SqlConnection();
        public UrunYonetimiController(IConfiguration configuration)
        {
            connection.ConnectionString = configuration.GetConnectionString("MenuSistemi");
        }
        public IActionResult Index(int id)
        {
            ViewBag.Categories = GetCategories();

            UrunYonetimiModel model = new UrunYonetimiModel();
            model.Category = GetCategory(id);
            model.Menus = GetMenus(id);

            return View(model);
        }

        public List<Category> GetCategories()
        {

            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.Categories", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<Category> categories = new List<Category>();

            foreach (DataRow row in dt.Rows)
            {
                categories.Add(new Category
                {
                    CategoryId = Convert.ToInt32(row["CategoryId"]),
                    CategoryName = row["CategoryName"].ToString()
                });
            }

            return categories;
        }

        public Category GetCategory(int categoryId)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.Categories where CategoryId=@CategoryId", connection);
            da.SelectCommand.Parameters.AddWithValue("CategoryId", categoryId);
            DataTable dt = new DataTable();
            da.Fill(dt);

            Category category = new Category
            {
                CategoryId = Convert.ToInt32(dt.Rows[0]["CategoryId"]),
                CategoryName = dt.Rows[0]["CategoryName"].ToString()
            };

            return category;
        }

        public List<Menu> GetMenus(int categoryId)
        {

            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.Menus where CategoryId=@CategoryId", connection);
            da.SelectCommand.Parameters.AddWithValue("CategoryId", categoryId);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<Menu> menus = new List<Menu>();

            foreach (DataRow row in dt.Rows)
            {
                Menu menu = new Menu
                {
                    MenuId = Convert.ToInt32(row["MenuId"]),
                    MenuName = row["MenuName"].ToString(),
                    CategoryId = Convert.ToInt32(row["CategoryId"]),
                    Details = row["Details"].ToString(),
                    ImageUrl = row["MenuName"].ToString(),
                    Price = Convert.ToDecimal(row["Price"])

                };
                menus.Add(menu);
            }

            return menus;
        }

    }
}

