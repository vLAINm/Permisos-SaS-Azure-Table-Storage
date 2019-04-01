using ClienteTablaEquipos.Models;
using ClienteTablaEquipos.Repositories;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClienteTablaEquipos.Controllers
{
    public class EquiposController : Controller
    {
        RepositoryEquipos repo;

        public EquiposController()
        {
            this.repo = new RepositoryEquipos();
        }

        public ActionResult Index(String division)
        {
            if(division != null)
            {
                List<Equipo> equipos = this.repo.BuscarDivision(division);
                if(equipos == null)
                {
                    ViewBag.Mensaje = "No existen equipos en la base de datos en esa división";
                    return View();
                }
                return View(equipos);
            }
            return View();
        }

        public ActionResult Edit(int id, String division)
        {
            CloudTable tabla = this.repo.GetTable(division);

            TableOperation retrieveOperation = TableOperation.Retrieve<Equipo>(division, id.ToString(), null);
            TableResult retrieveResult = tabla.Execute(retrieveOperation);

            Equipo updateEntity = (Equipo)retrieveResult.Result;

            return View(updateEntity);
        }

        [HttpPost]
        public ActionResult Edit(int id, String division, String nombre, String ciudad, String entrenador, String estadio)
        {
            this.repo.ModificarEquipo(id, division, nombre, ciudad, entrenador, estadio);
            return RedirectToAction("Edit", new { id = id, division = division });
        }

        public ActionResult Delete(int id, String division)
        {
            this.repo.EliminarEquipo(id, division);
            return RedirectToAction("Index", new { division = division });
        }
    }
}