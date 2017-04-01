using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bringpro.Web.Models.Requests
{
    public class PaginatedRequest
    {
        public int CurrentPage { get; set; } = 1;

        public int ItemsPerPage { get; set; } = 10;

        public string Query { get; set; }

        public int? Selector { get; set; }

        public int? QueryWebsiteId { get; set; }

        public int? QueryStatus { get; set; }

        public int? QueryJobType { get; set; }

        public DateTime? QueryStartDate { get; set; }

        public DateTime? QueryEndDate { get; set; }

        public int? QueryCategory { get; set; }

        public int? QuerySettingSection { get; set; }

        public int? QuerySettingType { get; set; }

        public string Slug { get; set; }

        public DateTime? Date { get; set; }

    }
}