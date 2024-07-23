#include "CRoomManager.h"
#include "CUserManager.h"
#include "CServer.h"

#include "PacketIdentifiers.h"

CRoomManager::CRoomManager(int MaxRooms)
	: m_MaxRooms(MaxRooms)
{
	m_Rooms.resize(MaxRooms * sizeof(CRoom));
}

ROOM_REQUEST_RESULT CRoomManager::NewRoom(std::string Name)
{
	for (int i = 0; i < m_MaxRooms; i++)
	{
		if (m_Rooms[i].GetName() == Name)
			return ROOM_REQUEST_RESULT::NAME_ALREADY_EXISTS;

		if (!m_Rooms[i].IsActive())
		{
			m_Rooms[i] = CRoom(i, Name);
			RakNet::BitStream bsOut;
			bsOut.Write(PacketID::CREATE_ROOM);
			bsOut.Write(i);
			bsOut.Write(Name.c_str());
			CServer::GetInstance()->GetUserManager()->SendBitStreamToAllUsers(&bsOut);

			return ROOM_REQUEST_RESULT::ROOM_CREATED;
		}

		return ROOM_REQUEST_RESULT::NONE;;
	}
	return ROOM_REQUEST_RESULT::NONE;;
}

int CRoomManager::GetRoomsCount()
{
	int count = 0;
	for (int i = 0; i < m_MaxRooms; i++)
	{
		if (m_Rooms[i].IsActive())
		{
			count++;
		}
	}
	return count;
}

void CRoomManager::AddUserToRoom(UserID userID, RoomID roomID)
{
	if (!IsRoomValid(roomID))
		return;

	CServer* server = CServer::GetInstance();
	CUserManager* userMngr = server->GetUserManager();
	CRoom& room = GetRoomByID(roomID);
	CUser& user = userMngr->GetUserByID(userID);
	room.AddUser(user);
}

CRoom CRoomManager::GetRoomByName(std::string Name)
{
	for (auto& room : m_Rooms)
	{
		if (room.GetName() == Name)
			return room;
	}

	return CRoom();
}

bool CRoomManager::IsRoomValid(RoomID roomID)
{
	for (auto& room : m_Rooms)
	{
		if (room.GetID() == roomID)
			return true;
	}

	return false;
}

void CRoomManager::SendChatMessage(UserID FromUserID, RoomID roomID, std::string message)
{
	m_Rooms[roomID].SendChatMessage(FromUserID, message);
}
