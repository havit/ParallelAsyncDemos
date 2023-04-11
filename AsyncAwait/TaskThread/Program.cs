using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskThread
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Console.WriteLine("Nastavit pozici výstupu");
			Console.ReadLine();

			Console.WriteLine("-= Čas na malou přestávku =-");
			Thread.Sleep(16);
			Console.WriteLine("Proč zrovna 16?");
			Console.ReadLine();

			/*
			 *
			 *
			 *
			 *
			 *
			 *
			 *
			 */

			Console.WriteLine("-= new Thread =-");
			var watch = Stopwatch.StartNew();

			for (int i = 2; i < 50; i++)
			{
				Thread thread = new Thread(_ => SumRootN(i));
				thread.Start();
			}

			Console.WriteLine(watch.Elapsed);
			Console.WriteLine("Moc threadů a taky docela pomalé.");
			Console.ReadLine();
			Console.WriteLine("A nepočkali jsme na výsledek.");
			Console.ReadLine();

			/*
			 *
			 *
			 *
			 *
			 *
			 *
			 *
			 */

			Console.WriteLine("-= new Thread, počkáme na konec =-");
			watch = Stopwatch.StartNew();
			var events = new List<ManualResetEvent>();

			for (int i = 2; i < 50; i++)
			{
				var resetEvent = new ManualResetEvent(false);
				Thread thread = new Thread(_ =>
				{
					SumRootN(i);
					resetEvent.Set();
				});
				events.Add(resetEvent);
				thread.Start();
			}

			WaitHandle.WaitAll(events.ToArray());
			Console.WriteLine(watch.Elapsed);
			Console.WriteLine("Pořád moc threadů a pomalé.");
			Console.ReadLine();

			/*
			 *
			 *
			 *
			 *
			 *
			 *
			 *
			 */

			Console.WriteLine("-= ThreadPool =-");
			watch = Stopwatch.StartNew();
			events = new List<ManualResetEvent>();

			for (int i = 2; i < 50; i++)
			{
				int j = i;
				var resetEvent = new ManualResetEvent(false);
				ThreadPool.QueueUserWorkItem(new WaitCallback(_ =>
				{
					SumRootN(j);
					resetEvent.Set();
				}));
				events.Add(resetEvent);
			}

			WaitHandle.WaitAll(events.ToArray());
			Console.WriteLine(watch.Elapsed);
			Console.WriteLine("Lepší, ale složité čekání na výsledek.");
			Console.ReadLine();

			/*
			 *
			 *
			 *
			 *
			 *
			 *
			 *
			 */

			Console.WriteLine("-= Task.Factory =-");
			watch = Stopwatch.StartNew();
			List<Task> tasks = new List<Task>();

			for (int i = 2; i < 50; i++)
			{
				int j = i;
				Task task = Task.Factory.StartNew(() => SumRootN(j));
				tasks.Add(task);
			}

			Task.WaitAll(tasks.ToArray());
			Console.WriteLine(watch.Elapsed);
			Console.WriteLine("Ještě lepší.");
			Console.ReadLine();

			/*
			 *
			 *
			 *
			 *
			 *
			 *
			 *
			 */

			Console.WriteLine("-= Task.Run =-");
			watch = Stopwatch.StartNew();
			tasks = new List<Task>();

			for (int i = 2; i < 50; i++)
			{
				int j = i;
				Task task = Task.Run(() => SumRootN(j));
				tasks.Add(task);
			}

			Task.WaitAll(tasks.ToArray());
			Console.WriteLine(watch.Elapsed);
			Console.WriteLine("Stejné jako předchozí.");
			Console.ReadLine();

			/*
			 *
			 *
			 *
			 *
			 *
			 *
			 *
			 */

			Console.WriteLine("-= Parallel.Invoke =-");
			watch = Stopwatch.StartNew();
			List<Action> actions = new List<Action>();

			for (int i = 2; i < 50; i++)
			{
				int j = i;
				actions.Add(() => SumRootN(j));
			}

			Parallel.Invoke(actions.ToArray());
			Console.WriteLine(watch.Elapsed);
			Console.WriteLine("Stejné jako předchozí.");
			Console.ReadLine();

			/*
			 *
			 *
			 *
			 *
			 *
			 *
			 *
			 */

			Console.WriteLine("-= parallel for =-");
			watch = Stopwatch.StartNew();
			tasks = new List<Task>();

			Parallel.For(2, 50, (i) =>
			{
				SumRootN(i);
			});

			Task.WaitAll(tasks.ToArray());
			Console.WriteLine(watch.Elapsed);
			Console.WriteLine("Stejné jako předchozí.");
			Console.ReadLine();
		}

		public static void SumRootN(int root)
		{
			double result = 0;
			for (int i = 1; i < 10_000_000; i++)
			{
				result += Math.Exp(Math.Log(i) / root);
			}
			Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} root {root}: {result}");
		}
	}
}