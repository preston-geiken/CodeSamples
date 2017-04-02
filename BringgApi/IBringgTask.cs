using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bringpro.Web.Classes.Tasks.Bringg;
using Bringpro.Web.Domain;
using Bringpro.Web.Models.Requests.Bringg;

namespace Bringpro.Web.Classes.Tasks.Bringg.Interfaces
{
    public interface IBringgTask<T> where T : BaseBringgRequest
    {
        Dictionary<string, object> Execute(T request);
    }
}