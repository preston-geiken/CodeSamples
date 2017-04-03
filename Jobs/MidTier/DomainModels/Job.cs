using Bringpro.Web.Enums;
using Bringpro.Web.Models.Requests.Bringg;
using System;
using System.Collections.Generic;

namespace Bringpro.Web.Domain
{
    public class Job : BaseBringgRequest
    {
        public int Id { get; set; }

        //testing enums for user profile jobs dashboard.
        public JobsType? JobType { get; set; }

        public JobStatus? JobStatus { get; set; }

        public decimal Price { get; set; }

        public string Phone { get; set; }

        public string ContactName { get; set; }

        public int JobRequest { get; set; }

        public string SpecialInstructions { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public int WebsiteId { get; set; }

        public int ExternalJobId { get; set; }

        public List<JobWaypoint> JobWaypoints { get; set; }

        public List <JobWaypointItem> ItemsToBeDroppedOff { get; set; }

        public UserProfile User { get; set; }

        public Website Website { get; set; }

        public int? ExternalCustomerId { get; set; } =1226426; //TODO delete hardcoding later

        public int BillingId { get; set; }

        public string PaymentNonce { get; set; }

        public int TeamId { get; set; }

    }
}