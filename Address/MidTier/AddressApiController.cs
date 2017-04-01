using Microsoft.Practices.Unity;
using Bringpro.Web.Domain;
using Bringpro.Web.Models.Requests;
using Bringpro.Web.Models.Responses;
using Bringpro.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Bringpro.Web.Services.Interfaces;

namespace Bringpro.Web.Controllers.Api
{
    [RoutePrefix("api/address")]
    public class AddressApiController : ApiController
    {
        [Dependency]
        public IAddressService _AddressService { get; set; }

        [Route(), HttpPost]
        public HttpResponseMessage Insert(AddressAddRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            ItemResponse<int> response = new ItemResponse<int>();

            response.Item = _AddressService.Insert(model);

            return Request.CreateResponse(response);
        }

        [Route("{AddressId:int}"), HttpPut]
        public HttpResponseMessage Update(int AddressId, AddressUpdateRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            _AddressService.UpdateForLatAndLong(model);
            ItemResponse<bool> response = new ItemResponse<bool>();

            response.Item = true;

            return Request.CreateResponse(response);
        }

        [Route("{AddressId:int}"), HttpGet]
        public HttpResponseMessage GetByAddressId(int AddressId)
        {

            ItemResponse<Address> response = new ItemResponse<Address>();

            response.Item = _AddressService.GetById(AddressId);

            return Request.CreateResponse(response);
        }

        [Route(), HttpGet]
        public HttpResponseMessage List()
        {
            ItemsResponse<Address> response = new ItemsResponse<Address>();

            response.Items = _AddressService.GetAll();

            return Request.CreateResponse(response);
        }

        [Route("delete/{AddressId:int}"), HttpDelete]
        public HttpResponseMessage Delete(int AddressId)
        {
            SuccessResponse response = new SuccessResponse();
            _AddressService.DeleteById(AddressId);
            return Request.CreateResponse(response);
        }

        [Route("getbyexternal/{ExternalPlaceId}"), HttpGet]
        public HttpResponseMessage GetByAddressId(string ExternalPlaceId)
        {

            ItemResponse<Address> response = new ItemResponse<Address>();

            response.Item = _AddressService.GetByExternalAddressId(ExternalPlaceId);

            return Request.CreateResponse(response);
        }

        [Route("deletebyexternal/{ExternalPlaceId}"), HttpDelete]
        public HttpResponseMessage DeleteByAddressId(string ExternalPlaceId)
        {

            _AddressService.DeleteByExternalAddressId(ExternalPlaceId);

            return Request.CreateResponse(HttpStatusCode.OK, "Succesfully Deleted Address");
        }

    }
}
