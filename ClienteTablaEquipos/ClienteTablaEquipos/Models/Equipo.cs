using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClienteTablaEquipos.Models
{
    public class Equipo : TableEntity
    {
        public String Division { get; set; }
        public int IdEquipo { get; set; }
        public String Nombre { get; set; }
        public String Ciudad { get; set; }
        public String Entrenador { get; set; }
        public String Estadio { get; set; }
    }
}