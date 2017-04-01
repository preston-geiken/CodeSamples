using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bringpro.Web.Models.Requests
{
    public class AddressAddRequest
    {
        public string UserId { get; set; }

        public string Name { get; set; }

        public string ExternalPlaceId { get; set; }

        [Required]
        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public string Street { get; set; }

        [Required]
        public string City { get; set; }

        public string State { get; set; }

        public int StateId { get; set; }

        [Required]
        public int ZipCode { get; set; }

        public string Country { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

    }
}