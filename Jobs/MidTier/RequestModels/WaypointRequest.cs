using System.Collections.Generic;

namespace Sabio.Web.Models.Requests
{
    public class WaypointRequest
    {
        public int Id { get; set; }

        public int JobId { get; set; }

        public int? AddressId { get; set; }

        public string ExternalAddressId { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public int ZipCode { get; set; }

        public AddressAddRequest Address { get; set; }

        public string SuiteNo { get; set; }

        public string ContactName { get; set; }

        public string Phone { get; set; }

        public string SpecialInstructions { get; set; }

        public string ServiceNote { get; set; }

        public List<WaypointItemPickupRequest> JobWaypointItemsPickup { get; set; }

        public List<WaypointItemDropoffRequest> JobWaypointItemsDropOff { get; set; }

        public List<int> Service { get; set; }

        public int ExternalCustomerId { get; set; } =1226426;


    }
}