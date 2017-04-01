using Microsoft.Practices.Unity;
using Bringpro.Web.Domain;
using Bringpro.Web.Models.Requests;
using Bringpro.Web.Models.Responses;
using Bringpro.Web.Services;
using Bringpro.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Bringpro.Web.Controllers.Api
{
    [RoutePrefix("api/customerrating")]
    public class CustomerRatingApiController : ApiController
    {
        [Dependency]
        public ICustomerRatingService _RatingService { get; set; }

        [Route(), HttpPost]
        public HttpResponseMessage Insert(CustomerRatingAddRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            ItemResponse<int> response = new ItemResponse<int>();

            //response.Item = CustomerRatingService.Insert(model);
            response.Item = _RatingService.Insert(model);


            return Request.CreateResponse(response);

        }

        [Route(), HttpGet]
        public HttpResponseMessage List()
        {
            ItemsResponse<CustomerRating> response = new ItemsResponse<CustomerRating>();

            //response.Items = CustomerRatingService.GetAll();
            response.Items = _RatingService.GetAll();


            return Request.CreateResponse(response);
        }
    }
}
