using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgLabs.Labs.Lab05;

public sealed record City(string Name, int Year, int Population) : IComparable<City>
{
    public int CompareTo(City? other)
    {
        if (other is null)
            return 1;

        return Population.CompareTo(other.Population);
    }
}