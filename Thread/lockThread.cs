using System;
using System.Threading;
using System.Collections.Generic;
namespace ConsoleApplication3
{
    class Program
    {

        private static bool _isDone = false;
        private static object _lock = new object();

        static void Main(string[] args)
        {
            //TestLock();
            TestSemaphore();
            //TestReadWriteLock();
            Console.ReadLine();
        }

        #region Lock exclusive
        static void TestLock()
        {
            Console.WriteLine("Test lock ----------------------------");
            new Thread(Done).Start();
            new Thread(Done).Start();
            new Thread(Done).Start();
            new Thread(Done).Start();
            new Thread(Done).Start();
            new Thread(Done).Start();
            new Thread(Done).Start();
            new Thread(Done).Start();
            new Thread(Done).Start();
            new Thread(Done).Start();
            new Thread(Done).Start();
            new Thread(Done).Start();
        }

        static void Done()
        {
            lock (_lock)
            {
                if (!_isDone)
                {
                    Console.WriteLine("Done");
               
                    _isDone = true;
                }
            }
        }

        #endregion

        #region Semaphore

        static void TestSemaphore()
        {
            for (int i = 1; i <= 5; i++)
                new Thread(Enter).Start(i);

        }

        static SemaphoreSlim _sem = new SemaphoreSlim(3); // limit the number of threads that can access the resouce at one time to 3

        static void Enter(object id)
        {
            Console.WriteLine(id + " begin to queue...");
            _sem.Wait();

            Console.WriteLine(id + "      begin to execute !");
            Thread.Sleep(1000 * (int)id);

            Console.WriteLine(id + "           done, leave!");

            _sem.Release();

        }

        #endregion

        #region Read/Write lock

        private static ReaderWriterLock _rwlock = new ReaderWriterLock();
        static List<int> list = new List<int>();

        public static void TestReadWriteLock()
        {
            new Thread(Read).Start();
            new Thread(Read).Start();
            new Thread(Read).Start();
            new Thread(Write).Start();
            new Thread(Write).Start();
        }

        private static void Read()
        {
            while (true)
            {
                try
                {
                    _rwlock.AcquireReaderLock(1000);
                }
                catch (ApplicationException)
                {
                    Console.WriteLine("Read thread: Time is out");
                    continue;
                }


                try
                {
                    if (list.Count > 0)
                    {
                        int result = list[list.Count - 1];
                        Console.WriteLine(result);

                        Thread.Sleep(1000);
                    }
                }
                finally
                {
                    // decrease the counter, release the lock
                    _rwlock.ReleaseReaderLock();
                }
            }
        }

        private static void Write()
        {
            int writeCount = 0;
            while (true)
            {
                try
                {
                    _rwlock.AcquireWriterLock(1000);
                }
                catch (ApplicationException)
                {
                    Console.WriteLine("Write thread: Time is out");
                    continue;
                }
                try
                {
                    list.Add(writeCount++);
                    Thread.Sleep(2000);
                }
                finally
                {
                    _rwlock.ReleaseWriterLock();
                }
            }
        }

        #endregion

    }
}