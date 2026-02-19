using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMMaterials.DAL.Model
{
    public class tblMaterialsLibrary
    {
        [Key]
        public int libraryId { get; set; }
        public int? mainId { get; set; }
        public int? collectionId { get; set; }
    }
}
