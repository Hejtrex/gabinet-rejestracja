using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using gabinet_rejestracja.Models;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System.Data.SqlTypes;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace gabinet_rejestracja.Controllers
{
    public class AppointmentController : Controller
    {

        private readonly SqlConnection _conn;

        public AppointmentController()
        {
            _conn = new SqlConnection("Data Source=servergabinet.database.windows.net;Initial Catalog=gabinetbaza;User ID=adming;Password=Qwerty231;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
        // GET: Appointment/Create
        public ActionResult Create()
        {
            return View();
        }
        // GET: Appointment/Lista
        public IActionResult Lista()
        {
            List<AppointmentModel> appointments = new List<AppointmentModel>();
            using (_conn)
            {
                string sql = "SELECT * FROM Appointments";
                SqlCommand cmd = new SqlCommand(sql, _conn);
                _conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    appointments.Add(new AppointmentModel
                    {
                        AppointmentId = (int)reader["AppointmentId"],
                        Comment = (string)reader["Comment"],
                        UserId = (int)reader["UserId"],
                        Date = (DateTime)reader["Date"]
                    });
                }
            }
            return View(appointments);
        }

        // POST: Appointment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AppointmentModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var db = new SqlConnection("Data Source=servergabinet.database.windows.net;Initial Catalog=gabinetbaza;User ID=adming;Password=Qwerty231;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                db.Open();


                // pobranie daty z rezerwacji
                DateTime appointmentDate = model.Date;

                // utworzenie obiektu DateTime złożonego z daty z rezerwacji
                DateTime today = DateTime.Today;
                DateTime appointmentDateTime = new DateTime(appointmentDate.Year, appointmentDate.Month, appointmentDate.Day, appointmentDate.Hour, 0, 0);
                string sql1 = "SELECT COUNT(*) FROM [dbo].[Appointments] WHERE Date = @Date";
                var command1 = new SqlCommand(sql1, db);
                command1.Parameters.AddWithValue("@Date", appointmentDateTime);

                int count = (int)command1.ExecuteScalar();
                if (appointmentDateTime >= today)
                {
                    if (count > 0)
                    {
                        ModelState.AddModelError("", "Wybrana data jest już zajęta");
                        return View(model);
                    }
                    else
                    {
                        var appointment = new AppointmentModel
                        {
                            UserId = model.UserId,
                            AppointmentId = model.AppointmentId,
                            Date = model.Date,
                            Comment = model.Comment
                        };

                        string sql = "INSERT INTO [dbo].[Appointments] ([UserId], [AppointmentId], [Date], [Comment]) " +
                            "VALUES (@UserId, ABS(CHECKSUM(NEWID()) % 2147483647) + 1, @Date, @Comment)";
                        var command = new SqlCommand(sql, db);
                        Request.Cookies.TryGetValue("UserId", out string UserId);

                        command.Parameters.AddWithValue("@UserId", UserId);
                        command.Parameters.AddWithValue("@Date", appointment.Date);
                        command.Parameters.AddWithValue("@Comment", appointment.Comment);

                        command.ExecuteNonQuery();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Rejestrowanie się z datą wstecz jest niedozwolone.");
                    return View(model);
                }
            }
            return RedirectToAction("Index", "User");
        }
        // POST : Appointment/Delete

        public IActionResult Delete(int id)
        {
            using (SqlConnection connection1 = new SqlConnection("Data Source = servergabinet.database.windows.net; Initial Catalog = gabinetbaza; User ID = adming; Password = Qwerty231; Connect Timeout = 30; Encrypt = True; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False"))
            {
                string sqlQuery = "DELETE FROM Appointments WHERE AppointmentId = @id";
                SqlCommand command1 = new SqlCommand(sqlQuery, connection1);
                command1.Parameters.AddWithValue("@id", id);

                connection1.Open();
                command1.ExecuteNonQuery();
            }

            return RedirectToAction("Lista");
        }
        public ActionResult Edit()
        {
            return View();
        }

        // POST : Appointment/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, AppointmentModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var db = new SqlConnection("Data Source=servergabinet.database.windows.net;Initial Catalog=gabinetbaza;User ID=adming;Password=Qwerty231;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                db.Open();


                // pobranie daty z rezerwacji
                DateTime appointmentDate = model.Date;

                // utworzenie obiektu DateTime złożonego z daty i godziny z rezerwacji
                DateTime appointmentDateTime = new DateTime(appointmentDate.Year, appointmentDate.Month, appointmentDate.Day, 0, 0, 0);

                string sql1 = "SELECT COUNT(*) FROM [dbo].[Appointments] WHERE Date = @Date";
                var command1 = new SqlCommand(sql1, db);
                command1.Parameters.AddWithValue("@Date", appointmentDateTime);

                int count = (int)command1.ExecuteScalar();
                if (count > 0)
                {
                    ModelState.AddModelError("", "Wybrana data jest już zajęta");
                    return View(model);
                }
                else
                {
                    var appointment = new AppointmentModel
                    {
                        UserId = model.UserId,
                        AppointmentId = model.AppointmentId,
                        Date = model.Date,
                        Comment = model.Comment
                    };

                    string sql = "UPDATE Appointments SET Date = @Date, Comment = @Comment WHERE AppointmentId = @AppointmentId";
                    var command = new SqlCommand(sql, db);
                    Request.Cookies.TryGetValue("UserId", out string UserId);

                    command.Parameters.AddWithValue("@AppointmentId", id);
                    command.Parameters.AddWithValue("@Date", appointment.Date);
                    command.Parameters.AddWithValue("@Comment", appointment.Comment);

                    command.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Lista");
        }
    }
            
}