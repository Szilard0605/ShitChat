
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

    public static void Main()
    {

        int option = 0;

        Console.WriteLine("1 client, 2 server");
        option = int.Parse(s: Console.ReadLine());

        if (option == 2)
        {
            SetClientConnectHandler(OnClientConnect);

            if (StartServer("192.168.0.34", 7000, 20))
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
            if (ConnectToServer("192.168.0.34", 7000, "tesztnev"))
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