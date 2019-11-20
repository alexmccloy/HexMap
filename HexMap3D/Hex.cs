using System;
using System.Drawing;

namespace HexMap3D {
    
    /// <summary>
    /// Represents a single hexagonal prism
    /// </summary>
    public class Hex {
        
        public Orientation Orientation { get; set; }
        
        /// <summary>
        /// The coordinate the Hex is at in cubic coordinates
        /// </summary>
        public CubicCoordinate Coordinate { get; set; }

        /// <summary>
        /// THe centre point of the hex in cartesian coordinates
        /// </summary>
        public Point CariteseanCoordinate => HexUtils.CubicToCartesian(Coordinate, Orientation, Width);

        /// <summary>
        /// The height (z axis) that the top of the hexagonal prism is at.
        /// </summary>
        public float Height { get; set; }
        
        /// <summary>
        /// How deep the hexagonal prism is, measured from the top
        /// </summary>
        public float Depth { get; set; }
        
        /// <summary>
        /// The radius of the hexagon
        /// </summary>
        public float Width { get; set; }
        
        public Hex(CubicCoordinate coordinate, Orientation orientation, float width = 1f) {
            Orientation = orientation;
            Coordinate = coordinate;
            Height = 0;
            Depth = 0;
            Width = width;
        }

        public Hex(int q, int r, Orientation orientation, float width = 1f) 
            : this(new CubicCoordinate(q, r), orientation, width) {
        }
        
        

    }
}