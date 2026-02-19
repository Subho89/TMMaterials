using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMMaterials.DAL.Model
{
    public class tblStandards
    {
        [Key]
        public int standardId { get; set; }
        public string? StandardName { get; set; } // e.g., "ASTM A36", "Eurocode"
    }
}
