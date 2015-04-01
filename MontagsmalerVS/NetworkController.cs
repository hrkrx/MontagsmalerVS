using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MontagsmalerVS
{
    public class NetworkController
    {
        String hostip = "";
        bool hostmode = false;
        BackgroundWorker bw = new BackgroundWorker();
        TcpClient client = new TcpClient();
        private void init()
        {
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += bw_DoWork;
        }
        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                client.Connect(hostip, 8000);
            }
            catch (Exception)
            {
                throw;
            }
            sendName(MainController.lname);
            while (!bw.CancellationPending)
            {
                var serverStream = client.GetStream();
                int buffSize = 0;
                buffSize = client.ReceiveBufferSize;
                byte[] inStream = new byte[buffSize];
                serverStream.Read(inStream, 0, buffSize);
                switch (inStream[0])
                {
                    case 0:
                        break;
                    case 1:
                        receivedChat(inStream);
                        break;
                    case 2:
                        if (!MainController.drawing)
                        {
                            received(inStream);                            
                        }
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        if (inStream[1] == 1)
                        {
                            MainController.drawing = true;
                        }
                        else
                        {
                            MainController.canv.Dispatcher.Invoke(new Action(() =>
                            {
                                MainController.canv.EditingMode = System.Windows.Controls.InkCanvasEditingMode.None;
                            }));
                            MainController.drawing = false;
                        }
                        break;
                    case 6:
                        receivedWords(inStream);
                        break;
                    case 7:
                        receivedNames(inStream);
                        break;
                    case 8:
                        MainController.timerCounter = inStream[1];
                        break;
                    default:
                        break;
                }
            }
        }
        public NetworkController(bool host)
        {
            hostmode = true;
            HostController.beginHosting();
        }
        public NetworkController(string IP)
        {
            hostip = IP;
            init();
        }
        public void send(byte[] pic)
        {
            if (client.Connected)
            {
                byte[] data = new byte[pic.Length + 1];
                data[0] = 2;
                for (int i = 0; i < pic.Length; i++)
                {
                    data[i + 1] = pic[i];
                }
                var stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                stream.Flush();
            }
        }
        public void received(byte[] pic)
        {
            byte[] data = new byte[pic.Length - 1];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = pic[i + 1];
            }
            MainController.setData(data);

        }
        public void receivedChat(byte[] chat)
        {
            byte[] data = new byte[chat.Length - 1];
             for (int i = 0; i < data.Length; i++)
            {
                data[i] = chat[i + 1];
            }
             MainController.addChat(Encoding.UTF8.GetString(data).Split('~')[0]);
        }
        public void receivedNames(byte[] names)
        {
            byte[] data = new byte[names.Length - 1];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = names[i + 1];
            }
            String[] users = Encoding.UTF8.GetString(data).Split('~')[0].Split('#');
            MainController.setUserList(users);
        }
        private void receivedWords(byte[] inStream)
        {
            string[] s = HostController.getData(inStream).Split('#');
            sendWord(MainController.OpenChooser(s));
        }
        public void send(string chat)
        {
            if (client.Connected)
            {
                byte[] text = Encoding.UTF8.GetBytes(chat + "~");
                byte[] data = new byte[text.Length + 1];
                data[0] = 1;
                for (int i = 0; i < text.Length; i++)
                {
                    data[i + 1] = text[i];
                }
                var stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                stream.Flush();
                
            }
        }
        public void sendWord(string word)
        {
            if (client.Connected)
            {
                byte[] text = Encoding.UTF8.GetBytes(word + "~");
                byte[] data = new byte[text.Length + 1];
                data[0] = 9;
                for (int i = 0; i < text.Length; i++)
                {
                    data[i + 1] = text[i];
                }
                var stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                stream.Flush();

            }
        }
        public void sendName(string name)
        {
            if (client.Connected)
            {
                byte[] text = Encoding.UTF8.GetBytes(name + "~");
                byte[] data = new byte[text.Length + 1];
                data[0] = 0;
                for (int i = 0; i < text.Length; i++)
                {
                    data[i + 1] = text[i];
                }
                var stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                stream.Flush();
            }
        }
        public void deregister()
        {
            byte[] text = Encoding.UTF8.GetBytes(MainController.lname + "~");
            byte[] data = new byte[text.Length + 1];
            data[0] = 4;
            for (int i = 0; i < text.Length; i++)
            {
                data[i + 1] = text[i];
            }
            var stream = client.GetStream();
            stream.Write(data, 0, data.Length);
            stream.Flush();
        }
        public bool connect(String IP)
        {
            bool res = false;
            hostip = IP;
            bw.RunWorkerAsync();
            res = true;
            return res;
        }
    }
}
