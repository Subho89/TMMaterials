using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMMaterials.DAL;
using TMMaterials.DAL.Model;

namespace TMMaterials.Services.ViewModels
{
    public class AddMaterialsServicesVM
    {
        private readonly MaterialDbContext _db = new MaterialDbContext();

        public List<tblMain> GetAllRegions() => _db.tblMain.ToList();

        public List<tblMaterialTypes> GetAllTypes() => _db.tblMaterialTypes.ToList();

        public List<string> GetStandards(int? regionId, int materialTypeId)
        {
            // Ensure we filter by BOTH Region and Material Type for a true cascading effect
            return _db.tblCollectionStandards
                      .Where(x => x.mainId == regionId && x.materialTypeId == materialTypeId)
                      .Select(x => x.MaterialName) // Use the property name that holds the Standard string
                      .Distinct()
                      .ToList();
        }

        public List<tblCollectionStandards> GetGrades(int? regionId, int? materialTypeId, string standardName)
        {
            return _db.tblCollectionStandards
                      .Where(x => x.mainId == regionId &&
                                  x.materialTypeId == materialTypeId &&
                                  x.MaterialName == standardName)
                      .ToList();
        }
    }
}
