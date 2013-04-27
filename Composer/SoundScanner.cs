using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Composer.Wwise;

namespace Composer
{
    /// <summary>
    /// Provides data for the FoundSoundBankFile and FoundSoundPackFile events.
    /// </summary>
    /// <typeparam name="T">The type of the Wwise object representing the file that was found.</typeparam>
    public class SoundFileEventArgs<T> : EventArgs
        where T : IWwiseObject
    {
        public SoundFileEventArgs(T file, SoundBankEvent sourceEvent)
        {
            File = file;
            SourceEvent = sourceEvent;
        }

        /// <summary>
        /// The file that was found.
        /// </summary>
        public T File { get; private set; }

        /// <summary>
        /// The event that pointed to the file.
        /// </summary>
        public SoundBankEvent SourceEvent { get; private set; }
    }

    /// <summary>
    /// Provides methods of scanning sound banks and Wwise objects for sound files.
    /// </summary>
    public class SoundScanner : IWwiseObjectVisitor
    {
        private WwiseObjectCollection _globalObjects = new WwiseObjectCollection();
        private Dictionary<uint, SoundBank> _soundBanks = new Dictionary<uint, SoundBank>();
        private SoundBankEvent _currentEvent = null;
        private SoundBank _currentBank = null;

        /// <summary>
        /// Occurs when a SoundBankFile is found.
        /// </summary>
        public EventHandler<SoundFileEventArgs<SoundBankFile>> FoundSoundBankFile;

        /// <summary>
        /// Occurs when a SoundPackFile is found.
        /// </summary>
        public EventHandler<SoundFileEventArgs<SoundPackFile>> FoundSoundPackFile;

        /// <summary>
        /// Registers a sound bank with the scanner.
        /// </summary>
        /// <param name="bank">A sound bank which sounds can be loaded from.</param>
        public void RegisterSoundBank(SoundBank bank)
        {
            _soundBanks[bank.ID] = bank;
        }

        /// <summary>
        /// Registers a bank-less object (e.g. one from soundstream.pck) with the scanner.
        /// </summary>
        /// <param name="obj">The object to register.</param>
        public void RegisterGlobalObject(IWwiseObject obj)
        {
            _globalObjects.Add(obj);
        }

        /// <summary>
        /// Scans a SoundBankEvent for sound files.
        /// </summary>
        /// <param name="bank">The SoundBank that the event belongs to.</param>
        /// <param name="ev">The SoundBankEvent to scan.</param>
        public void ScanEvent(SoundBank bank, SoundBankEvent ev)
        {
            _currentBank = bank;
            Visit(ev);
        }

        public void Visit(SoundBankFile file)
        {
            // Raise the FoundSoundBankFile event
            SoundFileEventArgs<SoundBankFile> args = new SoundFileEventArgs<SoundBankFile>(file, _currentEvent);
            OnFoundSoundBankFile(args);
        }

        public void Visit(SoundPackFile file)
        {
            // Raise the FoundSoundPackFile event
            SoundFileEventArgs<SoundPackFile> args = new SoundFileEventArgs<SoundPackFile>(file, _currentEvent);
            OnFoundSoundPackFile(args);
        }

        public void Visit(SoundBankVoice voice)
        {
            // Jump to the audio file it references
            Dispatch(voice.AudioID, voice.SourceID);
        }

        public void Visit(SoundBankAction action)
        {
            // Only handle actions which involve playing sounds
            if (action.Type == SoundBankActionType.Play
                || action.Type == SoundBankActionType.Resume
                || action.Type == SoundBankActionType.Trigger
                || action.Type == SoundBankActionType.UnMute)
            {
                // Move to the object referenced by the action
                Dispatch(action.ObjectID);
            }
        }

        public void Visit(SoundBankEvent ev)
        {
            // Scan each action in the event
            _currentEvent = ev;
            DispatchAll(ev.ActionIDs);
        }

        public void Visit(SoundBankSequenceContainer container)
        {
            DispatchAll(container.ChildIDs);
        }

        public void Visit(SoundBankSwitchContainer container)
        {
            DispatchAll(container.ChildIDs);
        }

        public void Visit(SoundBankActorMixer mixer)
        {
            DispatchAll(mixer.ChildIDs);
        }

        public void Visit(SoundBankMusicPlaylist playlist)
        {
            DispatchAll(playlist.SegmentIDs);
        }

        public void Visit(SoundBankMusicSegment segment)
        {
            DispatchAll(segment.ChildIDs);
        }

        public void Visit(SoundBankMusicTrack track)
        {
            // Jump to the audio file it references
            Dispatch(track.AudioID, track.SourceID);
        }

        public void Visit(SoundBankMusicSwitchContainer container)
        {
            DispatchAll(container.SegmentIDs);
        }

        /// <summary>
        /// Raises the FoundSoundBankFile event.
        /// </summary>
        /// <param name="args">The SoundFileEventArgs to pass to handlers.</param>
        protected void OnFoundSoundBankFile(SoundFileEventArgs<SoundBankFile> args)
        {
            if (FoundSoundBankFile != null)
                FoundSoundBankFile(this, args);
        }

        /// <summary>
        /// Raises the FoundSoundPackFile event.
        /// </summary>
        /// <param name="args">The SoundFileEventArgs to pass to handlers.</param>
        protected void OnFoundSoundPackFile(SoundFileEventArgs<SoundPackFile> args)
        {
            if (FoundSoundPackFile != null)
                FoundSoundPackFile(this, args);
        }

        private bool Dispatch(uint id)
        {
            return _currentBank.Objects.Dispatch(id, this);
        }

        private void DispatchAll(IEnumerable<uint> ids)
        {
            foreach (uint id in ids)
                Dispatch(id);
        }

        private bool Dispatch(uint id, uint sourceId)
        {
            if (id != sourceId)
            {
                // If the ID and source ID are different, then the source ID is the ID of the sound bank
                SoundBank bank;
                if (_soundBanks.TryGetValue(sourceId, out bank) && bank.Objects.Dispatch(id, this))
                    return true;
            }
            return _globalObjects.Dispatch(id, this);
        }
    }
}
