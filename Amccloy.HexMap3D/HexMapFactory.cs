using System;

namespace Amccloy.HexMap3D
{
    public class HexMapFactory
    {
        public Orientation Orientation { get; private set; }
        public float Width { get; private set; }
        public float Height { get; private set; }
        public float Depth { get; private set; }

        public HexMapFactory(Orientation orientation, float width, float height = 0f, float depth = 0f)
        {
            Orientation = orientation;
            Width = width;
            Height = height;
            Depth = depth;
        }

        /// <summary>
        /// Creates a map with hexes in circles around a center hex. A radius of 0 with have 1 hex, radius 2 will have
        /// 7 hexes etc
        /// https://www.redblobgames.com/grids/hexagons/#range
        /// </summary>
        public HexMap GenerateCircularMap(int radius)
        {
            var map = new HexMap(Orientation, Width);

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