using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiTablaEquipos.Repositories
{
    public class RepositoryApiEquipos
    {
        private CloudTable GetTablaEquipos()
        {
            String clave = CloudConfigurationManager.GetSetting("azurecuenta");
            CloudStorageAccount cuenta = CloudStorageAccount.Parse(clave);
            CloudTableClient cliente = cuenta.CreateCloudTableClient();
            CloudTable tabla = cliente.GetTableReference("TablaEquipos");
            return tabla;
        }

        public String GetSeguridadSaS(String division)
        {
            CloudTable tabla = GetTablaEquipos();
            SharedAccessTablePolicy permisos = new SharedAccessTablePolicy()
            {
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(30),
                Permissions = SharedAccessTablePermissions.Query | SharedAccessTablePermissions.Update 
            };
            String token = tabla.GetSharedAccessSignature(permisos, null, division, null, division, null);
            return token;
        }
    }
}