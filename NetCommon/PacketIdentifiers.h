#pragma once


// RakNet user defined packets start at 135
enum PacketID : unsigned char
{
	CLIENT_DATA = 135,
	CHATROOM_MESSAGE,
	INTRODUCE_CLIENT,
	CLIENT_DISCONNECT,
	CREATE_ROOM_REQUEST,
	ROOM_JOIN_NOTIFICATION,
	JOIN_ROOM
};