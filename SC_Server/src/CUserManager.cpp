#include "CUserManager.h"

#include "CServer.h"
#include "PacketIdentifiers.h"

CUserManager::CUserManager(int MaxUsers)
	: m_MaxUsers(MaxUsers)
{
	m_Users.resize(MaxUsers * sizeof(CUser));
}

void CUserManager::AddUser(std::string Name, RakNet::SystemAddress Address)
{
	for(int i = 0; i < m_MaxUsers; i ++)
	{
		if (m_Users[i].IsValid())
			continue;

		m_Users[i] = CUser(Address, i, Name);

		// Send client data
		
		RakNet::BitStream bsOut;
		bsOut.Write(PacketID::CLIENT_DATA);
		bsOut.Write(i);
		CServer::GetInstance()->SendBitStream(m_Users[i], &bsOut);
		
		IntroduceUser(m_Users[i]);

		printf("%s [%d] connected at %s\n", Name.c_str(), i, m_Users[i].GetAddress().ToString());
		break;
	}
}

void CUserManager::RemoveUser(RakNet::SystemAddress Address)
{
	for (auto& user : m_Users)
	{
		if (user.GetAddress() == Address)
		{
			RakNet::BitStream bsOut;
			bsOut.Write(PacketID::CLIENT_DISCONNECT);
			bsOut.Write(user.GetID());
			bsOut.Write(user.GetName().c_str());
			SendBitStreamToAllUsers(&bsOut);

			printf("%s [%d] disconnected\n", user.GetName().c_str(), user.GetID());
			
			user.Deactivate();
			break;
		}
	}
}

int CUserManager::GetUserCount()
{
	int count = 0;
	for (int i = 0; i < m_MaxUsers; i++)
	{
		if (m_Users[i].IsValid())
			count++;
	}
	return count;
}

void CUserManager::IntroduceUser(CUser& user)
{
	RakNet::BitStream bsOut;
	// Introduce connected user to already connected users
	for (int i = 0; i < m_MaxUsers; i++)
	{
		if (!m_Users[i].IsValid() || m_Users[i] == user)
			continue;

		bsOut.Reset();
		bsOut.Write(PacketID::INTRODUCE_CLIENT);
		bsOut.Write(user.GetID());
		bsOut.Write(user.GetName().c_str());
		CServer::GetInstance()->SendBitStream(m_Users[i], &bsOut);

		// Introduce already connected users to recently the connected one
		bsOut.Reset();
		bsOut.Write(PacketID::INTRODUCE_CLIENT);
		bsOut.Write(m_Users[i].GetID());
		bsOut.Write(m_Users[i].GetName().c_str());
		CServer::GetInstance()->SendBitStream(user, &bsOut);
	}
}

void CUserManager::SendBitStreamToAllUsers(RakNet::BitStream* BitStream)
{
	for (auto& user : m_Users)
	{
		if (!user.IsValid())
			continue;

		CServer::GetInstance()->SendBitStream(user, BitStream);
	}
}

CUser CUserManager::GetUserByAddress(RakNet::SystemAddress Address)
{
	for (auto& user : m_Users)
	{
		if (user.GetAddress() == Address)
			return user;
	}
	return CUser();
}

