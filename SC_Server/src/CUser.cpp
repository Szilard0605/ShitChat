#include "CUser.h"

#include "CServer.h"

CUser::CUser(RakNet::SystemAddress Address, int ID, const std::string Name)
	: m_Address(Address), m_ID(ID), m_Name(Name)
{

}

void CUser::SendBitStream(RakNet::BitStream* BitStream)
{
	CServer::GetInstance()->SendBitStream(*this, BitStream);
}
