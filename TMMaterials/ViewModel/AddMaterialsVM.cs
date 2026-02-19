using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMMaterials.DAL.Model;
using TMMaterials.Services.ViewModels;

namespace TMMaterials.ViewModel
{
    public class AddMaterialsVM : BaseViewModel
    {
        private readonly AddMaterialsServicesVM _service;

        public ObservableCollection<tblMain> Regions { get; set; }
        public ObservableCollection<tblMaterialTypes> MaterialTypes { get; set; }
        public ObservableCollection<string> Standards { get; set; } = new();
        public ObservableCollection<tblCollectionStandards> Grades { get; set; } = new();

        // Selected items trigger cascading updates
        private tblMain _selectedRegion;
        public tblMain SelectedRegion
        {
            get => _selectedRegion;
            set { _selectedRegion = value; OnPropertyChanged(); UpdateStandards(); }
        }


        private tblMaterialTypes _selectedType;
        public tblMaterialTypes SelectedType
        {
            get => _selectedType;
            set
            {
                _selectedType = value;
                OnPropertyChanged();
                UpdateStandards(); // Triggers the cascade when type is selected
            }
        }

        private string _selectedStandard;
        public string SelectedStandard
        {
            get => _selectedStandard;
            set
            {
                _selectedStandard = value;
                OnPropertyChanged();
                UpdateGrades(); // Triggers the final cascade for Grades
            }
        }

        private tblCollectionStandards _selectedGrade;
        public tblCollectionStandards SelectedGrade
        {
            get => _selectedGrade;
            set
            {
                _selectedGrade = value;
                OnPropertyChanged();
            }
        }
        public AddMaterialsVM()
        {
            _service = new AddMaterialsServicesVM(); // From TMMaterials.Services
            Regions = new ObservableCollection<tblMain>(_service.GetAllRegions());
            MaterialTypes = new ObservableCollection<tblMaterialTypes>(_service.GetAllTypes());
        }

        private void UpdateStandards()
        {
            // Check if both selections exist to avoid null reference exceptions
            if (SelectedRegion == null || SelectedType == null)
            {
                Standards.Clear();
                return;
            }

            // Pass both IDs to match the service signature
            var list = _service.GetStandards(SelectedRegion.mainId, SelectedType.materialTypeId);

            Standards.Clear();
            foreach (var s in list)
            {
                Standards.Add(s);
            }
        }

        private void UpdateGrades()
        {
            // Ensure all prerequisites are met before calling the service
            if (SelectedRegion == null || SelectedType == null || string.IsNullOrEmpty(SelectedStandard))
            {
                Grades.Clear();
                return;
            }

            // Call the service method you just created
            var list = _service.GetGrades(SelectedRegion.mainId, SelectedType.materialTypeId, SelectedStandard);

            Grades.Clear();
            foreach (var g in list)
            {
                Grades.Add(g);
            }
        }
    }
}
