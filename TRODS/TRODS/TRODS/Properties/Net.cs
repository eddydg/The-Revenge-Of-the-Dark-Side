using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;


namespace EugLib.Net
{
    public class Server
    {
        private Socket server;
        private IPEndPoint ipendPoint;
        private List<Socket> clients;
        private bool initialized;
        private bool binded;
        private int connexionsRemining;
        private byte[] key;

        public Server(int port, ProtocolType protocol = ProtocolType.Tcp, int maxConnectionsNumber = 1, string connectionKey = "")
        {
            initialized = false;
            binded = false;
            connexionsRemining = maxConnectionsNumber;
            clients = new List<Socket>();
            key = System.Text.Encoding.UTF8.GetBytes(connectionKey);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, protocol);
            Initialize(port);
        }

        public void SendData(byte[] data)
        {
            if (initialized && binded)
            {
                try
                {
                    foreach (Socket c in clients)
                    {
                        c.Send(BitConverter.GetBytes(data.Length));
                        c.Send(data);
                    }
                }
                catch (Exception e)
                {
                    IO.FileStream.toStdOut("Server sending data error :\n" + e.ToString());
                }
            }
            else
                IO.FileStream.toStdOut("Trying to send data with uninitialised server.");
        }
        public void SendString(string s)
        {
            SendData(System.Text.Encoding.UTF8.GetBytes(s));
        }
        public byte[] RecieveData(int client = 0)
        {
            if (initialized && binded)
            {
                try
                {
                    if (client >= 0 && client < clients.Count)
                    {
                        byte[] len = new byte[4];
                        clients[client].Receive(len);
                        byte[] data = new byte[BitConverter.ToInt32(len, 0)];
                        clients[client].Receive(data);
                        return data;
                    }
                    else
                    {
                        IO.FileStream.toStdOut("Server trying to recieve data from undefined client.");
                        return null;
                    }
                }
                catch (Exception e)
                {
                    IO.FileStream.toStdOut("Server recieving data error :\n" + e.ToString());
                    return null;
                }
            }
            else
            {
                IO.FileStream.toStdOut("Trying to recieve data on uninitialised server.");
                return null;
            }
        }
        public string RecieveString(int client = 0)
        {
            return System.Text.Encoding.UTF8.GetString(RecieveData(client));
        }

        public bool Initialize(int port)
        {
            if (port >= IPEndPoint.MinPort && port <= IPEndPoint.MaxPort)
            {
                ipendPoint = new IPEndPoint(IPAddress.Any, port);
                initialized = true;
            }
            return initialized;
        }
        public bool BindPort()
        {
            if (initialized && !binded)
            {
                server.Bind(ipendPoint);
                server.Listen(connexionsRemining);
                IO.FileStream.toStdOut("Listening port : " + ipendPoint.Port);
                binded = true;
            }
            else
                IO.FileStream.toStdOut("Trying to bind port on uninitialised server.");
            return binded;
        }
        public void AcceptConnection(bool checkIdentity = false)
        {
            if (initialized && binded)
            {
                try
                {
                    clients.Add(server.Accept());
                    IO.FileStream.toStdOut("Client connected : " + clients.Last().RemoteEndPoint.ToString());
                    connexionsRemining--;
                    if (checkIdentity)
                    {
                        if (RecieveData(clients.Count - 1) == key)
                        {
                            IO.FileStream.toStdOut("Client authentified");
                        }
                        else
                        {
                            IO.FileStream.toStdOut("Removing unauthentified client");
                            clients.Last().Close();
                            clients.RemoveAt(clients.Count - 1);
                        }
                    }
                }
                catch (Exception e)
                {
                    IO.FileStream.toStdOut("Waiting connection error :\n" + e.ToString());
                }
            }
            else
                IO.FileStream.toStdOut("Trying to send data on uninitialised server.");
        }
        public List<Socket> CheckDisconnected()
        {
            List<Socket> disconnected = new List<Socket>();
            for (int i = 0; i < clients.Count; i++)
            {
                if (!clients[i].Connected)
                {
                    disconnected.Add(clients[i]);
                    clients.RemoveAt(i);
                    i--;
                }
            }
            foreach (Socket d in disconnected)
            {
                IO.FileStream.toStdOut("Client disconnected : " + d.RemoteEndPoint.ToString());
            }
            return disconnected;
        }
        public void DisconnectAll()
        {
            for (int i = 0; i < clients.Count; i++)
                DisconnectClient(0);
        }
        public void DisconnectClient(int client)
        {
            if (client >= 0 && client < clients.Count)
            {
                IO.FileStream.toStdOut("Disconnecting client : " + clients[client].RemoteEndPoint.ToString());
                clients[client].Close();
                clients.RemoveAt(client);
            }
            else
                IO.FileStream.toStdOut("Trying to disconnect unknown client");
        }
        public void Close()
        {
            IO.FileStream.toStdOut("Closing all connections.");
            try
            {
                foreach (Socket s in clients)
                    s.Close();
                server.Close();
            }
            catch (Exception e)
            {
                IO.FileStream.toStdOut("Error closing connections : " + e.ToString());
            }
        }

        public bool Initialized()
        {
            return initialized;
        }
        public bool Binded()
        {
            return binded;
        }
        public bool Connected()
        {
            return server.Connected;
        }
    }
    public class Client
    {
        private Socket client;
        private IPEndPoint ipendPoint;
        private bool initialized;
        private byte[] key;

        public Client(IPAddress ip, int port, ProtocolType protocol = ProtocolType.Tcp, string connectionKey = "")
        {
            initialized = false;
            key = System.Text.Encoding.UTF8.GetBytes(connectionKey);
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, protocol);
            Initialize(ip, port);
        }

        public void SendData(byte[] data)
        {
            if (initialized)
            {
                try
                {
                    client.Send(BitConverter.GetBytes(data.Length));
                    client.Send(data);
                }
                catch (Exception e)
                {
                    IO.FileStream.toStdOut("Client sending data error :\n" + e.ToString());
                }
            }
            else
                IO.FileStream.toStdOut("Trying to send data with uninitialised client.");
        }
        public void SendString(string s)
        {
            SendData(System.Text.Encoding.UTF8.GetBytes(s));
        }
        public byte[] RecieveData()
        {
            if (initialized)
            {
                try
                {
                    byte[] len = new byte[4];
                    client.Receive(len);
                    byte[] data = new byte[BitConverter.ToInt32(len, 0)];
                    client.Receive(data);
                    return data;
                }
                catch (Exception e)
                {
                    IO.FileStream.toStdOut("Client recieving data error :\n" + e.ToString());
                    return null;
                }
            }
            else
            {
                IO.FileStream.toStdOut("Trying to recieve data on uninitialised client.");
                return null;
            }
        }
        public string RecieveString(int client = 0)
        {
            return System.Text.Encoding.UTF8.GetString(RecieveData());
        }

        public bool Initialize(IPAddress ip, int port)
        {
            if (port >= IPEndPoint.MinPort && port <= IPEndPoint.MaxPort)
            {
                ipendPoint = new IPEndPoint(ip, port);
                initialized = true;
            }
            return initialized;
        }
        public bool Connect(bool identify = false)
        {
            if (initialized)
            {
                try
                {
                    client.Connect(ipendPoint);
                    IO.FileStream.toStdOut("Connected to server.");
                    if (identify)
                    {
                        IO.FileStream.toStdOut("Identifying.");
                        SendData(key);
                    }
                }
                catch (Exception e)
                {
                    IO.FileStream.toStdOut("Connection to server failed :\n" + e.ToString());
                }
            }
            else
                IO.FileStream.toStdOut("Trying to connect with an uninitialized client.");
            return client.Connected;
        }
        public void Disconnect()
        {
            if (initialized)
            {
                IO.FileStream.toStdOut("Disconnecting from server.");
                client.Disconnect(true);
            }
            else
                IO.FileStream.toStdOut("Disconnection from server failed.");
        }

        public bool Connected()
        {
            return client.Connected;
        }
        public bool Initialized()
        {
            return initialized;
        }
        public void Close()
        {
            IO.FileStream.toStdOut("Closing connection.");
            client.Close();
        }
    }
}
