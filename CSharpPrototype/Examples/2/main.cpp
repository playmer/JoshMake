#include <iostream>

#include "Zilch/Zilch.hpp"

int main ()
{
  Zilch::String test("Yes, this works");
  std::cout << test.c_str() << std::endl;
  return 0;
}