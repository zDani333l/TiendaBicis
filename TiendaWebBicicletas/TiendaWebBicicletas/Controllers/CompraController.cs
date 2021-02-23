using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaWebBicicletas.DAL;
using TiendaWebBicicletas.Models;
using TiendaWebBicicletas.Repository;

namespace TiendaWebBicicletas.Controllers
{
    
    public class CompraController : Controller
    {
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();

        [Authorize]
        public ActionResult ErrorStock(List<StockViewModels> list)
        {   
            foreach (var item in list)
            {
                ProcesoEliminarCarrito(item.idPro);
            }
            return View(list);
        }

        [Authorize]
        public ActionResult Checkout()
        {
            List<Articulo> cart = (List<Articulo>)Session["cart"];
            if (cart == null)
            {
                return RedirectToAction("ListaProductos","Home");
            }
            CheckoutViewModels model = new CheckoutViewModels { Productos = cart };
            //CARGAMOS EL TOTAL DE LA ORDEN
            model.ResTotalOrden();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Checkout(CheckoutViewModels model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            List<Articulo> cart = (List<Articulo>)Session["cart"];

            List<StockViewModels> sinStock = new List<StockViewModels>();

            //COMPROBAR QUE LAS LISTA DE PRODUCTOS DEL CARRITO TENGAN STOCK
            bool band = false;
            foreach (var item in cart)
            {
                var aux = _unitOfWork.GetRepositoryInstance<Tbl_Producto>().GetId(item.Producto.ProductoId);
                if (aux.Cantidad - item.Cantidad < 0)
                {
                    //HAY UN PRODUCTO QUE NO TIENE EL SUFICIENTE STOCK 
                    band = true;
                    sinStock.Add(new StockViewModels { nombrePro = item.Producto.NombreProducto,idPro = item.Producto.ProductoId });
                }
            }
            if(band == true)
            {
                return RedirectToAction("ErrorStock", new { list = sinStock});

            }
            //TODO OK CON EL STOCK DE LOS PRODUCTOS

            //CREAMOS EL CARRITO EN LA BD
            Tbl_Carro auxCarro = new Tbl_Carro { IdUsuario = User.Identity.GetUserId(), CarroId = _unitOfWork.GetRepositoryInstance<Tbl_Carro>().GetRegistros().Count()+1};
            _unitOfWork.GetRepositoryInstance<Tbl_Carro>().Agregar(auxCarro);

            //ASIGNAMOS LOS PRODUCTOS DEL CARRITO EN LA TABLA CarroProducto
            foreach (var item in cart)
            {
                //Agregamos un producto a la tabla carroProducto
                _unitOfWork.GetRepositoryInstance<Tbl_CarroProducto>().Agregar(new Tbl_CarroProducto { IdCarro = auxCarro.CarroId, ProductoId = item.Producto.ProductoId });

                //ACTUALIZAMOS EL STOCK DEL PRODUCTO
                //Traer el producto
                Tbl_Producto auxProducto = _unitOfWork.GetRepositoryInstance<Tbl_Producto>().GetId(item.Producto.ProductoId);
                //Editar el stock
                auxProducto.Cantidad = auxProducto.Cantidad - item.Cantidad;
                //Actualizar el stock en la tabla de la BD
                _unitOfWork.GetRepositoryInstance<Tbl_Producto>().Actualizar(auxProducto);
            }

            //Traer el registro Orden realizada de la tabla OrdenStatus 
            Tbl_OrdenStatus ordenStatus = _unitOfWork.GetRepositoryInstance<Tbl_OrdenStatus>().GetPorParametro(x => x.OrdenStatus == "Orden realizada");
            //Evaluar si existe este status en la base de datos
            if (ordenStatus == null)
            {
                //SI ES NULL ES PORQUE NO EXISTE ESTE STATUS ENTONCES AGREGAMOS ESTE STATUS A LA BASE DE DATOS
                _unitOfWork.GetRepositoryInstance<Tbl_OrdenStatus>().Agregar(new Tbl_OrdenStatus { OrdenStatus = "Orden realizada" });
                //VOLVEMOS A CONSULTAR LOS STATUS PARA TRAER ESTE STATUS
                ordenStatus = _unitOfWork.GetRepositoryInstance<Tbl_OrdenStatus>().GetPorParametro(x => x.OrdenStatus == "Orden realizada");
            }
            
            //CREAMOS LA ORDEN 
            Tbl_DetalleCompra orden = new Tbl_DetalleCompra {
                Ciudad = model.Ciudad,
                CodigoPostal = "",
                Direccion = model.Direccion,
                IdUsuario = User.Identity.GetUserId(),
                Monto = (decimal)model.TotalOrden,
                Pais = model.Pais,
                TipoPago = "Cualquiera",
                IdOrdenStatus = ordenStatus.OrdenStatusId,   
                IdCarrito = auxCarro.CarroId
            };
            //CARGAMOS LA ORDEN A LA BASE DE DATOS
            _unitOfWork.GetRepositoryInstance<Tbl_DetalleCompra>().Agregar(orden);

            //COMO SE HA CREADO EL PEDIDO VOLVEMOS A DEJAR NUESTRO CARRITO VACIO
            Session["cart"] = null;

            //PONER UN ALERT QUE SE REALIZO LA COMPRA CORRECTAMENTE


            _unitOfWork.SaveChanges();
            return RedirectToAction("Index","Home", new { compra = true});
        }
        public ActionResult Carrito()
        {
            List<Articulo> cart = (List<Articulo>)Session["cart"];
            if(cart == null)
            {
                //MOSTRAR ALERTA
                return RedirectToAction("ListaProductos","Home"); 
            }
            return View(cart);
        }
        public void ProcesoAgregarCarrito(int productId)
        {
            if (Session["cart"] == null)
            {
                List<Articulo> cart = new List<Articulo>();
                var producto = _unitOfWork.GetRepositoryInstance<Tbl_Producto>().GetId(productId);
                var img = _unitOfWork.GetRepositoryInstance<Tbl_imagen>().GetListaParametro(x => x.IdProducto == productId).ToList();
                foreach (var it in img)
                {
                    producto.Tbl_imagen.Add(it);
                }

                cart.Add(new Articulo()
                {
                    Producto = producto,
                    Cantidad = 1
                });
                Session["cart"] = cart;
            }
            else
            {
                List<Articulo> cart = (List<Articulo>)Session["cart"];
                var count = cart.Count();
                var producto = _unitOfWork.GetRepositoryInstance<Tbl_Producto>().GetId(productId);
                var img = _unitOfWork.GetRepositoryInstance<Tbl_imagen>().GetListaParametro(x => x.IdProducto == productId).ToList();
                foreach (var it in img)
                {
                    producto.Tbl_imagen.Add(it);
                }
                for (int i = 0; i < count; i++)
                {
                    if (cart[i].Producto.ProductoId == productId)
                    {
                        int cant = cart[i].Cantidad;
                        cart.Remove(cart[i]);
                        cart.Add(new Articulo()
                        {
                            Producto = producto,
                            Cantidad = cant + 1
                        });
                        break;
                    }
                    else
                    {
                        var prd = cart.Where(x => x.Producto.ProductoId == productId).SingleOrDefault();
                        if (prd == null)
                        {
                            cart.Add(new Articulo()
                            {
                                Producto = producto,
                                Cantidad = 1
                            });
                        }
                    }
                }
                Session["cart"] = cart;
            }
        }
        public void ProcesoEliminarCarrito(int productId)
        {
            List<Articulo> cart = (List<Articulo>)Session["cart"];
            foreach (var item in cart)
            {
                if (item.Producto.ProductoId == productId)
                {
                    cart.Remove(item);
                    break;
                }
            }
            Session["cart"] = cart;
        }
        public ActionResult AgregarCarrito(int productId, string search, int pag = 1)
        {
            ProcesoAgregarCarrito(productId);
            return RedirectToAction("ListaProductos", "Home", new { pagina = pag, buscar = search });
        }
        public ActionResult EliminarCarrito(int productId, string search, int pag = 1)
        {
            ViewBag.buscar = search;
            ViewBag.pag = pag;
            ProcesoEliminarCarrito(productId);
            return RedirectToAction("ListaProductos", "Home", new { pagina = pag, buscar = search });
        }
    }
}