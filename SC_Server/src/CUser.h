#pragma once


#include "RakNet/BitStream.h"
#include "RakNet/RakPeerInterface.h"
#include <string>
#include <stdint.h>

typedef int32_t UserID;

class CUser
{
public:
	CUser() = default;

	CUser(RakNet::SystemAddress Address, int ID, const std::string Name);

	void SendBitStream(RakNet::BitStream* BitStream);

	inline bool IsValid() const { return (m_ID != -1); }
	inline std::string GetName() const { return m_Name; }
	inline RakNet::SystemAddress GetAddress() { return m_Address; }
	inline void Deactivate() { m_ID = -1; }
	int GetID() { return m_ID; }

	bool operator == (CUser& other)
	{
		return this->m_ID == other.GetID();
	}

	bool operator != (CUser& other)
	{
		return this->m_ID != other.GetID();
	}

private:
	RakNet::SystemAddress m_Address;
	int m_ID = -1;
	std::string m_Name;
};

