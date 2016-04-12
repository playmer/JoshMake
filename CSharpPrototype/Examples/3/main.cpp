#include <iostream>

#include "Zilch/Zilch.hpp"

int main ()
{
  Zilch::String test("Futzing with Flags");
  std::cout << test.c_str() << std::endl;
  return 0;
}