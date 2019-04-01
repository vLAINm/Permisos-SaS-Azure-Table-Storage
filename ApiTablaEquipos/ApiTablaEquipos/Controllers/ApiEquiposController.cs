using ApiTablaEquipos.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiTablaEquipos.Controllers
{
    public class ApiEquiposController : ApiController
    {
        RepositoryApiEquipos repo;

        public ApiEquiposController()
        {
            this.repo = new RepositoryApiEquipos();
        }

        [HttpGet]
        [Route("api/RecuperarToken/{division}")]
        public String RecuperarToken(String division)
        {
            return this.repo.GetSeguridadSaS(division);
        }
    }
}
