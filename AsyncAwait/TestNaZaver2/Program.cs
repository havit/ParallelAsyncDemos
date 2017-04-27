using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TestNaZaver2
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Console.WriteLine("Nastavit pozici výstupu");
			Console.ReadLine();

			Console.WriteLine("Jaké slovo se zobrazí a proč?");
			TestAwait().NoWarning();
			Console.Write("MO ");
			Console.ReadKey();
		}

		private static async Task TestAwait()
		{
			Console.Write("LO ");
			await LongTaskAsync();
			Console.Write("VA ");
		}

		private static async Task LongTaskAsync()
		{
			Console.Write("KO ");
			await Task.Delay(1000);
			Console.Write("TI ");
		}
	}

	public static class Util
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void NoWarning(this Task task) { }
	}
}