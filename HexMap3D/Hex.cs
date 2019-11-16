namespace HexMap3D {
    
    /// <summary>
    /// Represents a single hexagonal prism
    /// </summary>
    public class Hex {
        
        /// <summary>
        /// The coordinate the Hex is at in cubic coordinates
        /// </summary>
        public CubicCoordinate Coordinate { get; set; }
        
        /// <summary>
        /// The height (z axis) that the top of the hexagonal prism is at.
        /// </summary>
        public float Height { get; set; }
        
        /// <summary>
        /// How deep the hexagonal prism is, measured from the top
        /// </summary>
        public float Depth { get; set; }
        
        public Hex(CubicCoordinate coordinate) {
            Coordinate = coordinate;
            Height = 0;
            Depth = 0;
        }

        public Hex(int q, int r) 
            : this(new CubicCoordinate(q, r)) {
        }

    }
}