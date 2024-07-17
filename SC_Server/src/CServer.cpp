#include "CServer.h"

#include <iostream>
#include "RakNet/RakString.h"
#include "RakNet/BitStream.h"
#include "RakNet/MessageIdentifiers.h"

#include "Base.h"
#include "PacketIdentifiers.h"

CServer::CServer(int MaxClients)
	: m_MaxClients(MaxClients)
{
	m_PeerInterface = RakNet::RakPeerInterface::GetInstance();
	m_PeerInterface->SetMaximumIncomingConnections(MaxClients);

	m_Users.resize(MaxClients * sizeof(CUser));
}

CServer::~CServer()
{
}

bool CServer::Start(const char* IP, int Port)
{

	RakNet::SocketDescriptor sd((unsigned short)Port, IP);
	sd.socketFamily = AF_INET;

	if (m_PeerInterface->Startup(m_MaxClients, &sd, 1) != RakNet::StartupResult::RAKNET_STARTED)
		return false;

	return true;
}

void CServer::Shutdown()
{
	m_PeerInterface->Shutdown(0);
	RakNet::RakPeerInterface::DestroyInstance(m_PeerInterface);
}

void CServer::Update()
{
	RakNet::Packet* packet;

	for (packet = m_PeerInterface->Receive(); packet; m_PeerInterface->DeallocatePacket(packet), packet = m_PeerInterface->Receive())
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

		case CLIENT_DISCONNECT:
		case ID_DISCONNECTION_NOTIFICATION:
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

		case CREATE_ROOM:
		{

		}

		default:
		{
			//printf("[Core]: Message with identifier %i has arrived:\n", packet->data[0]);
			break;
		}
		}
	}
}

int CServer::GetClientIDByAddress(const char* Address)
{
	return m_ClientIDMap[Address];
}

const char* CServer::GetClientAddressByID(int ID)
{
	auto it = std::find_if(std::begin(m_ClientIDMap), std::end(m_ClientIDMap),
		[&ID](auto&& p) { return p.second == ID; });

	if (it == std::end(m_ClientIDMap))
		return "unknown";

	return it->first.c_str();
}

__int64 CServer::GetConnectedClientsCount()
{
	return m_ClientIDMap.size();
}

void CServer::AddNewClient(RakNet::SystemAddress SystemAddress, const char* UserName)
{
	//int ID = GetConnectedClientsCount() + 1;

	int ID = -1;

	for (size_t i = 0; i < m_MaxClients; i++)
	{
		if (m_Users[i].GetID() == -1)
		{
			ID = i;
			break;
		}
	}

	m_ClientIDMap[SystemAddress.ToString()] = ID;
	m_ClientNameMap[ID] = UserName;

	m_Users[ID] = CUser(SystemAddress, ID, UserName);

	// Send client data
	{
		RakNet::BitStream bsOut;
		bsOut.Write(PacketID::CLIENT_DATA);
		bsOut.Write(ID);
		m_PeerInterface->Send(&bsOut, HIGH_PRIORITY, RELIABLE_ORDERED, 0, SystemAddress, false);
	}

	// Send already connected clients
	for (auto& Client : m_ClientIDMap)
	{
		if (Client.second == ID)
			continue;

		RakNet::BitStream bsOut;
		bsOut.Write(PacketID::INTRODUCE_CLIENT);
		//bsOut.Write(Client.second);
		//bsOut.Write(m_ClientNameMap[Client.second].c_str());
		bsOut.Write(m_Users[ID].GetID());
		bsOut.Write(m_Users[ID].GetName().c_str());
		m_PeerInterface->Send(&bsOut, HIGH_PRIORITY, RELIABLE_ORDERED, 0, SystemAddress, false);
	}

	// Introduce connected client to already connected clients
	for (auto& Client : m_ClientIDMap)
	{
		if (ID != Client.second)
		{
			RakNet::BitStream bsOut;
			bsOut.Write(PacketID::INTRODUCE_CLIENT);
			bsOut.Write(ID);
			bsOut.Write(UserName);
			m_PeerInterface->Send(&bsOut, HIGH_PRIORITY, RELIABLE_ORDERED, 0, RakNet::SystemAddress(Client.first.c_str()), false);
		}
	}

	printf("%s [ID: %d] connected from %s\n", UserName, ID, SystemAddress.ToString());
}

void CServer::RemoveClient(const char* IPAddress)
{
	/*for (auto& Client : m_ClientIDMap)
	{
		if (Client.first == IPAddress || m_ClientIDMap.find(IPAddress) == m_ClientIDMap.end())
			continue;

		RakNet::BitStream bsOut;
		bsOut.Write(PacketID::CLIENT_DISCONNECT);
		bsOut.Write(m_ClientIDMap[IPAddress]);
		m_PeerInterface->Send(&bsOut, HIGH_PRIORITY, RELIABLE_ORDERED, 0, RakNet::SystemAddress(Client.first.c_str()), false);
	}
	
	m_ClientNameMap.erase(GetClientIDByAddress(IPAddress));
	m_ClientIDMap.erase(IPAddress);

	*/

	for (auto& user : m_Users)
	{
		if (user.GetID() == -1 && user.GetAddress() == IPAddress)
			continue;

		RakNet::BitStream bsOut;
		bsOut.Write(PacketID::CLIENT_DISCONNECT);
		bsOut.Write(user.GetID());
		m_PeerInterface->Send(&bsOut, HIGH_PRIORITY, RELIABLE_ORDERED, 0, user.GetAddress(), false);
		printf("%s [ID: %d] disconnected\n", user.GetName().c_str(), user.GetID());
		user.Deactivate();
		return;
	}


	
}

void CServer::SendChatroomMessageFromClient(int ID, const char* Message, int RoomID)
{
	printf("[MSG] %s[%d] [RID: %d]: %s\n", m_ClientNameMap[ID].c_str(), ID, RoomID, Message);


	for (auto& Client : m_ClientIDMap)
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
			m_PeerInterface->Send(&bsOut, HIGH_PRIORITY, RELIABLE_ORDERED, 0, RakNet::SystemAddress(clientAddr.c_str()), false);
		}
	}
}
