#pragma once

#include <string>

#include "CUser.h"

class CRoom
{
public:
	CRoom() = default;
	CRoom(int ID, std::string Name);

	void SendChatMessage(CUser User, std::string Message);
private:
	int m_ID;
	std::string m_Name;
};

