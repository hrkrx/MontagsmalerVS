﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MontagsmalerVS
{
    public static class HostController
    {
        static BackgroundWorker bw = new BackgroundWorker();
        static TcpListener listener = new TcpListener(IPAddress.Any, 8000);
        static List<TcpClient> clientsList = new List<TcpClient>();
        static List<string> clientNames = new List<string>();
        static int counter = 0;
        static string word = "";
        static List<string> words = new List<string>();
        static List<int> points = new List<int>();
        static List<handleClient> hClients = new List<handleClient>();
        static int currentPlayerIndex = 0;
        static Random r = new Random(DateTime.Now.Millisecond);
        public static List<String> getNames()
        {
            return clientNames;
        }
        public static void beginHosting()
        {
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += bw_DoWork;
            bw.RunWorkerAsync();
        }
        public static void endHosting()
        {
            bw.CancelAsync();
            bw.Dispose();
        }
        private static void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            listener.Start();
            while (!bw.CancellationPending)
            {
                TcpClient client = listener.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem(ThreadProc, client);
            }
            listener.Stop();
        }
        private static void ThreadProc(object state)
        {
            var client = (TcpClient)state;
            handleClient hclient = new handleClient();
            hclient.startClient(client, counter.ToString() + " - " + client.Client.RemoteEndPoint.ToString());
            hClients.Add(hclient);
            clientsList.Add(client);
            clientNames.Add(counter.ToString());
            counter++;
        }
        public static int getClientNumber(string name)
        {
            int res = -1;
            res = clientNames.IndexOf(name);
            return res;
        }
        public static int getClientNumber(TcpClient client)
        {
            int res = -1;
            res = clientsList.IndexOf(client);
            return res;
        }
        public static void broadcast(byte[] msg, string uName, bool pic)
        {
            foreach (TcpClient Item in clientsList)
            {
                if (Item != null)
                {
                    try
                    {
                        TcpClient broadcastSocket;
                        broadcastSocket = Item;
                        NetworkStream broadcastStream = broadcastSocket.GetStream();
                        Byte[] broadcastBytes = null;

                        broadcastBytes = msg;

                        Debug.WriteLine("Send Data to everyone from " + uName);
                        broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                        broadcastStream.Flush();
                    }
                    catch (Exception)
                    {

                    }

                }
            }
        }
        public static void deregister(byte[] data)
        {
            string name = "";
            byte[] d = new byte[data.Length - 1];
            for (int i = 0; i < d.Length; i++)
            {
                d[i] = data[i + 1];
            }
            name = Encoding.UTF8.GetString(d);
            int index = clientNames.IndexOf(name.Split('~')[0]);
            clientNames.RemoveAt(index);
            clientsList.RemoveAt(index);
            hClients[index].cancel = true;
            hClients.RemoveAt(index);
        }
        public static void enabledrawing(int clientNo)
        {
            TcpClient cl = clientsList[clientNo];
            var str = cl.GetStream();
            byte[] data = new byte[2];
            data[0] = 5;
            data[1] = 1;
            str.Write(data, 0, data.Length);
            str.Flush();
        }
        public static void disabledrawing()
        {
            for (int i = 0; i < clientsList.Count; i++)
            {
                disabledrawing(i);
            }
        }
        public static void disabledrawing(int clientNo)
        {
            TcpClient cl = clientsList[clientNo];
            var str = cl.GetStream();
            byte[] data = new byte[2];
            data[0] = 5;
            data[1] = 0;
            str.Write(data, 0, data.Length);
            str.Flush();
        }
        public static void sendWords(int clientNo, string w1, string w2, string w3)
        {
            string str = w1 + "#" + w2 + "#" + w3 + "~";
            TcpClient cl = clientsList[clientNo];
            var stream = cl.GetStream();
            byte[] data = new byte[str.Length + 1];
            data[0] = 6;
            byte[] s = Encoding.UTF8.GetBytes(str);
            for (int i = 0; i < s.Length; i++)
            {
                data[1 + i] = s[i];
            }
            stream.Write(data, 0, data.Length);
            stream.Flush();
        }
        public static bool checkWord(byte[] dataFromClient)
        {
            byte[] data = new byte[dataFromClient.Length - 1];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = dataFromClient[i + 1];
            }
            string chat = Encoding.UTF8.GetString(data);
            if (word != "")
            {
                return chat.Split('~')[0].Split(':')[1].Trim() == word;
            }
            return false;
        }
        public static string[] randomWords()
        {
            string[] res = new string[3];
            res[0] = words[r.Next(words.Count - 1)];
            res[1] = words[r.Next(words.Count - 1)];
            res[2] = words[r.Next(words.Count - 1)];
            return res;
        }
        public static void loadWordList(string file)
        {
            words.Clear();
            StreamReader sr = new StreamReader(file);
            while(!sr.EndOfStream)
            {
                words.Add(sr.ReadLine());
            }
        }
        public static void setTimer(byte sec)
        {
            byte[] data = new byte[2];
            data[0] = 8;
            data[1] = sec;
            broadcast(data, "Server", false);
        }
        internal static void setWord(byte[] dataFromClient)
        {
            word = getData(dataFromClient);
        }
        internal static void beginGame()
        {
            enabledrawing(currentPlayerIndex);
            setTimer(60);
        }

        private static string getData(byte[] data)
        {
            string res = "";
            byte[] s = new byte[data.Length - 1];
            for (int i = 0; i < s.Length; i++)
            {
                s[i] = data[i + 1];
            }
            res = Encoding.UTF8.GetString(data).Split('~')[0];
            return res;
        }
    }

    public class handleClient
    {
        TcpClient clientSocket;
        string clNo;
        public bool cancel = false;
        public void startClient(TcpClient inClientSocket, string clineNo)
        {
            this.clientSocket = inClientSocket;
            this.clNo = clineNo;
            Thread ctThread = new Thread(doChat);
            Debug.WriteLine("Start Server");
            ctThread.Start();
        }

        private void doChat()
        {
            int requestCount = 0;
            Byte[] dataFromClient = null;
            string rCount = null;
            requestCount = 0;

            while ((!cancel))
            {
                try
                {

                    byte[] bytesFrom = new byte[(int)clientSocket.ReceiveBufferSize];

                    requestCount = requestCount + 1;
                    NetworkStream networkStream = clientSocket.GetStream();
                    networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                    dataFromClient = bytesFrom;
                    Debug.WriteLine("From client - " + clNo + " : " + dataFromClient[0] + " Size : " + dataFromClient.Length);
                    rCount = Convert.ToString(requestCount);
                    switch (dataFromClient[0])
                    {
                        case 0:
                            Debug.WriteLine("broadcast Login :" + dataFromClient[0]);
                            byte[] name = new byte[dataFromClient.Length - 1];
                            for (int i = 1; i < dataFromClient.Length; i++)
                            {
                                name[i - 1] = dataFromClient[i];
                            }
                            HostController.getNames()[HostController.getClientNumber(clientSocket)] = Encoding.UTF8.GetString(name).Split('~')[0];
                            string dn = "";
                            foreach (var item in HostController.getNames())
                            {
                                dn += item + "#";
                            }
                            byte[] data = new byte[dn.Length + 2];
                            byte[] str = Encoding.UTF8.GetBytes(dn + "~");
                            data[0] = 7;
                            for (int i = 0; i < str.Length; i++)
                            {
                                data[i + 1] = str[i];
                            }
                            HostController.broadcast(data, clNo, false);
                            break;
                        case 1:
                            Debug.WriteLine("broadcast Chat :" + dataFromClient[0]);
                            if (!HostController.checkWord(dataFromClient))
                            {
                                HostController.broadcast(dataFromClient, clNo, false);
                            }

                            break;
                        case 2:
                            Debug.WriteLine("broadcast Picture :" + dataFromClient[0]);
                            HostController.broadcast(dataFromClient, clNo, true);
                            break;
                        case 3:
                            Debug.WriteLine("broadcast Command :" + dataFromClient[0]);
                            HostController.broadcast(dataFromClient, clNo, false);
                            break;
                        case 4:
                            Debug.WriteLine("deregistering Client");
                            HostController.deregister(dataFromClient);
                            break;
                        case 9:
                            Debug.WriteLine("recieved Word");
                            HostController.setWord(dataFromClient);
                            HostController.beginGame();
                            break;
                        default:
                            Debug.WriteLine("Unknown Command :" + dataFromClient[0]);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }//end while
            //end doChat
        } //end class handleClinet
    }
}