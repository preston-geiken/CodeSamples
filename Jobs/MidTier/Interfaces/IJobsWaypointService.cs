using Bringpro.Web.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bringpro.Web.Services.Interfaces
{
    public interface IJobsWaypointService
    {
        int WaypointInsert(WaypointRequest model, int addressId);

        void WaypointUpdate(WaypointRequest model, int addressId);

        int WaypointItemInsert(WaypointItemPIckupRequest model, int? jobId, int waypointId);

        void WaypointItemUpdate(WaypointItemPIckupRequest model, int? jobId, int id);
    }
}