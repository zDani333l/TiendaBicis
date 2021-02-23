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
    public class EliminarEditarDetalleCompraController : Controller
    {
        private dbTiendaOnlineBicicletasEntities db = new dbTiendaOnlineBicicletasEntities();

        // GET: EliminarEditarDetalleCompra
        public ActionResult Index()
        {
            var tbl_DetalleCompra = db.Tbl_DetalleCompra.Include(t => t.AspNetUsers).Include(t => t.Tbl_Carro).Include(t => t.Tbl_OrdenStatus);
            return View(tbl_DetalleCompra.ToList());
        }

        // GET: EliminarEditarDetalleCompra/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_DetalleCompra tbl_DetalleCompra = db.Tbl_DetalleCompra.Find(id);
            if (tbl_DetalleCompra == null)
            {
                return HttpNotFound();
            }
            return View(tbl_DetalleCompra);
        }

        // GET: EliminarEditarDetalleCompra/Create
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Create()
        {
            ViewBag.IdUsuario = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.IdCarrito = new SelectList(db.Tbl_Carro, "CarroId", "IdUsuario");
            ViewBag.IdOrdenStatus = new SelectList(db.Tbl_OrdenStatus, "OrdenStatusId", "OrdenStatus");
            return View();
        }

        // POST: EliminarEditarDetalleCompra/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DetalleCompraId,Direccion,Ciudad,Estado,Pais,CodigoPostal,Monto,TipoPago,IdUsuario,IdCarrito,IdOrdenStatus")] Tbl_DetalleCompra tbl_DetalleCompra)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_DetalleCompra.Add(tbl_DetalleCompra);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdUsuario = new SelectList(db.AspNetUsers, "Id", "Email", tbl_DetalleCompra.IdUsuario);
            ViewBag.IdCarrito = new SelectList(db.Tbl_Carro, "CarroId", "IdUsuario", tbl_DetalleCompra.IdCarrito);
            ViewBag.IdOrdenStatus = new SelectList(db.Tbl_OrdenStatus, "OrdenStatusId", "OrdenStatus", tbl_DetalleCompra.IdOrdenStatus);
            return View(tbl_DetalleCompra);
        }

        // GET: EliminarEditarDetalleCompra/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_DetalleCompra tbl_DetalleCompra = db.Tbl_DetalleCompra.Find(id);
            if (tbl_DetalleCompra == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdUsuario = new SelectList(db.AspNetUsers, "Id", "Email", tbl_DetalleCompra.IdUsuario);
            ViewBag.IdCarrito = new SelectList(db.Tbl_Carro, "CarroId", "IdUsuario", tbl_DetalleCompra.IdCarrito);
            ViewBag.IdOrdenStatus = new SelectList(db.Tbl_OrdenStatus, "OrdenStatusId", "OrdenStatus", tbl_DetalleCompra.IdOrdenStatus);
            return View(tbl_DetalleCompra);
        }

        // POST: EliminarEditarDetalleCompra/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DetalleCompraId,Direccion,Ciudad,Estado,Pais,CodigoPostal,Monto,TipoPago,IdUsuario,IdCarrito,IdOrdenStatus")] Tbl_DetalleCompra tbl_DetalleCompra)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_DetalleCompra).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdUsuario = new SelectList(db.AspNetUsers, "Id", "Email", tbl_DetalleCompra.IdUsuario);
            ViewBag.IdCarrito = new SelectList(db.Tbl_Carro, "CarroId", "IdUsuario", tbl_DetalleCompra.IdCarrito);
            ViewBag.IdOrdenStatus = new SelectList(db.Tbl_OrdenStatus, "OrdenStatusId", "OrdenStatus", tbl_DetalleCompra.IdOrdenStatus);
            return View(tbl_DetalleCompra);
        }

        // GET: EliminarEditarDetalleCompra/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_DetalleCompra tbl_DetalleCompra = db.Tbl_DetalleCompra.Find(id);
            if (tbl_DetalleCompra == null)
            {
                return HttpNotFound();
            }
            return View(tbl_DetalleCompra);
        }

        // POST: EliminarEditarDetalleCompra/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_DetalleCompra tbl_DetalleCompra = db.Tbl_DetalleCompra.Find(id);
            db.Tbl_DetalleCompra.Remove(tbl_DetalleCompra);
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
