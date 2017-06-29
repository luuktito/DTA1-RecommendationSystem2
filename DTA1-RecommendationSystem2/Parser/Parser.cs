using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DTA1_RecommendationSystem2.Parser
{
    public class Parser
    {
        public static Tuple<Dictionary<int, Dictionary<int, double>>, Dictionary<int, HashSet<int>>> Parse(char delimiter, string path) {
            var result = new Dictionary<int, Dictionary<int, double>>();
            var resultItemUsers = new Dictionary<int, HashSet<int>>();

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


                var itemId = (int)line[1];
                var userId = (int)line[0];
                if (resultItemUsers.ContainsKey(itemId))
                    resultItemUsers[itemId].Add(userId);
                else
                    resultItemUsers.Add(itemId, new HashSet<int> { userId });
            }

            return new Tuple<Dictionary<int, Dictionary<int, double>>, Dictionary<int, HashSet<int>>>(result, resultItemUsers);

        }
    }
}
