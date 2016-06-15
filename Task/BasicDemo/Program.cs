using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BasicDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            DemoStatus();
            //TaskResultDemo();
            // wait for input before exiting
            Console.WriteLine("Main method complete. Press enter to finish.");
            Console.ReadLine();
        }

        private static void DemoStatus()
        {
            var tasks = new List<Task<int>>();
            var source = new CancellationTokenSource();
            var token = source.Token;
            int completedIterations = 0;

            for (int n = 0; n <= 19; n++)
                tasks.Add(Task.Run(() => {
                    int iterations = 0;
                    for (int ctr = 1; ctr <= 2000000; ctr++)
                    {
                        token.ThrowIfCancellationRequested();
                        iterations++;
                    }
                    Interlocked.Increment(ref completedIterations);
                    if (completedIterations >= 10)
                        source.Cancel();
                    return iterations;
                }, token));

            Console.WriteLine("Waiting for the first 10 tasks to complete...\n");
            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException)
            {
                Console.WriteLine("Status of tasks:\n");
                Console.WriteLine("{0,10} {1,20} {2,14:N0}", "Task Id",
                                  "Status", "Iterations");
                foreach (var t in tasks)
                    Console.WriteLine("{0,10} {1,20} {2,14}",
                                      t.Id, t.Status,
                                      t.Status != TaskStatus.Canceled ? t.Result.ToString("N0") : "n/a");
            }

        }

        private static void CreateTaskDemo()
        {
            // use an Action delegate and a named method
            Task task1 = new Task(new Action(printMessage));

            // use a anonymous delegate
            Task task2 = new Task(delegate
            {
                printMessage();
            });

            // use a lambda expression and a named method
            Task task3 = new Task(() => printMessage());

            // use a lambda expression and an anonymous method
            Task task4 = new Task(() =>
            {
                printMessage();
            });

            //TaskCreationOptions
            // PreferFairness
            // LongRunning
            // AttachToParent
            task1.Start();
            task2.Start();
            task3.Start();
            task4.Start();

            
        }

        static void TaskResultDemo()
        {
            // create the task
            Task<int> task1 = new Task<int>(() =>
            {
                Thread.Sleep(5000);
                int sum = 0;
                for (int i = 0; i <= 100; i++)
                {
                    sum += i;
                }
                return sum;
            });
                        
            task1.Start();
            //task1.Wait();

            // write out the result
            Console.WriteLine("Result 1: {0}", task1.Result);


        }


        static void PassParameters()
        {
            string[] messages = { "First task", "Second task",
"Third task", "Fourth task" };
            foreach (string msg in messages)
            {
                Task myTask = new Task(obj => printMessage((string)obj), msg);
                myTask.Start();
            }
            // wait for input before exiting
            Console.WriteLine("Main method complete. Press enter to finish.");
            Console.ReadLine();

        }

        static void printMessage(string message)
        {
            Console.WriteLine("Message: {0}", message);
        }

        static void printMessage()
        {
            Console.WriteLine("Hello World");
        }

    }
}
