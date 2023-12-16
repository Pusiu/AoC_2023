using System.Reflection;
using System.Net;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace AoC_2023
{
    public abstract class Day
    {
        protected string input;
        protected bool isTest = false;
        public virtual async void Run()
        {
            if (string.IsNullOrEmpty(input))
                input = await GetInput();
            var stopwatch = Stopwatch.StartNew();
            Console.WriteLine($"Part 1: {Part1()}\nin {stopwatch.Elapsed}");
            stopwatch.Restart();
            Console.WriteLine($"Part 2: {Part2()}\nin {stopwatch.Elapsed}");
        }

        public async Task<string> GetInput()
        {
            var baseDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..//..//..//"));
            var inputDirectory = baseDirectory + @"/Inputs/";
            if (!Directory.Exists(inputDirectory))
                Directory.CreateDirectory(inputDirectory);

            var match = Regex.Match(GetType().ToString(), @"\d+$");
            int dayNumber = int.Parse(match.Value);


            var filename = $"day{dayNumber}{(isTest ? "_test" : "")}.txt";
            if (isTest) Console.WriteLine("Reading test file");
            if (File.Exists(inputDirectory + filename))
            {
                input = File.ReadAllText(inputDirectory + filename);
            }
            else if (!isTest)
            {
                var cookiePath = baseDirectory + @"/cookie.txt";
                var cookie = "";
                if (File.Exists(cookiePath))
                {
                    cookie = File.ReadAllText(cookiePath);
                }
                else
                {
                    Console.WriteLine("Session cookie does not exist, please provie a session cookie: ");
                    cookie = Console.ReadLine();
                    File.WriteAllText(cookiePath, cookie);
                }
                var url = $"https://adventofcode.com/2023/day/{dayNumber}/input";
                var client = new HttpClient();
                try
                {
                    var resp = client.Send(new HttpRequestMessage(HttpMethod.Get, url)
                    {
                        Headers = { { "cookie", cookie } }
                    });
                    input = await resp.Content.ReadAsStringAsync();
                    File.WriteAllText(inputDirectory + filename, input);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error getting input: {e.Message}");
                }
            }
            else
            {
                throw new Exception("Test file does not exist");
            }

            return input.Replace("\r", "").Trim();
        }

        public virtual string Part1()
        {
            return "";
        }

        public virtual string Part2()
        {
            return "";
        }
    }
}