#pragma once

enum PacketID : unsigned char
{
	CLIENT_DATA = 135,
	CHATROOM_MESSAGE,
	INTRODUCE_CLIENT,
	CLIENT_DISCONNECT,
	CREATE_ROOM_REQUEST
};