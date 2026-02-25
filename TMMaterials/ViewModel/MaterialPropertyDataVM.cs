using GalaSoft.MvvmLight.Command;
using MvvmHelpers;
using System.Collections.ObjectModel;
using System.Text;
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
        public ICommand ExportCommand { get; }
        // Binds to DataGrid ItemsSource
        public ObservableCollection<MaterialPropertyItem> PropertyItems { get; set; } = new();

        public MaterialPropertyDataVM()
        {
            ExportCommand = new RelayCommand(OnExport);
        }

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

        private void OnExport()
        {
            var sfd = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv",
                FileName = $"{FullMaterialName}_Properties"
            };

            if (sfd.ShowDialog() == true)
            {
                var builder = new StringBuilder();
                // Header
                builder.AppendLine("Property,Value,Unit");

                // Data Rows from the Grid
                foreach (var item in PropertyItems)
                {
                    // Ensure commas in values don't break the CSV format
                    builder.AppendLine($"{item.PropertyName},{item.PropertyValue},{item.Unit}");
                }

                System.IO.File.WriteAllText(sfd.FileName, builder.ToString());
                System.Windows.MessageBox.Show("Data exported successfully!");
            }
        }
    }
}