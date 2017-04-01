using Bringpro.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bringpro.Web.Controllers
{
    [RoutePrefix("address")]
    public class AddressController : BaseController
    {
        public ActionResult Index()
        {
            return View("IndexNg");
        }

        [Route("create")]
        [Route("edit/{addressId:int}")]
        public ActionResult Edit(int? addressId = null)
        {
            ItemViewModel<int?> vm = new ItemViewModel<int?>();
            vm.Item = addressId;
            return View("AddressFormNg", vm);
        }
    }
}
