using MenuSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace MenuSystem.Controllers
{
    public class EditorController : Controller
    {
        SqlConnection connection = new SqlConnection();
        public EditorController(IConfiguration configuration)
        {
            connection.ConnectionString = configuration.GetConnectionString("MenuSistemi");
        }
        public IActionResult Index()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.Categories", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<Category> list = new List<Category>();
            foreach (DataRow row in dt.Rows)
            {
                Category c = new Category();
                c.CategoryId = Convert.ToInt32(row["CategoryId"]);
                c.CategoryName = row["CategoryName"].ToString();

                list.Add(c);
            }

            return View(list);
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(Category model)
        {
            if (ModelState.IsValid)
            {
                SqlCommand cmd = new SqlCommand("insert into dbo.Categories values (@name)", connection);
                cmd.Parameters.AddWithValue("name", model.CategoryName);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(model);
            }


        }


        public ActionResult Edit(int id)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.Categories where CategoryId=@categoryId", connection);
            da.SelectCommand.Parameters.AddWithValue("categoryId", id);

            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                Category category = new Category();
                category.CategoryId = Convert.ToInt32(dt.Rows[0]["CategoryId"]);
                category.CategoryName = dt.Rows[0]["CategoryName"].ToString();

                return View(category);
            }
        }


        [HttpPost]
        public ActionResult Edit(Category model)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.Categories where CategoryId=@CategoryId", connection);
            da.SelectCommand.Parameters.AddWithValue("CategoryId", model.CategoryId);

            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                return View(model);
            }
            else
            {
                SqlCommand command = new SqlCommand("update dbo.Categories set CategoryName=@CategoryName where CategoryId=@CategoryId", connection);
                command.Parameters.AddWithValue("CategoryId", model.CategoryId);
                command.Parameters.AddWithValue("CategoryName", model.CategoryName);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

                return RedirectToAction(nameof(Index));
            }

        }


        public ActionResult Delete(int id)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.Categories where CategoryId=@CategoryId", connection);
            da.SelectCommand.Parameters.AddWithValue("CategoryId", id);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                Category category = new Category();
                category.CategoryId = Convert.ToInt32(dt.Rows[0]["CategoryId"]);
                category.CategoryName = dt.Rows[0]["CategoryName"].ToString();

                return View(category);
            }
        }


        [HttpPost]
        public ActionResult Delete(Category model)
        {

            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.Categories where CategoryId=@CategoryId", connection);
            da.SelectCommand.Parameters.AddWithValue("CategoryId", model.CategoryId);

            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                return View(model);
            }
            else
            {
                SqlCommand cmd = new SqlCommand("delete from dbo.Categories where CategoryId=@CategoryId", connection);
                cmd.Parameters.AddWithValue("CategoryId", model.CategoryId);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();

                return RedirectToAction(nameof(Index));
            }
        }


    }
}
