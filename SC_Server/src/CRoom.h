#pragma once

#include "CUser.h"

#include <vector>
#include <string>
#include <map>

typedef int32_t RoomID;

class CRoom
{
public:
	CRoom() = default;
	CRoom(int ID, std::string Name, int MaxUsers = 50);

	void AddUser(CUser user);
	void RemoveUser(CUser user);
	void SendChatMessage(UserID FromUserID, std::string Message);
	inline int IsActive() { return (m_ID != -1); }
	inline std::string GetName() const { return m_Name; }
	inline int GetID() const { return m_ID; }

private:
	int m_UserCount	= 0;
	int m_ID		= -1;
	int m_MaxUsers	= 50;
	std::string m_Name;
	std::vector<UserID> m_Users;
	int m_MessageCount = 0;
	std::map<int, std::string> m_MessageMap;
};

