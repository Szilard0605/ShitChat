#pragma once

#include "Base.h"

extern "C"
{
	SC_EXPORT bool ConnectToServer(const char* IP, int Port, const char* Name);
	SC_EXPORT void DisconnectFromServer();
	SC_EXPORT void ClientUpdate();
}
