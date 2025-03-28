using System;
using System.Collections.Generic;

namespace Amccloy.HexMap3D {
    public class HexMap<T> where T : Hex
    {
        // public HexMap(Func<CubicCoordinate, T> hexGeneratorFunction) {
        public HexMap(Orientation orientation, float width, Func<CubicCoordinate, T> hexGeneratorFunction) {
            Orientation = orientation;
            Width = width;
            
            Hexes = new Dictionary<CubicCoordinate, T>();
            HexGeneratorFunction = hexGeneratorFunction;
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
        public Dictionary<CubicCoordinate, T> Hexes { get; private set; }
        
        /// <summary>
        /// The function called whenever this class needs to create a new hex
        /// </summary>
        public Func<CubicCoordinate, T> HexGeneratorFunction { get; private set; }
        
        #endregion
        
        public void AddHex(CubicCoordinate coord) {
            var hex = HexGeneratorFunction(coord);
            Hexes.Add(coord, hex);
        }

        public void AddHex(T hex)
        {
            Hexes.Add(hex.Coordinate, hex);
        }

        /// <summary>
        /// Gets the Hex at the given coordinate. If one does not exist creates it
        /// </summary>
        /// <param name="coord">The coord of the hex to retrieve</param>
        /// <returns>Either the existing hex or a new hex at the given coordinates</returns>
        public Hex GetHexAt(CubicCoordinate coord) {
            //TODO this seems like a kind of shit function why did i do this
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