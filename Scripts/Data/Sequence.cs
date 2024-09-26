using System.Collections.Generic;

namespace GTE;

public sealed class Sequence
{
    public string Name { get; init; }
    public Dictionary<string, Variant> Variants { get; init; }
}
