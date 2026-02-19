using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMMaterials.DAL.Model
{
    public class tblMaterialTypes
    {
        [Key]
        public int materialTypeId { get; set; }
        public string? TypeName { get; set; } // e.g., "Steel", "Concrete"
    }
}
