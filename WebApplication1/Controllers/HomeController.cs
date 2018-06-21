using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using AppCalculate;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private static readonly string LogFileName = "history_log";

        private static IEnumerable<string> ReadLog()
        {
            using (StreamReader sr = new StreamReader(LogFileName, System.Text.Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    yield return sr.ReadLine();
                }
            }
        }

        private static void WriteLog(string input)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(LogFileName, true))
                {
                    writer.WriteLine(input);
                    writer.Close();
                }
            }
            catch (IOException exc)
            {
                Console.Error.WriteLine(exc);
            }
        }

        public ViewResult Index()
        {
            return View(ReadLog());
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public void Submit(string txtEvaluate)
        {
            StringCalc once = new StringCalc();
            WriteLog(txtEvaluate);
            string text = once.DoCalculation(txtEvaluate).ToString();
            WriteLog(text);
            Index();
            return View("Index");
        }
    }
}
