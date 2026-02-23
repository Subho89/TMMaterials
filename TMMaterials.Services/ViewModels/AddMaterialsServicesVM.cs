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

        public List<StandardLookupVM> GetStandards(int? regionId, int materialTypeId)
        {
            return (from cs in _db.tblCollectionStandards
                    join s in _db.tblStandards on cs.standardId equals s.standardId
                    where cs.mainId == regionId && cs.materialTypeId == materialTypeId
                    select new StandardLookupVM
                    {
                        StandardId = s.standardId,
                        StandardName = s.StandardName
                    })
                    .Distinct()
                    .ToList();
        }

        public List<tblCollectionStandards> GetGrades(int? regionId, int materialTypeId, int standardId)
        {
            // Filtering by foreign keys (mainId, materialTypeId, and standardId) 
            // ensures we get the exact grades linked to that specific standard record.
            return _db.tblCollectionStandards
                      .Where(x => x.mainId == regionId &&
                                  x.materialTypeId == materialTypeId &&
                                  x.standardId == standardId)
                      .ToList();
        }

        public tblCollectionStandards GetMaterialDetailsByGrade(string gradeName)
        {
           
            {
                // Fetch the record containing Isotropic and Weight data
                return _db.tblCollectionStandards
                              .FirstOrDefault(m => m.MaterialGrade == gradeName);
            }
        }
    }

    public class StandardLookupVM
    {
        public int StandardId { get; set; }
        public string StandardName { get; set; }
    }
}
