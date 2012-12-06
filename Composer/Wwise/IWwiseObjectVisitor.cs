using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Composer.Wwise
{
    /// <summary>
    /// Interface for a class which can visit various types of Wwise objects.
    /// </summary>
    public interface IWwiseObjectVisitor
    {
        void Visit(SoundBankFile file);
        void Visit(SoundPackFile file);
        void Visit(SoundBankVoice voice);
        void Visit(SoundBankAction action);
        void Visit(SoundBankEvent ev);
    }
}
