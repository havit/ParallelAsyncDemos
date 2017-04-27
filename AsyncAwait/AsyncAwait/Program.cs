﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait
{
	public class Program
	{
		private static void Main(string[] args)
		{
			Console.WriteLine("Nastavit pozici výstupu");
			Console.ReadLine();

			Console.WriteLine("-= sekvenční =-");
			var watch = Stopwatch.StartNew();

			for (int i = 2; i < 20; i++)
			{
				var result = SumRootN(i);
				Console.WriteLine($"root {i} : {result}");
			}

			Console.WriteLine(watch.Elapsed);
			Console.WriteLine("Docela pomalé, nešlo by to rychleji?");
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

			Console.WriteLine("-= task vrací hodnotu =-");
			watch = Stopwatch.StartNew();

			for (int i = 2; i < 20; i++)
			{
				var result = Task.Run<double>(() => SumRootN(i)).Result;
				Console.WriteLine($"root {i} : {result}");
			}

			Console.WriteLine(watch.Elapsed);
			Console.WriteLine("Žádné zlepšení");
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

			Console.WriteLine("-= vše v tasku =-");
			watch = Stopwatch.StartNew();

			for (int i = 2; i < 20; i++)
			{
				Task.Run(() =>
				{
					var result = SumRootN(i);
					Console.WriteLine($"root {i} : {result}");
				});
			}

			Console.WriteLine(watch.Elapsed);
			Console.WriteLine("Něco se nepovedlo");
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

			Console.WriteLine("-= vše v tasku opravený parametr =-");
			watch = Stopwatch.StartNew();

			for (int i = 2; i < 20; i++)
			{
				int param = i;
				Task.Run(() =>
				{
					var result = SumRootN(param);
					Console.WriteLine($"root {param} : {result}");
				});
			}

			Console.WriteLine(watch.Elapsed);
			Console.WriteLine("A co to měření času?");
			Console.WriteLine("Mimochodem, k tomuhle se ještě vrátíme.");
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

			Console.WriteLine("-= vše v tasku opravený parametr =-");
			watch = Stopwatch.StartNew();

			List<Task> tasky = new List<Task>();
			for (int i = 2; i < 20; i++)
			{
				int param = i;
				Task task = Task.Run(() =>
					{
						var result = SumRootN(param);
						Console.WriteLine($"root {param} : {result}");
					});
				tasky.Add(task);
			}
			Task.WaitAll(tasky.ToArray());

			Console.WriteLine(watch.Elapsed);
			Console.WriteLine("To už je lepší");
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

			Parallel.For(2, 20, (i) =>
			{
				var result = SumRootN(i);
				Console.WriteLine($"root {i} : {result}");
			});

			Console.WriteLine(watch.Elapsed);
			Console.WriteLine("Tohle taky");
			Console.ReadLine();
		}

		public static double SumRootN(int root)
		{
			double result = 0;
			for (int i = 1; i < 10_000_000; i++)
			{
				result += Math.Exp(Math.Log(i) / root);
			}
			return result;
		}
	}
}