//===----------------------------------------------------------------------===//
//                     JoshMake Build Infrastructure
//
// This file is distributed under the MIT Open Source License. See 
// LICENSE.TXT for details.
//===----------------------------------------------------------------------===//

#include <chrono>

#include "JobSystem.h"

namespace JoshMake
{
  JobSystem::JobSystem() : mShouldClose(false)
  {
    auto threadsToMake = std::thread::hardware_concurrency();

    threadsToMake = threadsToMake <= 0 ? 1 : threadsToMake;

    for (unsigned i = 0; i < threadsToMake; ++i)
    {
      mThreads.emplace_back(&JobSystem::ThreadMain, this);
    }
  }

  void JobSystem::ThreadMain()
  {
    while (mShouldClose == false)
    {
      std::this_thread::sleep_for(std::chrono::milliseconds(10));
    }
  }

  void JobSystem::Join()
  {
    for (auto &thread : mThreads)
    {
      thread.join();
    }
  }
}