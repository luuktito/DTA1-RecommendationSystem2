using System;
using System.Collections.Generic;
using System.Linq;
using DTA1_RecommendationSystem2.Utils;
using System.Diagnostics;

namespace DTA1_RecommendationSystem2.Algorithms
{
    class ItemItem
    {
        private Dictionary<int, Dictionary<int, double>> ratings;
        private Matrix<Tuple<double, int>> deviationMatrix;
        private List<int> allItems;
        private Dictionary<int, HashSet<int>> itemAndUsers;

        public ItemItem(char delimiter, string pathDictionary)
        {
            var parseResult = Parser.Parser.Parse(delimiter, pathDictionary);
            ratings = parseResult.Item1;
            itemAndUsers = parseResult.Item2;
            allItems = GetItems();
            deviationMatrix = new Matrix<Tuple<double, int>>(allItems.Count, allItems.Count, GetLookUpTables());
        }

        private List<int> GetItems()
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
            allItems.Sort();
            return allItems;
        }

        private Dictionary<int, int> GetLookUpTables()
        {
            Dictionary<int, int> dictionaryItemIndex = new Dictionary<int, int>();

            for (var i = 0; i < allItems.Count; i++)
            {
                dictionaryItemIndex[allItems[i]] = i;
            }

            return dictionaryItemIndex;
        }

        public void CreateDeviationMatrix()
        {
            for (var i = 0; i < allItems.Count; i++)
            {
                for (var j = 0; j < allItems.Count; j++)
                {
                    var currentItemX = allItems[i];
                    var currentItemY = allItems[j];
                    var currentDeviation = 0.0;
                    var currentWeight = 0;

                    if (currentItemX != currentItemY)
                    {
                        if (i > j)
                        {
                            var oldDeviationTuple = deviationMatrix[j, i, false];
                            currentDeviation = oldDeviationTuple.Item1 * -1;
                            currentWeight = oldDeviationTuple.Item2;
                        }
                        else
                        {
                            var itemXUsers = itemAndUsers[currentItemX];
                            var itemYUsers = itemAndUsers[currentItemY];
                            foreach (var user in itemXUsers)
                            {
                                if (itemYUsers.Contains(user))
                                {
                                    var userData = ratings[user];
                                    currentDeviation += (userData[currentItemX] - userData[currentItemY]);
                                    currentWeight += 1;
                                }
                            }

                            currentDeviation = (currentWeight != 0) ? currentDeviation / currentWeight : 0;
                        }
                    }

                    var deviationWeight = new Tuple<double, int>(currentDeviation, currentWeight);
                    deviationMatrix[i, j, false] = deviationWeight;
                }
            }
        }

        public void UpdateDeviationMatrix(int userId, int itemId, double newRating)
        {
            var user = ratings[userId];

            foreach (var item in allItems)
            {
                if (item != itemId && user.ContainsKey(item))
                {
                    var oldDeviationTuple = deviationMatrix[itemId, item, true];
                    var currentDeviation = oldDeviationTuple.Item1;
                    var currentWeight = oldDeviationTuple.Item2;

                    currentDeviation = ((currentDeviation * currentWeight) + (newRating - user[item])) / (currentWeight + 1);

                    deviationMatrix[itemId, item, true] = new Tuple<double, int>(currentDeviation, (currentWeight + 1));
                    deviationMatrix[item, itemId, true] = new Tuple<double, int>(currentDeviation * -1, (currentWeight + 1));
                }

            }
        }

        public double GetPrediction(int userId, int itemId)
        {
            var currentRating = 0.0;
            var currentWeight = 0;

            foreach (var rating in ratings[userId])
            {
                if (itemId != rating.Key)
                {
                    var deviationTuple = deviationMatrix[itemId, rating.Key, true];
                    currentRating += ((rating.Value + deviationTuple.Item1) * deviationTuple.Item2);
                    currentWeight += deviationTuple.Item2;
                }
            }

            currentRating = currentWeight != 0 ? (currentRating / currentWeight) : 0;
            return currentRating;
        }

        public Dictionary<int, double> GetPredictionMultiple(int userId, int topAmount)
        {
            Dictionary<int, double> recommendations = new Dictionary<int, double>();

            foreach (var item in allItems)
            {
                if (!ratings[userId].ContainsKey(item))
                {
                    recommendations.Add(item, GetPrediction(userId, item));
                }
            }

            var orderedRecommendations = recommendations.OrderByDescending(x => x.Value).Take(topAmount).ToDictionary(x => x.Key, x => x.Value);

            return orderedRecommendations;
        }
    }
}