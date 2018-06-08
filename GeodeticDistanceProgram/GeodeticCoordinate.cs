using System;

namespace GeodeticDistanceProgram
{
    public class GeodeticCoordinate
    {
        public Double Latitude { get; set; }
        public Double Longitude { get; set; }

        public override String ToString()
        {
            return $"({Latitude}, {Longitude})";
        }
    }
}
