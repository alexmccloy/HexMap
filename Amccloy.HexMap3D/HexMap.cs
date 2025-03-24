using System;
using System.Collections.Generic;

namespace Amccloy.HexMap3D {
    public class HexMap {
        public HexMap(Orientation orientation, float width) {
            Orientation = orientation;
            Width = width;
            
            Hexes = new Dictionary<CubicCoordinate, Hex>();
        }

        #region Public Properties
        
        /// <summary>
        /// The orientation of all hexes within the map
        /// </summary>
        public readonly Orientation Orientation;
        
        /// <summary>
        /// The width of all hexes within the map
        /// </summary>
        public readonly float Width;
        
        /// <summary>
        /// The map of all hexes
        /// </summary>
        public Dictionary<CubicCoordinate, Hex> Hexes { get; private set; }
        
        #endregion
        
        public void AddHex(CubicCoordinate coord) {
            Hexes.Add(coord, new Hex(coord, Orientation, Width));
        }

        public void AddHex(Hex hex)
        {
            Hexes.Add(hex.Coordinate, hex);
        }

        /// <summary>
        /// Gets the Hex at the given coordinate. If one does not exist creates it
        /// </summary>
        /// <param name="coord">The coord of the hex to retrieve</param>
        /// <returns>Either the existing hex or a new hex at the given coordinates</returns>
        public Hex GetHexAt(CubicCoordinate coord) {
            if (Hexes.ContainsKey(coord)) {
                return Hexes[coord];
            }
            else {
                AddHex(coord);
                return GetHexAt(coord);
            }
        }
        
    }
}