using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApp.Models.Entities;
using WebApp.Models.UnitOfWork;

namespace WebApp.Controllers
{
    [Authorize(Roles = "admin")]
    public class PropertysController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public PropertysController()
        {
            _unitOfWork = new UnitOfWork();
        }

        public ActionResult Index()
        {
            return View(_unitOfWork.Properties.GetAll().OrderBy(property => property.Name));
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Property property = _unitOfWork.Properties.Get(id);
            if (property == null)
            {
                return HttpNotFound();
            }
            return View(property);
        }

        public ActionResult Create(Guid? TypeId)
        {
            ViewBag.TypeId = TypeId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Property property, Guid? TypeId)
        {
            if (_unitOfWork.Properties.Count(p => p.Name == property.Name) != 0)
                ModelState.AddModelError("", "Властивість з таким іменем вже існує");

            if (ModelState.IsValid)
            {
                property.Id = Guid.NewGuid();
                _unitOfWork.Properties.Add(property);
                if (TypeId != null)
                    _unitOfWork.Types.Get(TypeId).Properties.Add(property);
                _unitOfWork.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TypeId = TypeId;
            return View(property);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Property property = _unitOfWork.Properties.Get(id);
            if (property == null)
            {
                return HttpNotFound();
            }
            return View(property);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Property property)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Properties.Update(property);
                _unitOfWork.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(property);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Property property = _unitOfWork.Properties.Get(id);
            if (property == null)
            {
                return HttpNotFound();
            }
            return View(property);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Property property = _unitOfWork.Properties.Get(id);
            if (property.Types.Count != 0)
                ModelState.AddModelError("", "You cannot delete this property, because some type use it");
            if (property.Descriptions.Count != 0)
                ModelState.AddModelError("", "You cannot delete this property, because some product have description for this property");
            if (ModelState.IsValid)
            {
                _unitOfWork.Properties.Remove(property);
                _unitOfWork.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(property);
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
