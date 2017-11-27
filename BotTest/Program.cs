using System;
using System.Collections.Generic;
using Take.Blip.Client.Console;

namespace BotTest
{
    
    public class Program
    {
        public static List<double> valores = new List<double>();
        static int Main(string[] args) => ConsoleRunner.RunAsync(args).GetAwaiter().GetResult();
    }
}