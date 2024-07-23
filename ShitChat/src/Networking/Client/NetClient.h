#pragma once

#include "Base.h"

typedef void (*ConnectionAcceptedCallback)(int ID);
typedef void (*ChatroomMessageCallback)(int ClientID, const char* Message, int RoomID);
typedef void (*IntroduceClientCallback)(int ClientID, const char* Name);
typedef void (*ClientDisconnectCallback)(const char* Name, int ClientID);
typedef void (*RoomJoinNotificationCallback)(int RoomID, int ID, const char* Name);

#include <stdint.h>

extern "C"
{
	SC_EXPORT bool ConnectToServer(const char* IP, int Port, const char* Name);
	SC_EXPORT void DisconnectFromServer();
	SC_EXPORT void ClientUpdate();

	SC_EXPORT void SetConnectionAcceptedHandler(ConnectionAcceptedCallback Callback);

	SC_EXPORT void JoinRoom(int RoomID);
	SC_EXPORT void SendChatroomMessage(const char* Message, int RoomID);
	SC_EXPORT void RequestNewRoom(const char* Name, uint32_t* ResultPtr);
	SC_EXPORT void SetChatroomMessageHandler(ChatroomMessageCallback Callback);
	SC_EXPORT void SetRoomJoinNotificationHandler(RoomJoinNotificationCallback Callback);

	SC_EXPORT void SetIntroduceClientHandler(IntroduceClientCallback Callback);

	SC_EXPORT void SetClientDisconnectHandler(ClientDisconnectCallback Callback);
}
