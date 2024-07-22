
#include "CUser.h"

#include "RakNet/BitStream.h"
#include "RakNet/RakPeerInterface.h"

#include <vector>

class CUserManager
{
public:
	CUserManager() = default;
	CUserManager(int MaxUsers);

	void AddUser(std::string Name, RakNet::SystemAddress Address);
	void RemoveUser(RakNet::SystemAddress Address);
	int GetUserCount();

	void SendBitStreamToAllUsers(RakNet::BitStream* BitStream);
	
	CUser& GetUserByID(UserID userID) { return m_Users[userID]; }
	CUser GetUserByAddress(RakNet::SystemAddress Address);
private:
	int m_MaxUsers = 50;
	std::vector<CUser> m_Users;
};