using System.Collections.Generic;
using DuckOfDoom.SightReading.SheetMusic;
using NUnit.Framework;

namespace DuckOfDoom.SightReading.Tests
{
    [TestFixture]
    public class MusicUtilsTests
    {
        [Test][TestCaseSource(nameof(Notes))]
        public void TestNotePlacement(int octave, int note)
        {
            var position = MusicUtils.GetNotePosition((NoteName)note, octave);
            var expectedPosition = (note - 1) + (octave - MusicUtils.ZERO_POINT_OCTAVE) * 7;
            
            Assert.That(position, Is.EqualTo(expectedPosition));
        }

        private static IEnumerable<object[]> Notes()
        {
            for (var o = 0; o < 8; o++)
            {
                for (var n = 1; n < 8; n++)
                {
                    yield return new object[] {o, n};
                }
            }
            
        }
    }
}
