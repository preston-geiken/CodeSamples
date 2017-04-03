using Bringpro.Web.Models.Requests.Bringg;
using System;
using System.Collections.Generic;

namespace Bringpro.Web.Domain
{
    public class JobWaypoint : BaseBringgRequest
    {
        public int Id { get; set; }

        public int JobId { get; set; }

        public int AddressId { get; set; }

        public string SuiteNo { get; set; }

        public string ContactName { get; set; }

        public string Phone { get; set; }

        public string SpecialInstructions { get; set; }

        public string ServiceNote { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public int ExternalWaypointId { get; set; }

        public Address Address { get; set; }

        public List<JobWaypointItem> JobWaypointItemsPickup { get; set; }

        public List<JobWaypointItem> JobWaypointItemsDropOff { get; set; }

        public int? ExternalCustomerId { get; set; } =1226426; //TODO remove hardcoding later on

        public int? ExternalJobId { get; set; }


    }
}