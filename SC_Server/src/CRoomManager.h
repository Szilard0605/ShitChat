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

	void SendChatMessage(UserID FromUserID, RoomID roomID, std::string message);
	ROOM_REQUEST_RESULT NewRoom(std::string Name);
	int GetRoomsCount();
	void AddUserToRoom(UserID userID, RoomID roomID);

	CRoom& GetRoomByID(RoomID roomID) { return m_Rooms[roomID]; }
	CRoom GetRoomByName(std::string Name);
	bool IsRoomValid(RoomID roomID);
private:
	int m_MaxRooms = 50;
	std::vector<CRoom> m_Rooms;
};

