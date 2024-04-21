
using System.ComponentModel.Design;
using System.Runtime.InteropServices;

class Program
{
    //hardcoded dll path //a budos kurva anyjat neki amugy
    private const string SC_CoreDLL = @"SC_Core.dll";

    [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool StartServer(string ip, int port, int maxClients);

    [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ShutdownServer();

    [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void UpdateServer();

    [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool ConnectToServer(string ip, int port, string name);

    [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void DisconnectFromServer();

    [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ClientUpdate();

    // Import the SetEventCallback function from the DLL
    [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetClientConnectHandler(ClientConnectCallback callback);
    public delegate void ClientConnectCallback(string Name, int ID);

    // Client side event
    // gets called when the server accepts the connection,
    // and sends the client's ID
    [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetConnectionAcceptedHandler(ConnectionAcceptedCallback callback);
    public delegate void ConnectionAcceptedCallback(int ID);

    [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetClientIDByAddress(string Address);

    [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern string GetClientAddressByID(string ID);

    [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern Int64 GetConnectedClientsCount();

    [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SendChatroomMessage(string Message, int RoomID);

    [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetChatroomMessageHandler(ChatroomMessageCallback callback);

    public delegate void ChatroomMessageCallback(int ClientID, string Message, int RoomID);

    [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetIntroduceClientHandler(IntroduceClientCallback callback);
    public delegate void IntroduceClientCallback(int ClientID, string Name);

    public static void OnClientConnect(string name, int id)
    {
        Console.WriteLine($"{name}[{id}] connected");
    }

    public static void OnIntroduceClient(int ClientID, string Name)
    {
        Console.WriteLine($"[Introduce]: {Name}[{ClientID}]");
    }

    public static void OnIncomingChatroomMessage(int ClientID, string Message, int RoomID)
    {
        Console.WriteLine($"[Chatroom msg]{ClientID} sent \"{Message}\" to Room {RoomID}");
    }

    public static void OnConnectionAccepted(int ID)
    {
        Console.WriteLine($"Connected! Assigned ID: {ID}");
    }

    public static void Main()
    {

        int option = 0;

        Console.WriteLine("1 client, 2 server");
        option = int.Parse(s: Console.ReadLine());

        if (option == 2)
        {
            SetClientConnectHandler(OnClientConnect);

            if (StartServer("192.168.1.68", 7000, 20))
            {
                Console.WriteLine("Server started");
            }
            else
            {
                Console.WriteLine("Couldn't start server");
            }

            while (true)
            {
                UpdateServer();
            }
        }
        else
        {

            SetIntroduceClientHandler(OnIntroduceClient);
            SetChatroomMessageHandler(OnIncomingChatroomMessage);
            SetConnectionAcceptedHandler(OnConnectionAccepted);

            Console.WriteLine("gimme name");
            string Name = Console.ReadLine();

            if (ConnectToServer("192.168.1.68", 7000, Name))
            {
                Console.WriteLine("Connecting to server");
            }
            else Console.WriteLine("Couldn't connect to server");

            while(true)
            {
                ClientUpdate();
            }
        }
    }
    
 };