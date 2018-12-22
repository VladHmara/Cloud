using CloudClient.ViewModel;
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
using static CloudClient.MainWindow;

namespace DataCloud.View
{
    /// <summary>
    /// Interaction logic for AddDirectoryWindow.xaml
    /// </summary>
    public partial class AddDirectoryWindow : Window
    {
        CreateDirectoryDelegat callback;
        DirectoryTreeNodeVM parentDirectoryTreeNodeVM;
        public AddDirectoryWindow(CreateDirectoryDelegat callback, DirectoryTreeNodeVM parentDirectoryTreeNodeVM)
        {
            InitializeComponent();
            this.callback = callback;
            this.parentDirectoryTreeNodeVM = parentDirectoryTreeNodeVM;
        }

        private void btn_CreateDirectory(object sender, RoutedEventArgs e)
        {
            callback(tb_DirectoryName.Text, parentDirectoryTreeNodeVM);
            this.Close();
        }
    }
}
