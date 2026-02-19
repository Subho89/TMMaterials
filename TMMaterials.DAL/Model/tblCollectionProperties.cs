using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMMaterials.DAL.Model
{
    public class tblCollectionProperties
    {
        [Key]
        public int propertyId { get; set; }
        public string? PropertyName { get; set; } // e.g., "modulusOfElasticity", "poissonsRatio"
        public string? DataType { get; set; }     // e.g., "double", "string", "bool"
    }
}
