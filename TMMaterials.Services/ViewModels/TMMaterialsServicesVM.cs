using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using TMMaterials.DAL;
using TMMaterials.DAL.Model;

namespace TMMaterials.Services
{
    public class TMMaterialsServicesVM
    {
        private readonly MaterialDbContext _db;

        public TMMaterialsServicesVM()
        {
            _db = new MaterialDbContext();
        }

        //public async Task<bool> ProcessXmlFileAsync(string filePath, IProgress<int> progress, Action<string> logCallback)
        //{
        //    return await Task.Run(() =>
        //    {
        //        try
        //        {
        //            XDocument doc = XDocument.Load(filePath);
        //            XNamespace ns = doc.Root.GetDefaultNamespace();

        //            // 1. Process tblMain
        //            logCallback("Extracting Region Metadata...");
        //            var regionElem = doc.Descendants(ns + "region").First();
        //            var controlElem = doc.Descendants(ns + "control").First();
        //            var mainEntry = new tblMain
        //            {
        //                RegionName = regionElem.Attribute("name")?.Value,
        //                LengthUnits = regionElem.Attribute("lengthUnits")?.Value,
        //                ForceUnits = regionElem.Attribute("forceUnits")?.Value,
        //                FileID= controlElem.Attribute("fileID")?.Value,
        //                FileVersion= controlElem.Attribute("fileVersion")?.Value,
        //                FileDescription = controlElem.Attribute("fileDescription")?.Value,
        //            };
        //            _db.tblMain.Add(mainEntry);
        //            _db.SaveChanges();

        //            // 2. Process Collections & Library
        //            var colElem = regionElem.Element(ns + "collection");
        //            var collection = GetOrCreateCollection(colElem.Attribute("type")?.Value);

        //            _db.tblMaterialsLibrary.Add(new tblMaterialsLibrary
        //            {
        //                mainId = mainEntry.mainId,
        //                collectionId = collection.collectionId
        //            });

        //            // 3. Process Standards and Materials
        //            var standards = colElem.Elements(ns + "standard").ToList();
        //            for (int i = 0; i < standards.Count; i++)
        //            {
        //                var std = GetOrCreateStandard(standards[i].Attribute("name")?.Value);

        //                var colStd = new tblCollectionStandards
        //                {
        //                    mainId = mainEntry.mainId,
        //                    collectionId = collection.collectionId,
        //                    standardId = std.standardId,
        //                    IsDefault = standards[i].Attribute("isDefaultSteel")?.Value == "true"
        //                };
        //                _db.tblCollectionStandards.Add(colStd);
        //                _db.SaveChanges();

        //                foreach (var mat in standards[i].Elements(ns + "material"))
        //                {

        //                    var matType = GetOrCreateType( mat.Attribute("type")?.Value);

        //                    colStd.MaterialName = mat.Attribute("name")?.Value;
        //                    colStd.materialTypeId = (int)matType?.materialTypeId;
        //                    colStd.MaterialGrade = mat.Attribute("grade")?.Value;

        //                    // 4. EAV Property Values
        //                    foreach (var prop in mat.Elements())
        //                    {
        //                        var property = GetOrCreateProperty(prop.Name.LocalName);
        //                        _db.tblCollectionPropertiesValues.Add(new tblCollectionPropertiesValues
        //                        {
        //                            propertyId = property.propertyId,
        //                            collectionStandardId = colStd.collectionStandardId,
        //                            Value = prop.Value
        //                        });
        //                    }
        //                }

        //                // Report Progress back to UI
        //                int pct = (int)((i + 1.0) / standards.Count * 100);
        //                progress.Report(pct);
        //            }

        //            _db.SaveChanges();
        //            return true;
        //        }
        //        catch (Exception ex)
        //        {
        //            logCallback($"Database Error: {ex.Message}");
        //            return false;
        //        }
        //    });
        //}


        public async Task<bool> ProcessXmlFileAsync(string filePath, IProgress<int> progress, Action<string> logCallback)
        {
            return await Task.Run(() =>
            {
                try
                {
                    XDocument doc = XDocument.Load(filePath);
                    XNamespace ns = doc.Root.GetDefaultNamespace();

                    // 1. Process tblMain Metadata
                    logCallback("Extracting Region and Control Metadata...");
                    var regionElem = doc.Descendants(ns + "region").First();
                    var controlElem = doc.Descendants(ns + "control").First();

                    var mainEntry = new tblMain
                    {
                        RegionName = regionElem.Attribute("name")?.Value,
                        LengthUnits = regionElem.Attribute("lengthUnits")?.Value,
                        ForceUnits = regionElem.Attribute("forceUnits")?.Value,
                        FileID = controlElem.Element(ns + "fileID")?.Value,
                        FileVersion = controlElem.Element(ns + "fileVersion")?.Value,
                        FileDescription = controlElem.Element(ns + "fileDescription")?.Value,
                    };

                    _db.tblMain.Add(mainEntry);
                    _db.SaveChanges(); // Save to generate mainId

                    // 2. Process Collections & Materials Library
                    var colElem = regionElem.Element(ns + "collection");
                    var collection = GetOrCreateCollection(colElem.Attribute("type")?.Value);

                    _db.tblMaterialsLibrary.Add(new tblMaterialsLibrary
                    {
                        mainId = mainEntry.mainId,
                        collectionId = collection.collectionId
                    });

                    // 3. Process Standards and Materials
                    var standards = colElem.Elements(ns + "standard").ToList();
                    for (int i = 0; i < standards.Count; i++)
                    {
                        var std = GetOrCreateStandard(standards[i].Attribute("name")?.Value);
                        bool isDefault = standards[i].Attribute("isDefaultSteel")?.Value == "true";

                        // Loop through each material found inside this standard
                        foreach (var mat in standards[i].Elements(ns + "material"))
                        {
                            // Use the helper to get or create the Type entry
                            var matType = GetOrCreateType(mat.Attribute("type")?.Value);

                            // Create a NEW record for every individual material
                            var colStd = new tblCollectionStandards
                            {
                                mainId = mainEntry.mainId,
                                collectionId = collection.collectionId,
                                standardId = std.standardId,
                                IsDefault = isDefault,
                                MaterialName = mat.Attribute("name")?.Value,
                                MaterialGrade = mat.Attribute("grade")?.Value,
                                materialTypeId = matType?.materialTypeId ?? 0 // Map to the FK ID
                            };

                            _db.tblCollectionStandards.Add(colStd);
                            _db.SaveChanges(); // Save here to generate collectionStandardId for EAV mapping

                            // 4. EAV Property Values mapping for this specific material
                            foreach (var prop in mat.Elements())
                            {
                                var property = GetOrCreateProperty(prop.Name.LocalName);
                                _db.tblCollectionPropertiesValues.Add(new tblCollectionPropertiesValues
                                {
                                    propertyId = property.propertyId,
                                    collectionStandardId = colStd.collectionStandardId, // Links to the material record above
                                    Value = prop.Value
                                });
                            }
                        }

                        // Report Progress back to the UI
                        int pct = (int)((i + 1.0) / standards.Count * 100);
                        progress.Report(pct);
                    }

                    // Final save for all EAV property values
                    _db.SaveChanges();
                    logCallback("Import completed successfully.");
                    return true;
                }
                catch (Exception ex)
                {
                    // Log the error back to the UI log
                    logCallback($"Database Error: {ex.Message}");
                    if (ex.InnerException != null)
                        logCallback($"Detail: {ex.InnerException.Message}");

                    return false;
                }
            });
        }
        private tblCollections GetOrCreateCollection(string name)
        {
            var c = _db.tblCollections.FirstOrDefault(x => x.CollectionName.ToLower() == name.ToLower());
            if (c == null) { name = char.ToUpper(name[0]) + name.Substring(1).ToLower(); c = new tblCollections { CollectionName = name }; _db.tblCollections.Add(c); _db.SaveChanges(); }
            return c;
        }

        private tblStandards GetOrCreateStandard(string name)
        {
            var s = _db.tblStandards.FirstOrDefault(x => x.StandardName.ToLower() == name.ToLower());
            if (s == null) { name = char.ToUpper(name[0]) + name.Substring(1).ToLower(); s = new tblStandards { StandardName = name }; _db.tblStandards.Add(s); _db.SaveChanges(); }
            return s;
        }

        private tblCollectionProperties GetOrCreateProperty(string name)
        {
            var p = _db.tblCollectionProperties.FirstOrDefault(x => x.PropertyName.ToLower() == name.ToLower());
            if (p == null) { name = char.ToUpper(name[0]) + name.Substring(1).ToLower(); p = new tblCollectionProperties { PropertyName = name }; _db.tblCollectionProperties.Add(p); _db.SaveChanges(); }
            return p;
        }

        private tblMaterialTypes GetOrCreateType(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName)) return null;



            var existing = _db.tblMaterialTypes.FirstOrDefault(t => t.TypeName.ToLower() == typeName.ToLower());
            if (existing != null) return existing;

            typeName=char.ToUpper(typeName[0]) + typeName.Substring(1).ToLower();

            var newType = new tblMaterialTypes { TypeName = typeName };
            _db.tblMaterialTypes.Add(newType);
            _db.SaveChanges();
            return newType;
        }
    }
}