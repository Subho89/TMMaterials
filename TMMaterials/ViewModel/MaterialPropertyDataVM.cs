using MvvmHelpers;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TMMaterials.DAL.Model;
using TMMaterials.Services.ViewModels;
//using TMMaterials.Models; // Assuming your EF models are here

namespace TMMaterials.ViewModel
{
    // The Row Model for the DataGrid
    public class MaterialPropertyDataVM : BaseViewModel
    {
        public string FullMaterialName { get; set; } // Binds to TextBlock
        public string MaterialType { get; set; }     // Binds to TextBlock

        // Binds to DataGrid ItemsSource
        public ObservableCollection<MaterialPropertyItem> PropertyItems { get; set; } = new();

        public void LoadProperties(List<MaterialPropertyItem> properties, string selectedName)
        {
            FullMaterialName = selectedName;
            // Optionally fetch TypeName from standard object if available

            PropertyItems.Clear();
            foreach (var p in properties)
            {
                PropertyItems.Add(p);
            }

            // Notify the UI that the header text changed
            OnPropertyChanged(nameof(FullMaterialName));
        }
    }
}