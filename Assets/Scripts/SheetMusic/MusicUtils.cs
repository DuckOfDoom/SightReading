namespace DuckOfDoom.SightReading.SheetMusic
{
    public static class MusicUtils
    {
        public const int ZERO_POINT_OCTAVE = 4;
        private const NoteName ZERO_POINT_NOTE = NoteName.B;

        /// <summary>
        ///     Returns vertical note position relative to the B4 (middle line of the stave);
        /// </summary>
        public static int GetNotePosition(NoteName note, int octave)
        {
            return note - ZERO_POINT_NOTE + (octave - ZERO_POINT_OCTAVE) * 7;
        }
    }
}
