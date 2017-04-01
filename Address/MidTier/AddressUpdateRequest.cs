using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bringpro.Web.Models.Requests
{
    public class AddressUpdateRequest : AddressAddRequest
    {
        [Required]
        public int AddressId { get; set; }
    }
}