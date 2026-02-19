using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMMaterials.DAL.Model
{
    public class tblCollectionPropertiesValues
    {
        [Key]
        public int valueId { get; set; }
        public int? propertyId { get; set; }
        public int? collectionStandardId { get; set; }
        public string? Value { get; set; } // Stored as string to handle all types; cast in code
    }
}
