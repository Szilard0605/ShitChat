#pragma once

class RakPeerInterface;

#include "RakNet/RakPeerInterface.h"


#include "CUser.h"

#include <string>
#include <unordered_map>
#include <vector>

class CServer
{
public:
	CServer(int MaxClients);
	~CServer();
	bool Start(const char* IP, int Port);
	void Shutdown();
	void Update();

	int GetClientIDByAddress(const char* Address);
	const char* GetClientAddressByID(int ID);

	__int64 GetConnectedClientsCount();
private:
	void AddNewClient(RakNet::SystemAddress SystemAddress, const char* UserName);
	void RemoveClient(const char* IPAddress);
	void SendChatroomMessageFromClient(int ID, const char* Message, int RoomID);


	int m_MaxClients;
	std::unordered_map<std::string, int> m_ClientIDMap;
	std::unordered_map<int, std::string> m_ClientNameMap;

	std::vector<CUser> m_Users;

	RakNet::RakPeerInterface* m_PeerInterface;
};

