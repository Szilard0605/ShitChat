using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SC_App.Services
{

    public class NetworkingService
    {
        private const string SC_CoreDLL = @"SC_Core.dll";

        public class Client
        {
            public delegate void IntroduceClientCallback(int ClientID, string Name);
            public delegate void ConnectionAcceptedCallback(int ClientID);
            public delegate void ChatroomMessageCallback(int ClientID, string Message, int RoomID);

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

            [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern void SendChatroomMessage(string Message, int RoomID);

            [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetChatroomMessageHandler(ChatroomMessageCallback callback);

            [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetIntroduceClientHandler(IntroduceClientCallback callback);

        }

        public class Server
        {
            public delegate void ClientConnectCallback(string Name, int ID);

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
        }
    }
}
