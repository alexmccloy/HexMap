using System;

namespace Amccloy.HexMap3D.Designer {
    public static class Utils {

        /// <summary>
        /// Converts a System.Windows.Point to a System.Drawing.Point by rounding.
        /// This must be done as the .Net standard library does not have System.Windows.Point that is used in WPF
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static System.Drawing.Point PointToPoint(System.Windows.Point point) {
            return new System.Drawing.Point(Convert.ToInt32(point.X), Convert.ToInt32(point.Y));
        }
    }
}