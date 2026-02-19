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

namespace TMMaterials.Views
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void btnAdmin_Click(object sender, RoutedEventArgs e)
        {
            var frm = new TmAdminWindow();
            frm.Show();
        }

        private void btnMaterials_Click(object sender, RoutedEventArgs e)
        {
            var frm = new DefineMaterials();
            frm.Show();
        }
    }
}
