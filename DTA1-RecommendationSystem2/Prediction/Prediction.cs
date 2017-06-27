using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTA1_RecommendationSystem2.Prediction
{
    class Prediction
    {
        public static double GetPrediction(Dictionary<Tuple<int, int>, Tuple<double, int>> deviationMatrix, Dictionary<int, Dictionary<int, double>> ratings, int userId, int itemId)
        {
            var itemDeviations = deviationMatrix.Where(x => x.Key.Item1 == itemId);
            var currentRating = 0.0;
            var currentWeight = 0;

            foreach (var combination in itemDeviations)
            {
                if (combination.Key.Item2 != itemId && ratings[userId].ContainsKey(combination.Key.Item2))
                {
                    currentRating += ((ratings[userId][combination.Key.Item2] + combination.Value.Item1) * combination.Value.Item2);
                    currentWeight += combination.Value.Item2;
                }
            }

            currentRating = currentWeight != 0 ? (currentRating / currentWeight) : 0; 
            return currentRating;
        }
    }
}
