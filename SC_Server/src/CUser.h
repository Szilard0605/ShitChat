#pragma once

#include "RakNet/RakPeerInterface.h"
#include <string>

class CUser
{
public:
	CUser() = default;

	CUser(RakNet::SystemAddress Address, int ID, const std::string Name);

	inline int GetID() const { return m_ID; }
	inline std::string GetName() const { return m_Name; }
	inline RakNet::SystemAddress GetAddress() { return m_Address; }
	inline void Deactivate() { m_ID = -1; }

private:
	RakNet::SystemAddress m_Address;
	int m_ID = -1;
	std::string m_Name;
};

