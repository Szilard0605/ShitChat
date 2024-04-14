#include "NetServer.h"

#include "MessageIdentifiers.h"
#include "BitStream.h"
#include "RakNetTypes.h"
#include "Gets.h"

#include "RakSleep.h"

#include "Networking/PacketIdentifiers.h"

#include <unordered_map>
#include <string>


struct ServerData
{
	int ConnectCount = 0;
	std::unordered_map<std::string, int> Clients;

	RakNet::RakPeerInterface* PeerInterface;

	ClientConnectCallback ConnectCallback;
	ClientDisconnectCallback DisconnectCallback;
} s_ServerData;

// Returns true if successful, otherwise returns false
bool StartServer(const char* IP, int Port, int MaxClients)
{
	printf("[Core] Server started at address %s:%d (%d)\n", IP, Port, MaxClients);

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

void AddNewClient(const char* IPAddress, const char* UserName)
{
	s_ServerData.ConnectCount++;
	int ID = s_ServerData.ConnectCount;
	s_ServerData.Clients[IPAddress] = ID;

	if(s_ServerData.ConnectCallback)
		s_ServerData.ConnectCallback(UserName, ID);

	// Send client data to server
	RakNet::BitStream bsOut;
	bsOut.Write(PacketID::CLIENT_DATA);
	bsOut.Write(ID);
	s_ServerData.PeerInterface->Send(&bsOut, HIGH_PRIORITY, RELIABLE_ORDERED, 0, RakNet::SystemAddress(IPAddress), false);

}

void RemoveClient(const char* IPAddress)
{
	s_ServerData.Clients.erase(IPAddress);
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

				printf("[Core]: user %s connected from %s\n", userName, packet->systemAddress.ToString());

				AddNewClient(packet->systemAddress.ToString(), userName);
				break;
			}

			case ID_CONNECTION_LOST:
			{
				RemoveClient(packet->systemAddress.ToString());
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
