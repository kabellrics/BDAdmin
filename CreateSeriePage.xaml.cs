using BDAdmin.ViewModel;
using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BDAdmin
{
    /// <summary>
    /// Logique d'interaction pour CreateSeriePage.xaml
    /// </summary>
    public partial class CreateSeriePage : Page
    {
        CreateSerieViewModel viewModel;
        public CreateSeriePage()
        {
            InitializeComponent();
            viewModel = this.DataContext as CreateSerieViewModel;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //var viewModel = this.DataContext as CreateSerieViewModel;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Img files (*.jpg;*.png)|*.jpg;*.png";
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == true)
                viewModel.ImgPath = openFileDialog.FileName;
        }
    }
}
