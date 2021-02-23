using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TiendaWebBicicletas.DAL;
using TiendaWebBicicletas.Repository;

namespace TiendaWebBicicletas.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CRUDProductoController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        private dbTiendaOnlineBicicletasEntities db = new dbTiendaOnlineBicicletasEntities();

        public ActionResult EliminarFoto(int idFoto=-999, int idProducto=-999)
        {
            if(idFoto == -999)
            {
                return HttpNotFound("Details"); 
            }
            var img = _unitOfWork.GetRepositoryInstance<Tbl_imagen>().GetId(idFoto);
            _unitOfWork.GetRepositoryInstance<Tbl_imagen>().Eliminar(img);
            _unitOfWork.SaveChanges();
            var file = Path.Combine(HttpContext.Server.MapPath("/ImagenesProductos/"), img.NombreImagen);
            if (System.IO.File.Exists(file))
                System.IO.File.Delete(file);

            if (idProducto == -999)
            {
                return RedirectToAction("Index");
            }
                return RedirectToAction("Details", new { id = idProducto });
            

        }

        public ActionResult ModificarFoto(int? id)
        {
            if(id != null)
            {
                var img = _unitOfWork.GetRepositoryInstance<Tbl_imagen>().GetId((int)id);
                return View(img);

            }
            return RedirectToAction("Index");
            
        }
        [HttpPost, ActionName("ModificarFoto")]
        public ActionResult MoificarImg(int? id)
        {
            if(id != null)
            {
                HttpPostedFileBase archivos = Request.Files[0];
                if (archivos.ContentLength > 0)
                {
                    var ruta = Request.MapPath("~/ImagenesProductos/");
                    string ficheroNmbre = _unitOfWork.GetRepositoryInstance<Tbl_imagen>().GetId((int)id).NombreImagen;
                    var file = Path.Combine(HttpContext.Server.MapPath("/ImagenesProductos/"), ficheroNmbre);
                    if (System.IO.File.Exists(file))
                        System.IO.File.Delete(file);

                    string dirImg = ruta + ficheroNmbre;
                    archivos.SaveAs(dirImg);
                }

                var img = _unitOfWork.GetRepositoryInstance<Tbl_imagen>().GetId((int)id);
                int Idprod = _unitOfWork.GetRepositoryInstance<Tbl_Producto>().GetId((int)img.IdProducto).ProductoId;
                return RedirectToAction("Edit", new { id = Idprod });
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult SubCaterogia(int idCategoria)
        {
            List<ElementJsonIntKey> lst = new List<ElementJsonIntKey>();
                lst = (from d in db.Tbl_SubCategoria
                       where d.CategoriaId == idCategoria
                       select new ElementJsonIntKey
                       {
                           Value = d.IdSubCategoria,
                           Text = d.NombreSubCategoria
                       }
                       ).ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public class ElementJsonIntKey
        {
            public int Value { get; set; }
            public string Text { get; set; }
        }


        // GET: Tbl_Producto
        public ActionResult Index()
        {
    
            var tbl_Producto = db.Tbl_Producto.Include(t => t.Tbl_Categoria).Include(t => t.Tbl_SubCategoria).Include(t => t.Tbl_Proveedor).Include(t => t.Tbl_imagen);
            foreach (var item in tbl_Producto)
            {
                var img = _unitOfWork.GetRepositoryInstance<Tbl_imagen>().GetListaParametro(x => x.IdProducto == item.ProductoId).FirstOrDefault();
                item.Tbl_imagen.Add(img);
            }
            
            return View(tbl_Producto.ToList());
        }

        // GET: Tbl_Producto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Producto tbl_Producto = db.Tbl_Producto.Find(id);
            if (tbl_Producto == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Producto);
        }

        // GET: Tbl_Producto/Create
        public ActionResult Create()
        {
            ViewBag.IdCategoria = new SelectList(db.Tbl_Categoria, "CategoriaId", "NombreCategoria");
            ViewBag.IdSubCategoria = new SelectList(db.Tbl_SubCategoria, "IdSubCategoria", "NombreSubCategoria");
            ViewBag.ProveedorId = new SelectList(db.Tbl_Proveedor, "ProveedorId", "nombre");
            return View();
        }

        // POST: Tbl_Producto/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductoId,NombreProducto,Descripcion,Cantidad,Precio,Talla,Color,ProveedorId,IdSubCategoria,IdCategoria")] Tbl_Producto tbl_Producto)
        {
            if (ModelState.IsValid)
            {
                //DEFINIR FECHA CREACION
                tbl_Producto.FechaCreacion = DateTime.Now;

                _unitOfWork.GetRepositoryInstance<Tbl_Producto>().Agregar(tbl_Producto);
                _unitOfWork.SaveChanges();
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase archivos = Request.Files[i];
                    if (archivos.ContentLength > 0)
                    {
                        var ruta = Request.MapPath("~/ImagenesProductos/");
                        // Si el directorio no existe, crearlo
                        if (!Directory.Exists(ruta))
                        {
                            Directory.CreateDirectory(ruta);
                        }
                        Random r = new Random();
                        int numero = r.Next(5, 10000000);

                        string ficheroNmbre = Path.GetFileName(archivos.FileName);
                        string extension = Path.GetExtension(ficheroNmbre);
                        ficheroNmbre = ficheroNmbre.Substring(ficheroNmbre.LastIndexOf(".") + 1).ToLower();
                        ficheroNmbre = "Imagen_" + numero + extension;

                        string dirImg = ruta + ficheroNmbre;
                        archivos.SaveAs(dirImg);

                        Tbl_imagen imagenesProducto = new Tbl_imagen { NombreImagen = ficheroNmbre, DirImagen = dirImg, IdProducto = tbl_Producto.ProductoId };
                        _unitOfWork.GetRepositoryInstance<Tbl_imagen>().Agregar(imagenesProducto);
                        _unitOfWork.SaveChanges();

                    }
                }

                return RedirectToAction("Index");
            }

            ViewBag.IdCategoria = new SelectList(db.Tbl_Categoria, "CategoriaId", "NombreCategoria", tbl_Producto.IdCategoria);
            ViewBag.IdSubCategoria = new SelectList(db.Tbl_SubCategoria, "IdSubCategoria", "NombreSubCategoria", tbl_Producto.IdSubCategoria);
            ViewBag.ProveedorId = new SelectList(db.Tbl_Proveedor, "ProveedorId", "nombre", tbl_Producto.ProveedorId);
            return View(tbl_Producto);
        }

        // GET: Tbl_Producto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Producto tbl_Producto = db.Tbl_Producto.Find(id);
            if (tbl_Producto == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCategoria = new SelectList(db.Tbl_Categoria, "CategoriaId", "NombreCategoria", tbl_Producto.IdCategoria);
            ViewBag.IdSubCategoria = new SelectList(db.Tbl_SubCategoria, "IdSubCategoria", "NombreSubCategoria", tbl_Producto.IdSubCategoria);
            ViewBag.ProveedorId = new SelectList(db.Tbl_Proveedor, "ProveedorId", "nombre", tbl_Producto.ProveedorId);
            return View(tbl_Producto);
        }

        // POST: Tbl_Producto/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductoId,NombreProducto,Descripcion,Cantidad,Precio,Talla,Color,ProveedorId,IdSubCategoria,IdCategoria")] Tbl_Producto tbl_Producto)
        {
            if (ModelState.IsValid)
            {
                //DEFINIR FECHA DE MODIFICACION
                tbl_Producto.FechaModificacion = DateTime.Now;
                db.Entry(tbl_Producto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCategoria = new SelectList(db.Tbl_Categoria, "CategoriaId", "NombreCategoria", tbl_Producto.IdCategoria);
            ViewBag.IdSubCategoria = new SelectList(db.Tbl_SubCategoria, "IdSubCategoria", "NombreSubCategoria", tbl_Producto.IdSubCategoria);
            ViewBag.ProveedorId = new SelectList(db.Tbl_Proveedor, "ProveedorId", "nombre", tbl_Producto.ProveedorId);
            return View(tbl_Producto);
        }

        // GET: Tbl_Producto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Producto tbl_Producto = db.Tbl_Producto.Find(id);
            if (tbl_Producto == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Producto);
        }

        // POST: Tbl_Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var prod = _unitOfWork.GetRepositoryInstance<Tbl_Producto>().GetId(id);
            
            var imgs = _unitOfWork.GetRepositoryInstance<Tbl_imagen>().GetListaParametro(x => x.IdProducto == prod.ProductoId);
            foreach (var item in imgs)
            {
                _unitOfWork.GetRepositoryInstance<Tbl_imagen>().Eliminar(item);
                _unitOfWork.SaveChanges();
                var file = Path.Combine(HttpContext.Server.MapPath("/ImagenesProductos/"), item.NombreImagen);
                if (System.IO.File.Exists(file))
                    System.IO.File.Delete(file);
            }
            _unitOfWork.GetRepositoryInstance<Tbl_Producto>().Eliminar(prod);
            _unitOfWork.SaveChanges();
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
