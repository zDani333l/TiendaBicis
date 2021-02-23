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
    public class CRUDCategoriaController : Controller
    {
        private dbTiendaOnlineBicicletasEntities db = new dbTiendaOnlineBicicletasEntities();

        // GET: CRUDCategoria
        public ActionResult Index()
        {
            return View(db.Tbl_Categoria.ToList());
        }

        // GET: CRUDCategoria/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Categoria tbl_Categoria = db.Tbl_Categoria.Find(id);
            if (tbl_Categoria == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Categoria);
        }

        // GET: CRUDCategoria/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CRUDCategoria/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoriaId,NombreCategoria,DescripcionCategoria,IsActive,IsDelete")] Tbl_Categoria tbl_Categoria)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_Categoria.Add(tbl_Categoria);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_Categoria);
        }

        // GET: CRUDCategoria/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Categoria tbl_Categoria = db.Tbl_Categoria.Find(id);
            if (tbl_Categoria == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Categoria);
        }

        // POST: CRUDCategoria/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoriaId,NombreCategoria,DescripcionCategoria,IsActive,IsDelete")] Tbl_Categoria tbl_Categoria)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Categoria).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_Categoria);
        }

        // GET: CRUDCategoria/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Categoria tbl_Categoria = db.Tbl_Categoria.Find(id);
            if (tbl_Categoria == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Categoria);
        }

        // POST: CRUDCategoria/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Categoria tbl_Categoria = db.Tbl_Categoria.Find(id);
            db.Tbl_Categoria.Remove(tbl_Categoria);
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
