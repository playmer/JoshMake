//===----------------------------------------------------------------------===//
//                     JoshMake Build Infrastructure
//
// This file is distributed under the MIT Open Source License. See 
// LICENSE.TXT for details.
//===----------------------------------------------------------------------===//
#pragma once

#ifndef JoshMake_Types_h
#define JoshMake_Types_h

#include <filesystem>
#include <inttypes.h>

namespace std
{
  namespace filesystem = std::experimental::filesystem;
}

using byte = char;

using i8  = int8_t;
using i16 = int16_t;
using i32 = int32_t;
using i64 = int64_t;

using u8  = uint8_t;
using u16 = uint16_t;
using u32 = uint32_t;
using u64 = uint64_t;

#endif
