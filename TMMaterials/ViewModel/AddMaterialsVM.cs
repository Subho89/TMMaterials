using GalaSoft.MvvmLight.Command;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TMMaterials.DAL.Model;
using TMMaterials.Services.ViewModels;

namespace TMMaterials.ViewModel
{
    public class AddMaterialsVM : BaseViewModel
    {
        private readonly AddMaterialsServicesVM _service;
        public Action CloseAction { get; set; }
        public ObservableCollection<tblMain> Regions { get; set; }
        public ObservableCollection<tblMaterialTypes> MaterialTypes { get; set; }
        public ObservableCollection<StandardLookupVM> Standards { get; set; } = new();
        public ObservableCollection<tblCollectionStandards> Grades { get; set; } = new();

        public ICommand OkCommand { get; }

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

        private StandardLookupVM _selectedStandard;
        public StandardLookupVM SelectedStandard
        {
            get => _selectedStandard;
            set
            {
                _selectedStandard = value;
                OnPropertyChanged();
                UpdateGrades(); // Now we can use SelectedStandard.StandardId
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

        private readonly Action<string> _onMaterialAdded;
        public AddMaterialsVM(Action<string> onMaterialAdded)
        {
            _onMaterialAdded = onMaterialAdded;
            _service = new AddMaterialsServicesVM(); // From TMMaterials.Services
            Regions = new ObservableCollection<tblMain>(_service.GetAllRegions());
            MaterialTypes = new ObservableCollection<tblMaterialTypes>(_service.GetAllTypes());

            OkCommand = new RelayCommand(OnOk);
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
            if (SelectedRegion == null || SelectedType == null || SelectedStandard == null)
            {
                Grades.Clear();
                return;
            }

            // Use the StandardId for a more accurate filter
            var list = _service.GetGrades(
                SelectedRegion.mainId,
                SelectedType.materialTypeId,
                SelectedStandard.StandardId
            );

            Grades.Clear();
            foreach (var g in list) Grades.Add(g);
        }

        private void OnOk()
        {
            if (SelectedStandard == null || SelectedGrade == null) return;

            string standardName = SelectedStandard.StandardName;

            // Check if ':' exists to remove the year part
            if (standardName.Contains(":"))
            {
                standardName = standardName.Split(':')[0].Trim();
            }

            // Merge Standard and Grade
            string mergedDisplay = $"{standardName}-{SelectedGrade.MaterialGrade}";

            // Execute the callback to update the main MaterialsList
            _onMaterialAdded?.Invoke(mergedDisplay);
            CloseAction?.Invoke();
            // TODO: Send 'mergedDisplay' back to the calling window
        }
    }

    
}
