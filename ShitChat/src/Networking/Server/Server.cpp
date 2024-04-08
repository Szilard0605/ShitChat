#include "Server.h"

#include "MessageIdentifiers.h"
#include "BitStream.h"
#include "RakNetTypes.h"
#include "Gets.h"


static RakNet::RakPeerInterface* s_PeerInterface;
static RakNet::SocketDescriptor s_SocketDesc;

// Returns true if successful, otherwise returns false
bool StartServer(const char* IP, int Port, int MaxClients)
{
	s_PeerInterface = RakNet::RakPeerInterface::GetInstance();
	s_SocketDesc = RakNet::SocketDescriptor(Port, IP);
	s_SocketDesc.socketFamily = AF_INET;

	if (s_PeerInterface->Startup(MaxClients, &s_SocketDesc, 1) != RakNet::StartupResult::RAKNET_STARTED)
		return false;

	return true;
}

void ShutdownServer()
{
	s_PeerInterface->Shutdown(0);
	RakNet::RakPeerInterface::DestroyInstance(s_PeerInterface);
}

void UpdateServer()
{
	RakNet::Packet* packet;
	for (packet = s_PeerInterface->Receive(); packet; s_PeerInterface->DeallocatePacket(packet), packet = s_PeerInterface->Receive())
	{
		switch (packet->data[0])
		{
			case ID_DISCONNECTION_NOTIFICATION:
			{
				break;
			}

			case ID_CONNECTION_LOST:
			{
				break;
			}

		}
	}
}
