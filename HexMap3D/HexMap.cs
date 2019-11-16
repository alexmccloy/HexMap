using System;
using System.Collections.Generic;

namespace HexMap3D {
    public class HexMap {
        
        public Orientation Orientation { get; set; }
        
        public Dictionary<CubicCoordinate, Hex> Hexes { get; private set; }
        
        
    }
}