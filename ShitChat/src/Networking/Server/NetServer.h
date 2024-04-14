#pragma once

#include "Base.h"
#include "RakPeerInterface.h"

typedef void (*ClientConnectCallback)(const char* Name, int ID);
typedef void (*ClientDisconnectCallback)(int ID);


extern "C"
{
	SC_EXPORT bool StartServer(const char* IP, int Port, int MaxClients);
	SC_EXPORT void ShutdownServer();
	SC_EXPORT void UpdateServer();

	SC_EXPORT void SetClientConnectHandler(ClientConnectCallback callback);
	SC_EXPORT void SetClientDisconnectHandler(ClientDisconnectCallback callback);

	SC_EXPORT int GetClientIDByAddress(const char* Address);
	SC_EXPORT const char* GetClientAddressByID(int ID);

	SC_EXPORT __int64 GetConnectedClientsCount();
}
