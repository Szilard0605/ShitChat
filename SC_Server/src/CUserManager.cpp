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
		if (m_Users[i].IsActive())
			continue;

		m_Users[i] = CUser(Address, i, Name);

		// Send client data
		{
			RakNet::BitStream bsOut;
			bsOut.Write(PacketID::CLIENT_DATA);
			bsOut.Write(i);
			CServer::GetInstance()->SendBitStream(m_Users[i], &bsOut);
		}

		
		for (int j = 0; j < m_MaxUsers; j++)
		{
			if (m_Users[j].GetID() == i)
				continue;

			if (m_Users[j].GetID() != i)
			{
				// Introduce connected client to already connected clients	
				RakNet::BitStream bsOut;
				bsOut.Write(PacketID::INTRODUCE_CLIENT);
				bsOut.Write(i);
				bsOut.Write(Name.c_str());
				CServer::GetInstance()->SendBitStream(m_Users[j], &bsOut);
			}

			if (m_Users[j].GetID() == i)
			{
				// Introduce already connected clients to recently connected client
				RakNet::BitStream bsOut;
				bsOut.Write(PacketID::INTRODUCE_CLIENT);
				bsOut.Write(j);
				bsOut.Write(m_Users[j].GetName().c_str());
				CServer::GetInstance()->SendBitStream(m_Users[i], &bsOut);
			}
		}
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
		if (m_Users[i].IsActive())
			count++;
	}
	return count;
}

void CUserManager::SendBitStreamToAllUsers(RakNet::BitStream* BitStream)
{
	for (auto& user : m_Users)
	{
		if (!user.IsActive())
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

