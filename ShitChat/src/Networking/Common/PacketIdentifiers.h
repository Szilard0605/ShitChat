#pragma once

#include "MessageIdentifiers.h"

enum PacketID : unsigned char
{
	CLIENT_DATA = ID_USER_PACKET_ENUM + 1,
	CHATROOM_MESSAGE,
	INTRODUCE_CLIENT
};