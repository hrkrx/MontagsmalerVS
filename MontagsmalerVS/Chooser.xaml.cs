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
    /// Interaktionslogik für Chooser.xaml
    /// </summary>
    public partial class Chooser : Window
    {
        public Chooser()
        {
            InitializeComponent();
        }

        public void setWords(string[] words)
        {
            btChoice1.Content = words[0];
            btChoice2.Content = words[1];
            btChoice3.Content = words[2];
        }

        private void btChoice1_Click(object sender, RoutedEventArgs e)
        {
            MainController.word = btChoice1.Content as String;
            Close();
        }

        private void btChoice2_Click(object sender, RoutedEventArgs e)
        {
            MainController.word = btChoice2.Content as String;
            Close();
        }

        private void btChoice3_Click(object sender, RoutedEventArgs e)
        {
            MainController.word = btChoice3.Content as String;
            Close();
        }
    }
}
