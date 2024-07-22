#pragma once

#include "CRoom.h"
#include <vector>
#include <stdint.h>

enum class ROOM_REQUEST_RESULT : uint32_t
{
	NONE = 0,
	ROOM_CREATED,
	NAME_ALREADY_EXISTS
};

class CRoomManager
{
public:
	CRoomManager() = default;
	CRoomManager(int MaxRooms);

	void SendChatMessage(UserID userID, RoomID roomID, std::string message);
	ROOM_REQUEST_RESULT NewRoom(std::string Name);
	int GetRoomsCount();

	CRoom& GetRoomByID(RoomID roomID) { return m_Rooms[roomID]; }
private:
	int m_MaxRooms = 50;
	std::vector<CRoom> m_Rooms;
};

