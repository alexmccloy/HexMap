using System;

namespace Amccloy.HexMap3D
{
    public class HexMapFactory<T> where T : Hex
    {
        private readonly Func<CubicCoordinate, T> _hexGeneratorFunc;
        private readonly Orientation _orientation;
        private readonly float _width;

        public HexMapFactory(Orientation orientation, float width, Func<CubicCoordinate, T> hexGeneratorFunc)
        {
            _hexGeneratorFunc = hexGeneratorFunc;
            _orientation = orientation;
            _width = width;
        }

        /// <summary>
        /// Creates a map with hexes in circles around a center hex. A radius of 0 with have 1 hex, radius 2 will have
        /// 7 hexes etc
        /// https://www.redblobgames.com/grids/hexagons/#range
        /// </summary>
        public HexMap<T> GenerateCircularMap(int radius)
        {
            Console.WriteLine("Generating circular map");
            var map = new HexMap<T>(_orientation, _width, _hexGeneratorFunc);

            for (int q = -radius; q <= radius; q++)
            {
                for (int r = Math.Max(-radius, -q - radius); r <= Math.Min(radius, -q + radius); r++)
                {
                    map.AddHex(new CubicCoordinate(q, r));
                }
            }
            
            return map;
        }
        
    }
}