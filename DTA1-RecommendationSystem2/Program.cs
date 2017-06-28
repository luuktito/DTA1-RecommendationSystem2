using System;
using DTA1_RecommendationSystem2.Algorithms;

namespace DTA1_RecommendationSystem2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("#Assignment 2: Item-Item ");
            Console.WriteLine();

            var ratings = Parser.Parser.Parse(',', "userItem.data");

            var itemItem = new ItemItem(ratings);
            itemItem.CreateDeviationMatrix();


            Console.WriteLine("#Predicted ratings for user 7 for the following items:");
            var predictedRatingUser7Item101 = itemItem.GetPrediction(7, 101);
            Console.WriteLine("Item 101 has a predicted rating of: " + predictedRatingUser7Item101);

            var predictedRatingUser7Item103 = itemItem.GetPrediction(7, 103);
            Console.WriteLine("Item 103 has a predicted rating of: " + predictedRatingUser7Item103);

            var predictedRatingUser7Item106 = itemItem.GetPrediction(7, 106);
            Console.WriteLine("Item 106 has a predicted rating of: " + predictedRatingUser7Item106);
            Console.WriteLine();


            Console.WriteLine("#Predicted ratings for user 3 for the following items:");
            var predictedRatingUser3Item103 = itemItem.GetPrediction(7, 103);
            Console.WriteLine("Item 103 has a predicted rating of: " + predictedRatingUser7Item103);

            var predictedRatingUser7Item105 = itemItem.GetPrediction(7, 105);
            Console.WriteLine("Item 105 has a predicted rating of: " + predictedRatingUser7Item105);
            Console.WriteLine();


            itemItem.UpdateDeviationMatrix(3, 105, 4);

            Console.WriteLine("#Predicted ratings for user 7 for the following items (after updating the rating):");
            predictedRatingUser7Item101 = itemItem.GetPrediction(7, 101);
            Console.WriteLine("Item 101 has a predicted rating of: " + predictedRatingUser7Item101);

            predictedRatingUser7Item103 = itemItem.GetPrediction(7, 103);
            Console.WriteLine("Item 103 has a predicted rating of: " + predictedRatingUser7Item103);

            predictedRatingUser7Item106 = itemItem.GetPrediction(7, 106);
            Console.WriteLine("Item 106 has a predicted rating of: " + predictedRatingUser7Item106);
            Console.WriteLine();


            Console.WriteLine("#Calculation the deviation matrix for the movie lens dataset, this might take a minute :(");
            Console.WriteLine();

            var ratingsMovie = Parser.Parser.Parse('\t', "u.data");
           
            var itemItemMovies = new ItemItem(ratingsMovie);
            itemItemMovies.CreateDeviationMatrix();

            var top8RecommendationsUser186 = itemItemMovies.GetPredictionMultiple(186, 5);
            Console.WriteLine("#The top 5 predicted ratings for user 186 are as follows:");
            foreach (var movie in top8RecommendationsUser186)
            {
                Console.WriteLine("Movie ID " + movie.Key + " has a predicted rating of: " + movie.Value);
            }

            Console.ReadLine();
        }
    }
}
