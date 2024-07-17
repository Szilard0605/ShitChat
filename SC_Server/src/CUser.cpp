#include "CUser.h"

CUser::CUser(RakNet::SystemAddress Address, int ID, const std::string Name)
	: m_Address(Address), m_ID(ID), m_Name(Name)
{

}
