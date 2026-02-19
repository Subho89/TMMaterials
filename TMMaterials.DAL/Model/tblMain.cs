using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMMaterials.DAL.Model
{
    public class tblMain
    {
        [Key]
        public int mainId { get; set; }
        public string? RegionName { get; set; }
        public string? FileID { get; set; }
        public string? FileVersion { get; set; }
        public string? FileDescription { get; set; }
        public string? LengthUnits { get; set; }
        public string? ForceUnits { get; set; }
        //public bool IsDefault { get; set; }
    }
}
