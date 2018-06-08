using System;
using System.Collections.Generic;
using System.Linq;

namespace GeodeticDistanceProgram
{
    public class Program
    {
        public static void Main(String[] args)
        {
            var quantityArg = args.FirstOrDefault();
            if (Int32.TryParse(quantityArg, out var quantity) && quantity >= 2)
            {
                var allCoordinates = GenerateCoordinates(quantity);
                var minDistance =
                    GeodeticUtils.FindClosestCoordinates(allCoordinates, out var closest1, out var closest2);
                var maxDistance =
                    GeodeticUtils.FindFirthestCoordinates(allCoordinates, out var furthest1, out var furthest2);

                Console.WriteLine("Random location distance calculator");
                Console.WriteLine($"\nGenerated {quantity} locations:");

                foreach (var coordinate in allCoordinates)
                {
                    Console.WriteLine($"Location: {coordinate}");
                }

                Console.WriteLine($"\nShortest: {minDistance}km");
                Console.WriteLine($"{closest1} ~ {closest2}");
                Console.WriteLine($"\nLongest: {maxDistance}km");
                Console.WriteLine($"{furthest1} ~ {furthest2}");
            }
            else
            {
                Console.WriteLine("Error: first argument must be an integer no less than 2.");
            }
        }

        private static IList<GeodeticCoordinate> GenerateCoordinates(Int32 quantity)
        {
            var random = new Random();
            var coordinates = new List<GeodeticCoordinate>();
            for (var i = 0; i < quantity; ++i)
            {
                coordinates.Add(random.NextGeodeticCoordinate());
            }

            return coordinates;
        }
    }
}
