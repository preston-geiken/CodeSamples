using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bringpro.Web.Models.Requests
{
    public class JobUpdateRequest :JobInsertRequest
    {
        public int Id { get; set; }

    }
}