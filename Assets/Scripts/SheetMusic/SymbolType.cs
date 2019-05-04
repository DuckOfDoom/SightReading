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
        
        B = 1,
        C = 2,
        D = 3,
        E = 4,
        F = 5,
        G = 6,
        A = 7,
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