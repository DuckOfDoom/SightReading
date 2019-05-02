namespace DuckOfDoom.SightReading.SheetMusic
{
    public enum SymbolType
    {
        Undefined = 0,
        Note = 1,
        Rest = 2
    }

    public enum Duration
    {
        Undefined = 0,
        
        Whole = 1,
        Half = 2, 
        Quarter = 3,
        Eighth = 4,
        Sixteenth = 5
    }

    public enum NoteName
    {
        Undefined = 0,
        
        C = 0,
        D = 1,
        E = 2,
        F = 3,
        G = 4,
        A = 5,
        B = 6
    }

    public enum Accidental
    {
        Undefined = 0,
        
        Natural = 1,
        Sharp = 2,
        Flat = 3,
        
        DoubleSharp = 4,
        DoubleFlat = 5
    }
}