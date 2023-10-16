using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Principal;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Web.Security;
using techwales.Models;
using System.Configuration;
using System.Web.ModelBinding;
using System.Data.Entity.Validation;
using System;
using System.Data.Entity;
using techwales.Models.Account;
using System.Runtime.InteropServices.WindowsRuntime;

namespace techwales.Controllers.Account
{
    public class AccountController : Controller
    {
        // GET: Account
        PrimeScoutsEntities1 db = new PrimeScoutsEntities1();
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Logout()
        {

            FormsAuthentication.SignOut();
            Session.Clear();
            
            Session.Abandon();
            return RedirectToAction("Login");
        }

        public ActionResult UserList(string search_type)
        {

            using (PrimeScoutsEntities1 db = new PrimeScoutsEntities1())
            {
                List<USER> users = db.USERs.ToList();
                //return View(db.USERs.ToList());
                return View(db.USERs.Where(x => x.Type.StartsWith(search_type) || search_type == null).ToList());
            }
        }

        public ActionResult Useredit(int id)
        {
            USER user = db.USERs.SingleOrDefault(x => x.Id == id);
            //get students from db
            return View(user);
        }
        [HttpPost]

        public ActionResult Useredit(USER us)
        {
            if(ModelState.IsValid)
            {
                PrimeScoutsEntities1 dbc = new PrimeScoutsEntities1();
               
                dbc.Entry(us).State = System.Data.Entity.EntityState.Modified;
                dbc.SaveChanges();
                return RedirectToAction("UserList");
            }
            return View();
        }

        //public ActionResult Useredit([Bind(Include = "Id, USERNAME, PHONE, TYPE , EMAIL, PASSWORD")] USER usr)
        //{
        //    using (var datab = db)
        //    {
        //        //if (ModelState.IsValid)
        //        {
        //            var entry = datab.Entry(usr);
        //           // entry.Property(x => x.Password).IsModified = false;


        //            //datab.(usr).Property(x => x.Password).IsModified = false;
        //            //datab.Entry(usr).State = (System.Data.Entity.EntityState)EntityState.Modified;

        //            // db.Single(x => x.Id == usu.Id);
        //            datab.SaveChanges();

        //        }
        //        return View();
        //    }
        //}
        //}
        
        
        //public ActionResult Useredit(USER model)
        //{
        //    if (ModelState.IsValid)
        //    { updateuser(model.Id, model);
        //        return RedirectToAction("updateuser");
        //    }
        //    return RedirectToAction("updateuser");

        //    //return View();
        //}

        //        public bool updateuser( int id, USER model)
        //{
        //    using (var context = new PrimeScoutsEntities1())
        //    {

        //        var eduser = context.USERs.SingleOrDefault(x => x.Id == id);
        //        if(eduser != null)
        //        {
        //            eduser.Username = model.Username; 
        //            eduser.Phone = model.Phone;
        //            eduser.Email = model.Email;
        //            eduser.Type = model.Type;
        //            eduser.Password = model.Password;
        //        }
        //        //context.SaveChanges();

        //        try
        //        {
        //            context.SaveChanges();
        //        }
        //        catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
        //        {
        //            Exception raise = dbEx;
        //            foreach (var validationErrors in dbEx.EntityValidationErrors)
        //            {
        //                foreach (var validationError in validationErrors.ValidationErrors)
        //                {
        //                    string message = string.Format("{0}:{1}",
        //                        validationErrors.Entry.Entity.ToString(),
        //                        validationError.ErrorMessage);
        //                    // raise a new exception nesting
        //                    // the current instance as InnerException
        //                    raise = new InvalidOperationException(message, raise);
        //                }
        //            }
        //            throw raise;
        //        }

        //        return true;
        //    }


        //}
        public ActionResult userdel(USER model)
        {
            if (ModelState.IsValid)
            {
                deluser(model.Id, model);
               
            }
            return RedirectToAction("UserList");
        }

        public bool deluser(int id, USER model)
        {
            using (var context = new PrimeScoutsEntities1())
            {

                var eduser = context.USERs.SingleOrDefault(x => x.Id == id);
                if (eduser != null)
                {
                   context.USERs.Remove(eduser);    
                    context.SaveChanges();
                    return true;
                }
                return false;
            }


        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(USER UA)
        {
            using (PrimeScoutsEntities1 db = new PrimeScoutsEntities1())
            {
                
                
                try
                {

                    var usr = db.USERs.Single(u => u.Username.ToUpper() == UA.Username.ToUpper() && u.Password == UA.Password);
                    if (usr != null)
                    {  int uid = usr.Id;
                        TempData["UID"]=uid;
                        Session["UserID"] = usr.Id.ToString();
                        Session["Username"] = UA.Username.ToString();
                                          
                        if (usr.Type == "Athlete")
                        {
                            if (usr.REG_STATUS == "INCOMPLETE")
                                return RedirectToAction("PlayerReg");
                            else if(usr.REG_STATUS == "COMPLETE")
                            {
                                TempData["shortMessage"] = "Registration Details Submitted, We will inform you on your registered mail after Approval";
                                return RedirectToAction("Index", "Home", new { Area = "" });
                            } else
                            return RedirectToAction("LoggedIn"); }
                        else
                        {
                            if (usr.REG_STATUS == "INCOMPLETE")
                                return RedirectToAction("AgentReg");
                            else if (usr.REG_STATUS == "COMPLETE")
                            {
                                TempData["shortMessage"] = "Registration Details Submitted, We will inform you on your registered mail after Approval";
                                return RedirectToAction("Index", "Home", new { Area = "" });
                            }
                            else
                                return RedirectToAction("LoggedIn");
                        }
                    }
                }
                catch
                {
                    //else
                    {
                        ModelState.AddModelError("", "UserName or Password incorrect ..!");
                    }
                }
            }

            return View();
        }


        public ActionResult AgentReg()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AgentReg(Agent_details Agent)
        {
            if (ModelState.IsValid)
            {
                using (PrimeScoutsEntities1 db = new PrimeScoutsEntities1())
                {
                    db.Agent_details.Add(Agent);
                    int user_id = (int)TempData["UID"];
                    string conn = ConfigurationManager.ConnectionStrings["loccon"].ConnectionString;
                    SqlConnection connection = new SqlConnection(conn);
                    connection.Open();
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "SELECT NEXT VALUE FOR Agent_details_SEQ;";
                        var obj = cmd.ExecuteScalar();
                        int anInt = (int)obj;
                        Agent.Id = anInt;
                        Agent.uid = user_id;
                     }
                    connection.Close();

                    connection.Open();
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "update [user] set REG_STATUS = 'COMPLETE' where id =" + user_id + ";";
                        var obj = cmd.ExecuteScalar();
                    }
                    connection.Close();
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                    {
                        Exception raise = dbEx;
                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                string message = string.Format("{0}:{1}",
                                    validationErrors.Entry.Entity.ToString(),
                                    validationError.ErrorMessage);
                                // raise a new exception nesting
                                // the current instance as InnerException
                                raise = new InvalidOperationException(message, raise);
                            }
                        }
                        throw raise;
                    }

                }
                ModelState.Clear();
                TempData["shortMessage"] = "Registration Details Submitted, We will inform you on your registered mail after Approval";

                return RedirectToAction("Index", "Home", new {Area = ""});
            }
            else

                return View();
        }

        public ActionResult LoggedIn()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
       
        public ActionResult SignUp(USER account)
        {
            int id = 0;
            string Uname;
            string Utype;

            if (ModelState.IsValid)
            {
                using (PrimeScoutsEntities1 db = new PrimeScoutsEntities1())
                {
                    db.USERs.Add(account);
                    int exist;
                    string conn = ConfigurationManager.ConnectionStrings["loccon"].ConnectionString;
                     SqlConnection connection = new SqlConnection(conn);
                    //var connection = db.Database.Connection;
                    connection.Open();
                    using (var cmd1 = connection.CreateCommand())
                    {
                        string command = "SELECT count(*) from [USER] where username = '"+account.Username.ToString()+"';";
                        cmd1.CommandText = command;
                        var obj = cmd1.ExecuteScalar();
                        exist = (int)obj;
                    }
                    if (exist > 0)
                    {
                        ModelState.Clear();
                        ViewBag.Message = account.Username + " is Already Registered. Use a different Account";

                    }
                    else
                    {
                        using (var cmd = connection.CreateCommand())
                        {
                            cmd.CommandText = "SELECT NEXT VALUE FOR USER_SEQ;";
                            var obj = cmd.ExecuteScalar();
                            int anInt = (int)obj;
                            account.Id = anInt;
                            id = anInt;
                            account.REG_STATUS = "INCOMPLETE";
                            account.Username = account.Username.ToUpper();
                        }
                        //Uname = account.Username.ToString();
                        //Utype = account.Type.ToString();
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                        {
                            Exception raise = dbEx;
                            foreach (var validationErrors in dbEx.EntityValidationErrors)
                            {
                                foreach (var validationError in validationErrors.ValidationErrors)
                                {
                                    string message = string.Format("{0}:{1}",
                                        validationErrors.Entry.Entity.ToString(),
                                        validationError.ErrorMessage);
                                    // raise a new exception nesting
                                    // the current instance as InnerException
                                    raise = new InvalidOperationException(message, raise);
                                }
                            }
                            throw raise;
                        }
                        ViewBag.Message = account.Username + " is Successfully Registered";
                    }
                    ModelState.Clear();
                   
                }
            }

            return View();

        }
       
        public ActionResult PlayerReg()
        {
            return View();
        }

        public TempDataDictionary GetTempData()
        {
            return TempData;
        }

        [HttpPost]
        public ActionResult PlayerReg(Player_details plr, TempDataDictionary tempData)
        {
            if (ModelState.IsValid)
            {
                using (PrimeScoutsEntities1 db = new PrimeScoutsEntities1())
                {
                    db.Player_details.Add(plr);
                    string conn = ConfigurationManager.ConnectionStrings["loccon"].ConnectionString;
                    SqlConnection connection = new SqlConnection(conn);
                    connection.Open();
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "SELECT NEXT VALUE FOR Player_details_SEQ;";
                        var obj = cmd.ExecuteScalar();
                        int anInt = (int)obj;
                        plr.Id = anInt;
                        plr.uid = (int)TempData["UID"];
                    }
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                    {
                        Exception raise = dbEx;
                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                string message = string.Format("{0}:{1}",
                                    validationErrors.Entry.Entity.ToString(),
                                    validationError.ErrorMessage);
                                // raise a new exception nesting
                                // the current instance as InnerException
                                raise = new InvalidOperationException(message, raise);
                            }
                        }
                        throw raise;
                    }

                }
                ModelState.Clear();
                TempData["shortMessage"] = "Registration Details Submitted, We will inform you on your registered mail after Approval";

                return RedirectToAction("Index", "Home", new { Area = "" });
            }
            else

                return View();



        }
    }



}