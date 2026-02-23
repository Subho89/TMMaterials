using GalaSoft.MvvmLight.Command;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TMMaterials.DAL;
using TMMaterials.Services.ViewModels;
using TMMaterials.Views;

namespace TMMaterials.ViewModel
{
    public class DefineMaterialsVM : BaseViewModel
    {
        public ICommand AddMaterialCommand { get; }
        public ICommand ModifyMaterialCommand { get; }
        public ObservableCollection<string> MaterialsList { get; set; } = new();

        private string _selectedMaterial;
        public string SelectedMaterial
        {
            get => _selectedMaterial;
            set { _selectedMaterial = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }
        public DefineMaterialsVM()
        {
            AddMaterialCommand = new RelayCommand(OnAddMaterial);
            // ModifyMaterialCommand = new RelayCommand(OnModifyMaterial, () => !string.IsNullOrEmpty(SelectedMaterial));
            ModifyMaterialCommand = new RelayCommand(OnModifyMaterial);
        }

        private void OnAddMaterial()
        {
            // 1. Create the ViewModel with the callback logic
            var viewModel = new AddMaterialsVM(newMaterialString =>
            {
                if (!MaterialsList.Contains(newMaterialString))
                {
                    MaterialsList.Add(newMaterialString); // Updates the ListBox in the main view
                }
            });

            // 2. Create the View
            var addWin = new AddMaterials();

            // 3. IMPORTANT: Link the View to the ViewModel
            addWin.DataContext = viewModel;

            // 4. Display the window
            addWin.ShowDialog();
        }

        private void OnModifyMaterial()
        {
            // 1. Get the service to fetch full data
            var service = new AddMaterialsServicesVM();

            // 2. Parse the SelectedMaterial string (e.g., "AS/NZS 1163-Grade C250")
            // You may need to split by '-' to get the Grade name
            string[] parts = SelectedMaterial.Split('-');
            string gradeName = parts.Last().Trim();

            // 3. Fetch the specific database record
            var collectionStandard = service.GetMaterialDetailsByGrade(gradeName);

            if (collectionStandard != null)
            {
                // FIX: Instantiate the specific property service
                var propService = new MaterialPropertyDataServicesVM();

                // Use the propService to fetch the property-value pairs
                var properties = propService.GetPropertiesForStandard(collectionStandard.collectionStandardId);

                // 3. Setup the Property VM
                var propVM = new MaterialPropertyDataVM();
                propVM.LoadProperties(properties, SelectedMaterial);

                // 4. Open the Window
                var propWin = new MaterialPropertyData();
                propWin.DataContext = propVM;
                propWin.ShowDialog();
            }
        }
    }
}
