using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerGeneration.Domain
{
    public partial class EnergyGeneration : IEquatable<EnergyGeneration>
    {
        public DateTime Day { get; set; }
        public decimal Energy { get; set; }
        public decimal Price { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as EnergyGeneration);
        }

        public bool Equals(EnergyGeneration other)
        {
            return other != null &&
                   Day == other.Day &&
                   Energy == other.Energy &&
                   Price == other.Price;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Day, Energy, Price);
        }

        public static bool operator ==(EnergyGeneration left, EnergyGeneration right)
        {
            return EqualityComparer<EnergyGeneration>.Default.Equals(left, right);
        }

        public static bool operator !=(EnergyGeneration left, EnergyGeneration right)
        {
            return !(left == right);
        }
    }
}
