#pragma once

#include "CRoom.h"
#include <vector>

class CRoomManager
{
public:
	CRoomManager() = default;

	

private:
	int m_RoomCount;
	std::vector<CRoom> m_Rooms;
};

