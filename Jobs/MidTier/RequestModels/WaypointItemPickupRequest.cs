using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bringpro.Web.Models.Requests
{
    public class WaypointItemPIckupRequest
    {
        public int Id { get; set; }

        public int JobId { get; set; }

        public int WaypointId { get; set; }

        public int ItemTypeId { get; set; }

        public string ItemNote { get; set; }

        public int Quantity { get; set; }

        public int? MediaId { get; set; }

        public int? Operation { get; set; } = 1; // TODO: switch this back to not nullable once front end get this in

        public int? ParentItemId { get; set; }

    }
}