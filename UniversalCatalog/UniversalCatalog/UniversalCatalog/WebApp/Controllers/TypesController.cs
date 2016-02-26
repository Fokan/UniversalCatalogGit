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
    public class TypesController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public TypesController()
        {
            _unitOfWork = new UnitOfWork();
        }

        public ActionResult Index()
        {
            return View(_unitOfWork.Types.GetAll().Where(t=>t.Id!=Guid.Empty).ToList().OrderBy(type=>type.Name));
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Entities.Type type = _unitOfWork.Types.Get(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        public ActionResult Create()
        {
            ViewBag.Types = _unitOfWork.Types.GetAll();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,ParentTypeId")] Models.Entities.Type type, HttpPostedFileBase File)
        {
            if(_unitOfWork.Types.Count(t=>t.Name==type.Name)!=0)
            {
                ModelState.AddModelError("", "Тип з іменем '"+type.Name+"' вже існує!");
            }
            if (File != null)
            {
                Image Image = new Image()
                {
                    Id = Guid.NewGuid(),
                    ImageType = File.ContentType,
                    Name = File.FileName,
                    Data = new byte[File.ContentLength]
                };
                File.InputStream.Read(Image.Data, 0, File.ContentLength);
                type.Image = Image;
            }
            if (ModelState.IsValid)
            {
                type.Id = Guid.NewGuid();
                
                _unitOfWork.Types.Add(type);

                _unitOfWork.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Types = _unitOfWork.Types.GetAll();
            return View(type);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Entities.Type type = _unitOfWork.Types.Get(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            var types = _unitOfWork.Types.Where(t => t.Id != id && (t.ParentTypeId == null ? true : (t.ParentTypeId == id ? false : true)));
            List<SelectListItem> sListItems = new List<SelectListItem>();
            foreach(var Type in types)
            {
                sListItems.Add(new SelectListItem() { Value = Type.Id.ToString(), Text = Type.Name, Selected = (Type.Id == type.ParentTypeId) });
            }
            ViewBag.ParentTypeId = sListItems;
            return View(type);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ParentTypeId")] Models.Entities.Type type, Guid? OldImageId, HttpPostedFileBase File)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Types.Update(type);
                _unitOfWork.SaveChanges();
                if (File != null)
                {
                    var t = _unitOfWork.Types.Get(type.Id);
                    if (OldImageId == null)
                    {
                        Image Image = new Image()
                        {
                            Id = Guid.NewGuid(),
                            ImageType = File.ContentType,
                            Name = File.FileName,
                            Data = new byte[File.ContentLength]
                        };
                        File.InputStream.Read(Image.Data, 0, File.ContentLength);
                        _unitOfWork.Images.Add(Image);
                        _unitOfWork.SaveChanges();
                        type.Image = Image;
                        _unitOfWork.Types.Update(type);
                        _unitOfWork.SaveChanges();
                    }
                    else
                    {
                        var oldImage = _unitOfWork.Images.Get(OldImageId);
                        oldImage.ImageType = File.ContentType;
                        oldImage.Name = File.FileName;
                        oldImage.Data = new byte[File.ContentLength];
                        File.InputStream.Read(oldImage.Data, 0, File.ContentLength);
                        _unitOfWork.Images.Update(oldImage);
                        _unitOfWork.SaveChanges();
                    }
                }
                return RedirectToAction("Index");
            }
            return View(type);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Entities.Type type = _unitOfWork.Types.Get(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Models.Entities.Type type = _unitOfWork.Types.Get(id);
            if (_unitOfWork.Types.IsThisTypeAsParent(id))
            {
                ModelState.AddModelError("", "You can not remove this type because he has descendant types");
            }
            if(type.Items.Count!=0)
            {
                ModelState.AddModelError("", "You can not remove this type. Exist products of this type.");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Types.Remove(type);
                _unitOfWork.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(type);
        }

        [HttpGet]
        public ActionResult RemoveProperty(Guid TypeId, Guid PropertyId)
        {
            _unitOfWork.Types.Get(TypeId).Properties.Remove(_unitOfWork.Properties.Get(PropertyId));
            _unitOfWork.SaveChanges();
            return RedirectToAction("Details", new { Id = TypeId });
        }

        [HttpGet]
        public ActionResult ChangeConnectionBetweenTypeAndProperties(Guid TypeId)
        {
            ViewBag.TypeId = TypeId;
            var type = _unitOfWork.Types.Get(TypeId);
            ViewBag.TypeName = type.Name;
            var properties = _unitOfWork.Properties.GetAll().Select(p => new { Property = p, IsChecked = type.Properties.Any(pr => pr.Id == p.Id) }).ToDictionary(x => x.Property, x => x.IsChecked);
            return View(properties);
        }

        [HttpPost]
        public void ChangeConnectionBetweenTypeAndProperties(Guid TypeId, Guid PropertyId, bool Checked)
        {
            if (Checked)
                _unitOfWork.Types.Get(TypeId).Properties.Add(_unitOfWork.Properties.Get(PropertyId));
            else
                _unitOfWork.Types.Get(TypeId).Properties.Remove(_unitOfWork.Properties.Get(PropertyId));
            _unitOfWork.SaveChanges();
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
