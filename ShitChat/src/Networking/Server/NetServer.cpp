#include "NetServer.h"

#include "MessageIdentifiers.h"
#include "BitStream.h"
#include "RakNetTypes.h"
#include "Gets.h"

#include "RakSleep.h"

#include "Networking/Common/PacketIdentifiers.h"
#include "Networking/Common/Utils.h"

#include <unordered_map>
#include <string>


struct ServerData
{
	int ConnectCount = 0;
	std::unordered_map<std::string, int> Clients;
	std::unordered_map<int, std::string> ClientNameMap;

	RakNet::RakPeerInterface* PeerInterface;

	ClientConnectCallback ConnectCallback;
	ClientDisconnectCallback DisconnectCallback;
} s_ServerData;

// Returns true if successful, otherwise returns false
bool StartServer(const char* IP, int Port, int MaxClients)
{
	s_ServerData.PeerInterface = RakNet::RakPeerInterface::GetInstance();

	RakNet::SocketDescriptor sd((unsigned short)Port, IP);
	sd.socketFamily = AF_INET;

	if (s_ServerData.PeerInterface->Startup(MaxClients, &sd, 1) != RakNet::StartupResult::RAKNET_STARTED)
		return false;

	s_ServerData.PeerInterface->SetMaximumIncomingConnections(20);

	return true;
}

void ShutdownServer()
{
	s_ServerData.PeerInterface->Shutdown(0);
	RakNet::RakPeerInterface::DestroyInstance(s_ServerData.PeerInterface);
}

void AddNewClient(RakNet::SystemAddress SystemAddress, const char* UserName)
{
	s_ServerData.ConnectCount++;
	int ID = s_ServerData.ConnectCount;
	s_ServerData.Clients[SystemAddress.ToString()] = ID;
	s_ServerData.ClientNameMap[ID] = UserName;

	CALL_HANDLER(s_ServerData.ConnectCallback, UserName, ID);

	// Send client data
	{
		RakNet::BitStream bsOut;
		bsOut.Write(PacketID::CLIENT_DATA);
		bsOut.Write(ID);
		s_ServerData.PeerInterface->Send(&bsOut, HIGH_PRIORITY, RELIABLE_ORDERED, 0, SystemAddress, false);
	}

	// Send already connected clients
	for (auto& Client : s_ServerData.Clients)
	{
		if (Client.second == ID)
			continue;

		RakNet::BitStream bsOut;
		bsOut.Write(PacketID::INTRODUCE_CLIENT);
		bsOut.Write(Client.second);
		bsOut.Write(s_ServerData.ClientNameMap[Client.second].c_str());
		s_ServerData.PeerInterface->Send(&bsOut, HIGH_PRIORITY, RELIABLE_ORDERED, 0, SystemAddress, false);
	}

	// Introduce connected client to already connected clients
	for (auto& Client : s_ServerData.Clients)
	{
		if (ID != Client.second)
		{
			RakNet::BitStream bsOut;
			bsOut.Write(PacketID::INTRODUCE_CLIENT);
			bsOut.Write(ID);
			bsOut.Write(UserName);
			s_ServerData.PeerInterface->Send(&bsOut, HIGH_PRIORITY, RELIABLE_ORDERED, 0, RakNet::SystemAddress(Client.first.c_str()), false);
		}
	}
}

void RemoveClient(const char* IPAddress)
{
	for (auto& Client : s_ServerData.Clients)
	{
		if (Client.first == IPAddress || s_ServerData.Clients.find(IPAddress) == s_ServerData.Clients.end())
			continue;

		RakNet::BitStream bsOut;
		bsOut.Write(PacketID::CLIENT_DISCONNECT);
		bsOut.Write(s_ServerData.Clients[IPAddress]);
		s_ServerData.PeerInterface->Send(&bsOut, HIGH_PRIORITY, RELIABLE_ORDERED, 0, RakNet::SystemAddress(Client.first.c_str()), false);
	}

	s_ServerData.ClientNameMap.erase(GetClientIDByAddress(IPAddress));
	s_ServerData.Clients.erase(IPAddress);
}

void SendChatroomMessageFromClient(int ID, const char* Message, int RoomID)
{
	for (auto& Client : s_ServerData.Clients)
	{
		std::string clientAddr = Client.first;
		int clientID = Client.second;

		if (clientID != ID)
		{
			// Send client data to server
			RakNet::BitStream bsOut;
			bsOut.Write(PacketID::CHATROOM_MESSAGE);
			bsOut.Write(ID);
			bsOut.Write(RoomID);
			bsOut.Write(Message);
			s_ServerData.PeerInterface->Send(&bsOut, HIGH_PRIORITY, RELIABLE_ORDERED, 0, RakNet::SystemAddress(clientAddr.c_str()), false);
		}
	}
}

void UpdateServer()
{
	RakNet::Packet* packet;
	
	for (packet = s_ServerData.PeerInterface->Receive(); packet; s_ServerData.PeerInterface->DeallocatePacket(packet), packet = s_ServerData.PeerInterface->Receive())
	{
		switch (packet->data[0])
		{
			case PacketID::CLIENT_DATA:
			{
				RakNet::RakString userName;
				RakNet::BitStream bsIn(packet->data, packet->length, false);
				bsIn.IgnoreBytes(sizeof(PacketID));
				bsIn.Read(userName);
				AddNewClient(packet->systemAddress, userName);
				break;
			}

			case ID_CONNECTION_LOST:
			{
				RemoveClient(packet->systemAddress.ToString());
				break;
			}

			case CHATROOM_MESSAGE:
			{
				int RoomID;
				RakNet::RakString Message;
				RakNet::BitStream bsIn(packet->data, packet->length, false);
				bsIn.IgnoreBytes(sizeof(PacketID));
				bsIn.Read(RoomID);
				bsIn.Read(Message);

				SendChatroomMessageFromClient(GetClientIDByAddress(packet->systemAddress.ToString()), Message, RoomID);
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

void SetClientConnectHandler(ClientConnectCallback callback)
{
	s_ServerData.ConnectCallback = callback;
}

SC_EXPORT void SetClientDisconnectHandler(ClientDisconnectCallback callback)
{
	s_ServerData.DisconnectCallback = callback;
}

SC_EXPORT int GetClientIDByAddress(const char* Address)
{
	return s_ServerData.Clients[Address];
}

SC_EXPORT const char* GetClientAddressByID(int ID)
{
	auto it = std::find_if(std::begin(s_ServerData.Clients), std::end(s_ServerData.Clients),
		[&ID](auto&& p) { return p.second == ID; });

	if (it == std::end(s_ServerData.Clients))
		return "unknown";

	return it->first.c_str();
}

__int64 GetConnectedClientsCount()
{
	return s_ServerData.Clients.size();
}
