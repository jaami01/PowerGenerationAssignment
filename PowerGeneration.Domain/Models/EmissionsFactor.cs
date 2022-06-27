using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerGeneration.Domain.Models
{
    public class EmissionsFactor : IEquatable<EmissionsFactor>
    {
        public decimal High { get; set; }
        public decimal Medium { get; set; }
        public decimal Low { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as EmissionsFactor);
        }

        public bool Equals(EmissionsFactor other)
        {
            return other != null &&
                   High == other.High &&
                   Medium == other.Medium &&
                   Low == other.Low;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(High, Medium, Low);
        }

        public static bool operator ==(EmissionsFactor left, EmissionsFactor right)
        {
            return EqualityComparer<EmissionsFactor>.Default.Equals(left, right);
        }

        public static bool operator !=(EmissionsFactor left, EmissionsFactor right)
        {
            return !(left == right);
        }
    }
}
