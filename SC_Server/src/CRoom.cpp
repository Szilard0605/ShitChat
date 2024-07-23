#include "CRoom.h"

#include "CServer.h"

#include "RakNet/BitStream.h"
#include <PacketIdentifiers.h>

#include "CUserManager.h"

CRoom::CRoom(int ID, std::string Name, int MaxUsers)
	: m_ID(ID), m_Name(Name), m_MaxUsers(MaxUsers)
{
	m_Users.resize(MaxUsers * sizeof(int), -1);
	m_MessageMap.clear();
}

void CRoom::AddUser(CUser user)
{
	// discard if user is already in the room
	if (std::find(m_Users.begin(), m_Users.end(), user.GetID()) != m_Users.end())
	{
		printf("User %s[%d] is already in the room %s [%d]\n", user.GetName().c_str(), user.GetID(), m_Name.c_str(), m_ID);
		return;
	}

	m_Users[m_UserCount] = user.GetID();
	m_UserCount++;
	// Notify all clients in room of this join

	for (int i = 0; i < m_MaxUsers; i ++)
	{
		if (m_Users[i] == -1)
			continue;

		CUser& sendUser = CServer::GetInstance()->GetUserManager()->GetUserByID(m_Users[i]);

		RakNet::BitStream bsOut;
		bsOut.Write(PacketID::ROOM_JOIN_NOTIFICATION);
		bsOut.Write(m_ID);
		bsOut.Write(user.GetID());
		bsOut.Write(user.GetName().c_str());
		sendUser.SendBitStream(&bsOut);
	}

	printf("%s [%d] joined room ID: %d\n", user.GetName().c_str(), user.GetID(), m_ID);
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

void CRoom::SendChatMessage(UserID FromUserID, std::string Message)
{
	// discard if user is not in the room
	if (std::find(m_Users.begin(), m_Users.end(), FromUserID) == m_Users.end())
		return;

	CServer* server = CServer::GetInstance();

	// Send client data to server
	RakNet::BitStream bsOut;
	bsOut.Write(PacketID::CHATROOM_MESSAGE);
	bsOut.Write(FromUserID);
	bsOut.Write(m_ID);
	bsOut.Write(Message.c_str());

	for (int i = 0; i < m_UserCount; i++)
	{
		CUser& toUser = server->GetUserManager()->GetUserByID(m_Users[i]);
		
		if (!toUser.IsValid())
			continue;

		server->SendBitStream(toUser, &bsOut);

		m_MessageMap[m_MessageCount] = Message;
	}

	m_MessageCount++;

	printf("[Room %d, MSGID: %d] %s: %s\n", m_ID, m_MessageCount, server->GetUserManager()->GetUserByID(FromUserID).GetName().c_str(), Message.c_str());
}