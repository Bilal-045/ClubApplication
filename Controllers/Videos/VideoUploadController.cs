using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using techwales.Models;
using System.Data;
using System.Data.SqlClient;

namespace techwales.Controllers.Videos
{
    public class VideoUploadController : Controller
    {
        // GET: VideoUpload
        public ActionResult U_video()
        {


            List<Video> videoList = new List<Video>();  
            string mainconn = ConfigurationManager.ConnectionStrings["videoconn"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string sqlquery = "Select * from [dbo].[Videos]";
            SqlCommand sqlcom = new SqlCommand(sqlquery, sqlconn);
            sqlconn.Open();
            SqlDataReader reader = sqlcom.ExecuteReader();
       while (reader.Read())
            {

                Video video = new Video();
                video.Vname = reader["Vname"].ToString();
                video.Vpath = reader["Vpath"].ToString();
                videoList.Add(video);

            }
            return View(videoList);
         
        }
        [HttpPost]
        public ActionResult U_video(HttpPostedFileBase videofile)
        {

            if (videofile != null)
            {


                String filename = Path.GetFileName(videofile.FileName);
                if (videofile.ContentLength < 20971520)
                {
                    videofile.SaveAs(Server.MapPath("/Videofiles/" + filename));

                    String mainconn = ConfigurationManager.ConnectionStrings["clubEntities"].ConnectionString;
                    SqlConnection sqlconn = new SqlConnection(mainconn);
                    string sqlquery = "insert into [dbo].[Videos] values (@Vname, @Vpath)";
                    SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn);
                    sqlconn.Open();
                    sqlcomm.Parameters.AddWithValue("@Vname", filename);
                    sqlcomm.Parameters.AddWithValue("@Vpath", "/Videofiles/" + filename);
                    sqlcomm.ExecuteNonQuery();
                    sqlconn.Close();
                    ViewData["Message"] = "Record Saved Successfully";
                    //or
                    ViewBag.Message = "Video is Successfully Saved";
                }
            }
            return RedirectToAction("U_video");   
        }
    }
}


