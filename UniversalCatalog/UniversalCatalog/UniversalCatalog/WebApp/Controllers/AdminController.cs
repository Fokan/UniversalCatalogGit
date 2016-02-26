﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.Models.UnitOfWork;

namespace WebApp.Controllers
{
    [Authorize(Roles ="admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        private IUnitOfWork _unitOfWork;

        public AdminController()
        {
            _unitOfWork = new UnitOfWork();
        }
        public ActionResult Index()
        {
            ViewBag.Paths = GetUrlsForSliderImages();
            return View();
        }
        private string[] GetUrlsForSliderImages()
        {
            //string[] extensions = new[] { "jpeg", "jpg", "png", "bmp" };
            string[] paths = Directory.GetFileSystemEntries(HttpContext.Server.MapPath("/Content/Sources/img/slides"));
            for (int i = 0; i < paths.Length; i++)
            {
                int index = paths[i].LastIndexOf('\\');
                paths[i] = "/Content/Sources/img/slides" + paths[i].Substring(index, paths[i].Length - index);
            }
            return paths;
        }

        public void DeleteSlideImage(string ImageSRC)
        {
            FileInfo file = new FileInfo(HttpContext.Server.MapPath(ImageSRC));
            file.Delete();
        }

        [HttpPost]
        public ActionResult AddSlideImage(HttpPostedFileBase File)
        {
            File.SaveAs(HttpContext.Server.MapPath("/Content/Sources/img/slides")+ "/" + File.FileName);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Users()
        {
            //var app = new ApplicationDbContext();
            //var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(app));
            //UserManager.AddToRole("6971465c-d6bd-4f17-bc4b-26a724099750", "admin");
            var Users = new ApplicationDbContext().Users.ToList();
            //var admin = Users.FirstOrDefault(u => u.UserName == "admin@gmail.com");
            //var user1 = Users.FirstOrDefault(u => u.UserName == "user1@gmail.com");
            //var adminroles = admin.Roles;
            //var user1Roles = user1.Roles;
            var allRoles = new ApplicationDbContext().Roles.ToList();
            //ViewBag.Roles = allRoles.ToDictionary<Guid, string>();
            return View(Users);
        }

        public ActionResult DeleteUser(Guid UserId)
        {
            
            var user=_unitOfWork.Users.SingleOrDefault(u=>u.Id==UserId.ToString());
            _unitOfWork.Users.Remove(user);
            _unitOfWork.SaveChanges();
            return RedirectToAction("Users");
        }

        public void DeleteComment(Guid CommentId)
        {
            var comment = _unitOfWork.Comments.Get(CommentId);
            _unitOfWork.Comments.Remove(comment);
            _unitOfWork.SaveChanges();
        }     
        

       

        

    }
}