using System.Web.Mvc;
using techwales.Models;

namespace club.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        PrimeScoutsEntities1 db = new PrimeScoutsEntities1();

        [HttpGet]

        public ActionResult Index()
        {
            ViewBag.Message = TempData["shortMessage"];
            return View();
        }

        [HttpPost]
        public ActionResult Index(CONTACT cot)
        {


            db.CONTACTs.Add(cot);

            var connection = db.Database.Connection;
            connection.Open();
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT NEXT VALUE FOR CONTACT_SEQ;";
                var obj = cmd.ExecuteScalar();
                int anInt = (int)obj;
                cot.Id = anInt;
               // Session["Username"] = "ABC";
               // ViewBag.US = "ACC";
            }

            db.SaveChanges();
            return RedirectToAction("Index");




            //try
            //{
            //    db.Database.ExecuteSqlCommand("SET IDENTITY_INSERT CONTACT ON");
            //    db.CONTACTs.Add(cot);
            //    db.SaveChanges();
            //    db.Database.ExecuteSqlCommand("SET IDENTITY_INSERT CONTACT OFF");
            //}
            //finally
            //{
            //    //db.Database.CloseConnection();
            //}

            //try
            //{
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //catch (DbEntityValidationException dbEx)
            //{
            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //    {
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //        {
            //            System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
            //        }
            //    }
            //}




        }



        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Services()
        {
            return View();
        }
    }
}