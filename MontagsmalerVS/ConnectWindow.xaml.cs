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
    /// Interaktionslogik für ConnectWindow.xaml
    /// </summary>
    public partial class ConnectWindow : Window
    {
        public string ip = "";
        public ConnectWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ip = tbIP.Text;
            this.Close();
        }
    }
}
