using System;

namespace Bringpro.Web.Models.Requests
{
    public class JobInsertRequest
    {

        public string UserId { get; set; }

        public int Status { get; set; }
       
        public int JobType { get; set; }

        public decimal? Price { get; set; }
       
        public string Phone { get; set; }

        public int JobRequest { get; set; }

        public string ContactName { get; set; }

        public string SpecialInstructions { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public int? WebsiteId { get; set; }

        public int? ExternalJobId { get; set; }

        public int? ExternalCustomerId { get; set; }

        public string PaymentNonce { get; set; }

        public int? BillingId { get; set; }

    }
}