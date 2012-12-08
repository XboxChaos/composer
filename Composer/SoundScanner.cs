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
        private WwiseObjectCollection _objects = new WwiseObjectCollection();
        private List<SoundBank> _banks = new List<SoundBank>();
        private SoundBankEvent _currentEvent = null;

        /// <summary>
        /// Occurs when a SoundBankFile is found.
        /// </summary>
        public EventHandler<SoundFileEventArgs<SoundBankFile>> FoundSoundBankFile;

        /// <summary>
        /// Occurs when a SoundPackFile is found.
        /// </summary>
        public EventHandler<SoundFileEventArgs<SoundPackFile>> FoundSoundPackFile;

        /// <summary>
        /// Registers the contents of a sound bank to be scanned.
        /// </summary>
        /// <param name="bank">The SoundBank to register.</param>
        public void RegisterSoundBank(SoundBank bank)
        {
            _banks.Add(bank);
            _objects.Import(bank.Objects);
        }

        /// <summary>
        /// Registers a Wwise object to be scanned.
        /// </summary>
        /// <param name="obj">The object to register.</param>
        public void RegisterObject(IWwiseObject obj)
        {
            _objects.Add(obj);
        }

        /// <summary>
        /// Scans through all registered events and objects to find sounds.
        /// </summary>
        public void ScanAll()
        {
            foreach (SoundBank bank in _banks)
            {
                foreach (SoundBankEvent ev in bank.Events)
                    ProcessEvent(ev);
            }
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
            _objects.Dispatch(voice.AudioID, this);
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
                _objects.Dispatch(action.ObjectID, this);
            }
        }

        public void Visit(SoundBankEvent ev)
        {
            // Scan each action in the event
            _currentEvent = ev;
            foreach (uint actionId in ev.ActionIDs)
                _objects.Dispatch(actionId, this);
        }

        public void Visit(SoundBankMusicPlaylist playlist)
        {
            foreach (uint id in playlist.SegmentIDs)
                _objects.Dispatch(id, this);
        }

        public void Visit(SoundBankMusicSegment segment)
        {
            foreach (uint id in segment.ChildIDs)
                _objects.Dispatch(id, this);
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

        private void ProcessEvent(SoundBankEvent ev)
        {
            Visit(ev);
        }
    }
}
