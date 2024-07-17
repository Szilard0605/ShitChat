#include "NetClient.h"


#include "Networking/RakNet/RakPeerInterface.h"
#include "Networking/RakNet/MessageIdentifiers.h"
#include "Networking/RakNet/BitStream.h"
#include "Networking/RakNet/RakNetTypes.h"
#include "Networking/RakNet/Gets.h"

#include "PacketIdentifiers.h"
#include "Networking/Common/Utils.h"

#include <string>
#include "Networking/RakNet/RakNetTypes.h"

static RakNet::RakPeerInterface* s_PeerInterface;
static RakNet::SystemAddress s_ServerSysAddr;

#define MAX_NAME_LENGTH 14

static ConnectionAcceptedCallback s_ConnectionAcceptedCallback;
static ChatroomMessageCallback s_ChatroomMessageCallback;
static IntroduceClientCallback s_IntroduceClientCallback;
static ClientDisconnectCallback s_ClientDisconnectCallback;

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
	s_PeerInterface->CloseConnection(s_ServerSysAddr, true);
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
				// Send client data to server
				RakNet::BitStream bsOut;
				bsOut.Write(PacketID::CLIENT_DATA);
				bsOut.Write(s_ClientData.Name.c_str());
				s_PeerInterface->Send(&bsOut, HIGH_PRIORITY, RELIABLE_ORDERED, 0, packet->systemAddress, false);
			
				s_ServerSysAddr = packet->systemAddress;
				break;
			}

			case CLIENT_DATA:
			{
				int inID;
				RakNet::RakString userName;
				RakNet::BitStream bsIn(packet->data, packet->length, false);
				bsIn.IgnoreBytes(sizeof(PacketID));
				bsIn.Read(inID);
	
				CALL_HANDLER(s_ConnectionAcceptedCallback, inID);
				break;
			}

			case CHATROOM_MESSAGE:
			{
				int ClientID, RoomID;
				RakNet::RakString Message;
				RakNet::BitStream bsIn(packet->data, packet->length, false);
				bsIn.IgnoreBytes(sizeof(PacketID));
				bsIn.Read(ClientID);
				bsIn.Read(RoomID);
				bsIn.Read(Message);

				CALL_HANDLER(s_ChatroomMessageCallback, ClientID, Message.C_String(), RoomID);
				break;
			}

			case INTRODUCE_CLIENT:
			{
				int inID;
				RakNet::RakString userName;
				RakNet::BitStream bsIn(packet->data, packet->length, false);
				bsIn.IgnoreBytes(sizeof(PacketID));
				bsIn.Read(inID);
				bsIn.Read(userName);

				CALL_HANDLER(s_IntroduceClientCallback, inID, userName);
				break;
			}

			case CLIENT_DISCONNECT:
			{
				int inID;
				RakNet::BitStream bsIn(packet->data, packet->length, false);
				bsIn.IgnoreBytes(sizeof(PacketID));
				bsIn.Read(inID);

				CALL_HANDLER(s_ClientDisconnectCallback, inID);
			}

			default:
			{
				//printf("[Core]: Message with identifier %i has arrived:\n", packet->data[0]);
				break;
			}
		}
	}
}

void SetConnectionAcceptedHandler(ConnectionAcceptedCallback Callback)
{
	s_ConnectionAcceptedCallback = Callback;
}

void SendChatroomMessage(const char* Message, int RoomID)
{
	// Send client data to server
	RakNet::BitStream bsOut;
	bsOut.Write(PacketID::CHATROOM_MESSAGE);
	bsOut.Write(RoomID);
	bsOut.Write(Message);
	s_PeerInterface->Send(&bsOut, HIGH_PRIORITY, RELIABLE_ORDERED, 0, s_ServerSysAddr, false);

}

void SetChatroomMessageHandler(ChatroomMessageCallback Callback)
{
	s_ChatroomMessageCallback = Callback;
}

void SetIntroduceClientHandler(IntroduceClientCallback Callback)
{
	s_IntroduceClientCallback = Callback;
}

void SetClientDisconnectCallback(ClientDisconnectCallback Callback)
{
	s_ClientDisconnectCallback = Callback;
}
