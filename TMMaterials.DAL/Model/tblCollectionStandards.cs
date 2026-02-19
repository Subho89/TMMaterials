using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMMaterials.DAL.Model
{
    public class tblCollectionStandards
    {
        [Key]
        public int collectionStandardId { get; set; }
        public int? collectionId { get; set; }
        public int? standardId { get; set; }
        public int? mainId { get; set; }
        public bool? IsDefault { get; set; }

        // Links to the actual material entry (e.g., "A36 Steel")
        public string? MaterialName { get; set; }
        public int? materialTypeId { get; set; }
        public string? MaterialGrade { get; set; }
    }
}
