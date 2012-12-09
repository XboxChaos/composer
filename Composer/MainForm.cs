using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Composer.IO;
using Composer.Wwise;

namespace Composer
{
    public partial class MainForm : Form
    {
        private string _defaultTitle;
        private EndianReader _soundbankReader = null;
        private EndianReader _soundstreamReader = null;
        private SoundPack _soundbankPack = null;
        private SoundPack _soundstreamPack = null;
        private NameLookup _soundNames = null;
        private WwiseObjectCollection _wwiseObjects = new WwiseObjectCollection();
        private TreeViewBuilder _treeBuilder = null;
        private List<SoundBank> _soundbanks = null;
        private List<SoundFileInfo> _soundsFound = new List<SoundFileInfo>();

        private class SoundFileInfo
        {
            public EndianReader Reader;
            public int Offset;
            public int Size;
            public uint ID;
            public SoundFormat Format;
        }

        public MainForm()
        {
            InitializeComponent();

            _defaultTitle = Text;
            _soundNames = INILookupLoader.LoadFromFile("soundnames.lst");

            xwmaCompression.SelectedIndex = 0;
        }

        private void openSoundbank_Click(object sender, EventArgs e)
        {
            string path = AskForPackFile("soundbank.pck");
            if (path != null)
                LoadPackFile(path, soundbankPath, ref _soundbankReader, ref _soundbankPack);
        }

        private void openSoundstream_Click(object sender, EventArgs e)
        {
            string path = AskForPackFile("soundstream.pck");
            if (path != null)
                LoadPackFile(path, soundstreamPath, ref _soundstreamReader, ref _soundstreamPack);
        }

        private void loadFromFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select the folder to load soundbank.pck and soundstream.pck from.";
            fbd.ShowNewFolderButton = false;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                LoadPackFile(Path.Combine(fbd.SelectedPath, "soundbank.pck"), soundbankPath, ref _soundbankReader, ref _soundbankPack);
                LoadPackFile(Path.Combine(fbd.SelectedPath, "soundstream.pck"), soundstreamPath, ref _soundstreamReader, ref _soundstreamPack);
            }
        }

        private void fileTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null && e.Node.Tag is SoundPackFolder)
            {
                extractFile.Text = "Extract Selected Folder...";
                extractFile.Enabled = true;
            }
            else if (e.Node != null && e.Node.Tag is SoundFileInfo)
            {
                extractFile.Text = "Extract Selected File...";
                extractFile.Enabled = true;
            }
            else
            {
                extractFile.Text = "Extract Selected File...";
                extractFile.Enabled = false;
            }
        }

        private void extractFile_Click(object sender, EventArgs e)
        {
            if (fileTree.SelectedNode == null)
                return;

            /*SoundPackFolder folder = fileTree.SelectedNode.Tag as SoundPackFolder;
            if (folder != null)
            {
                // Extract the folder
                string outPath = AskForExtractionFolder();
                if (outPath != null)
                {
                    ExtractFolder(folder, outPath);
                    MessageBox.Show("Folder extracted successfully!", _defaultTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }*/

            SoundFileInfo info = fileTree.SelectedNode.Tag as SoundFileInfo;
            if (info == null)
                return;

            try
            {
                string fileName = Path.GetFileNameWithoutExtension(fileTree.SelectedNode.Text);

                if (!convertFiles.Checked || info.Format == SoundFormat.Unknown)
                {
                    // Extract the file's raw contents
                    string extractPath = AskForDestinationFile("Save Embedded Data", "Embedded Wwise Files|*.wem|All Files|*.*", fileName, "");
                    if (extractPath == null)
                        return;
                    SoundExtraction.ExtractRaw(info.Reader, info.Offset, info.Size, extractPath);
                }
                else
                {
                    // Read the RIFX data
                    info.Reader.SeekTo(info.Offset);
                    RIFX rifx = new RIFX(info.Reader, info.Size);

                    switch (info.Format)
                    {
                        case SoundFormat.XWMA:
                            {
                                if (!File.Exists("Helpers/xWMAEncode.exe"))
                                {
                                    MessageBox.Show("Composer has detected that the file you want to extract is an xWMA audio file.\n\nIn order to decode files of this type, you must get the \"xWMAEncode.exe\" utility from the Microsoft DirectX SDK and place it into the \"Helpers\" folder. The utility cannot be distributed along with Composer for legal reasons.\n\nSee the README.txt file for more information.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                if (compressXwma.Checked)
                                {
                                    // Get the file extension based upon the selected compression format
                                    string extension = (string)xwmaCompression.SelectedItem;
                                    extension = extension.Remove(extension.IndexOf(' '));

                                    string extractPath = AskForDestinationFile("Save " + extension + " File", extension + " Files|*." + extension.ToLower(), fileName, extension.ToLower());
                                    if (extractPath == null)
                                        return;

                                    // Extract the WAV to a temporary file and then compress it with FLAC
                                    string tempPath = Path.GetTempFileName();
                                    try
                                    {
                                        SoundExtraction.ExtractXWMAToWAV(info.Reader, info.Offset, rifx, tempPath);
                                        SoundExtraction.CompressWAV(tempPath, extractPath);
                                    }
                                    finally
                                    {
                                        // Delete the temporary file
                                        if (File.Exists(tempPath))
                                            File.Delete(tempPath);
                                    }
                                }
                                else
                                {
                                    // Just extract a WAV
                                    string extractPath = AskForDestinationFile("Save WAV File", "WAV Files|*.wav", fileName, "");
                                    if (extractPath == null)
                                        return;
                                    SoundExtraction.ExtractXWMAToWAV(info.Reader, info.Offset, rifx, extractPath);
                                }
                            }
                            break;
                        case SoundFormat.XMA:
                            {
                                string extractPath = AskForDestinationFile("Save WAV File", "WAV Files|*.wav", fileName, "");
                                if (extractPath == null)
                                    return;
                                SoundExtraction.ExtractXMAToWAV(info.Reader, info.Offset, rifx, extractPath);
                            }
                            break;
                        case SoundFormat.WwiseOGG:
                            {
                                string extractPath = AskForDestinationFile("Save OGG File", "OGG Files|*.ogg", fileName, "");
                                if (extractPath == null)
                                    return;
                                SoundExtraction.ExtractWwiseToOGG(info.Reader, info.Offset, info.Size, extractPath);
                            }
                            break;
                        default:
                            MessageBox.Show("Unsupported RIFX codec 0x" + ((ushort)rifx.Codec).ToString("X4"), _defaultTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                    }
                }

                MessageBox.Show("File extracted successfully!", _defaultTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, _defaultTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void extractAll_Click(object sender, EventArgs e)
        {
            string outPath = AskForExtractionFolder();
            if (outPath == null)
                return;

            /*foreach (SoundPackFolder folder in _soundbankPack.Folders)
                ExtractFolder(folder, outPath);*/

            MessageBox.Show("BROKEN: FIX!"/*"All files extracted successfully!"*/, _defaultTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void compressXwma_CheckedChanged(object sender, EventArgs e)
        {
            xwmaCompression.Enabled = compressXwma.Checked;
        }

        private void fileTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            extractFile_Click(sender, e);
        }

        private string AskForPackFile(string name)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open " + name;
            ofd.Filter = "Sound Packs|*.pck|All Files|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
                return ofd.FileName;
            return null;
        }

        /// <summary>
        /// Shows a FolderBrowserDialog, prompting the user for a directory to extract a group of files to.
        /// </summary>
        /// <returns>The selected path if the user pressed OK or null otherwise.</returns>
        private string AskForExtractionFolder()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select the folder to store extracted files to.";
            fbd.ShowNewFolderButton = true;
            if (fbd.ShowDialog() == DialogResult.Cancel)
                return null;
            return fbd.SelectedPath;
        }

        /// <summary>
        /// Shows a SaveFileDialog, prompting the user to save a file with a certain extension.
        /// </summary>
        /// <param name="title">The title that should be shown in the dialog.</param>
        /// <param name="filter">The filter string to use.</param>
        /// <param name="baseName">The suggested name of the destination file, without the extension.</param>
        /// <param name="extension">The file extension without a leading period.</param>
        /// <returns>The selected path if the user pressed OK or null otherwise.</returns>
        private string AskForDestinationFile(string title, string filter, string baseName, string extension)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = title;
            sfd.Filter = filter;
            sfd.DefaultExt = extension;
            sfd.FileName = baseName;
            if (sfd.ShowDialog() == DialogResult.OK)
                return sfd.FileName;
            return null;
        }

        private void LoadPackFile(string path, TextBox pathBox, ref EndianReader reader, ref SoundPack pack)
        {
            if (reader != null)
                reader.Close();

            // Load the sound pack
            reader = new EndianReader(File.OpenRead(path), Endian.BigEndian);
            pack = new SoundPack(reader);
            pathBox.Text = path;

            if (_soundbankPack != null && _soundstreamPack != null)
            {
                // Soundbank and soundstream have both been loaded - display the sound tree
                BuildFileTree();
                controls.Enabled = true;
            }
        }

        /// <summary>
        /// Builds and displays the file TreeView.
        /// </summary>
        private void BuildFileTree()
        {
            _treeBuilder = new TreeViewBuilder(fileTree.Nodes);
            
            SoundScanner scanner = new SoundScanner();
            scanner.FoundSoundBankFile += FoundSoundBankFile;
            scanner.FoundSoundPackFile += FoundSoundPackFile;

            // Load everything from the packs into the scanner
            _soundbanks = new List<SoundBank>();
            LoadObjects(_soundbankPack, _soundbankReader, scanner);
            LoadObjects(_soundstreamPack, _soundstreamReader, scanner);

            // Clear the TreeView and scan everything
            // The event handlers attached above are responsible for adding nodes to the tree
            fileTree.Nodes.Clear();
            foreach (SoundBank bank in _soundbanks)
            {
                foreach (SoundBankEvent ev in bank.Events)
                {
                    scanner.ScanEvent(ev);
                    AddSoundNodes(ev);
                    _soundsFound.Clear();
                }
            }
            _soundbanks = null;
        }

        /// <summary>
        /// Event handler for the SoundScanner.FoundSoundBankFile event.
        /// </summary>
        private void FoundSoundBankFile(object sender, SoundFileEventArgs<SoundBankFile> e)
        {
            // Find the sound bank's pack file so we can determine the offset of the audio data
            uint bankId = e.File.ParentBank.ID;
            SoundPackFile packFile = _soundbankPack.FindFileByID(bankId);
            if (packFile == null)
                return;

            // Calculate the offset of the audio data
            int offset = packFile.Offset + e.File.ParentBank.DataOffset + e.File.Offset;

            // Read the sound's RIFX data so we can identify its format
            _soundbankReader.SeekTo(offset);
            RIFX rifx = new RIFX(_soundbankReader, e.File.Size);
            SoundFormat format = FormatIdentification.IdentifyFormat(rifx);

            // Create information to tag the tree node with
            SoundFileInfo info = new SoundFileInfo
            {
                Reader = _soundbankReader,
                Offset = offset,
                Size = e.File.Size,
                ID = e.File.ID,
                Format = format
            };
            _soundsFound.Add(info);
        }

        /// <summary>
        /// Event handler for the SoundScanner.FoundSoundPackFile event.
        /// </summary>
        private void FoundSoundPackFile(object sender, SoundFileEventArgs<SoundPackFile> e)
        {
            // Make sure the file isn't in soundbank for some reason
            EndianReader reader = _soundstreamReader;
            if (_soundbankPack.FindFileByID(e.File.ID) != null)
                reader = _soundbankReader;

            // Read the sound's RIFX data so we can identify its format
            reader.SeekTo(e.File.Offset);
            RIFX rifx = new RIFX(reader, e.File.Size);
            SoundFormat format = FormatIdentification.IdentifyFormat(rifx);

            // Create information to tag the tree node with
            SoundFileInfo info = new SoundFileInfo
            {
                Reader = reader,
                Offset = e.File.Offset,
                Size = e.File.Size,
                ID = e.File.ID,
                Format = format
            };
            _soundsFound.Add(info);
        }

        private void AddSoundNodes(SoundBankEvent sourceEvent)
        {
            string eventName = _soundNames.FindName(sourceEvent.ID);
            
            if (eventName != null && _soundsFound.Count == 1)
            {
                // Just add a leaf node
                string extension = GetFormatExtension(_soundsFound[0].Format);
                _treeBuilder.AddNode(eventName + extension, 1, _soundsFound[0]);
            }
            else
            {
                if (eventName == null)
                    eventName = "unknown";

                // Add the sounds to a folder
                foreach (SoundFileInfo info in _soundsFound)
                {
                    string folderName = Path.GetFileName(eventName);
                    string extension = GetFormatExtension(info.Format);
                    _treeBuilder.AddNode(eventName + '/' + folderName + '_' + info.ID.ToString("X8") + extension, 1, info);
                }
            }
        }

        /// <summary>
        /// Retrieves the file extension for a sound format.
        /// </summary>
        /// <param name="format">The SoundFormat to retrieve the file extension for.</param>
        /// <returns>The format's file extension (including the leading period).</returns>
        private static string GetFormatExtension(SoundFormat format)
        {
            switch (format)
            {
                case SoundFormat.WwiseOGG:
                    return ".ogg";
                case SoundFormat.XMA:
                    return ".xma";
                case SoundFormat.XWMA:
                    return ".xwma";
                default:
                    return ".wem";
            }
        }

        private void LoadObjects(SoundPack pack, EndianReader reader, SoundScanner scanner)
        {
            foreach (SoundPackFolder folder in pack.Folders)
            {
                foreach (SoundPackFile file in folder.Files)
                {
                    reader.SeekTo(file.Offset);
                    int magic = reader.ReadInt32();

                    switch (magic)
                    {
                        case 0x52494658: // RIFX
                            scanner.RegisterObject(file);
                            break;

                        case 0x424B4844: // BKHD
                            reader.SeekTo(file.Offset);
                            SoundBank bank = new SoundBank(reader, file.Size);
                            scanner.RegisterObjects(bank.Objects);
                            _soundbanks.Add(bank);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Extracts all of the files in a folder to a directory.
        /// </summary>
        /// <param name="folder">The SoundPackFolder to extract files from.</param>
        /// <param name="outPath">The path of the directory to store extracted files to.</param>
        /*private void ExtractFolder(SoundPackFolder folder, string outPath)
        {
            string folderPath = Path.Combine(outPath, folder.Name);
            Directory.CreateDirectory(folderPath);

            foreach (SoundPackFile file in folder.Files)
            {
                try
                {
                    // Get the directoryPath to extract to, minus the file extension
                    string pathBase = Path.Combine(folderPath, file.ID.ToString("X8"));

                    if (!convertFiles.Checked)
                    {
                        // Just extract the raw contents
                        ExtractRaw(file, pathBase + ".bin");
                    }
                    else
                    {
                        // Read the RIFX data
                        _soundbankReader.SeekTo(file.Offset);
                        RIFX rifx = new RIFX(_soundbankReader);

                        // Choose the output format based upon its codec
                        switch (rifx.Codec)
                        {
                            case 0x166: // XMA
                                ExtractWAV(file, rifx, pathBase + ".wav");
                                break;
                            case -1: // WWISE OGG
                                ExtractOGG(file, pathBase + ".ogg");
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // TODO: Show a status message or something
                }
            }
        }*/
    }
}
