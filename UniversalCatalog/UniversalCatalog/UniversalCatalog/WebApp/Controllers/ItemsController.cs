using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.Models.Entities;
using WebApp.Models.UnitOfWork;

namespace WebApp.Controllers
{
    [Authorize(Roles = "admin, moderator")]
    public class ItemsController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public ItemsController()
        {
            _unitOfWork = new UnitOfWork();
        }

        public ActionResult Index()
        {
            return View(_unitOfWork.Items.GetAll());
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = _unitOfWork.Items.Get(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        public FileContentResult GetImage(Guid ImageId)
        {
            var image = _unitOfWork.Images.Get(ImageId);
            if (image!=null)
            {
                return File(image.Data, image.ImageType, image.Name);
            }
            else
            {
                return null;
            }
        }

        public JsonResult GetPropertiesByType(Guid TypeId)
        {
            var properties = _unitOfWork.Types.Get(TypeId).Properties.OrderBy(p=>p.Name).Select(x => new
            {
                Id = x.Id,
                Name = x.Name
            });
            return Json(properties, JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<SelectListItem> SelectListOfTypes()
        {
            List<SelectListItem> types = new List<SelectListItem>();
            foreach (var type in _unitOfWork.Types.GetAll().Where(type => type.Id != Guid.Empty))
            {
                types.Add(new SelectListItem() { Value = type.Id.ToString(), Text = type.Name });
            }
            return types;
        }

        public ActionResult Create()
        {
            ViewBag.TypeId = SelectListOfTypes();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ItemViewModel itemVM)
        {
            var files=Request.Files;
            if (_unitOfWork.Items.Count(i => i.Name == itemVM.Name) != 0)
            {
                ModelState.AddModelError("", "Product with that name already exists");
            }
            string[] types = { "image/jpeg", "image/png", "image/gif" };
            if (itemVM.Images.Count(file => (types.Contains(file.ContentType))) != itemVM.Images.Count())
            {
                ModelState.AddModelError("", "Allowed file extensions: " + "\"image/jpeg\", \"image/png\", \"image/gif\"");
            }
            int index = 0;
            //foreach(var description in itemVM.props)
            //{
            //    if (_unitOfWork.Descriptions.Count(desc => (desc.Value == description) && (desc.Property.Id == itemVM.IdsOfProps[index])) != 0)
            //        ModelState.AddModelError("", " Descriptions error ");

            //}
            if (ModelState.IsValid)
            {
                Item item = new Item()
                {
                    Id = Guid.NewGuid(),
                    Name = itemVM.Name,
                    TypeId = itemVM.TypeId,
                    Price = itemVM.Price
                };
                int i = 0;
                foreach (var description in itemVM.props)
                {
                    item.Descriptions.Add(new Description()
                    {
                        Id = Guid.NewGuid(),
                        Property = _unitOfWork.Properties.Get(itemVM.IdsOfProps[i]),
                        Value = description
                    });
                    i++;
                }
                foreach(var file in itemVM.Images)
                {
                    Image Image = new Image()
                    {
                        Id = Guid.NewGuid(),
                        ImageType = file.ContentType,
                        Name = file.FileName,
                        Data = new byte[file.ContentLength]
                    };
                    file.InputStream.Read(Image.Data, 0, file.ContentLength);
                    item.Images.Add(Image);
                }
                _unitOfWork.Items.Add(item);
                _unitOfWork.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TypeId = SelectListOfTypes();
            return View(itemVM);
        }

        // GET: Items/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = _unitOfWork.Items.Get(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        public void EditDescriptions(List<string[]> Descriptions, Guid ItemId)
        {
            if (Descriptions == null)
                return;
            var Item = _unitOfWork.Items.Get(ItemId);
            foreach(var newDescription in Descriptions)
            {
                var Description = Item.Descriptions.SingleOrDefault(desc => desc.Id.ToString() == newDescription[0]);
                if(Description!=null)
                {
                    if (Description.Value != newDescription[1])
                        Description.Value = newDescription[1];
                }
            }
            _unitOfWork.SaveChanges();
        }

        public JsonResult AddImage(Guid ItemId, HttpPostedFileBase File)
        {
            Image Image = new Image()
            {
                Id = Guid.NewGuid(),
                ImageType = File.ContentType,
                Name = File.FileName,
                Data = new byte[File.ContentLength]
            };
            File.InputStream.Read(Image.Data, 0, File.ContentLength);
            _unitOfWork.Items.Get(ItemId).Images.Add(Image);
            _unitOfWork.SaveChanges();
            return Json(new { id = Image.Id }, JsonRequestBehavior.AllowGet);
        }

        public void DeleteImage(Guid ItemId, Guid ImageId)
        {
            var Image = _unitOfWork.Images.Get(ImageId);
            _unitOfWork.Items.Get(ItemId).Images.Remove(Image);
            _unitOfWork.Images.Remove(Image);
            _unitOfWork.SaveChanges();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,TypeId,Price")] Item item)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Items.Update(item);
                _unitOfWork.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(item);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = _unitOfWork.Items.Get(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Item item = _unitOfWork.Items.Get(id);
            var Descriptions = item.Descriptions.ToList();
            foreach (var description in Descriptions)
            {
                _unitOfWork.Descriptions.Remove(description);
            }
            var Images = item.Images.ToList();
            foreach (var image in Images)
            {
                _unitOfWork.Images.Remove(image);
            }
            _unitOfWork.Items.Remove(item);
            _unitOfWork.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
