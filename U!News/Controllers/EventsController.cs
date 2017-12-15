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
    public class EventsController : Controller
    {
        // GET: Events
        public ActionResult Index()
        {
            List<Events> list = new List<Events>();
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"SELECT EventsID, EventName, Description, Location,
                    DateAdded FROM Events";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            list.Add(new Events
                            {
                                ID = int.Parse(data["EventsID"].ToString()),
                                EventName = data["EventName"].ToString(),
                                Description = data["Description"].ToString(),
                                Location = data["Location"].ToString(),
                                DateAdded = DateTime.Parse(data["DateAdded"].ToString())
                                

                            });
                        }
                    }
                }
            }
            return View(list);
        }

        // GET: Events/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            {
                return View();
            }
        }

        // POST: Events/Create
        [HttpPost]
        public ActionResult Create(Events Event)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"INSERT INTO Events VALUES
                    (@EventName, @Description, @Location, @DateAdded)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@EventName", Event.EventName);
                    cmd.Parameters.AddWithValue("@Description", Event.Description);
                    cmd.Parameters.AddWithValue("@Location", Event.Location);
                    cmd.Parameters.AddWithValue("@DateAdded", DateTime.Now);
                    cmd.ExecuteNonQuery();
                    ViewBag.Success = "<div class='alert alert-success'>Record added.</div>"; // displays alert message when record is successfully added
                    ModelState.Clear(); // removes existing user input
                    return RedirectToAction("Index");
                }
            }
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Events/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Events/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}