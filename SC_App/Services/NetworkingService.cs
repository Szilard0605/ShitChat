using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SC_App.Services
{
    public delegate void ClientConnectCallback(string Name, int ID);
    public delegate void ConnectionAcceptedCallback(int ID);

    public class NetworkingService
    {
        private const string SC_CoreDLL = @"SC_Core.dll";

        public class Client
        {
            public static bool IsConnected;

            [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool ConnectToServer(string ip, int port, string name);

            [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DisconnectFromServer();

            [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ClientUpdate();

            // Client side event
            // gets called when the server accepts the connection,
            // and sends the client's ID
            [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetConnectionAcceptedHandler(ConnectionAcceptedCallback callback);

            /*public static void OnConnectionAccepted(int id)
            {
                Debug.WriteLine($"Connection accepted by the server. {id}");
            }*/
        }

        public class Server
        {
            public static bool IsStarted;

            [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool StartServer(string ip, int port, int maxClients);

            [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ShutdownServer();

            [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern void UpdateServer();

            // Import the SetEventCallback function from the DLL
            [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetClientConnectHandler(ClientConnectCallback callback);

            [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetClientIDByAddress(string Address);

            [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern string GetClientAddressByID(string ID);

            [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern Int64 GetConnectedClientsCount();

            /*public static void OnClientConnect(string name, int id)
            {
                Debug.WriteLine($"{name}[{id}] connected");
            }*/
        }
    }
}
