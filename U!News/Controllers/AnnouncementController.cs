using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using U_News.App_Code;
using U_News.Models;

namespace U_News.Controllers
{
    public class AnnouncementController : Controller
    {
        public ActionResult Index()
        {
            List<Announcements> list = new List<Announcements>();
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = "SELECT AnnouncementName, Details, Date FROM Announcement";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            list.Add(new Announcements
                            {
                                AN = data["AnnouncementName"].ToString(),
                                Details = data["Details"].ToString(),
                                DateAdded = DateTime.Parse(data["Date"].ToString()),
                            });
                        }
                    }
                    return View(list);
                }
            }
        }

        public ActionResult Add()
        {
            Announcements Ann = new Announcements();
            
            return View(Ann);
        }

        [HttpPost]
        public ActionResult Add(Announcements Ann)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"INSERT INTO Announcement (AnnouncementName, Date, Details) VALUES
                       (@AnnouncementName, @Date, @Details)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    
                    cmd.Parameters.AddWithValue("@AnnouncementName", Ann.AN);
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Details", Ann.Details);
                    cmd.ExecuteNonQuery();
                   
                    return RedirectToAction("Index");
                }
            }
        }
    }
}