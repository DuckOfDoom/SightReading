namespace DuckOfDoom.SightReading.SheetMusic
{
    /// <summary>
    ///     A struct representing a symbol.
    ///     Note, rest, e.t.c. 
    /// </summary>
    public struct Symbol
    {
        public SymbolType Type;
        public Duration Duration;

        public override string ToString() { return $"Symbol: {Type} - {Duration}"; }
    }
}
