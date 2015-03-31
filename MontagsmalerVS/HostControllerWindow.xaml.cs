using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace MontagsmalerVS
{
    /// <summary>
    /// Interaktionslogik für HostControllerWindow.xaml
    /// </summary>
    public partial class HostControllerWindow : Window
    {
        Timer t = new Timer();
        public HostControllerWindow()
        {
            InitializeComponent();
            t.Elapsed += t_Elapsed;
            t.Interval = 300;
            t.Start();
        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            setListBox(HostController.getNames());
        }

        private void setListBox(List<String> names)
        {
            lbNames.Dispatcher.Invoke(new Action(() =>
            {
                int index = lbNames.SelectedIndex;
                lbNames.Items.Clear();
                foreach (var item in names)
                {
                    lbNames.Items.Add(item);
                }
                lbNames.SelectedIndex = index;
            }));
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (lbNames.SelectedItem != null)
            {
                HostController.disabledrawing();
                HostController.enabledrawing(HostController.getClientNumber((String)lbNames.SelectedItem));
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            HostController.disabledrawing();
            string[] s = HostController.randomWords();
            HostController.sendWords(HostController.getClientNumber((String)lbNames.SelectedItem), s[0], s[1], s[2]);     
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "*.txt | Textfiles";
            ofd.ShowDialog();
            HostController.loadWordList(ofd.FileName);
        }
    }
}
