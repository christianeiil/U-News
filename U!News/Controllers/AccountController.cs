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
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Users record)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"SELECT userID, typeID FROM users
                    WHERE userEmail=@userEmail AND userPW=@userPW
                    AND userStatus!=@userStatus";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@userEmail", record.Email);
                    cmd.Parameters.AddWithValue("@userPW", Helper.Hash(record.Password));
                    cmd.Parameters.AddWithValue("@userStatus", "Archived");

                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        if (data.HasRows)
                        { 
                            
                            while (data.Read())
                            {
                                Session["userid"] = data["userID"].ToString();
                                Session["typeid"] = data["typeID"].ToString();
                            }
                            return RedirectToAction("Index", "Announcement");
                        }
                        else
                        {
                            
                            ViewBag.Message = "<div class='alert alert-danger'>Incorrect email or password.</div>"; // displays error message if credentials are incorrect
                            return View();
                        }
                    }
                }
            }
        }

        public List<UserType> GetUserTypes()
        {
            List<UserType> list = new List<UserType>();
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"SELECT typeID, typeName
                    FROM types
                    ORDER BY typeName";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            list.Add(new UserType
                            {
                                ID = int.Parse(data["typeID"].ToString()),
                                Name = data["typeName"].ToString()
                            });
                        }
                        return list;
                    }
                }
            }
        }
         
        public ActionResult Register()
        {
            Users record = new Users();
            record.Types = GetUserTypes();
            return View(record);
        }
   
        public static bool IsExisting(string email)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"SELECT userEmail FROM users WHERE userEmail=@userEmail";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@userEmail", email);
                    return cmd.ExecuteScalar() == null ? false : true;
                }
            }
        }

        [HttpPost]
        public ActionResult Register(Users record)
        {
            if (IsExisting(record.Email))
            {
 
                ViewBag.Message = "<div class='alert alert-danger'>Email address already existing.</div>";
                record.Types = GetUserTypes();
                return View(record);
            }
            else
            {
                using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
                {
                    con.Open();
                    string query = @"INSERT INTO users  
                            VALUES (@typeID, @userEmail, @userPW, @userFN, @userLN, @userPhone, @userStatus)";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@typeID", 2);
                        cmd.Parameters.AddWithValue("@userEmail", record.Email);
                        cmd.Parameters.AddWithValue("@userPW", Helper.Hash(record.Password));
                        cmd.Parameters.AddWithValue("@userFN", record.FN);
                        cmd.Parameters.AddWithValue("@userLN", record.LN);
                        cmd.Parameters.AddWithValue("@userPhone", DBNull.Value);
                        cmd.Parameters.AddWithValue("@userStatus", "Pending");
                        cmd.ExecuteNonQuery();

                        return RedirectToAction("Login");

                        record.Types = GetUserTypes();
                        return View(record);

                        

                        
                    }
                }
            }
        }

        public ActionResult Manage()
        {
            if (Session["userid"] == null)
                return RedirectToAction("Index", "Home");

            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"SELECT userID, userEmail,
                    userFN, userLN, userPhone
                    FROM users
                    WHERE userID=@userID AND userStatus!=@userStatus";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@userID", Session["userid"].ToString());
                    cmd.Parameters.AddWithValue("@userStatus", "Archived");
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            Users record = new Users();
                            while (data.Read())
                            {
                                record.ID = int.Parse(data["userID"].ToString());
                                record.Email = data["userEmail"].ToString();
                                record.FN = data["userFN"].ToString();
                                record.LN = data["userLN"].ToString();
                                record.Phone = data["userPhone"].ToString();
                             
                            }
                            return View(record);
                        }
                        else
                        {
                            return RedirectToAction("Index");
                        }
                    }
                }
            }
        }

        [HttpPost]
        public ActionResult Manage(Users record)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = "";
                if (record.Password == string.Empty)
                {
                    query = @"UPDATE users SET userEmail=@userEmail,
                        userFN=@userFN, userLN=@userLN, userPhone=@userPhone
                
                        WHERE userID=@userID";
                }
                else
                {
                    query = @"UPDATE users SET userEmail=@userEmail, userPW=@userPW,
                        userFN=@userFN, userLN=@userLN, userPhone=@userPhone
        
                        WHERE userID=@userID";
                }

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@userEmail", record.Email);
                    cmd.Parameters.AddWithValue("@userPW", Helper.Hash(record.Password));
                    cmd.Parameters.AddWithValue("@userFN", record.FN);
                    cmd.Parameters.AddWithValue("@userLN", record.LN);
                    cmd.Parameters.AddWithValue("@userPhone", record.Phone);
                    cmd.Parameters.AddWithValue("@userID", Session["userid"].ToString());
                    cmd.ExecuteNonQuery();
                    ViewBag.Message = "<div class='alert alert-success'>Profile updated.</div>"; // displays alert message when record is successfully updated
                    return View(record);
                }
            }
        }

        public ActionResult Logout()
        {
            Session.Clear(); // removes all existing session activities
            return RedirectToAction("Index", "Home");
        }
    }

}