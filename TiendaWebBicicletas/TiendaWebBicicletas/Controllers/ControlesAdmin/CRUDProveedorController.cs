using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TiendaWebBicicletas.DAL;

namespace TiendaWebBicicletas.Controllers.ControlesAdmin
{
    [Authorize(Roles = "Admin")]
    public class CRUDProveedorController : Controller
    {
        private dbTiendaOnlineBicicletasEntities db = new dbTiendaOnlineBicicletasEntities();

        // GET: CRUDProveedor
        public ActionResult Index()
        {
            return View(db.Tbl_Proveedor.ToList());
        }

        // GET: CRUDProveedor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Proveedor tbl_Proveedor = db.Tbl_Proveedor.Find(id);
            if (tbl_Proveedor == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Proveedor);
        }

        // GET: CRUDProveedor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CRUDProveedor/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProveedorId,nombre,direccion")] Tbl_Proveedor tbl_Proveedor)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_Proveedor.Add(tbl_Proveedor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_Proveedor);
        }

        // GET: CRUDProveedor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Proveedor tbl_Proveedor = db.Tbl_Proveedor.Find(id);
            if (tbl_Proveedor == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Proveedor);
        }

        // POST: CRUDProveedor/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProveedorId,nombre,direccion")] Tbl_Proveedor tbl_Proveedor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Proveedor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_Proveedor);
        }

        // GET: CRUDProveedor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Proveedor tbl_Proveedor = db.Tbl_Proveedor.Find(id);
            if (tbl_Proveedor == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Proveedor);
        }

        // POST: CRUDProveedor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Proveedor tbl_Proveedor = db.Tbl_Proveedor.Find(id);
            db.Tbl_Proveedor.Remove(tbl_Proveedor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
