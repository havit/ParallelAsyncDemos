using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestNaZaver
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			Console.WriteLine("Nastavit pozici výstupu");
			Console.ReadLine();

			Console.WriteLine("Jaké slovo se zobrazí a proč?");
			TestTask();
			Console.Write("KOP ");
			Console.ReadKey();
		}

		private static void TestTask()
		{
			Console.Write("HE ");
			Task.Factory.StartNew(LongTask);
			//Thread.Sleep(1000);
			Console.Write("LI ");
		}

		private static void LongTask()
		{
			Console.Write("TÉ ");
			Thread.Sleep(1000);
			Console.Write("RA ");
		}
	}
}