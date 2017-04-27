using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SlowWeb.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			Random rnd = null;
			if (int.TryParse(Request.Query["m"], out int result))
			{
				rnd = new Random(result);
			}
			else
			{
				rnd = new Random();
			}

			int sleep = rnd.Next(1000, 3000);
			Thread.Sleep(sleep);
			ViewData["Message"] = sleep;
			return View();
		}
	}
}