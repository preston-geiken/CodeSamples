using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bringpro.Web.Domain
{
    public class Address
    {

        public int AddressId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public string UserId { get; set; }

        public string Name { get; set; }

        public string ExternalPlaceId { get; set; }

        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public int StateId { get; set; }

        public int ZipCode { get; set; }

        public string Country { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }
    }
}