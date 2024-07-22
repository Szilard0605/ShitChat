#pragma once

class RakPeerInterface;

#include "RakNet/RakPeerInterface.h"
#include "RakNet/BitStream.h"

class CUserManager;
class CRoomManager;

#include "CUser.h"

#include <string>
#include <unordered_map>
#include <vector>

class CServer
{
public:
	CServer(int MaxClients);
	~CServer();

	static CServer* GetInstance() { return s_Instance; }

	bool Start(const char* IP, int Port);
	void Shutdown();
	void Update();

	void SendBitStream(CUser User, RakNet::BitStream* BitStream);

	int GetClientIDByAddress(const char* Address);
	const char* GetClientAddressByID(int ID);

	__int64 GetConnectedClientsCount();

	CUserManager* GetUserManager() { return m_UserManager; }
	CRoomManager* GetRoomManager() { return m_RoomManager; }
private:
	static CServer* s_Instance;

	int m_MaxClients;

	CUserManager* m_UserManager;
	CRoomManager* m_RoomManager;

	std::unordered_map<std::string, int> m_ClientIDMap;
	std::unordered_map<int, std::string> m_ClientNameMap;

	RakNet::RakPeerInterface* m_PeerInterface;
};

