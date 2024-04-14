#include "NetClient.h"


#include "RakPeerInterface.h"
#include "MessageIdentifiers.h"
#include "BitStream.h"
#include "RakNetTypes.h"
#include "Gets.h"

#include "Networking/PacketIdentifiers.h"

#include <string>

static RakNet::RakPeerInterface* s_PeerInterface;

#define MAX_NAME_LENGTH 14

static ConnectionAcceptedCallback s_ConnectionAcceptedCallback;

struct ClientData
{
	std::string Name;
	int ID;
} s_ClientData;

bool ConnectToServer(const char* IP, int Port, const char* Name)
{
	s_ClientData.Name = Name;

	s_PeerInterface = RakNet::RakPeerInterface::GetInstance();

	RakNet::SocketDescriptor sd;
	if (s_PeerInterface->Startup(1, &sd, 1) != RakNet::StartupResult::RAKNET_STARTED)
		return false;

	if (s_PeerInterface->Connect(IP, (unsigned short)Port, 0, 0) != RakNet::ConnectionAttemptResult::CONNECTION_ATTEMPT_STARTED)
		return false;

	return true;
}

void DisconnectFromServer()
{
	s_PeerInterface->Shutdown(1);
}

void ClientUpdate()
{
	RakNet::Packet* packet;

	for (packet = s_PeerInterface->Receive(); packet; s_PeerInterface->DeallocatePacket(packet), packet = s_PeerInterface->Receive())
	{
		switch (packet->data[0])
		{
			case ID_CONNECTION_REQUEST_ACCEPTED:
			{
				printf("[Core]: sending name: %s\n", s_ClientData.Name.c_str());

				// Send client data to server
				RakNet::BitStream bsOut;
				bsOut.Write(PacketID::CLIENT_DATA);
				bsOut.Write(s_ClientData.Name.c_str());
				s_PeerInterface->Send(&bsOut, HIGH_PRIORITY, RELIABLE_ORDERED, 0, packet->systemAddress, false);
				
				printf("[Core]: Connection request accepted\n");

				break;
			}

			case CLIENT_DATA:
			{
				int inID;
				RakNet::RakString userName;
				RakNet::BitStream bsIn(packet->data, packet->length, false);
				bsIn.IgnoreBytes(sizeof(PacketID::CLIENT_DATA));
				bsIn.Read(inID);
				
				if(s_ConnectionAcceptedCallback)
					s_ConnectionAcceptedCallback(inID);
				break;
			}

			default:
			{
				//printf("[Core]: Message with identifier %i has arrived:\n", packet->data[0]);
				break;
			}
		}
	}
}

SC_EXPORT void SetConnectionAcceptedHandler(ConnectionAcceptedCallback callback)
{
	s_ConnectionAcceptedCallback = callback;
}
