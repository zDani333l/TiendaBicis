using Microsoft.Reporting.WebForms;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using TiendaWebBicicletas.DAL;
using TiendaWebBicicletas.Models;
using TiendaWebBicicletas.Reportes;
using TiendaWebBicicletas.Repository;

namespace TiendaWebBicicletas.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AdministradorController : Controller
    {

        // GET: Admin
        MyDataSet ds = new MyDataSet();
        public ActionResult Reportes()
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(900);
            reportViewer.Height = Unit.Percentage(900);

            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            SqlConnection conx = new SqlConnection(connectionString); SqlDataAdapter adp = new SqlDataAdapter("SELECT * FROM Tbl_Producto", conx);

            adp.Fill(ds, ds.Tbl_Producto.TableName);

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reportes\Report1.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MyDataSet", ds.Tables[0]));
            ViewBag.ReportViewer = reportViewer;


            var lista = new List<DetalleProductoViewModels> {
                new DetalleProductoViewModels {  NombreProducto = "nombre", Cantidad = 2 },
                new DetalleProductoViewModels { NombreProducto = "nombre1", Cantidad = 3} };

            return View(lista);
        }

        MyDataSet2 ss = new MyDataSet2();
        public ActionResult ReportesProovedor()
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(900);
            reportViewer.Height = Unit.Percentage(900);

            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            SqlConnection conx = new SqlConnection(connectionString); SqlDataAdapter adp = new SqlDataAdapter("SELECT * FROM Tbl_Proveedor", conx);

            adp.Fill(ss, ss.Tbl_Proveedor.TableName);

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reportes\Report2.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MyDataSet2", ss.Tables[0]));
            ViewBag.ReportViewer = reportViewer;
            return View();
        }

        MyDataSet3 aa = new MyDataSet3();
        public ActionResult ReportesUsuario()
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(900);
            reportViewer.Height = Unit.Percentage(900);

            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            SqlConnection conx = new SqlConnection(connectionString); SqlDataAdapter adp = new SqlDataAdapter("SELECT * FROM AspNetUsers", conx);

            adp.Fill(aa, aa.AspNetUsers.TableName);

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reportes\Report3.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MyDataSet3", aa.Tables[0]));
            ViewBag.ReportViewer = reportViewer;
            return View();
        }

        MyDataSet4 ee = new MyDataSet4();
        public ActionResult ReportesCategorias()
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(900);
            reportViewer.Height = Unit.Percentage(900);

            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            SqlConnection conx = new SqlConnection(connectionString); SqlDataAdapter adp = new SqlDataAdapter("SELECT * FROM Tbl_Categoria", conx);

            adp.Fill(ee, ee.Tbl_Categoria.TableName);

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reportes\Report4.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MyDataSet4", ee.Tables[0]));
            ViewBag.ReportViewer = reportViewer;
            return View();
        }
    }
}