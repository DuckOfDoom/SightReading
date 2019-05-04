using Optional;

namespace DuckOfDoom.SightReading.SheetMusic
{
    /// <summary>
    ///     A struct representing a symbol.
    ///     Note, rest, e.t.c. 
    /// </summary>
    public struct Symbol
    {
        public SymbolType Type => Note.HasValue ? SymbolType.Note : SymbolType.Undefined;
        
        public Duration Duration;
        public Option<Note> Note;

        public Symbol(Duration duration, Option<Note> note)
        {
            Duration = duration;
            Note = note;
        }
        
        public override string ToString() { return $"Symbol: {Type} - {Duration}"; }
    }

    public struct Note
    {
        public NoteName Name;
        public Accidental Accidental;
        public int Octave;
        
        public Note(NoteName name, int octave) : this(name, Accidental.Natural, octave) { }

        public Note(NoteName name, Accidental accidental, int octave)
        {
            Name = name;
            Accidental = accidental;
            Octave = octave;
        }
        
    }
}
