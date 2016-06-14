using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskPropertiesDemo
{
    class Program
    {


        static void Main(string[] args)
        {
            //Demo1BasicException();
            //Demo3Properties();
            Demo4EscalationPolicy();

            Console.WriteLine("Press enter to finish.");
            Console.ReadLine();

        }

        /*
         * 微软的 .NET 框架会把 Task 抛出的任何没有被 Catch 的异常储存起来，
         * 直到你调用了一些触发成员(trigger members)如 Task.Wait(), Task.WaitAll(), Task.WaitAny()或者Task.Result，
         * 然后这些成员会抛出一个 AggregateException 的实例。
         * AggregateException 类型是为了包装一个或多个异常在一个实例里面，
         * 这是很实用的，我们想想，当你在调用 Task.WaitAll() 方法时候，
         * 所等待的 task 可能不止一个，如果这些 task 有多个抛出了异常，
         * 那么这些异常就应该全都被包装起来等我们处理。要遍历这个实例里面包含了的异常，
         * 可以获取它的 InnerExceptions 属性，它提供一个可枚举的容器(enumerable collection)，
         * 里面容纳了所有的异常。
         */
        private static void Demo1BasicException()
        {
            Task task1 = new Task(() => Console.WriteLine("Task 1: Hello World!"));
            //task2 抛出一个异常
            Task task2 = new Task(() =>
            {
                Exception ex = new NullReferenceException("I'm thrown by task2!");
                //Console.WriteLine("Task 2: Throw an exception: {0}", ex.GetType());
                throw ex;
            });

            task1.Start();
            task2.Start();

            try
            {
                Thread.Sleep(1000);
                Task.WaitAll(task1, task2);
            }
            //捕捉 AggregateException
            catch (AggregateException ex)
            {
                //从 InnerExceptions 属性获取一个可枚举的异常集合， 用 foreach 语句可以遍历其中包含的所有异常
                foreach (Exception inner in ex.InnerExceptions)
                {
                    Console.WriteLine("Exception: {0}\nType: {2}\nFrom {2}",
                                        inner.Message, inner.GetType(), inner.Source);
                }

            }

        }

        /*
         * 上述代码中加粗的那段就是一个迭代的处理程序(Iterative Handler)的应用，
         * Handle() 方法接受函数委托或者lambda表达式，用它们来处理异常。
         * 而且它们必须返回 true 或者 false。
         * 如果返回 true 说明你已经处理了异常，这个异常将不会被传递、抛出，
         * 返回 false 说明你无法处理异常，这个异常将继续传递下去。
         * 上述代码中处理了 OperationCancelledException，而对于 NullReferenceException 没有做出处理，
         * 那么它将继续被传递抛出并没有被捕捉，所以程序停止工作了。
         */
        private static void Demo2InterativeHandler()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;

            //Throw NullReferenceException
            Task task1 = new Task(() =>
            {
                Exception ex = new NullReferenceException("I'm thrown by task2!");
                Console.WriteLine("Task 2: Throw an exception: {0}", ex.GetType());
                throw ex;
            });

            //Throw OperationCanceledException when task2 receive tokenSource's cancel signal.
            Task task2 = new Task(() =>
            {
                token.WaitHandle.WaitOne();
                Exception ex = new OperationCanceledException(token);
            }, token);

            task1.Start();
            task2.Start();

            //发出取消任务执行的信号, task2 在这个时候检测到信号并抛出异常
            tokenSource.Cancel();

            try
            {
                Task.WaitAll(task1, task2);
            }
            catch (AggregateException ex)
            {
                //利用 Handle 方法, 传递委托或者lambda表达式作为异常处理程序来处理自己感兴趣的异常并传递自己不感兴趣的异常
                ex.Handle((inner) =>
                {
                    if (inner is OperationCanceledException)
                    {
                        Console.WriteLine("Ok, I'll handle the OperationCanceledException here...");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("No, I dont know how to handle such an exception, it will be propgated...");
                        return false;
                    }
                });
            }

        }



        //这里的 IsCompleted 属性在 task 正常或者非正常结束后为 true，为 fasle 说明 task 还在运行；
        // IsCanceled 属性在 task 被取消后为 true，否则为 false；
        // IsFaulted 属性在 task 抛出异常后为 true， 否则为 false；
        // Exception 属性在抛出异常后为该异常的引用，否则空。
        private static void Demo3Properties()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;

            Task task1 = new Task(() =>
            {
                Exception ex = new NullReferenceException("I'm thrown by task2!");
                Console.WriteLine("Task 2: Throw an exception: {0}", ex.GetType());
                throw ex;
            });

            Task task2 = new Task(() =>
            {
                token.WaitHandle.WaitOne();
                Exception ex = new OperationCanceledException(token);
            }, token);


            task1.Start();
            task2.Start();

            tokenSource.Cancel();

            try
            {
                Task.WaitAll(task1, task2);
            }
            catch (AggregateException ex)
            {
                //Ignore the excpetions now

            }
            //print out the properties
            Console.WriteLine("task1: IsCompleted({0}) IsCanceled({1}) IsFaulted({2}) Exception({3})",
                task1.IsCompleted, task1.IsCanceled, task1.IsFaulted, task1.Exception);
            Console.WriteLine("task2: IsCompleted({0}) IsCanceled({1}) IsFaulted({2}) Exception({3})",
                task2.IsCompleted, task2.IsCanceled, task2.IsFaulted, task2.Exception);
        }

        /*
         * 注意这里并没有打印"specical data"的内容，这是因为在 task 没有被 GC 回收的时候，第 "specical data" 的代码是不会执行的。
         * 另外注意的是 27 行不能调用例如 Task.WaitAll( task1, task2 )这样的方法，
         * 这样的触发式成员会抛出异常导致异常不能被捕捉而使程序不能继续运行。
         * 关于第 4 – 12 行代码的执行问题
         * TaskScheduler.UnobservedTaskException never gets called - StackOverflow
         */
        private static void Demo4EscalationPolicy()
        {
            //给 TaskScheduler.UnobservedTaskException 添加异常处理程序

            #region specical data
            // Note 1
            TaskScheduler.UnobservedTaskException +=
               (object sender, UnobservedTaskExceptionEventArgs eventArgs) =>
               {
                   eventArgs.SetObserved();
                   ((AggregateException)eventArgs.Exception).Handle(ex =>
                   {
                       // Note 2
                       Console.WriteLine("Exception Type: {0} Message : {1}", ex.GetType(), ex.Message);
                       return true;
                   });
               };
            #endregion

            Task task1 = new Task(() =>
            {
                throw new NullReferenceException("I'm thrown by task1!");
            });

            Task task2 = new Task(() =>
            {
                throw new ArgumentOutOfRangeException("I'm thrown by task2!");
            });

            task1.Start();
            task2.Start();
            //注意这里不能调用 Task.WaitAll() 等一些列的触发式成员方法
            while (!task1.IsCompleted || !task2.IsCompleted)
            {
                Thread.Sleep(500);
            }
            //Task.WaitAll(task1, task2);
            //task1.Dispose();
            //task2.Dispose();
            //task1 = null;
            //task2 = null;
            GC.Collect();
            Console.WriteLine("Press enter to finish.");
            Console.ReadLine();

        }
    }
}
