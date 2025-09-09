using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        //// ================================
        ////  CPU, CORES, LOGICAL PROCESSORS
        //// ================================
        //// 1. Your machine has 10 physical cores.
        //// 2. Because of Intel Hybrid + HyperThreading:
        ////    → Some cores handle 2 threads, others 1.
        ////    → That’s why you see 12 "logical processors" in Task Manager.
        //// 3. Each logical processor = one "lane" that Windows can run a thread on.
        //// 4. So, max parallelism on your CPU = 12 threads at the SAME TIME.

        //Console.WriteLine($"Logical processors: {Environment.ProcessorCount}");

        //// ================================
        ////  WINDOWS & .NET THREADS
        //// ================================
        //// - Windows can create thousands of software threads.
        //// - Example: You already had ~4500 threads in Task Manager!
        //// - BUT only 12 run at once (rest wait their turn).
        //// - Think: 12 seats in a classroom, many students waiting.

        //ThreadPool.GetMinThreads(out int minWorker, out int minIOC);
        //ThreadPool.GetMaxThreads(out int maxWorker, out int maxIOC);

        //Console.WriteLine($"ThreadPool Min: {minWorker}, {minIOC}");
        //Console.WriteLine($"ThreadPool Max: {maxWorker}, {maxIOC}");

        //// ================================
        ////  THREADPOOL BASICS
        //// ================================
        //// - ThreadPool reuses threads instead of creating new ones.
        //// - By default, Min threads ≈ logical processors (≈12 here).
        //// - Max threads can be very high (e.g., 32767).
        //// - .NET uses ThreadPool for Tasks and async/await.

        //// Example: Run 5 tasks in parallel
        //var tasks = new Task[5];
        //for (int i = 0; i < 5; i++)
        //{
        //    int taskId = i;
        //    tasks[i] = Task.Run(() =>
        //    {
        //        Console.WriteLine($"Task {taskId} running on Thread {Thread.CurrentThread.ManagedThreadId}");
        //        Thread.Sleep(1000); // simulate work
        //    });
        //}

        //await Task.WhenAll(tasks);

        //Console.WriteLine("All tasks completed");



        //// ============================================================
        //// WORKER THREADS vs I/O THREADS in .NET
        //// ============================================================
        //// - Worker threads:
        ////     * Come from the ThreadPool.
        ////     * Used for CPU-bound work (calculations, loops, compression).
        ////     * Example: Task.Run(() => DoHeavyWork()) uses a worker thread.
        ////
        //// - I/O threads:
        ////     * Special threads in the ThreadPool.
        ////     * Handle COMPLETION of I/O operations (file, HTTP, DB).
        ////     * No thread is blocked while waiting; the OS signals completion.
        ////     * Example: HttpClient.GetStringAsync() or SqlCommand.ExecuteReaderAsync().
        ////
        //// - Why it matters:
        ////     * Worker threads are LIMITED (start ≈ logical processors, 12 on your CPU).
        ////     * Blocking them = bad scalability.
        ////     * Async I/O frees worker threads → lets few threads handle thousands of requests.
        ////
        //// ============================================================
        //// DEMO 1: CPU-bound work → uses a WORKER THREAD
        //// ============================================================
        //Console.WriteLine("== CPU work ==");
        //await Task.Run(() =>
        //{
        //    Console.WriteLine($"Heavy work running on Worker Thread {Thread.CurrentThread.ManagedThreadId}");
        //    Thread.Sleep(1000); // simulate CPU work (blocking!)
        //});

        //// ============================================================
        //// DEMO 2: I/O-bound work (HTTP request) → uses I/O THREADS
        //// ============================================================
        //Console.WriteLine("\n== I/O work (HTTP request) ==");
        //using var http = new HttpClient();

        //// When you 'await' this, the request goes to the OS.
        //// No worker thread is blocked while waiting.
        //// When the response arrives, IOCP (I/O Completion Port) signals .NET,
        //// then your code resumes on an available thread.
        //var response = await http.GetStringAsync("https://example.com");

        //Console.WriteLine($"I/O completed and resumed on Thread {Thread.CurrentThread.ManagedThreadId}");

        //// ============================================================
        //// DEMO 3: Database async call (pseudo-example)
        //// ============================================================
        //// In real apps, database drivers (like SqlClient for SQL Server)
        //// use async I/O under the hood (network socket).
        //// Example (not runnable without DB connection):
        ////
        //// using var conn = new SqlConnection(connString);
        //// await conn.OpenAsync(); // async network I/O
        //// var cmd = new SqlCommand("SELECT * FROM Users", conn);
        //// using var reader = await cmd.ExecuteReaderAsync(); // async I/O
        ////
        //// - This does NOT block a worker thread.
        //// - OS signals completion when SQL Server responds.
        //// - Your code continues on an I/O thread.
        ////
        //// ============================================================
        //// Summary:
        //// - Worker thread → does active work (CPU tasks).
        //// - I/O thread → just delivers "done!" signals from OS (files, DB, network).
        //// - Database async calls (like ExecuteReaderAsync) = I/O → they use I/O threads.
        //// ============================================================

        //Console.WriteLine("\nDemo finished. Press ENTER to exit.");
        //Console.ReadLine();

        //Console.WriteLine("Starting 20 tasks...");

        //var tasks = new Task[20];
        //for (int i = 0; i < 20; i++)
        //{
        //    int id = i;
        //    tasks[i] = Task.Run(() =>
        //    {
        //        Console.WriteLine($"Task {id} running on Thread {Thread.CurrentThread.ManagedThreadId}");
        //        Thread.Sleep(1000); // simulate work
        //    });
        //}

        //await Task.WhenAll(tasks);



        //Console.WriteLine("=== THREADPOOL DEMO ===");
        //Console.WriteLine($"Logical processors (hardware limit): {Environment.ProcessorCount}");
        //Console.WriteLine("We will run 20 tasks on a machine with limited threads.\n");

        //// ============================================================
        //// Step 1: Prepare 20 tasks
        //// ============================================================
        //// Each task will:
        ////  - Print its ID
        ////  - Show which Thread it's running on
        ////  - Sleep 2 seconds to keep the Thread busy
        ////
        //// The Thread.Sleep ensures that the first batch of tasks
        //// keeps threads occupied, so extra tasks will go into the queue.
        //// ============================================================

        //var tasks = new Task[30]; // an array to hold all 20 Task objects

        //for (int i = 0; i < 30; i++)
        //{
        //    int taskId = i; // copy loop variable (avoid closure issue)

        //    tasks[i] = Task.Run(() =>
        //    {
        //        // Print task + thread info
        //        Console.WriteLine(
        //            $"[START] Task {taskId} running on Thread {Thread.CurrentThread.ManagedThreadId}"
        //        );

        //        // Keep thread busy for 2 seconds
        //        Thread.Sleep(2000);

        //        Console.WriteLine(
        //            $"[END]   Task {taskId} finished on Thread {Thread.CurrentThread.ManagedThreadId}"
        //        );
        //    });
        //}

        //// ============================================================
        //// Step 2: Wait for all tasks
        //// ============================================================
        //// Task.WhenAll waits until ALL tasks are complete before continuing.
        //// Without this, the program might exit while tasks are still running.
        //// ============================================================
        //await Task.WhenAll(tasks);

        //// ============================================================
        //// Step 3: Summary
        //// ============================================================
        //// Watch the console carefully:
        //// - First ~12 tasks will start immediately (since you have ~12 logical processors).
        //// - The remaining tasks will be QUEUED in the ThreadPool.
        //// - As threads become free, queued tasks will reuse them.
        //// - Sometimes you will see NEW thread IDs appear -> ThreadPool created extra threads.
        //// ============================================================

        //Console.WriteLine("\n=== DEMO COMPLETE ===");
        //Console.WriteLine("Press ENTER to exit.");
        //Console.ReadLine();

        #region ThreadClass
        //        Console.WriteLine("=== THREAD CLASS DEMO ===");
        //        Console.WriteLine("We will start 5 threads manually using the Thread class.\n");

        //         ============================================================
        //         STEP 1: Create and start 5 threads manually
        //         ============================================================
        //         -Each Thread is a * dedicated OS thread*
        //         -Once started, it runs independently of Main()
        //         - When it finishes, it dies(not reused like ThreadPool)
        //         ============================================================

        //        for (int i = 0; i < 5; i++)
        //        {
        //            int threadId = i; // copy loop variable (avoid closure issue)

        //            Create a thread and point it to a method
        //           Thread t = new Thread(() => DoWork(threadId));

        //            Start the thread
        //            t.Start();
        //        }

        //        Console.WriteLine("Main thread finished scheduling threads.\n");
        //    }

        //     ============================================================
        //     STEP 2: Work method executed by each Thread
        //     ============================================================
        //     - Thread.CurrentThread.ManagedThreadId → unique ID assigned by.NET
        //     - Thread.Sleep(2000) → keep thread busy for 2 seconds
        //     - After work is done, the thread dies(cannot be reused)
        //     ============================================================
        //    static void DoWork(int id)
        //    {
        //        Console.WriteLine($"[START] Worker {id} running on Thread {Thread.CurrentThread.ManagedThreadId}");

        //        Simulate heavy work by blocking this thread for 2 seconds

        //       Thread.Sleep(2000);

        //       Console.WriteLine($"[END]   Worker {id} finished on Thread {Thread.CurrentThread.ManagedThreadId}");
        //    }
        //    /*
        //================================================================
        //PROS of Thread Class
        //================================================================
        //1. Direct Control
        //   - You decide when to create, start, and stop a thread.
        //   - You can set priority, mark as background thread, etc.
        //2. True Parallelism
        //   - Multiple threads can run on different CPU cores at once.
        //3. Educational Value
        //   - Good for learning how OS-level threads work.

        //================================================================
        //CONS of Thread Class
        //================================================================
        //1. Heavyweight
        //   - Each thread costs ~1MB stack memory + OS overhead.
        //   - Creating/destroying many threads is expensive.
        //2. Poor Scalability
        //   - With hundreds/thousands of threads, CPU wastes time
        //     context switching between them instead of doing work.
        //3. No Reuse
        //   - Once a thread finishes, it is destroyed.
        //   - New work requires creating a brand new thread.
        //4. No Easy Composition
        //   - Waiting for many threads = manual (Thread.Join).
        //   - No built-in error handling or return values.

        //================================================================
        //WHEN TO USE
        //================================================================
        //✅ Very rare in modern .NET code.
        //✅ When you need a dedicated long-running thread
        //   (e.g., monitoring hardware, background logging).
        //✅ Good for **learning purposes**.

        //🚫 NOT recommended for high-scale apps (use ThreadPool/Task).
        //🚫 Not for async I/O (use async/await instead).

        //================================================================
        //*/

        #endregion

        #region task Class
        Console.WriteLine("=== TASK CLASS DEMO ===\n");

        // ============================================================
        // DEMO 1: Run tasks in parallel using Task.Run
        // ============================================================
        var tasks = new Task[5];

        for (int i = 0; i < 5; i++)
        {
            int taskId = i;

            // Task.Run schedules work on the ThreadPool
            tasks[i] = Task.Run(() =>
            {
                Console.WriteLine($"[START] Task {taskId} on Thread {Thread.CurrentThread.ManagedThreadId}");

               // simulate work
               Thread.Sleep(1000);
                Console.WriteLine($"[END]   Task {taskId} on Thread {Thread.CurrentThread.ManagedThreadId}");
            });
        }

        // Wait until all tasks complete
        await Task.WhenAll(tasks);

        // ============================================================
        // DEMO 2: Task with return value
        // ============================================================
        Task<int> sumTask = Task.Run(() =>
        {
            int sum = 0;

            for (int i = 1; i <= 10; i++)
            {
                // Print current thread info
                Console.WriteLine($"[WORK] Iteration {i} on Thread {Thread.CurrentThread.ManagedThreadId}");

                // Check threadpool status
                ThreadPool.GetAvailableThreads(out int workerAvail, out int ioAvail);
                ThreadPool.GetMaxThreads(out int workerMax, out int ioMax);

                int workerInUse = workerMax - workerAvail;
                int ioInUse = ioMax - ioAvail;

                Console.WriteLine($"[POOL] Worker Threads in use: {workerInUse}, Available: {workerAvail}");
                Console.WriteLine($"[POOL] I/O Threads in use: {ioInUse}, Available: {ioAvail}\n");

                sum += i;
                 // slow down loop to see output
            }

            return sum;
        });

        int result = await sumTask;
        Console.WriteLine($"\nTask with result: sum = {result}");

        // ============================================================
        // DEMO 3: Compare with raw Thread
        // ============================================================
        // Notice: With Thread you must manage lifecycle manually.
        Thread t = new Thread(() =>
        {
            Console.WriteLine($"[THREAD] Running on Thread {Thread.CurrentThread.ManagedThreadId}");
        });
        t.Start();
        t.Join(); // wait manually

        Console.WriteLine("\n=== DEMO COMPLETE ===");
    }
    #endregion


}
/*
================================================================
ASYNC / AWAIT BEHAVIOR: SERVER vs UI APPLICATION
================================================================

1. SERVER APPLICATIONS (Console, ASP.NET Core, Services)
--------------------------------------------------------
- Async methods run on THREADPOOL threads (not the program entry thread).
- When 'await' is hit:
    * The current ThreadPool thread is released back to the pool.
    * The async operation continues without blocking any thread.
    * Once the operation completes, the continuation is scheduled 
      on a ThreadPool thread (could be the same one or a different one).
- There is NO special SynchronizationContext in ASP.NET Core or Console apps.
- Key benefit: scalability → many requests/tasks can share a small number of threads.
- Example: an API server handling 10,000 requests only needs a few hundred 
  ThreadPool threads because async I/O frees them efficiently.

--------------------------------------------------------
2. UI APPLICATIONS (WinForms, WPF, MAUI, Xamarin)
--------------------------------------------------------
- The application has a SINGLE dedicated UI thread.
- This UI thread handles:
    * Drawing the interface
    * User interactions (button clicks, typing, resizing)
    * Event dispatch
- If the UI thread is blocked (e.g., Thread.Sleep), the entire app freezes 
  and shows "Not Responding".
- Async/await solves this by:
    * Running initial code on the UI thread.
    * When 'await' is hit, the UI thread is released so it can remain responsive.
    * After the async operation completes, the continuation is posted back 
      onto the UI thread using the SynchronizationContext.
    * This ensures you can safely update UI elements after await.
- Example: Button_Click event handler:
    * Code before 'await' runs on the UI thread.
    * At 'await', UI thread is freed for painting and input.
    * Code after 'await' resumes on the UI thread (so controls can be updated).

--------------------------------------------------------
KEY DIFFERENCES
--------------------------------------------------------
- Server apps: 
    * Continuations after 'await' resume on any ThreadPool thread.
    * Focus is on scalability, not preserving original thread identity.

- UI apps: 
    * Continuations after 'await' resume on the original UI thread.
    * Focus is on responsiveness and safe UI updates.

--------------------------------------------------------
SUMMARY
--------------------------------------------------------
- Async/await always frees the current thread at 'await'.
- The thread where continuation resumes depends on environment:
    * SERVER / CONSOLE → ThreadPool thread (no guarantee of same thread).
    * UI → the UI thread (SynchronizationContext captures and restores it).
*/

/*
================================================================
ASYNC / AWAIT: CONCURRENCY vs MULTITHREADING
================================================================

1. WHAT ASYNC / AWAIT REALLY DOES
---------------------------------
- Async/await is NOT automatic multithreading.
- It is compiler-generated "state machine" code that:
    * Runs method until first 'await'.
    * At 'await', current thread is FREED (returned to ThreadPool).
    * When the awaited operation completes, .NET schedules the 
      continuation on a ThreadPool thread (may or may not be the same one).

- Result: you get CONCURRENCY (many operations in flight without blocking),
  not guaranteed PARALLELISM.

---------------------------------
2. RELATION TO THREADPOOL
---------------------------------
- Async/await always needs a thread to run your code segments.
- Before 'await': code runs on caller's thread (ThreadPool in ASP.NET/Console,
  UI thread in WPF/WinForms).
- At 'await': thread is released; no thread is tied up during I/O wait.
- After 'await': continuation scheduled on ThreadPool (server/console) or
  back on UI thread (desktop/mobile with SynchronizationContext).

- If all ThreadPool threads are busy when an async operation completes:
    * Continuation waits until a thread is free, OR
    * ThreadPool may CREATE A NEW OS THREAD (up to maxWorkerThreads).
- Important: thread creation cost = same as raw Thread, but pool reuses them
  for efficiency.

---------------------------------
3. CONCURRENCY vs MULTITHREADING
---------------------------------
- MULTITHREADING:
    * Multiple OS threads execute code in parallel on different CPU cores.
    * Example: Task.Run(() => HeavyComputation()).
    * Good for CPU-bound work (math, loops, encryption, image processing).

- ASYNC / AWAIT:
    * Focused on I/O-bound work (HTTP calls, DB queries, file I/O).
    * Frees threads during waits → small number of threads can handle
      large number of requests.
    * Code before and after 'await' still runs on threads,
      but never more than one thread at a time for a given async method.
    * Not parallelism by itself — just efficient use of threads.

---------------------------------
4. WHY THIS MATTERS
---------------------------------
- SERVER (ASP.NET Core):
    * Async controllers free ThreadPool threads during I/O waits.
    * Increases scalability: thousands of concurrent requests can be served
      with relatively few threads.
    * Without async, threads block until I/O completes → pool exhaustion.

- UI (WPF/WinForms/MAUI):
    * Async/await keeps UI thread responsive.
    * Await frees the UI thread so it can process user input/painting.
    * Continuations resume on UI thread automatically (safe for updating controls).

---------------------------------
5. KEY TAKEAWAY
---------------------------------
- Async/await = CONCURRENCY mechanism, not parallelism.
- Still uses real OS threads (via ThreadPool), but reuses them smartly.
- ThreadPool may grow new threads if demand is high, but async/await by itself
  does NOT spawn new threads per operation.
- Use async/await for I/O-bound tasks.
- Use Task.Run / Parallel.For / raw threads for CPU-bound parallelism.
================================================================
*/
/*
================================================================
PARALLEL PROGRAMMING IN .NET (Parallel.For / Parallel.ForEach)
================================================================

1. PURPOSE
---------------------------------
- Parallel programming (Task Parallel Library) is designed for
  CPU-BOUND work (e.g., math, data processing, image manipulation).
- It tries to maximize CPU usage by splitting work across multiple
  ThreadPool threads and running them in parallel on multiple cores.

---------------------------------
2. HOW IT WORKS
---------------------------------
- Parallel.For / Parallel.ForEach take a workload (loop/collection)
  and break it into "chunks".
- These chunks are scheduled onto ThreadPool threads.
- By default, the number of active threads ≈ Environment.ProcessorCount
  (your logical CPU cores).
- Example: On a 12-core machine, ~12 threads work in parallel,
  each processing a chunk of iterations.
- If there are more iterations than threads, the extra work
  is placed in a QUEUE. When a thread finishes, it takes
  more work from the queue.

---------------------------------
3. THREADPOOL BEHAVIOR
---------------------------------
- Parallel.For uses ThreadPool threads, not raw OS threads.
- ThreadPool may create EXTRA threads if:
    * Many tasks are queued, and
    * Some threads are blocked (e.g., waiting on I/O).
- But more threads ≠ more CPU power:
    * Real parallelism is limited by logical processor count.
    * Too many threads causes context switching overhead.
- For CPU-bound work, TPL avoids spawning more threads than cores,
  because extra threads slow things down.

---------------------------------
4. WHAT IF WORK IS I/O-BOUND?
---------------------------------
- Parallel.ForEach with blocking I/O (e.g., .Result on HttpClient):
    * Each ThreadPool thread blocks until the response arrives.
    * Threads are not released like in async/await.
    * The loop body does not move forward until response comes back.
    * If many requests > threads, backlog forms, and ThreadPool may
      create new threads.
    * This scales poorly and can lead to thread explosion.
- Async/await should be used instead for I/O-bound work.

---------------------------------
5. COMPARISON WITH ASYNC/AWAIT
---------------------------------
- Parallel.For (CPU-bound):
    * Threads stay busy doing calculations.
    * True parallelism across cores.
    * Max throughput limited by hardware cores.
    * Best for math-heavy workloads.
- Async/await (I/O-bound):
    * Threads are released during waits.
    * Scales to thousands of concurrent operations.
    * Best for network, database, and file I/O.

---------------------------------
6. WHEN TO USE
---------------------------------
✅ Use Parallel.For / ForEach when:
    * The work is CPU-heavy and can be divided into independent parts.
    * Example: image processing, cryptography, simulations.

🚫 Avoid Parallel.For for I/O-bound operations:
    * Threads will block and waste resources.
    * Use async/await instead.

---------------------------------
7. KEY TAKEAWAY
---------------------------------
- Parallel.For/ForEach = CPU parallelism, limited by cores.
- Async/await = I/O concurrency, scalable beyond threads.
- Rule of thumb:
    * CPU-bound → Parallelism (TPL, Parallel.For).
    * I/O-bound → Async/await.
================================================================
*/


/*
================================================================
MAIN THREAD IN .NET APPLICATIONS
================================================================

1. ASP.NET CORE / CONSOLE APPLICATIONS (SERVER-SIDE)
---------------------------------------------------
- A "main thread" exists only at process startup.
- Responsibilities of the main thread:
    * Run Program.Main().
    * Initialize configuration, logging, DI container.
    * Start the Kestrel web server (ASP.NET Core).
    * Keep the process alive while the server runs.
- After startup:
    * The main thread is essentially idle or just monitoring.
    * It does NOT handle business logic or HTTP requests.
- All requests and async methods run on THREADPOOL threads.
- Each incoming HTTP request is dispatched from Kestrel
  to a ThreadPool worker thread.
- Async/await in controllers frees/reuses those ThreadPool threads.

Key Point:
- In server apps, the main thread is only for initialization
  and hosting. The real work is done by ThreadPool threads.

---------------------------------------------------
2. UI APPLICATIONS (WinForms, WPF, MAUI, Xamarin)
---------------------------------------------------
- The main thread = the UI thread.
- Responsibilities of the main/UI thread:
    * Run the application's message loop (event loop).
    * Render the interface (draw windows, update controls).
    * Handle user input events (clicks, typing, gestures).
- All UI code must run on this thread.
- If the UI thread is blocked (e.g., Thread.Sleep), the
  application freezes and shows "Not Responding."
- Async/await in UI apps:
    * Frees the UI thread during I/O waits.
    * Continuations after await are posted back onto the
      UI thread (via SynchronizationContext).
    * This keeps UI responsive and safe for updates.

Key Point:
- In UI apps, the main thread is critical: it drives rendering
  and input. Async/await is vital to keep it free.

---------------------------------------------------
3. COMPARISON
---------------------------------------------------
- ASP.NET Core / Console:
    * Main thread: startup only.
    * Business logic: ThreadPool threads.
    * Goal: scalability (free threads during I/O).
- UI apps:
    * Main thread: UI loop.
    * Business logic + events: UI thread.
    * Goal: responsiveness (keep UI free).

================================================================
SUMMARY
================================================================
- The "main thread" role depends on application type:
    * Server apps → initialization only.
    * UI apps → event loop + rendering.
- In both cases, async/await ensures efficient use of threads:
    * Server: scales to thousands of requests.
    * UI: keeps interface responsive.
================================================================
*/
