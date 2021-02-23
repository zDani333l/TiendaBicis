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
    public class CRUDSubCategoriaController : Controller
    {
        private dbTiendaOnlineBicicletasEntities db = new dbTiendaOnlineBicicletasEntities();

        // GET: CRUDSubCategoria
        public ActionResult Index()
        {
            var tbl_SubCategoria = db.Tbl_SubCategoria.Include(t => t.Tbl_Categoria);
            return View(tbl_SubCategoria.ToList());
        }

        // GET: CRUDSubCategoria/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_SubCategoria tbl_SubCategoria = db.Tbl_SubCategoria.Find(id);
            if (tbl_SubCategoria == null)
            {
                return HttpNotFound();
            }
            return View(tbl_SubCategoria);
        }

        // GET: CRUDSubCategoria/Create
        public ActionResult Create()
        {
            ViewBag.CategoriaId = new SelectList(db.Tbl_Categoria, "CategoriaId", "NombreCategoria");
            return View();
        }

        // POST: CRUDSubCategoria/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdSubCategoria,NombreSubCategoria,DescripcionSubCategoria,CategoriaId")] Tbl_SubCategoria tbl_SubCategoria)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_SubCategoria.Add(tbl_SubCategoria);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoriaId = new SelectList(db.Tbl_Categoria, "CategoriaId", "NombreCategoria", tbl_SubCategoria.CategoriaId);
            return View(tbl_SubCategoria);
        }

        // GET: CRUDSubCategoria/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_SubCategoria tbl_SubCategoria = db.Tbl_SubCategoria.Find(id);
            if (tbl_SubCategoria == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoriaId = new SelectList(db.Tbl_Categoria, "CategoriaId", "NombreCategoria", tbl_SubCategoria.CategoriaId);
            return View(tbl_SubCategoria);
        }

        // POST: CRUDSubCategoria/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdSubCategoria,NombreSubCategoria,DescripcionSubCategoria,CategoriaId")] Tbl_SubCategoria tbl_SubCategoria)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_SubCategoria).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoriaId = new SelectList(db.Tbl_Categoria, "CategoriaId", "NombreCategoria", tbl_SubCategoria.CategoriaId);
            return View(tbl_SubCategoria);
        }

        // GET: CRUDSubCategoria/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_SubCategoria tbl_SubCategoria = db.Tbl_SubCategoria.Find(id);
            if (tbl_SubCategoria == null)
            {
                return HttpNotFound();
            }
            return View(tbl_SubCategoria);
        }

        // POST: CRUDSubCategoria/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_SubCategoria tbl_SubCategoria = db.Tbl_SubCategoria.Find(id);
            db.Tbl_SubCategoria.Remove(tbl_SubCategoria);
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
