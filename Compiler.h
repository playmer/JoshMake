//===----------------------------------------------------------------------===//
//                     JoshMake Build Infrastructure
//
// This file is distributed under the MIT Open Source License. See 
// LICENSE.TXT for details.
//===----------------------------------------------------------------------===//
#pragma once

#ifndef JoshMake_Compiler_h
#define JoshMake_Compiler_h

#include <string>

namespace JoshMake
{
  class Compiler
  {
    virtual ~Compiler();
    virtual std::wstring ConstructCommand(std::wstring aFileName) = 0;
  };
}


#endif