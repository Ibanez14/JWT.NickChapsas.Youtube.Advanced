using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tutorial.WebAPI.JWT.NickChapsas.Contract;

namespace Tutorial.WebAPI.JWT.NickChapsas.Controllers
{
    // Versioning
    //[Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ValuesController : ControllerBase
    {
        #region Ctor and fields

        private readonly List<ValueModel> _models;

        public ValuesController()
        {
            _models = new List<ValueModel>()
            {
                new ValueModel(){ Id = Guid.NewGuid().ToString(), Name = "Steve"},
                new ValueModel(){ Id = Guid.NewGuid().ToString(), Name = "John"},
                new ValueModel(){ Id = Guid.NewGuid().ToString(), Name = "Roger"},
                new ValueModel(){ Id = Guid.NewGuid().ToString(), Name = "Water"},
                new ValueModel(){ Id = Guid.NewGuid().ToString(), Name = "David"},
                new ValueModel(){ Id = Guid.NewGuid().ToString(), Name = "Gilmour"},
            };
        }

        #endregion

        [HttpGet(ApiRoutes.Values.Get)]
        public IActionResult Get ([FromRoute] string id)
        {
            // Getting a model from repository by ids
            var reModel = _models.FirstOrDefault();
            
            // return found model
            return Ok(reModel);
        }
       
        // api/v1/values
        [HttpGet(ApiRoutes.Values.GetAll)]
        [Authorize("View")]
        public IActionResult GetAll()
        {
            return Ok(_models);
        }
        
        [HttpPost(ApiRoutes.Values.Create)]
        public IActionResult Create([FromBody] ValueModel model)
        {
            if (string.IsNullOrEmpty(model.Id))
                model.Id = "123123";

            _models.Add(model);

            var scheme = Request.Scheme; // https
            var host = Request.Host.ToUriComponent(); // localhost:5051

            //                         Get = api/v1/values/id
            var url = ApiRoutes.Values.Get.Replace("id", model.Id); // api/v1/values/123123

            // building string like https://localhost:5051/api/v1/values/model.Id
            var location = string.Concat(scheme, "://", host, url);

            // Produce Status code 201
            // and returns location in reponse header that indicated the location to get this model
            return Created(uri: location,
                         value: model);
        }
    }
}
