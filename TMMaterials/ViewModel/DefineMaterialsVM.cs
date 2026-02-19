using GalaSoft.MvvmLight.Command;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TMMaterials.DAL;
using TMMaterials.Views;

namespace TMMaterials.ViewModel
{
    public class DefineMaterialsVM : BaseViewModel
    {
        public ICommand AddMaterialCommand { get; }

        public DefineMaterialsVM()
        {
            AddMaterialCommand = new RelayCommand(OnAddMaterial);
        }

        private void OnAddMaterial()
        {
            var addWin = new AddMaterials();
            addWin.ShowDialog();
            // The AddMaterialVM will need the DbContext to load regions/types
            //addWin.DataContext = new AddMaterials();

            //if (addWin.ShowDialog() == true)
            //{
            //    // Refresh main materials list logic here
            //}
        }
    }
}
