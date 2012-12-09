using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Composer.Wwise;

namespace Composer
{
    /// <summary>
    /// Sound formats which can be returned from FormatIdentification.IdentifyFormat().
    /// </summary>
    public enum SoundFormat
    {
        Unknown,
        XMA,
        XWMA,
        WwiseOGG
    }

    /// <summary>
    /// RIFX codec values used by Wwise.
    /// </summary>
    public static class RIFXCodec
    {
        public const short WMA = 0x161;
        public const short WMAPro = 0x162;
        public const short XMA = 0x166;
        public const short WwiseOGG = -1;
    }

    /// <summary>
    /// Provides methods for identifying Wwise sound formats.
    /// </summary>
    public static class FormatIdentification
    {
        /// <summary>
        /// Identifies the format of a Wwise sound based upon RIFX header information.
        /// </summary>
        /// <param name="rifx">The RIFX header information for the sound.</param>
        /// <returns>A SoundFormat value indicating the type of the sound, or SoundFormat.Unknown if identification failed.</returns>
        public static SoundFormat IdentifyFormat(RIFX rifx)
        {
            if (rifx.FormatMagic == RIFFFormat.XWMA)
            {
                if (rifx.Codec == RIFXCodec.WMA || rifx.Codec == RIFXCodec.WMAPro)
                    return SoundFormat.XWMA;
            }
            else if (rifx.FormatMagic == RIFFFormat.WAVE)
            {
                switch (rifx.Codec)
                {
                    case RIFXCodec.WwiseOGG:
                        return SoundFormat.WwiseOGG;
                    case RIFXCodec.XMA:
                        return SoundFormat.XMA;
                }
            }
            return SoundFormat.Unknown;
        }
    }
}
