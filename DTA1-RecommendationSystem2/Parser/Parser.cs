using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DTA1_RecommendationSystem2.Parser
{
    public class Parser
    {
        public static Dictionary<int, Dictionary<int, double>> Parse(char delimiter, string path) {
            var result = new Dictionary<int, Dictionary<int, double>>();

            var lines = File.ReadAllLines(path)
                .Select(line => line
                    .Split(delimiter)
                    .Take(3)
                    .Select(double.Parse)
                    .ToList())
                .ToList();

            foreach (var line in lines)
            {
                if (result.ContainsKey((int)line[0]))
                    result[(int)line[0]].Add((int)line[1], line[2]);
                else
                    result.Add((int)line[0], new Dictionary<int, double> { { (int)line[1], line[2] } });
            }
            return result;

        }
    }
}
