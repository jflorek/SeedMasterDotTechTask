using System;
using System.Collections.Generic;

namespace GeodeticDistanceProgram
{
    internal static class Tests
    {
        private static IList<GeodeticCoordinate> TestPoints => new[]
        {
            new GeodeticCoordinate
            {
                Latitude = 43.4933429507042,
                Longitude = 1.50271833012938
            },
            new GeodeticCoordinate
            {
                Latitude = 54.2388742157439,
                Longitude = -55.3079767875876
            },
            new GeodeticCoordinate
            {
                Latitude = 36.4924011130316,
                Longitude = 64.7513347606879
            },
            new GeodeticCoordinate
            {
                Latitude = -44.7831188630234,
                Longitude = -138.05504433248
            }
        };

        public static void AllTests()
        {
            DistanceCalculationTest();
            GeneratorTest();
            ClosestFurthestTest();
        }

        public static void DistanceCalculationTest()
        {
            var testPoints = TestPoints;
            var shortDistance = GeodeticUtils.DistanceBetweenKilometers(testPoints[0], testPoints[1]);
            var expectedShortDistance = 4198.71459;
            AssertDifferenceMinimal(shortDistance, expectedShortDistance);
            var longDistance = GeodeticUtils.DistanceBetweenKilometers(testPoints[2], testPoints[3]);
            var expectedLongDistance = 17891.05884;
            AssertDifferenceMinimal(longDistance, expectedLongDistance);
        }

        private static void AssertDifferenceMinimal(Double val1, Double val2)
        {
            var isDifferenceMinimal = Math.Abs(val1 - val2) < 0.0001;
            if (!isDifferenceMinimal)
            {
                throw new Exception();
            }
        }

        public static void GeneratorTest()
        {
            var random = new Random();
            for (var i = 0; i < 100000; ++i)
            {
                var coordinate = random.NextGeodeticCoordinate();
                var coordinateValuesInRange = coordinate.Latitude >= -90 &&
                                              coordinate.Latitude <= 90 &&
                                              coordinate.Longitude >= -180 &&
                                              coordinate.Longitude <= 180;
                if (!coordinateValuesInRange)
                {
                    throw new Exception();
                }
            }
        }

        public static void ClosestFurthestTest()
        {
            var points = TestPoints;
            GeodeticUtils.FindClosestCoordinates(points, out var first, out var second);
            if (first != points[0] || second != points[1])
            {
                throw new Exception();
            }
            GeodeticUtils.FindFirthestCoordinates(points, out var one, out var two);
            if (one != points[2] || two != points[3])
            {
                throw new Exception();
            }
        }
    }
}
