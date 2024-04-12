#include "Server.h"

#include "MessageIdentifiers.h"
#include "BitStream.h"
#include "RakNetTypes.h"
#include "Gets.h"

#include "RakSleep.h"

#include "Networking/PacketIdentifiers.h"


static RakNet::RakPeerInterface* s_PeerInterface;
static ClientConnectCallback s_ClientConnectCallback;

// Returns true if successful, otherwise returns false
bool StartServer(const char* IP, int Port, int MaxClients)
{
	printf("[Core] Server started at address %s:%d (%d)\n", IP, Port, MaxClients);

	s_PeerInterface = RakNet::RakPeerInterface::GetInstance();

	RakNet::SocketDescriptor sd((unsigned short)Port, IP);
	sd.socketFamily = AF_INET;

	if (s_PeerInterface->Startup(MaxClients, &sd, 1) != RakNet::StartupResult::RAKNET_STARTED)
		return false;

	s_PeerInterface->SetMaximumIncomingConnections(20);

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
		printf("[Core]: Incoming packet: %d\n", packet->data[0]);

		switch (packet->data[0])
		{
			case PacketID::CLIENT_DATA:
			{
				if (s_ClientConnectCallback)
				s_ClientConnectCallback("teszt69420", 69);

				printf("[Core]: Client connected\n");

				break;
			}

			case 0x10:
			{
	
				break;
			}

			case ID_CONNECTION_LOST:
			{
				break;
			}

			default:
			{
				printf("[Core]: Message with identifier %i has arrived:\n", packet->data[0]);
				break;
			}
		}
	}
}

void SetClientConnectHandler(ClientConnectCallback callback)
{
	s_ClientConnectCallback = callback;
}
