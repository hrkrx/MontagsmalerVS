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

namespace MontagsmalerVS
{
    /// <summary>
    /// Interaktionslogik für Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainController.Init(tbName.Text.Replace("#",""));
            if (cbHost.IsChecked.Value)
            {
                HostController.beginHosting();
                HostControllerWindow hcw = new HostControllerWindow();
                hcw.Show();
            }
            MainWindow mw = new MainWindow();
            this.Hide();
            mw.ShowDialog();
            if (cbHost.IsChecked.Value)
            {
                HostController.endHosting();
            }
            this.Close();
        }
    }
}
