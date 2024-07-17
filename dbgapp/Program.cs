
using Microsoft.VisualBasic;
using System.ComponentModel.Design;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Windows.Input;


class Program
{
    //hardcoded dll path //a budos kurva anyjat neki amugy
    private const string SC_CoreDLL = @"SC_Core.dll";


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

    private enum CtrlType
    {
        CTRL_C_EVENT = 0,
        CTRL_BREAK_EVENT = 1,
        CTRL_CLOSE_EVENT = 2,
        CTRL_LOGOFF_EVENT = 5,
        CTRL_SHUTDOWN_EVENT = 6
    }


    [DllImport("Kernel32")]
    private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

    private delegate bool EventHandler(CtrlType sig);
    static EventHandler _handler;
    private static bool Handler(CtrlType signal)
    {

        switch (signal)
        {
            case CtrlType.CTRL_BREAK_EVENT:
            case CtrlType.CTRL_C_EVENT:
            case CtrlType.CTRL_LOGOFF_EVENT:
            case CtrlType.CTRL_SHUTDOWN_EVENT:
            case CtrlType.CTRL_CLOSE_EVENT:
                Console.Beep();
                DisconnectFromServer();
                Thread.Sleep(100);
                return false;

            default:
                return false;
        }
    }


    public static void Main()
    {

        SetIntroduceClientHandler(OnIntroduceClient);
        SetChatroomMessageHandler(OnIncomingChatroomMessage);
        SetConnectionAcceptedHandler(OnConnectionAccepted);

        Console.WriteLine("gimme name");
        string? Name = Console.ReadLine();

        if (Name == null)
            return;

        if (ConnectToServer("127.0.0.1", 7000, Name))
        {
            Console.WriteLine("Connecting to server");
        }
        else Console.WriteLine("Couldn't connect to server");


        _handler += new EventHandler(Handler);

        // Register the handler
        if (!SetConsoleCtrlHandler(_handler, true))
        {
            Console.WriteLine("Failed to set Console Control Handler");
        }

        while (true)
        {
            ClientUpdate();

            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {

                    case ConsoleKey.F1:
                    {
                        Console.WriteLine("Sent chatroom message");
                        SendChatroomMessage("FAASGMNLASGFASTZ", 69);
                        break;
                    }


                    case ConsoleKey.F2:
                    {
                        DisconnectFromServer();
                        Console.WriteLine("Disconnected from server.");
                        break;
                    }

                    default:
                        break;
                }
            }
        }
    }
    
 };