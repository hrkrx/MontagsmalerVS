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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MontagsmalerVS
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NetworkController nw = null;
        Timer t = new Timer();
        Timer stopWatch = new Timer();     
        public MainWindow()
        {
            InitializeComponent();
            MainController.canv = icPaint;
            MainController.UserList = this.lbUsers;
            MainController.Chat = this.lbChat;
            this.Title = MainController.lname;
            icPaint.EditingMode = InkCanvasEditingMode.None;
            t.Elapsed += t_Elapsed;
            t.Interval = 500;
            stopWatch.Elapsed += stopWatch_Elapsed;
            stopWatch.Interval = 1000;
            stopWatch.Start();
        }

        void stopWatch_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (MainController.timerCounter > 0)
            {
                MainController.timerCounter--;
                lbTime.Dispatcher.Invoke(new Action(() =>
                {
                    lbTime.Content = MainController.timerCounter.ToString() + " seconds";
                }));
            }
            else
            {
                lbTime.Dispatcher.Invoke(new Action(() =>
                {
                    lbTime.Content = DateTime.Now.ToShortTimeString();
                }));
            }
        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (nw != null && MainController.drawing)
            {
                nw.send(MainController.getData());
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (MainController.drawing)
            {
                icPaint.EditingMode = InkCanvasEditingMode.Ink;
                icPaint.DefaultDrawingAttributes.Color = Color.FromRgb(255, 0, 0);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (MainController.drawing)
            {
                icPaint.EditingMode = InkCanvasEditingMode.Ink;
                icPaint.DefaultDrawingAttributes.Color = Color.FromRgb(0, 255, 0);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (MainController.drawing)
            {
                icPaint.EditingMode = InkCanvasEditingMode.Ink;
                icPaint.DefaultDrawingAttributes.Color = Color.FromRgb(0, 0, 255);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (MainController.drawing)
            {
                icPaint.EditingMode = InkCanvasEditingMode.Ink;
                icPaint.DefaultDrawingAttributes.Color = Color.FromRgb(255, 255, 0);
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (MainController.drawing)
            {
                icPaint.EditingMode = InkCanvasEditingMode.Ink;
                icPaint.DefaultDrawingAttributes.Color = Color.FromRgb(255, 0, 255);
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (MainController.drawing)
            {
                icPaint.EditingMode = InkCanvasEditingMode.Ink;
                icPaint.DefaultDrawingAttributes.Color = Color.FromRgb(0, 0, 0);
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if (MainController.drawing)
            {
                icPaint.EditingMode = InkCanvasEditingMode.Ink;
                icPaint.DefaultDrawingAttributes.Color = Color.FromRgb(255, 255, 255);
            }
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            icPaint.DefaultDrawingAttributes.Width = 5;
            icPaint.DefaultDrawingAttributes.Height = 5;
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            icPaint.DefaultDrawingAttributes.Width = 15;
            icPaint.DefaultDrawingAttributes.Height = 15;
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            icPaint.DefaultDrawingAttributes.Width = 30;
            icPaint.DefaultDrawingAttributes.Height = 30;
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            if (MainController.drawing)
            {
                icPaint.EditingMode = InkCanvasEditingMode.EraseByPoint;
            }
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            t.Stop();
            if (nw != null)
            {
                nw.deregister();
            }
            ConnectWindow cw = new ConnectWindow();
            cw.ShowDialog();
            nw = new NetworkController(cw.ip);
            nw.connect(cw.ip);
            t.Start();
        }

        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            if (MainController.drawing)
            {
                icPaint.EditingMode = InkCanvasEditingMode.Ink;
                icPaint.DefaultDrawingAttributes.Color = Color.FromRgb(120, 60, 40);
            }
        }

        private void Button_Click_13(object sender, RoutedEventArgs e)
        {
            if (MainController.drawing)
            {
                icPaint.Strokes.Clear();
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (nw != null)
                {
                    nw.send(MainController.lname + ": " + tbChat.Text);
                    tbChat.Text = "";
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (nw != null)
            {
                nw.deregister();
            }
        }
    }
}
