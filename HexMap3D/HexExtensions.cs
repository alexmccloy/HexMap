using System;
using System.Collections.Generic;

namespace HexMap3D {
    public static class HexExtensions {

        /// <summary>
        /// Calculate the distance from one hex to another. Adjacent hexes have a distance of 1.
        /// All units are in Hexes
        /// </summary>
        /// <returns>
        /// The Distance from one hex to another in 'Hex' units
        /// </returns>
        public static int DistanceTo(this Hex fromHex, Hex toHex) =>
            (Math.Abs(fromHex.Coordinate.Q - toHex.Coordinate.Q)
             + Math.Abs(fromHex.Coordinate.R - toHex.Coordinate.R)
             + Math.Abs(fromHex.Coordinate.S - toHex.Coordinate.S)) / 2;

        /// <summary>
        /// Calculate the distance from one hex to another. Adjacent hexes have a distance of 1.
        /// All units are in Hexes
        /// </summary>
        /// <returns>
        /// The Distance from one hex to another in 'Hex' units
        /// </returns>
        public static int CalculateDistanceTo(Hex from, Hex to) => from.DistanceTo(to);

        /// <summary>
        /// Gets all the hexes in a line from one hex to another. If the <see cref="map"/> parameter is not null, then
        /// the hexes returned will be the real hexes from the map (provided they exist).
        /// </summary>
        /// <param name="fromHex">The hex to start the line from</param>
        /// <param name="toHex">The hex to draw to</param>
        /// <param name="map">The map of hexes to draw the path with. If this is not null then the real hexes from the map will be returned</param>
        /// <param name="stopWhenLineBroken">If this is set to true then any gap in the line (non-contiguous hexes) will cause the list to be cut short</param>
        /// <param name="blockingValue">Additional value to determine when to not include the hex. Only applies when using the map, defaults to the IsLineOfSight value for the hex</param>
        /// <returns></returns>
        public static List<Hex> LineTo(this Hex fromHex, Hex toHex, HexMap map = null, bool stopWhenLineBroken = false, Predicate<Hex> blockingValue = null) {
            blockingValue = blockingValue ?? new Predicate<Hex>(hex => hex.IsLineOfSight);
            
            //Calculate list of coordinates in the line
            var line = new List<Hex>();

            //If using a real map then pull hexes out of the map

            //Else create the hexes and return them
            
            throw new NotImplementedException();
        }


        private static float Lerp() {
            throw new NotImplementedException();
        }
    }
}