using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMMaterials.DAL;

namespace TMMaterials.Services.ViewModels
{
    public class MaterialPropertyDataServicesVM
    {
        private readonly MaterialDbContext _db = new MaterialDbContext();
        //public List<MaterialPropertyItem> GetPropertiesForStandard(int standardId)
        //{
        //    // Query joining values with their property names
        //    var query = from val in _db.tblCollectionPropertiesValues
        //                join prop in _db.tblCollectionProperties on val.propertyId equals prop.propertyId
        //                where val.collectionStandardId == standardId
        //                select new MaterialPropertyItem
        //                {
        //                    PropertyName = prop.PropertyName, // From tblCollectionProperties
        //                    PropertyValue = val.Value.ToString(), // From tblCollectionPropertiesValues
        //                    Unit = "" // You can add a Unit column to tblCollectionProperties if needed
        //                };

        //    return query.ToList();
        //}

        public List<MaterialPropertyItem> GetPropertiesForStandard(int standardId)
        {
            var query = from val in _db.tblCollectionPropertiesValues
                        join prop in _db.tblCollectionProperties on val.propertyId equals prop.propertyId
                        where val.collectionStandardId == standardId
                        select new { prop.PropertyName, val.Value };

            var results = query.ToList();
            var items = new List<MaterialPropertyItem>();

            foreach (var row in results)
            {
                // 1. Apply spacing and capitalization logic
                string formattedName = FormatPropertyDisplay(row.PropertyName);

                items.Add(new MaterialPropertyItem
                {
                    PropertyName = formattedName,
                    PropertyValue = row.Value.ToString(),
                    // 2. Map the engineering unit from the reference image
                    Unit = GetUnitForProperty(formattedName)
                });
            }
            return items;
        }


        private string FormatPropertyDisplay(string rawName)
        {
            if (string.IsNullOrEmpty(rawName)) return string.Empty;

            // Hardcoded mapping for complex engineering terms found in your DB
            var specialCases = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        { "modulusofelasticity", "Modulus Of Elasticity" },
        { "poissonsratio", "Poisson's Ratio" },
        { "coefficientofthermalexpansion", "Coefficient Of Thermal Expansion" },
        { "weightdensity", "Weight Density" },
        { "massdensity", "Mass Density" },
        { "compressivestrength", "Compressive Strength" },
        { "shearstrengthreductionfactor", "Shear Strength Reduction Factor" }
    };

            if (specialCases.TryGetValue(rawName, out string mappedName))
                return mappedName;

            // General logic: Insert space before capitals and Capitalize every word
            string spaced = System.Text.RegularExpressions.Regex.Replace(rawName, "([a-z])([A-Z])", "$1 $2");
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(spaced.ToLower());
        }

        private string CleanPropertyName(string rawName)
        {
            if (string.IsNullOrWhiteSpace(rawName)) return string.Empty;

            // 1. Insert spaces before capital letters (if camelCase)
            string spaced = System.Text.RegularExpressions.Regex.Replace(rawName, "([a-z])([A-Z])", "$1 $2");

            // 2. Handle specific concatenated cases like "Modulusofelasticity"
            // If the database strings are all lowercase, we need a manual mapping or smarter logic:
            if (rawName.ToLower() == "modulusofelasticity") return "Modulus Of Elasticity";
            if (rawName.ToLower() == "poissonsratio") return "Poisson's Ratio";
            if (rawName.ToLower() == "weightdensity") return "Weight Density";

            // 3. Fallback: Capitalize the first letter of every word
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(spaced.ToLower());
        }

        private string GetUnitForProperty(string propertyName)
        {
            switch (propertyName)
            {
                case "Modulus Of Elasticity":
                case "Compressive Strength":
                case "Expected Compressive Strength":
                case "Shear Modulus":
                    return "N/mm²"; //
                case "Weight Density":
                    return "N/mm³"; //
                case "Mass Density":
                    return "N-s²/mm⁴"; //
                case "Coefficient Of Thermal Expansion":
                    return "1/C"; //
                default:
                    return ""; // Dimensionless properties like Poisson's Ratio
            }
        }


    }

    public class MaterialPropertyItem
    {
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
        public string Unit { get; set; }
    }
}
