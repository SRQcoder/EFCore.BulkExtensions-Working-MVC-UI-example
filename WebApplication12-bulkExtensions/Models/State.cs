using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication12_bulkExtensions.Models
{
    public class State
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Abbreviation { get; set; }

        public string Type { get; set; }

        public string Country { get; set; }

        public int? Region { get; set; }

        public string RegionName { get; set; }

        public int? Division { get; set; }

        public string DivisionName { get; set; }

        public string Flag { get; set; }

    }
}
