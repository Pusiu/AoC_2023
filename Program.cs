using System.Reflection;
namespace AoC_2023
{
    class Program
    {
        static void Main(string[] args)
        {
            var days = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x => typeof(Day).IsAssignableFrom(x) && !x.IsAbstract).Select(x => Activator.CreateInstance(x) as Day).ToList();
            days[days.Count-1].Run();
            //days[0].Run();

            // Console.ReadLine();
            
        }
    }
}