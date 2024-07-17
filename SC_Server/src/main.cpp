// SC_Server.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>

#include "CServer.h"

int main()
{
    CServer server(30);

    if (!server.Start("127.0.0.1", 7000))
    {
        std::cout << "Couldn't start server!\n";
        return 1;
    }

    std::cout << "Server started!\n";

    while (true)
    {
        server.Update();
    }
}