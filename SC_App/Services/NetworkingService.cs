using System;
using System.Runtime.InteropServices;

namespace SC_App.Services
{
    public class NetworkingService
    {
        public static bool IsServerStarted;
        public static bool IsClientConnected;
        // hardcoded dll path
        private const string SC_CoreDLL = @"SC_Core.dll";

        [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool StartServer(string ip, int port, int maxClients);

        [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ShutdownServer();

        [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void UpdateServer();

        // Import the SetEventCallback function from the DLL
        [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetClientConnectHandler(ClientConnectCallback callback);
        public delegate void ClientConnectCallback(string Name, int ID);


        [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ConnectToServer(string ip, int port, string name);

        [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void DisconnectFromServer();

        [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ClientUpdate();

        public static void OnClientConnect(string name, int id)
        {
            Console.WriteLine($"{name}[{id}] connected");
        }
    }
}
