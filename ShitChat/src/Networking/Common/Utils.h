#pragma once

#define CALL_HANDLER(func, ...) if(func) func(__VA_ARGS__);
