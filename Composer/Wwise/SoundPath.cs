using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Composer.IO;

namespace Composer.Wwise
{
    public class SoundPathPoint
    {
        public SoundPathPoint(IReader reader)
        {
            X = reader.ReadFloat();
            reader.Skip(4); // Unknown
            Y = reader.ReadFloat();
            Duration = reader.ReadInt32();
        }

        public float X { get; private set; }
        public float Y { get; private set; }
        public int Duration { get; private set; }
    }

    public class SoundPathRandomRange
    {
        public SoundPathRandomRange(IReader reader)
        {
            HorizontalRange = reader.ReadFloat();
            VerticalRange = reader.ReadFloat();
        }

        public float HorizontalRange { get; private set; }
        public float VerticalRange { get; private set; }
    }

    public class SoundPath
    {
        public SoundPath(IReader reader)
        {
            FirstPointIndex = reader.ReadInt32();
            PointCount = reader.ReadInt32();
        }

        public int FirstPointIndex { get; private set; }
        public int PointCount { get; private set; }
    }
}
