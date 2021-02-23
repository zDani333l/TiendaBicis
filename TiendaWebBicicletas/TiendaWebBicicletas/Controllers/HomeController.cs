using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaWebBicicletas.DAL;
using TiendaWebBicicletas.Models;
using TiendaWebBicicletas.Repository;

namespace TiendaWebBicicletas.Controllers
{
    public class HomeController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        public ActionResult DetalleProducto(string buscar, int pagina = 1, int check = 0, int id = -1)
        {
            ViewBag.buscar = buscar;
            ViewBag.pag = pagina;
            ViewBag.check = check;

            if(id == -1)
            {
                return RedirectToAction("Index");
            }
            else
            {
            //CONSULTAS A LA BASE DE DATOS
            var auxProducto = _unitOfWork.GetRepositoryInstance<Tbl_Producto>().GetId(id);
            var SubCat = _unitOfWork.GetRepositoryInstance<Tbl_SubCategoria>().GetId(auxProducto.Tbl_SubCategoria.IdSubCategoria);
            var auxImgs = _unitOfWork.GetRepositoryInstance<Tbl_imagen>().GetListaParametro(x => x.IdProducto == auxProducto.ProductoId).ToList();
            var Cat = _unitOfWork.GetRepositoryInstance<Tbl_Categoria>().GetId((int)SubCat.CategoriaId);
            IEnumerable<Tbl_Producto> productosRelacionados = _unitOfWork.GetRepositoryInstance<Tbl_Producto>().GetListaParametro(x => x.IdSubCategoria == auxProducto.IdSubCategoria).ToList();

            //FILTRAR LOS PRODUCTOS RELACIONADOS QUE TIENEN STOCK Y NO SON EL MISMO QUE ESTAMOS VIENDO
            productosRelacionados = productosRelacionados.Where(x => x.Cantidad > 0 && x.ProductoId != id);

            //CARAGAR IMAGENES RELACIONADAS AL PRODUCTO RELACIONADO
            var auxP = productosRelacionados.ToList();
            foreach (var item in auxP)
            {
                var img = _unitOfWork.GetRepositoryInstance<Tbl_imagen>().GetListaParametro(x => x.IdProducto == item.ProductoId).ToList();
                foreach (var it in img)
                {
                    item.Tbl_imagen.Add(it);
                }
            }
            productosRelacionados = auxP;

            //FILTRAR LOS PRODUCTOS RELACIONADOS QUE NO TIENEN LAS IMAGENES COMPLETAS
            productosRelacionados = productosRelacionados.Where(x => x.Tbl_imagen.Count >= 1);

            //CARAGAR SUBCATEGORIA RELACIONADA AL PRODUCTO RELACIONADO
            var auxC = productosRelacionados.ToList();
            foreach (var item in auxC)
            {
                var auxCat = _unitOfWork.GetRepositoryInstance<Tbl_SubCategoria>().GetPorParametro(x => x.IdSubCategoria == item.IdSubCategoria);
                item.Tbl_SubCategoria = auxCat;
            }
            productosRelacionados = auxC;


            double auxTach = (double)auxProducto.Precio;
            //CARGAR DATOS AL MODELO PARA LA VISTA
            DetalleProductoViewModels producto = new DetalleProductoViewModels
            {
                Cantidad = (int)auxProducto.Cantidad,
                Color = auxProducto.Color,
                NombreProducto = auxProducto.NombreProducto,
                Precio = (double)auxProducto.Precio,
                Talla = auxProducto.Talla,
                Descripcion = auxProducto.Descripcion,
                ProductoId = auxProducto.ProductoId,
                SubCategoria = SubCat.NombreSubCategoria,
                Categoria = Cat.NombreCategoria,
                Imagenes = auxImgs,
                PrecioTachado = (double)auxProducto.Precio+((double)auxProducto.Precio * (1 / 2)),
                ProductosRelacionados = _unitOfWork.GetRepositoryInstance<Tbl_Producto>().GetRegistros().ToList()
            };
                //INICIAR VISTA CON EL MODELO
                return View(producto);
            }
        }
        public ActionResult ListaProductos(string buscar, int pagina = 1)
        {
            ViewBag.buscar = buscar;
            ViewBag.pag = pagina;

            int _RegistrosPorPagina = 6;
            PaginadorGenerico<Tbl_Producto> _PaginadorProductos;

            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@search",buscar??(object)DBNull.Value)
            };

            // Almacenar la consulta de la tabla (objeto) 
            IEnumerable<Tbl_Producto> _productos = _unitOfWork.GetRepositoryInstance<Tbl_Producto>().GetResuladoSqlProcedure("GetBySearch @search", param).ToList();

            //FILTRAR LOS PRODUCTOS QUE TIENEN STOCK
            _productos = _productos.Where(x => x.Cantidad >= 1);

            //CARAGAR IMAGENES RELACIONADAS AL PRODUCTO
            var auxProductos = _productos.ToList();
            foreach (var item in auxProductos)
            {
                var img = _unitOfWork.GetRepositoryInstance<Tbl_imagen>().GetListaParametro(x => x.IdProducto == item.ProductoId).ToList();
                foreach (var it in img)
                {
                    item.Tbl_imagen.Add(it);
                }
            }
            _productos = auxProductos;

            //FILTRAR LOS PRODUCTOS QUE NO TIENEN LA CANTIDAD MINIMA DE IMAGENES
            _productos = _productos.Where(x => x.Tbl_imagen.Count >= 1);

            //CARAGAR LA SUBCATEGORIA RELACIONADA A LOS PRODUCTOS
            var auxC = _productos.ToList();
            foreach (var item in auxC)
            {
                item.Tbl_SubCategoria = _unitOfWork.GetRepositoryInstance<Tbl_SubCategoria>().GetPorParametro(x => x.IdSubCategoria == item.IdSubCategoria);
            }
            foreach (var item in auxC)
            {
                item.Tbl_Categoria = _unitOfWork.GetRepositoryInstance<Tbl_Categoria>().GetPorParametro(x => x.CategoriaId == item.IdCategoria);
            }

            _productos = auxC;


            // Número total de registros de la tabla 
            int _TotalRegistros = _productos.Count();

            // Obtenemos la 'páginacion de registros' de la tabla
            _productos = _productos.OrderBy(x => x.NombreProducto)
                                                 .Skip((pagina - 1) * _RegistrosPorPagina)
                                                 .Take(_RegistrosPorPagina)
                                                 .ToList();
            // Número total de páginas de la tabla
            var _TotalPaginas = (int)Math.Ceiling((double)_TotalRegistros / _RegistrosPorPagina);

            _PaginadorProductos = new PaginadorGenerico<Tbl_Producto>
            {
                RegistrosPorPagina = _RegistrosPorPagina,
                TotalRegistros = _TotalRegistros,
                TotalPaginas = _TotalPaginas,
                PaginaActual = pagina,
                Resultado = _productos

            };

            return View(_PaginadorProductos);
        }

        public ActionResult Index(bool? compra)
        {
            String NombreUser = User.Identity.Name;
            SqlParameter[] param = new SqlParameter[]{
                    new SqlParameter("@NombreUser",NombreUser??(object)DBNull.Value)
                    };

            AspNetRoles UserRol = _unitOfWork.GetRepositoryInstance<AspNetRoles>().GetResuladoSqlProcedure("GetIdRol @NombreUser", param).FirstOrDefault();

            if(UserRol != null)
            {
                if (UserRol.Name.Equals("Admin"))
                {
                    return RedirectToAction("Reportes", "Administrador");
                }
            }
            if (compra != null)
            {
                ViewBag.Script = compra;
            }
            else
            {
                ViewBag.Script = false;
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}