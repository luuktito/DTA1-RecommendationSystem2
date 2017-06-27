using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTA1_RecommendationSystem2.Algorithms
{
    class Deviation
    {
        public static List<int> GetItems(Dictionary<int, Dictionary<int, double>> ratings)
        {
            List<int> allItems = new List<int>();

            foreach (var user in ratings)
            {
                foreach (var itemId in user.Value.Keys)
                {
                    if (!allItems.Contains(itemId))
                        allItems.Add(itemId);
                }
            }

            return allItems;
        }

        public static Dictionary<Tuple<int, int>, Tuple<double, int>> GetDeviationMatrix(Dictionary<int, Dictionary<int, double>> ratings, List<int> allItems)
        {
            Dictionary<Tuple<int, int>, Tuple<double, int>> deviationMatrix = new Dictionary<Tuple<int, int>, Tuple<double, int>>();

            for (var i = 0; i < allItems.Count; i++) 
            {
                for (var j = 0; j < allItems.Count; j++)
                {
                    var currentItemX = allItems[i];
                    var currentItemY = allItems[j];
                    var currentDeviation = 0.0;
                    var currentWeight = 0;
                    if (currentItemX != currentItemY) {
                        foreach (var user in ratings) {
                            if (user.Value.ContainsKey(currentItemX) && user.Value.ContainsKey(currentItemY))
                            {
                                currentDeviation += (user.Value[currentItemX] - user.Value[currentItemY]);
                                currentWeight += 1;
                            }
                        }
                    }

                    currentDeviation = (currentWeight != 0) ? currentDeviation / currentWeight : 0;
                    var combinationTuple = new Tuple<int, int>(currentItemX, currentItemY);
                    var deviationWeight = new Tuple<double, int>(currentDeviation, currentWeight);
                    deviationMatrix.Add(combinationTuple, deviationWeight);
                }
            }

            return deviationMatrix;
        }

        public static Dictionary<Tuple<int, int>, Tuple<double, int>> UpdateDeviationMatrix(Dictionary<Tuple<int, int>, Tuple<double, int>> deviationMatrix, Dictionary<int, Dictionary<int, double>> ratings, int userId, int itemId, double newRating)
        {
            var itemDeviations = deviationMatrix.Where(x => x.Key.Item1 == itemId || x.Key.Item2 == itemId).ToDictionary(x => x.Key, x => x.Value);

            foreach (var combination in itemDeviations)
            {
                var currentItemX = combination.Key.Item1;
                var currentItemY = combination.Key.Item2;
                var currentDeviation = combination.Value.Item1;
                var currentWeight = combination.Value.Item2;
                var user = ratings[userId];

                if ((currentItemX == itemId && user.ContainsKey(currentItemY)) || (currentItemY == itemId && user.ContainsKey(currentItemX)))
                {
                    var leftRating = 0.0;
                    var rightRating = 0.0;

                    if (currentItemX == itemId)
                    {
                        leftRating = newRating;
                        rightRating = user[currentItemY];
                    }
                    else
                    {
                        leftRating = user[currentItemX];
                        rightRating = newRating;
                    }

                    currentDeviation = ((currentDeviation * currentWeight) + (leftRating - rightRating)) / (currentWeight + 1);

                    deviationMatrix[combination.Key] = new Tuple<double, int>(currentDeviation, (currentWeight + 1));
                }
            }

            return deviationMatrix;
        }
    }
}
