using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bringpro.Web.Models.Requests
{
    public class JobItemsUpdateRequest : JobsItemsInsertRequest
    {
        public int? Id { get; set; }

    }
}