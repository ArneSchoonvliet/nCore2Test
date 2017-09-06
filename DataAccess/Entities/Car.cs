using Digipolis.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nCore2Test.DataAccess.Entities
{
    public class Car : EntityBase
    {
        public string Make { get; set; }
        public int Year { get; set; }
    }
}
