using System;
using System.Net.Http.Headers;
using Point = System.Drawing.Point;

namespace HexMap3D {
    /// <summary>
    /// Collection of utilities to help with common hex based operations
    /// </summary>
    public static class HexUtils {

        /// <summary>
        /// Converts Cartesian coordinates to Cubic coordinates - for example clicking on a 2D plane and calculating
        /// the coordinates of the hex that was clicked
        /// </summary>
        /// 
        /// <param name="point">The Cartesian coordinates that were clicked</param>
        /// <param name="orientation">The orientation of the Hexes</param>
        /// <param name="width">The width of the hexes in the same units as <see cref="point"/></param>
        /// 
        /// <returns>The Cubic coordinates of the hex that would be under the point</returns>
        /// <see href="https://www.redblobgames.com/grids/hexagons/#pixel-to-hex"/>
        public static CubicCoordinate CartesianToCubic(Point point, Orientation orientation, float width) {
            if (orientation == Orientation.FlatTop) {
                var q = (2f / 3f * point.X) / width;
                var r = (-1f / 3f * point.X + Math.Sqrt(3f) / 3f * point.Y) / width;
                return RoundHex(q, r);
            }
            else {
                var q = (Math.Sqrt(3f) / 3f * point.X - 1f / 3f * point.Y) / width;
                var r = (2f / 3f * point.Y) / width;
                return RoundHex(q, r);
            }
        }

        public static CubicCoordinate ToCubic(this Point point, Orientation orientation, float width) {
            return CartesianToCubic(point, orientation, width);
        }

        /// <summary>
        /// Converts Cubic coordinates to Cartesian coordinates
        /// </summary>
        /// 
        /// <param name="coord">The coordinate to convert</param>
        /// <param name="orientation">The orientation of the Hexes</param>
        /// <param name="width">The width of the hexes in the same units as the return value</param>
        /// 
        /// <returns>The point at the center of the cubic coordinates given</returns>
        /// <see href="https://www.redblobgames.com/grids/hexagons/#hex-to-pixel"/>
        public static Point CubicToCartesian(CubicCoordinate coord, Orientation orientation, float width) {
            if (orientation == Orientation.FlatTop) {
                var x = width * (1.5f * coord.Q);
                var y = width * (Math.Sqrt(3) / 2f * coord.Q + Math.Sqrt(3) * coord.R);
                return new Point((int)x, (int)y);
            }
            else { // Pointy Top
                var x = width * (Math.Sqrt(3) * coord.Q + Math.Sqrt(3) / 2f * coord.R);
                var y = width * (1.5 * coord.R);
                return new Point((int) x, (int) y);
            }
        }

        public static Point ToCartesian(this CubicCoordinate coord, Orientation orientation, float width) {
            return CubicToCartesian(coord, orientation, width);
        }

        /// <summary>
        /// Rounds a hex with Cubic / Axial coordinates to the nearest hex with natual indices
        /// </summary>
        /// 
        /// <param name="q">The Q or X value of the hex</param>
        /// <param name="r">The R or Y value of the hex</param>
        /// 
        /// <returns>A hex that is the closest match for the given coordinates</returns>
        /// <see href="https://www.redblobgames.com/grids/hexagons/#rounding"/>
        private static CubicCoordinate RoundHex(double q, double r) {
            double s = -q - r;
            var rq = Convert.ToInt32(q);
            var rr = Convert.ToInt32(r);
            var rs = Convert.ToInt32(s);

            var qDiff = Math.Abs(rq - q);
            var rDiff = Math.Abs(rr - r);
            var sDiff = Math.Abs(rs - s);

            if (qDiff > rDiff && qDiff > sDiff) {
                rq = -rr - rs;
            }
            else if (rDiff > sDiff) {
                rr = -rq - rs;
            }
            else {
                rs = -rq - rr;
            }

            return new CubicCoordinate(rq, rr);
        }
    }
}