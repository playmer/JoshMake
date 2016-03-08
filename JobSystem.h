//===----------------------------------------------------------------------===//
//                     JoshMake Build Infrastructure
//
// This file is distributed under the MIT Open Source License. See 
// LICENSE.TXT for details.
//===----------------------------------------------------------------------===//
#pragma once

#ifndef JoshMake_JobSystem_h
#define JoshMake_JobSystem_h

#include <atomic>
#include <thread>
#include <vector>

namespace JoshMake
{
  class JobSystem
  {
  public:
    JobSystem();

      // Threads enter this and wait for jobs.
    void ThreadMain(); 
      // Asks the JobSystem to begin closing threads. Threads close after they 
      // complete whatever job they're working on.
    inline void Close() { mShouldClose = true; }
      // Waits for all Jobs to finish.
    void Join();
  private:
    std::vector<std::thread> mThreads;
    std::atomic_bool mShouldClose;
  };
}


#endif