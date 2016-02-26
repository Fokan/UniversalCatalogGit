using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.Models.UnitOfWork;
using WebApp.Models.Entities;
using System.IO;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public HomeController()
        {
            _unitOfWork = new UnitOfWork();
        }
        
        public ActionResult Index()
        {
            var types = _unitOfWork.Types.Where(type => type.ParentTypeId == Guid.Empty && type.Id!=Guid.Empty).OrderBy(type=>type.Name);
            ViewBag.ParentName = "Goods types";
            ViewBag.Paths = GetUrlsForSliderImages();
            return View(types);
        }

        public ActionResult GetContent(Guid TypeId)
        {
            var type = _unitOfWork.Types.Get(TypeId);
            if(type==null)
            {
                return HttpNotFound();
            }
            var childsType = _unitOfWork.Types.Where(child => child.ParentTypeId == TypeId).ToList();
            if(childsType.Count()==0)
            {
                return RedirectToAction("Items", new { TypeId = TypeId });
            }
            ViewBag.Paths = GetUrlsForSliderImages();
            ViewBag.ParentName = type.Name;
            return View("Index", childsType);
        }
        
        public ActionResult AutocompleteSearch(string term)
        {
            var items = _unitOfWork.Items.Where(a => a.Name.Contains(term))
                            .Select(a => new { value = a.Name, Id=a.Id })
                            .Distinct();

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ItemByName(string searchLine)
        {
            var Item = _unitOfWork.Items.SingleOrDefault(i => i.Name == searchLine);
            if (Item == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Item", new { ItemId = Item.Id });
        }
        public ActionResult Item(Guid ItemId)
        {
            var Item = _unitOfWork.Items.Get(ItemId);
            if(Item==null)
            {
                return HttpNotFound();
            }
            return View(Item);
        }

        [HttpGet]
        public ActionResult Items(Guid TypeId, int page=1)
        {            
            var type = _unitOfWork.Types.Get(TypeId);
            if(type==null)
            {
                return HttpNotFound();
            }
            int pageSize = 10;
            var items = _unitOfWork.Items.GetPartOfItemsByType(page, pageSize, TypeId);
            ViewBag.TypeName = type.Name;
            ViewBag.TypeId = TypeId;
            ItemsPaginationViewModel viewModel = new ItemsPaginationViewModel()
            {
                Items = items,
                PageInfo = new PageInfo()
                {
                    Number = page,
                    Size = pageSize,
                    TotalSize = _unitOfWork.Items.Count(item => item.Type.Id == TypeId)
                }
            };
            return View(viewModel);
        }

        public FileContentResult GetImage(Guid ImageId)
        {
            var image = _unitOfWork.Images.Get(ImageId);
            if (image != null)
            {
                return File(image.Data, image.ImageType, image.Name);
            }
            else
            {
                return null;
            }
        }

        private string[] GetUrlsForSliderImages()
        {
            string[] paths = Directory.GetFileSystemEntries(HttpContext.Server.MapPath("/Content/Sources/img/slides"));
            for (int i = 0; i < paths.Length; i++)
            {
                int index = paths[i].LastIndexOf('\\');
                paths[i] = "/Content/Sources/img/slides" + paths[i].Substring(index, paths[i].Length-index);
            }
            return paths;
        }

        [Authorize]
        public JsonResult NewComment(Guid ItemId, string Comment)
        {
            var id=Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(User.Identity);
            var user = _unitOfWork.Users.SingleOrDefault(u => u.Id == id);
            var Item = _unitOfWork.Items.Get(ItemId);
            if (user != null && Item != null)
            {
                var Com = new Comment()
                {
                    Id = Guid.NewGuid(),
                    Item = Item,
                    Date = DateTime.Now,
                    Value = Comment,
                    User = user
                };
                _unitOfWork.Comments.Add(Com);
                _unitOfWork.SaveChanges();
                return Json(new { date = DateTime.Now.ToLongDateString(), id = Com.Id, admin=User.IsInRole("admin") }, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public ActionResult About()
        {
            ViewBag.Message = "About Universal Catalog";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact";

            return View();
        }
    }
}