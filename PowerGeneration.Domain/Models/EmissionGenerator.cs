using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerGeneration.Domain.Models
{
    public class EmissionGenerator : IEquatable<EmissionGenerator>
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public decimal Emission { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as EmissionGenerator);
        }

        public bool Equals(EmissionGenerator other)
        {
            return other != null &&
                   Name == other.Name &&
                   Date == other.Date &&
                   Emission == other.Emission;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Date, Emission);
        }

        public static bool operator ==(EmissionGenerator left, EmissionGenerator right)
        {
            return EqualityComparer<EmissionGenerator>.Default.Equals(left, right);
        }

        public static bool operator !=(EmissionGenerator left, EmissionGenerator right)
        {
            return !(left == right);
        }
    }
}
