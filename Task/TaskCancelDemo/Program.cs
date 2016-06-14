using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskCancelDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Demo1Cancel();
            Demo2CancelMultiTask();
            //Demo4DetectCancel();

            // wait for input before exiting
            Console.WriteLine("Main method complete. Press enter to finish.");
            Console.ReadLine();
        }


        ////如果有释放的资源
        //while (true)
        //{
        //    if (token.IsCancellationRequested)
        //    {
        //        // tidy up and release resources
        //        throw new OperationCanceledException(token);
        //    }
        //    else
        //    {
        //        // do a unit of work
        //    }
        //}
        //如果没有要释放的资源过
        //while (true)
        //{
        //    token.ThrowIfCancellationRequested();
        //    // do a unit of work
        //}
        private static void Demo1Cancel()
        {
            // create the cancellation token source
            CancellationTokenSource tokenSource = new CancellationTokenSource();

            // create the cancellation token
            CancellationToken token = tokenSource.Token;

            // create the task
            Task task = new Task(() =>
            {
                for (int i = 0; i < int.MaxValue; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Task cancel detected");
                        // Must throw this excpetion
                        throw new OperationCanceledException(token);
                    }
                    else
                    {
                        int line = i % 10 + 10;
                        Console.SetCursorPosition(5, line);
                        Console.WriteLine("Int value {0}", i);
                    }
                }
            }, token);

            // wait for input before we start the task
            Console.WriteLine("Press enter to start task");
            Console.ReadLine();
            // start the task
            task.Start();

            Console.WriteLine("Press enter again to cancel task");
            Console.ReadLine();
            tokenSource.Cancel();

        }

        /*
         * 1 取消多个Task, 可以使用一个CancellationToken 创建多个不同的Tasks, 
         * 这样当这个Token的Cancel()方法调用的时候，就可以取消多个Tasks.
         * 2 可以用CancellationTokenSource.CreateLinkedTokenSource()创建一个组合Token，
         * 这个组合的token有很多的CancellationToken组成。主要组合token中任意一个token调用
         * 了Cancel()方法，那么使用这个组合token的所有task就会被取消。
        */
        private static void Demo2CancelMultiTask()
        {
            // create the cancellation token sources
            CancellationTokenSource tokenSource1 = new CancellationTokenSource();
            CancellationTokenSource tokenSource2 = new CancellationTokenSource();
            CancellationTokenSource tokenSource3 = new CancellationTokenSource();

            // create a composite token source using multiple tokens
            CancellationTokenSource compositeSource =
                CancellationTokenSource.CreateLinkedTokenSource(
            tokenSource1.Token, tokenSource2.Token, tokenSource3.Token);

            // create a cancellable task using the composite token
            Task.Factory.StartNew(() =>
            {
                // wait until the token has been cancelled
                compositeSource.Token.WaitHandle.WaitOne();
                Console.WriteLine(">>>>> Wait handle released 1");

                // throw a cancellation exception
                throw new OperationCanceledException(compositeSource.Token);
            }, compositeSource.Token);

            Task.Factory.StartNew(() =>
            {
                // wait until the token has been cancelled
                compositeSource.Token.WaitHandle.WaitOne();
                Console.WriteLine(">>>>> Wait handle released 2");

                // throw a cancellation exception
                throw new OperationCanceledException(compositeSource.Token);
            }, compositeSource.Token);

            Task.Factory.StartNew(() =>
            {
                // wait until the token has been cancelled
                compositeSource.Token.WaitHandle.WaitOne();
                Console.WriteLine(">>>>> Wait handle released 3");

                // throw a cancellation exception
                throw new OperationCanceledException(compositeSource.Token);
            }, compositeSource.Token);
            

            // make sure the task is running
            Thread.Sleep(5000);
            // cancel one of the original tokens
            tokenSource2.Cancel();

            //compositeSource.Cancel();
        }

        private static void Demo3DetectCancel()
        {
            // create the cancellation token source
            CancellationTokenSource tokenSource = new CancellationTokenSource();

            // create the cancellation token
            CancellationToken token = tokenSource.Token;

            // create the task
            Task task = new Task(() =>
            {
                for (int i = 0; i < int.MaxValue; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Task cancel detected");
                        // Must throw this excpetion
                        throw new OperationCanceledException(token);
                    }
                    else
                    {
                        int line = i % 10 + 10;
                        Console.SetCursorPosition(5, line);
                        Console.WriteLine("Int value {0}", i);
                    }
                }
            }, token);

            // register a cancellation delegate
            token.Register(() =>
            {
                Console.WriteLine(">>>>>> Delegate Invoked\n");
            });

            // wait for input before we start the task
            Console.WriteLine("Press enter to start task");
            Console.ReadLine();
            // start the task
            task.Start();

            Console.WriteLine("Press enter again to cancel task");
            Console.ReadLine();
            tokenSource.Cancel();
        }

        private static void Demo4DetectCancel()
        {
            // create the cancellation token source
            CancellationTokenSource tokenSource = new CancellationTokenSource();

            // create the cancellation token
            CancellationToken token = tokenSource.Token;

            // create the task
            Task task1 = new Task(() =>
            {
                for (int i = 0; i < /*100000*/int.MaxValue; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Task cancel detected");
                        // Must throw this excpetion
                        throw new OperationCanceledException(token);
                    }
                    else
                    {
                        int line = i % 10 + 10;
                        Console.SetCursorPosition(5, line);
                        Console.WriteLine("Int value {0}", i);
                    }
                }
            }, token);

            // create a second task that will use the wait handle
            Task task2 = new Task(() =>
            {
                // wait on the handle
                token.WaitHandle.WaitOne();
                // write out a message
                Console.WriteLine(">>>>> Wait handle released");
            });

            // wait for input before we start the task
            Console.WriteLine("Press enter to start task");
            Console.WriteLine("Press enter again to cancel task");
            Console.ReadLine();
            // start the tasks
            task1.Start();
            task2.Start();

            // read a line from the console.
            Console.ReadLine();

            // cancel the task
            Console.WriteLine("Cancelling task");
            tokenSource.Cancel();


        }

    }
}
