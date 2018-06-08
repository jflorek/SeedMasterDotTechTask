using System;
using System.Collections.Generic;

namespace GeodeticDistanceProgram
{
    public static class GeodeticUtils
    {
        private const Double EarthRadiusKilometers = 6371;

        public static GeodeticCoordinate NextGeodeticCoordinate(this Random random)
        {
            // generate random coordinates on a sphere using Alternative Method 2 in
            // http://corysimon.github.io/articles/uniformdistn-on-sphere/

            while (true)
            {
                var uniformCartesianX = random.NextDoubleFromNegativeOneToOne();
                var uniformCartesianY = random.NextDoubleFromNegativeOneToOne();
                var uniformCartesianZ = random.NextDoubleFromNegativeOneToOne();

                var distanceFromOrigin = DistanceFromOrigin(uniformCartesianX, uniformCartesianY, uniformCartesianZ);
                var isCurrentPointOutsideUnitSphere = distanceFromOrigin > 1;

                if (isCurrentPointOutsideUnitSphere)
                {
                    continue;
                }

                // convert to spherical using conversions on
                // https://en.wikipedia.org/wiki/Spherical_coordinate_system

                var sphericalTheta = Math.Acos(uniformCartesianZ / distanceFromOrigin);
                var sphericalPhi = Math.Atan2(uniformCartesianY, uniformCartesianX);

                var latitude = RadiansToDegrees(sphericalTheta) - 90.0;
                var longitude = RadiansToDegrees(sphericalPhi);

                var result = new GeodeticCoordinate
                {
                    Latitude = latitude,
                    Longitude = longitude
                };
                return result;
            }
        }

        private static Double RadiansToDegrees(Double radians) => radians * 180.0 / Math.PI;

        private static Double DegreesToRadians(Double degrees) => degrees * Math.PI / 180.0;

        private static Double DistanceFromOrigin(Double x, Double y, Double z) => Math.Sqrt(x * x + y * y + z * z);

        private static Double NextDoubleFromNegativeOneToOne(this Random random) => random.NextDouble() * 2 - 1;

        public static Double DistanceBetweenKilometers(GeodeticCoordinate from, GeodeticCoordinate to)
        {
            var fromLatitudeRadians = DegreesToRadians(from.Latitude);
            var toLatitudeRadians = DegreesToRadians(to.Latitude);

            var deltaLongitudeRadians = DegreesToRadians(to.Longitude - from.Longitude);
            var deltaLatidudeRadians = DegreesToRadians(to.Latitude - from.Latitude);

            // haversine formula https://en.wikipedia.org/wiki/Haversine_formula

            var distance = EarthRadiusKilometers * 2 *
                           Math.Asin(Math.Sqrt(
                               Math.Pow(Math.Sin(deltaLatidudeRadians / 2), 2) +
                               Math.Cos(fromLatitudeRadians) *
                               Math.Cos(toLatitudeRadians) *
                               Math.Pow(Math.Sin(deltaLongitudeRadians / 2), 2))
                           );
            return distance;
        }

        public static Double FindClosestCoordinates(
            IList<GeodeticCoordinate> allCoordinates,
            out GeodeticCoordinate firstCoordinateResult,
            out GeodeticCoordinate secondCoordinateResult)
        {
            firstCoordinateResult = null;
            secondCoordinateResult = null;
            var minDistance = Double.MaxValue;
            var count = allCoordinates.Count;
            for (var i = 0; i < count; ++i)
            {
                var firstCoordinate = allCoordinates[i];
                for (var j = i + 1; j < count; ++j)
                {
                    var secondCoordinate = allCoordinates[j];
                    var distance = DistanceBetweenKilometers(firstCoordinate, secondCoordinate);
                    if (minDistance > distance)
                    {
                        minDistance = distance;
                        firstCoordinateResult = firstCoordinate;
                        secondCoordinateResult = secondCoordinate;
                    }
                }
            }

            return minDistance;
        }

        public static Double FindFirthestCoordinates(
            IList<GeodeticCoordinate> allCoordinates,
            out GeodeticCoordinate firstCoordinateResult,
            out GeodeticCoordinate secondCoordinateResult)
        {
            firstCoordinateResult = null;
            secondCoordinateResult = null;
            var maxDistance = 0.0;
            var count = allCoordinates.Count;
            for (var i = 0; i < count; ++i)
            {
                var firstCoordinate = allCoordinates[i];
                for (var j = i + 1; j < count; ++j)
                {
                    var secondCoordinate = allCoordinates[j];
                    var distance = DistanceBetweenKilometers(firstCoordinate, secondCoordinate);
                    if (maxDistance < distance)
                    {
                        maxDistance = distance;
                        firstCoordinateResult = firstCoordinate;
                        secondCoordinateResult = secondCoordinate;
                    }
                }
            }

            return maxDistance;
        }
    }
}
