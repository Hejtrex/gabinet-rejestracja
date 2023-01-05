using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using gabinet_rejestracja.Models;
using System.Data.SqlClient;

namespace gabinet_rejestracja.Controllers
{
    public class AppointmentController : Controller
    {
        // GET: Appointment/Create
        public ActionResult Create()
        {
            return View();
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

                // utworzenie obiektu DateTime złożonego z daty i godziny z rezerwacji
                DateTime appointmentDateTime = new DateTime(appointmentDate.Year, appointmentDate.Month, appointmentDate.Day, 0, 0, 0);

                string sql1 = "SELECT COUNT(*) FROM [dbo].[Appointments] WHERE DateTime = @DateTime";
                var command1 = new SqlCommand(sql1, db);
                command1.Parameters.AddWithValue("@DateTime", appointmentDateTime);

                int count = (int)command1.ExecuteScalar();
                if (count > 0)
                {
                    ModelState.AddModelError("", "Wybrana data/godzina jest już zajęta");
                    return View(model); ;
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
                        "VALUES (@UserId, @AppointmentId, @Date, @Comment)";
                    var command = new SqlCommand(sql, db);
                    command.Parameters.AddWithValue("@UserId", appointment.UserId);
                    command.Parameters.AddWithValue("@AppointmentId", appointment.AppointmentId);
                    command.Parameters.AddWithValue("@Date", appointment.Date);
                    command.Parameters.AddWithValue("@Comment", appointment.Comment);

                    command.ExecuteNonQuery();
                }                
            }
            return RedirectToAction("Index", "Home");
        }
    }
}