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
    [Authorize(Roles = "admin, moderator")]
    public class PropertysController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public PropertysController()
        {
            _unitOfWork = new UnitOfWork();
        }

        // GET: Properties
        public ActionResult Index()
        {
            return View(_unitOfWork.Properties.GetAll().OrderBy(property => property.Name));
        }

        // GET: Properties/Details/5
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

        // GET: Properties/Create
        public ActionResult Create(Guid? TypeId)
        {
            ViewBag.TypeId = TypeId;
            return View();
        }

        // POST: Properties/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

            return View(property);
        }

        // GET: Properties/Edit/5
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

        // POST: Properties/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Properties/Delete/5
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

        // POST: Properties/Delete/5
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
