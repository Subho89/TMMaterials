using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TMMaterials.DAL.Model;
using TMMaterials.ViewModel;

namespace TMMaterials.Views
{
    /// <summary>
    /// Interaction logic for MaterialPropertyData.xaml
    /// </summary>
    public partial class MaterialPropertyData : Window
    {
        tblCollectionStandards collectionStandards;
        public MaterialPropertyData()
        {
            InitializeComponent();
        }
    }
}
