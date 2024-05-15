using MenuSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace MenuSystem.Controllers
{
    public class MenuController : Controller
    {
        SqlConnection connection = new SqlConnection();
        public MenuController(IConfiguration configuration)
        {
            connection.ConnectionString = configuration.GetConnectionString("MenuSistemi");
        }
        public IActionResult Index()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select m.*, c.CategoryName as CategoryName from dbo.Menus as m inner join dbo.Categories as c on c.CategoryId=m.CategoryId ", connection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            List<MenuPropModel> menuList = new List<MenuPropModel>();

            foreach (DataRow row in dataTable.Rows)
            {
                MenuPropModel menu = new MenuPropModel();
                menu.MenuId = Convert.ToInt32(row["MenuId"]);
                menu.MenuName = row["MenuName"].ToString();
                menu.Price = Convert.ToDecimal(row["Price"]);
                menu.Details = row["Details"].ToString();
                menu.CategoryId = Convert.ToInt32(row["CategoryId"]);
                menu.ImageUrl = row["ImageUrl"].ToString();
                menu.CategoryName = row["CategoryName"].ToString();

                menuList.Add(menu);
            }
            return View(menuList);
        }


        public IActionResult Create()
        {
            MenuCreateModel createModel = new MenuCreateModel
            {
                Menu = new Menu(),
                Categories = GetCategories()
            };

            return View(createModel);
        }

        [HttpPost]
        public IActionResult Create(MenuCreateModel model)
        {
            if (ModelState.IsValid)
            {
                SqlCommand cmd = new SqlCommand("insert into dbo.Menus values (@menuName, @details, @price, @imageUrl, @categoryId)", connection);
                cmd.Parameters.AddWithValue("menuName", model.Menu.MenuName);
                cmd.Parameters.AddWithValue("details", model.Menu.Details);
                cmd.Parameters.AddWithValue("price", model.Menu.Price);
                cmd.Parameters.AddWithValue("imageUrl", model.Menu.ImageUrl);
                cmd.Parameters.AddWithValue("categoryId", model.Menu.CategoryId);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                MenuCreateModel createModel = new MenuCreateModel
                {
                    Menu = model.Menu,
                    Categories = GetCategories()
                };

                return View(createModel);
            }

        }

        public List<Category> GetCategories() //veritabanından categorileri çektiğimiz liste gerektiği için bu metodu oluşturduk
        {
            List<Category> list = new List<Category>();

            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.Categories", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Category
                {
                    CategoryId = Convert.ToInt32(row["CategoryId"]),
                    CategoryName = row["CategoryName"].ToString()

                });
            }

            return list;
        }

        public Menu GetMenu(int menuId)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.Menus where MenuId=@MenuId", connection);
            da.SelectCommand.Parameters.AddWithValue("MenuId", menuId);
            DataTable dt = new DataTable();
            da.Fill(dt);

            
            for (int i = 0; i <= dt.Rows.Count; i++)
            {
                Menu menu = new Menu();
                {
                    menu.MenuId = Convert.ToInt32(dt.Rows[0]["MenuId"]);
                    menu.CategoryId = Convert.ToInt32(dt.Rows[0]["CategoryId"]);
                    menu.Details = dt.Rows[0]["Details"].ToString();
                    menu.MenuName = dt.Rows[0]["MenuName"].ToString();
                    menu.Price = Convert.ToDecimal(dt.Rows[0]["Price"]);
                    menu.ImageUrl = dt.Rows[0]["ImageUrl"].ToString();
                }
            }

                return View(Menu);
           
        }


        public MenuPropModel GetMenuPropModel(int id)
        {
            SqlDataAdapter da = new SqlDataAdapter("select m.*, c.CategoryName as CategoryName from dbo.Menus as m inner join dbo.Categories as c on c.CategoryId=m.CategoryId where m.MenuId=@MenuId", connection);
            da.SelectCommand.Parameters.AddWithValue("MenuId", id);
            DataTable dt = new DataTable();
            da.Fill(dt);

            MenuPropModel menu = new MenuPropModel();
            {
                menu.MenuId = Convert.ToInt32(dt.Rows[0]["MenuId"]);
                menu.CategoryId = Convert.ToInt32(dt.Rows[0]["CategoryId"]);
                menu.Details = dt.Rows[0]["Details"].ToString();
                menu.MenuName = dt.Rows[0]["MenuName"].ToString();
                menu.Price = Convert.ToDecimal(dt.Rows[0]["Price"]);
                menu.ImageUrl = dt.Rows[0]["ImageUrl"].ToString();
                menu.CategoryName = dt.Rows[0]["CategoryName"].ToString();
            };

            return menu;
        }


        public IActionResult Edit(int menuId)
        {
            ViewBag.KategoriListesi = GetCategories();

            return View(GetMenu(menuId));
        }

        [HttpPost]
        public IActionResult Edit(Menu model)
        {

            if (ModelState.IsValid)
            {

                SqlCommand cmd = new SqlCommand("update dbo.Menus set MenuName=@MenuName, Details=@Details, Price=@Price, ImageUrl=@ImageUrl, CategoryId=@CategoryId where MenuId=@MenuId", connection);
                cmd.Parameters.AddWithValue("MenuId", model.MenuId);
                cmd.Parameters.AddWithValue("MenuName", model.MenuName);
                cmd.Parameters.AddWithValue("Details", model.Details);
                cmd.Parameters.AddWithValue("Price", model.Price);
                cmd.Parameters.AddWithValue("ImageUrl", model.ImageUrl);
                cmd.Parameters.AddWithValue("CategoryId", model.CategoryId);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.KategoriListesi = GetCategories();
                return View(model);
            }
        }


        public IActionResult Delete(int id)
        {
            return View(GetMenuPropModel(id));
        }


        [HttpPost]
        public IActionResult Delete(Menu menu)
        {

            MenuPropModel menu1 = GetMenuPropModel(menu.MenuId);
            if (menu1 == null)
            {
                ModelState.AddModelError(string.Empty, menu.MenuId + "numaralı kayıt sistemde bulunamadı.");
                return View(menu1);
            }
            else
            {
                SqlCommand cmd = new SqlCommand("delete from dbo.Menus where MenuId=@MenuId", connection);
                cmd.Parameters.AddWithValue("MenuId", menu.MenuId);


                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();

                return RedirectToAction(nameof(Index));
            }
        }
    }
}
