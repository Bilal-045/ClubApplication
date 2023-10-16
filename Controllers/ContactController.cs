using System.Web.Mvc;
using techwales.Models;

namespace techwales.Controllers

{

    public class ContactController : Controller
    {

        PrimeScoutsEntities1 db = new PrimeScoutsEntities1();
        [HttpGet]
        // GET: Home

        [HttpPost]

        public ActionResult AddContact()
        {
            return View();
        }

        // POST: Employee/AddContact    
        [HttpPost]




        // @@@@@@@@@@@@@@@@@@@@@@@@@@@
        public ActionResult SaveContactObj(CONTACT Contt)
        {


            CONTACT con = new CONTACT();
            con.Name = Contt.Name;
            con.email = Contt.email;
            con.Subject = Contt.Subject;
            con.Message = Contt.Message;
            ////return RedirectToAction("Home/Contact");



            return View(con);
        }
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@




        //To Add Employee details

    }
}