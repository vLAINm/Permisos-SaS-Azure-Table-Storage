using ClienteTablaEquipos.Models;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;

namespace ClienteTablaEquipos.Repositories
{
    public class RepositoryEquipos
    {
        public String GetToken(String division)
        {
            String url = "http://localhost:54644/api/RecuperarToken/" + division;
            WebClient cliente = new WebClient();
            cliente.Headers["content-type"] = "application/xml";
            String datosxml = cliente.DownloadString(url);
            XDocument docxml = XDocument.Parse(datosxml);
            XElement element = (XElement)docxml.FirstNode;
            String key = element.Value;
            return key;
        }

        public CloudTable GetTable(String division)
        {
            String token = GetToken(division);
            String uri = "https://storagetajamarvlm.table.core.windows.net/";
            Uri tablauri = new Uri(uri);
            StorageCredentials credenciales = new StorageCredentials(token);
            CloudTableClient cliente = new CloudTableClient(tablauri, credenciales);
            CloudTable tabla = cliente.GetTableReference("TablaEquipos");
            return tabla;
        }

        public List<Equipo> BuscarDivision(String division)
        {
            CloudTable tabla = GetTable(division);
            TableQuery<Equipo> consulta = new TableQuery<Equipo>();

            List<Equipo> equipos = tabla.ExecuteQuery(consulta).ToList();
            if (equipos.Count() == 0)
            {
                return null;
            }
            else
            {
                return equipos;
            }
        }

        public void ModificarEquipo(int id, String division, String nombre, String ciudad, String entrenador, String estadio)
        {
            CloudTable tabla = GetTable(division);

            TableOperation retrieveOperation = TableOperation.Retrieve<Equipo>(division, id.ToString(), null);
            TableResult retrieveResult = tabla.Execute(retrieveOperation);

            Equipo updateEntity = (Equipo)retrieveResult.Result;

            if(updateEntity != null)
            {
                updateEntity.Nombre = nombre;
                updateEntity.Ciudad = ciudad;
                updateEntity.Entrenador = entrenador;
                updateEntity.Estadio = estadio;

                TableOperation insertOrMergeOperation = TableOperation.Merge(updateEntity);

                tabla.Execute(insertOrMergeOperation);
            }
        }

        public void EliminarEquipo(int id, String division)
        {
            CloudTable tabla = GetTable(division);
            TableQuery<Equipo> consulta = new TableQuery<Equipo>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, id.ToString()));
            foreach(var item in tabla.ExecuteQuery(consulta))
            {
                var operacion = TableOperation.Delete(item);
                tabla.Execute(operacion);
            }
        }
    }
}