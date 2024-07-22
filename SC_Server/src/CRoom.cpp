#include "CRoom.h"

#include "CServer.h"

#include "RakNet/BitStream.h"
#include <PacketIdentifiers.h>

#include "CUserManager.h"

CRoom::CRoom(int ID, std::string Name, int MaxUsers)
	: m_ID(ID), m_Name(Name), m_MaxUsers(MaxUsers)
{
	m_Users.resize(MaxUsers * sizeof(int));
}

void CRoom::AddUser(CUser user)
{
	m_UserCount++;
	m_Users[m_UserCount] = user.GetID();
}

void CRoom::RemoveUser(CUser user)
{
	for (int i = 0; i < m_UserCount; i++)
	{
		if (m_Users[i] == user.GetID())
			m_Users[i] = -1;
	}

	m_UserCount --;
}

void CRoom::SendChatMessage(UserID User, std::string Message)
{
	CServer* server = CServer::GetInstance();
	for (int i = 0; i < m_UserCount; i++)
	{
		if (m_Users[i] == -1)
			continue;

		CUser& user = server->GetUserManager()->GetUserByID(m_Users[i]);

		if (!user.IsActive())
			continue;

		std::string clientAddr = user.GetAddress().ToString();
		int clientID = m_Users[i];

		
		// Send client data to server
		RakNet::BitStream bsOut;
		bsOut.Write(PacketID::CHATROOM_MESSAGE);
		bsOut.Write(User);
		bsOut.Write(m_ID);
		bsOut.Write(Message);
		server->SendBitStream(user, &bsOut);

		printf("[Room %d] %s: %s\n", m_ID, user.GetName().c_str(), Message.c_str());
	}
}