using System;
using System.IO;
using System.Runtime.InteropServices;

namespace SC_App.Services
{
    public class NetworkingService
    {
        public static bool IsServerStarted;
        //hardcoded dll path //a budos kurva anyjat neki amugy
        private const string SC_CoreDLL = @"E:\Projects\ShitChat\x64\Debug\SC_Core.dll";

        [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void StartServer(string ip, int port, int maxClients);

        [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ShutdownServer();

        [DllImport(SC_CoreDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void UpdateServer();
    }
}
