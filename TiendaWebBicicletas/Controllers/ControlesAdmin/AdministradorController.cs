using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaWebBicicletas.DAL;
using TiendaWebBicicletas.Repository;

namespace TiendaWebBicicletas.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AdministradorController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        // GET: Admin


        //CRUD SUBCATEGORIAS
        public ActionResult SubCategorias()

        {
            IEnumerable<Tbl_SubCategoria> subCat = _unitOfWork.GetRepositoryInstance<Tbl_SubCategoria>().GetRegistros();
            return View(subCat);
        }

        public ActionResult SubCategoriaAdd()
        {
            ViewBag.CategoriaId = new SelectList(_unitOfWork.GetRepositoryInstance<Tbl_Categoria>().GetRegistros(), "CategoriaId", "NombreCategoria");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubCategoriaAdd([Bind(Include = "IdSubCategoria,NombreSubCategoria,DescripcionSubCategoria,CategoriaId")] Tbl_SubCategoria model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.GetRepositoryInstance<Tbl_SubCategoria>().Agregar(model);
                return RedirectToAction("SubCategorias");
                
            }
            ViewBag.CategoriaId = new SelectList(_unitOfWork.GetRepositoryInstance<Tbl_Categoria>().GetRegistros(), "CategoriaId", "NombreCategoria", model.CategoriaId);
            return View(model);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("SubCategorias");
            }
            Tbl_SubCategoria tbl_SubCategoria = _unitOfWork.GetRepositoryInstance<Tbl_SubCategoria>().GetId((int)id);

            if (tbl_SubCategoria == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoriaId = new SelectList(_unitOfWork.GetRepositoryInstance<Tbl_Categoria>().GetRegistros(), "CategoriaId", "NombreCategoria", tbl_SubCategoria.CategoriaId);
            return View(tbl_SubCategoria);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdSubCategoria,NombreSubCategoria,DescripcionSubCategoria,CategoriaId")] Tbl_SubCategoria tbl_SubCategoria)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.GetRepositoryInstance<Tbl_SubCategoria>().Actualizar(tbl_SubCategoria);
                return RedirectToAction("SubCategorias");
            }
            ViewBag.CategoriaId = new SelectList(_unitOfWork.GetRepositoryInstance<Tbl_Categoria>().GetRegistros(), "CategoriaId", "NombreCategoria", tbl_SubCategoria.CategoriaId);
            return View(tbl_SubCategoria);
        }
        public ActionResult Reportes()
        {
            return View();
        }
    }
}