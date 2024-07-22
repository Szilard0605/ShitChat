#include "CServer.h"

#include <iostream>
#include "RakNet/RakString.h"
#include "RakNet/BitStream.h"
#include "RakNet/MessageIdentifiers.h"

#include "Base.h"
#include "PacketIdentifiers.h"

#include "CUserManager.h"
#include "CRoomManager.h"

CServer* CServer::s_Instance = nullptr;

CServer::CServer(int MaxClients)
	: m_MaxClients(MaxClients)
{
	m_UserManager = new CUserManager(MaxClients);
	m_RoomManager = new CRoomManager(50);

	m_PeerInterface = RakNet::RakPeerInterface::GetInstance();
	m_PeerInterface->SetMaximumIncomingConnections(MaxClients);

	s_Instance = this;
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
			//AddNewClient(packet->systemAddress, userName);
			m_UserManager->AddUser(userName.C_String(), packet->systemAddress);
			break;
		}

		case CLIENT_DISCONNECT:
		case ID_DISCONNECTION_NOTIFICATION:
		case ID_CONNECTION_LOST:
		{
			//RemoveClient(packet->systemAddress.ToString());
			m_UserManager->RemoveUser(packet->systemAddress);
			break;
		}

		case CHATROOM_MESSAGE:
		{
			int roomID;
			RakNet::RakString Message;
			RakNet::BitStream bsIn(packet->data, packet->length, false);
			bsIn.IgnoreBytes(sizeof(PacketID));
			bsIn.Read(roomID);
			bsIn.Read(Message);

			//SendChatroomMessageFromClient(GetClientIDByAddress(packet->systemAddress.ToString()), Message, RoomID);
			UserID userID = m_UserManager->GetUserByAddress(packet->systemAddress).GetID();
			m_RoomManager->SendChatMessage(userID, roomID, Message.C_String());
			break;
		}

		case CREATE_ROOM_REQUEST:
		{
			// Request room and send the result to requesting client


			// Create room for all clients (if request was succesful)
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

void CServer::SendBitStream(CUser User, RakNet::BitStream* BitStream)
{
	m_PeerInterface->Send(BitStream, HIGH_PRIORITY, RELIABLE_ORDERED, 0, User.GetAddress(), false);
}
