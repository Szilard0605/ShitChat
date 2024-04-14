#pragma once

#include "Base.h"

typedef void (*ConnectionAcceptedCallback)(int ID);

extern "C"
{
	SC_EXPORT bool ConnectToServer(const char* IP, int Port, const char* Name);
	SC_EXPORT void DisconnectFromServer();
	SC_EXPORT void ClientUpdate();

	SC_EXPORT void SetConnectionAcceptedHandler(ConnectionAcceptedCallback callback);
}
