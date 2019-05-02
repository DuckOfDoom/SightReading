using Optional;

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
        public Option<Note> Note;

        public override string ToString() { return $"Symbol: {Type} - {Duration}"; }
    }

    public struct Note
    {
        public NoteName Name;
        public Accidental Accidental;
        public int Octave;
    }
}
