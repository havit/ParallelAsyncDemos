using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AssyncAwait
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Console.WriteLine("Nastavit pozici výstupu");
			Console.WriteLine("Spustit SlowWeb z vedlejší solution");
			Console.ReadLine();

			var urls = new[] { "http://localhost:51952/home?m=1",
				"http://localhost:51952/home?m=2",
				"http://localhost:51952/home?m=3",
				"http://localhost:51952/home?m=4",
				"http://localhost:51952/home?m=5",
				"http://localhost:51952/home?m=6",
				"http://localhost:51952/home?m=7",
				"http://localhost:51952/home?m=8",
				"http://localhost:51952/home?m=9",
				"http://localhost:51952/home?m=10",
				"http://localhost:51952/home?m=11",
				"http://localhost:51952/home?m=12",
				"http://localhost:51952/home?m=13",
				"http://localhost:51952/home?m=14",
				"http://localhost:51952/home?m=15",
				"http://localhost:51952/home?m=16",
				"http://localhost:51952/home?m=17",
				"http://localhost:51952/home?m=18",
				"http://localhost:51952/home?m=19",
				"http://localhost:51952/home?m=20" };

			Console.WriteLine("-= paralelní =-");
			var watch = Stopwatch.StartNew();

			Parallel.ForEach(urls, url =>
			{
				var http = WebRequest.CreateHttp(url);
				Console.WriteLine($"{url} start {Thread.CurrentThread.ManagedThreadId}");
				using (WebResponse response = http.GetResponse())
				{
					Console.WriteLine($"{url} end {Thread.CurrentThread.ManagedThreadId}");
				}
			});

			Console.WriteLine(watch.Elapsed);
			Console.WriteLine("Nic moc");
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

			Console.WriteLine("-= rozdělíme na asynchroní volání =-");
			watch = Stopwatch.StartNew();

			foreach (var url in urls)
			{
				var webRequest = WebRequest.CreateHttp(url);
				Console.WriteLine($"{url} start {Thread.CurrentThread.ManagedThreadId}");
				webRequest.BeginGetResponse(new AsyncCallback((result) =>
				{
					WebResponse response = webRequest.EndGetResponse(result);
					Console.WriteLine($"{url} end {Thread.CurrentThread.ManagedThreadId}");
				}), /* případné parametry */ null);
			}

			Console.WriteLine(watch.Elapsed);
			Console.WriteLine("To už je lepší, i když jsme to nezměřili.");
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

			Console.WriteLine("-= rozdělíme na asynchroní =-");

			foreach (var url in urls.Take(10))
			{
				var webRequest = WebRequest.CreateHttp(url);
				Console.WriteLine($"{url} start {Thread.CurrentThread.ManagedThreadId}");
				// takto to nelze použít
				// WebResponse response = await webRequest.GetResponseAsync();
				// zkusíme to jinak
				WebResponse response = webRequest.GetResponseAsync().Result;
				Console.WriteLine($"{url} end {Thread.CurrentThread.ManagedThreadId}");
			}

			Console.WriteLine("Tak to nevyšlo.");
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

			async Task LoadAsync(string url)
			{
				var webRequest = WebRequest.CreateHttp(url);
				Console.WriteLine($"{url} start {Thread.CurrentThread.ManagedThreadId}");
				WebResponse response = await webRequest.GetResponseAsync();
				Console.WriteLine($"{url} end {Thread.CurrentThread.ManagedThreadId}");
			}

			Console.WriteLine("-= oddělíme do samostatné metody =-");
			foreach (var url in urls)
			{
				LoadAsync(url).NoWarning();
			}

			Console.WriteLine("To už vypadá dobře.");
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

			Console.WriteLine("-= zkusím to zkombinovat všechno dohromady a počkat na výsledek =-");
			watch = Stopwatch.StartNew();

			List<Task> tasks = new List<Task>();
			foreach (var url in urls)
			{
				Task task = Task.Run(async () => await LoadAsync(url));
				tasks.Add(task);
			}
			Task.WaitAll(tasks.ToArray());

			Console.WriteLine(watch.Elapsed);
			Console.WriteLine("A tohle ještě lépe.");
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

			Console.WriteLine("-= zkusím to zkombinovat všechno dohromady a počkat na výsledek =-");
			watch = Stopwatch.StartNew();

			tasks = new List<Task>();
			Parallel.ForEach(urls, url =>
			{
				Task task = LoadAsync(url);
				tasks.Add(task);
			});
			Task.WaitAll(tasks.ToArray());

			Console.WriteLine(watch.Elapsed);
			Console.WriteLine("A tohle ještě lépe.");
			Console.ReadLine();
		}
	}

	public static class Util
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void NoWarning(this Task task) { }
	}
}