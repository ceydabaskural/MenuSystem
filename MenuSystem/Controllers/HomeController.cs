using MenuSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace MenuSistemi.Controllers
{
    public class HomeController : Controller
    {
        SqlConnection connection = new SqlConnection();
        public HomeController(IConfiguration configuration)
        {
            connection.ConnectionString = configuration.GetConnectionString("MenuSistemi");
        }

        public IActionResult Index()
        {
            ViewBag.Categories = GetCategories();
            ViewBag.Menus = GetMenus();


            return View();
        }

        private List<Category> GetCategories()
        {
            List<Category> categories = new List<Category>();

            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.Categories", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);


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




        private List<Menu> GetMenus()
        {
            List<Menu> menu = new List<Menu>();

            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.Menus", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);


            foreach (DataRow row in dt.Rows)
            {
                menu.Add(new Menu
                {
                    MenuId = Convert.ToInt32(row["MenuId"]),
                    MenuName = row["MenuName"].ToString(),
                    CategoryId = Convert.ToInt32(row["CategoryId"]),
                    Details = row["Details"].ToString(),
                    ImageUrl = row["MenuName"].ToString(),
                    Price = Convert.ToDecimal(row["Price"])

                });

            }

            return menu;
        }
    }
}