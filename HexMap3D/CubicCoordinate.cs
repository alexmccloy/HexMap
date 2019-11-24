using System;

namespace HexMap3D {
    
    /// <summary>
    /// Position of a hexagon in cubic coordinates
    /// https://www.redblobgames.com/grids/hexagons/#coordinates
    /// </summary>
    public class CubicCoordinate : IEquatable<CubicCoordinate> {
        
        public readonly int Q;

        public readonly int R;

        public readonly int S;
        
        
        public CubicCoordinate(int q, int r) {
            Q = q;
            R = r;
            S = (q + r) * -1;
        }


        public bool Equals(CubicCoordinate other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Q == other.Q && 
                   R == other.R && 
                   S == other.S;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CubicCoordinate) obj);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = Q;
                hashCode = (hashCode * 397) ^ R;
                hashCode = (hashCode * 397) ^ S;
                return hashCode;
            }
        }

        public override string ToString() {
            return $"({Q}, {R}, {S})";
        }
    }
}