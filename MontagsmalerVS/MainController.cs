using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MontagsmalerVS
{
    public static class MainController
    {
        public static string lname = "";
        public static byte[] picture = null;
        public static InkCanvas canv = null;
        public static bool drawing = false;
        public static ListBox UserList = null;
        public static ListBox Chat = null;
        public static ListBox Points = null;
        public static int timerCounter = 0;
        public static string word = "";
        public static Label lhint = null;
        public static Label lword = null;
        public static void Init(string name)
        {
            lname = name;
        }
        public static byte[] getData()
        {
            byte[] res = null;
            using (MemoryStream ms = new MemoryStream())
            {
                canv.Dispatcher.Invoke(new Action(() => 
                {
                    canv.Strokes.Save(ms);
                }));
                res = ms.ToArray();
            }
            return res;
        }
        public static void setData(byte[] pic)
        {
            using (MemoryStream ms = new MemoryStream(pic))
            {
                canv.Dispatcher.Invoke(new Action(() =>
                {
                    canv.Strokes = new System.Windows.Ink.StrokeCollection(ms);
                }));
                ms.Close();
            }
        }
        public static void setUserList(String[] names)
        {
            UserList.Dispatcher.Invoke(new Action(() => 
            {
                UserList.Items.Clear();
                foreach (var item in names)
                {
                    if (!String.IsNullOrEmpty(item))
                    {
                        UserList.Items.Add(item);                        
                    }
                }
            }));
        }
        public static void addChat(String msg)
        {
            Chat.Dispatcher.Invoke(new Action(() =>
            {
                Chat.Items.Add(msg);
            }));
        }
        public static string OpenChooser(string[] s)
        {
            canv.Dispatcher.Invoke(new Action(() =>
            {
                Chooser c = new Chooser();
                c.setWords(s);
                c.ShowDialog();
                lword.Content = word;
            }));
            return word;
        }
        public static void SetPoints(byte[] list)
        {
            Points.Dispatcher.Invoke(new Action(() =>
            {
                Points.Items.Clear();
                string s = HostController.getData(list);
                string[] ps = s.Split('#');
                foreach (var item in ps)
                {
                    Points.Items.Add(item.Replace("\n",""));
                }
            }));
        }
        public static void SetHint(byte p)
        {
            string s = "";
            for (int i = 0; i < p; i++)
            {
                s += "_ ";
            }
            lhint.Dispatcher.Invoke(new Action(() => 
            {
                lhint.Content = s;
            }));
        }
        internal static void ClearLabels()
        {
            lhint.Content = "";
            lword.Content = "";
        }
    }
}
