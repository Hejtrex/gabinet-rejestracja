using System.Data.Entity;
using System.Configuration;
using gabinet_rejestracja.Models;
using Microsoft.AspNetCore.Mvc;
using ConfigurationManager = System.Configuration.ConfigurationManager;
using System.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace gabinet_rejestracja.Controllers
{
    public class UserController : Controller
    {
        // User/Index
        public IActionResult Index()
        {
            return View();
        }


        // GET: User/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: User/Register
        [HttpPost]
        public ActionResult Register(UserModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                //var ConnStr = ConfigurationManager.ConnectionStrings["ConnectionStrings"].ConnectionString;
                using (var db = new SqlConnection("Data Source=servergabinet.database.windows.net;Initial Catalog=gabinetbaza;User ID=adming;Password=Qwerty231;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                {
                    var user = new UserModel
                    {
                        Email = model.Email,
                        Password = model.Password,
                        ConfirmPassword = model.ConfirmPassword,
                    };
                    db.Open();
                    string sql = "INSERT INTO [dbo].[Users] ([UserId], [Email], [Password], [ConfirmPassword]) VALUES (ABS(CHECKSUM(NEWID()) % 2147483647) + 1, @Email, @Password, @ConfirmPassword)";
                    var command = new SqlCommand(sql, db);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@ConfirmPassword", user.ConfirmPassword);
                    
                    command.ExecuteNonQuery();
                    //db.Users.Add(user);
                    //db.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: User/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: User/Login
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string userId = null;
            //var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (var db = new SqlConnection("Data Source=servergabinet.database.windows.net;Initial Catalog=gabinetbaza;User ID=adming;Password=Qwerty231;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                db.Open();
                // sprawdzanie, czy istnieje użytkownik o podanym adresie e-mail i haśle
                string sql = "SELECT COUNT(*) FROM [dbo].[Users] WHERE Email = @Email AND Password = @Password";
                var command = new SqlCommand(sql, db);
                command.Parameters.AddWithValue("@Email", model.Email);
                command.Parameters.AddWithValue("@Password", model.Password);

                
                int count = (int)command.ExecuteScalar();
                if (count == 0)
                {
                    ModelState.AddModelError("", "Nieprawidłowy adres e-mail lub hasło.");
                    return View(model);
                }
                else
                {
                    // zapisanie informacji o zalogowanym użytkowniku
                    // możesz użyć np. sesji lub ciasteczek
                    // przykład z użyciem ciasteczek:
                    Response.Cookies.Append("Email", model.Email);
                    Response.Cookies.Append("Password", model.Password);
                    string sql1 = "SELECT UserId FROM Users WHERE Email = @Email";
                    SqlCommand command1 = new SqlCommand(sql1, db);
                    command1.Parameters.AddWithValue("@Email", model.Email);
                    SqlDataReader reader = command1.ExecuteReader();
                    if (reader.Read())
                    {
                        Response.Cookies.Append("UserId", reader["UserId"].ToString());
                    }
                    reader.Close();

                    return RedirectToAction("Lista", "Appointment");
                }
            }
        }
    }
}
