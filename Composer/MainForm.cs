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
        private int _filesProcessed = 0;
        private int _totalFiles = 0;
        private int _totalSounds = 0;

        private const int ObjectLoadProgressWeight = 80;
        private const int EventLoadProgressWeight = 20;

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
            {
                LoadPackFile(path, soundbankPath, ref _soundbankReader, ref _soundbankPack);
                if (_soundbankPack != null && _soundstreamPack != null)
                    BuildFileTree();
            }
        }

        private void openSoundstream_Click(object sender, EventArgs e)
        {
            string path = AskForPackFile("soundstream.pck");
            if (path != null)
            {
                LoadPackFile(path, soundstreamPath, ref _soundstreamReader, ref _soundstreamPack);
                if (_soundbankPack != null && _soundstreamPack != null)
                    BuildFileTree();
            }
        }

        private void loadFromFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select the folder to load soundbank.pck and soundstream.pck from. This is sound\\English(US) in Halo 4's extracted ISO.";
            fbd.ShowNewFolderButton = false;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                LoadPackFile(Path.Combine(fbd.SelectedPath, "soundbank.pck"), soundbankPath, ref _soundbankReader, ref _soundbankPack);
                LoadPackFile(Path.Combine(fbd.SelectedPath, "soundstream.pck"), soundstreamPath, ref _soundstreamReader, ref _soundstreamPack);

                if (_soundbankPack != null && _soundstreamPack != null)
                    BuildFileTree();
            }
        }

        private void fileTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null && e.Node.Nodes.Count > 0)
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

            if (fileTree.SelectedNode.Nodes.Count > 0)
            {
                // Folder
                string extractPath = AskForExtractionFolder();
                if (extractPath != null)
                {
                    ExtractFolder(fileTree.SelectedNode, extractPath);
                    MessageBox.Show("Folder extracted successfully!", _defaultTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }

            SoundFileInfo info = GetSoundInfo(fileTree.SelectedNode);
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
                    RIFX rifx = ReadRIFX(info);
                    switch (info.Format)
                    {
                        case SoundFormat.XWMA:
                            {
                                if (!File.Exists("Helpers/xWMAEncode.exe"))
                                {
                                    MessageBox.Show("Composer has detected that the file you want to extract is an xWMA audio file.\n\nIn order to decode files of this type, you must get the \"xWMAEncode.exe\" utility from the Microsoft DirectX SDK and place it into the \"Helpers\" folder. The utility cannot be distributed along with Composer for legal reasons.\n\nSee the README.txt file for more information.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                string extension = GetOutputExtension(info.Format);
                                string extractPath = AskForDestinationFile("Save " + extension.ToUpper() + " File", extension.ToUpper() + " Files|*." + extension, fileName, extension);
                                if (extractPath == null)
                                    return;

                                ExtractSound(info, rifx, extractPath, compressXwma.Checked);
                            }
                            break;
                        case SoundFormat.XMA:
                            {
                                string extractPath = AskForDestinationFile("Save WAV File", "WAV Files|*.wav", fileName, "");
                                if (extractPath == null)
                                    return;

                                ExtractSound(info, rifx, extractPath, compressXwma.Checked);
                            }
                            break;
                        case SoundFormat.WwiseOGG:
                            {
                                string extractPath = AskForDestinationFile("Save OGG File", "OGG Files|*.ogg", fileName, "");
                                if (extractPath == null)
                                    return;

                                ExtractSound(info, rifx, extractPath, compressXwma.Checked);
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

            foreach (TreeNode node in fileTree.Nodes)
                ExtractFolder(node, outPath);

            MessageBox.Show("All files extracted successfully!", _defaultTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void convertFiles_CheckedChanged(object sender, EventArgs e)
        {
            compressXwma.Enabled = convertFiles.Checked;
            xwmaCompression.Enabled = convertFiles.Checked && compressXwma.Checked;
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
            ofd.Filter = "Wwise Sound Packs|*.pck|All Files|*.*";
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
            fbd.Description = "Select the folder to store extracted files and folders to. New folders will be created for you.";
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
        }

        /// <summary>
        /// Builds and displays the file TreeView.
        /// </summary>
        private void BuildFileTree()
        {
            _treeBuilder = new TreeViewBuilder(fileTree.Nodes);

            loadingControls.Enabled = false;
            controls.Enabled = false;
            fileTree.Nodes.Clear();

            // Compute totals for the progress bar
            _filesProcessed = 0;
            _totalSounds = 0;
            _totalFiles = 0;
            _totalFiles += GetTotalFiles(_soundbankPack);
            _totalFiles += GetTotalFiles(_soundstreamPack);
            progressBar.Value = 0;
            
            // Load the banks in a separate thread
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += ObjectLoadWorker;
            worker.ProgressChanged += ObjectLoadProgressChanged;
            worker.RunWorkerCompleted += ObjectLoadComplete;
            worker.RunWorkerAsync();
        }

        private void ObjectLoadWorker(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;

            SoundScanner scanner = new SoundScanner();
            scanner.FoundSoundBankFile += FoundSoundBankFile;
            scanner.FoundSoundPackFile += FoundSoundPackFile;

            // Load everything from the packs into the scanner
            _soundbanks = new List<SoundBank>();
            LoadObjects(_soundbankPack, _soundbankReader, scanner, worker);
            LoadObjects(_soundstreamPack, _soundstreamReader, scanner, worker);

            // Clear the TreeView and scan everything
            // The event handlers attached above are responsible for adding nodes to the tree
            for (int i = 0; i < _soundbanks.Count; i++)
            {
                worker.ReportProgress(ObjectLoadProgressWeight + i * EventLoadProgressWeight / _soundbanks.Count, "Scanning sound bank " + _soundbanks[i].ID.ToString("X8") + ".bnk...");
                foreach (SoundBankEvent ev in _soundbanks[i].Events)
                {
                    scanner.ScanEvent(ev);
                    AddSoundNodes(ev);
                    _soundsFound.Clear();
                }
            }
        }

        private void ObjectLoadProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            statusLabel.Text = (string)e.UserState;
        }

        private void ObjectLoadComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                controls.Enabled = true;
                statusLabel.Text = _totalSounds + " sounds loaded.";
            }
            else
            {
                MessageBox.Show(e.Error.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusLabel.Text = "Ready";
            }

            progressBar.Value = progressBar.Minimum;
            loadingControls.Enabled = true;

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

            // Calculate the offset of the audio data and add it
            int offset = packFile.Offset + e.File.ParentBank.DataOffset + e.File.Offset;
            AddSound(_soundbankReader, offset, e.File.Size, e.File.ID);
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

            AddSound(reader, e.File.Offset, e.File.Size, e.File.ID);
        }

        private void AddSound(EndianReader reader, int offset, int size, uint id)
        {
            // Read the sound's RIFX data so we can identify its format
            reader.SeekTo(offset);
            RIFX rifx = new RIFX(reader, size);
            SoundFormat format = FormatIdentification.IdentifyFormat(rifx);

            // Create information to tag the tree node with
            SoundFileInfo info = new SoundFileInfo
            {
                Reader = reader,
                Offset = offset,
                Size = size,
                ID = id,
                Format = format
            };
            _soundsFound.Add(info);
        }

        private void AddSoundNodes(SoundBankEvent sourceEvent)
        {
            string eventName = _soundNames.FindName(sourceEvent.ID);
            _totalSounds += _soundsFound.Count;
            
            if (eventName != null && _soundsFound.Count == 1)
            {
                // Just add a leaf node
                string extension = GetFormatExtension(_soundsFound[0].Format);
                AddNodeAsync(eventName + '.' + extension, 1, _soundsFound[0]);
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
                    AddNodeAsync(eventName + '/' + folderName + '_' + info.ID.ToString("X8") + '.' + extension, 1, info);
                }
            }
        }

        private void AddNodeAsync(string path, int image, object tag)
        {
            fileTree.BeginInvoke(new Action<string, int, object>(AddNodeDelegate), path, image, tag);
        }

        private void AddNodeDelegate(string path, int image, object tag)
        {
            _treeBuilder.AddNode(path, image, tag);
        }

        private static SoundFileInfo GetSoundInfo(TreeNode node)
        {
            if (node == null)
                return null;

            return node.Tag as SoundFileInfo;
        }

        private static RIFX ReadRIFX(SoundFileInfo info)
        {
            info.Reader.SeekTo(info.Offset);
            return new RIFX(info.Reader, info.Size);
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
                    return "ogg";
                case SoundFormat.XMA:
                    return "xma";
                case SoundFormat.XWMA:
                    return "xwma";
                default:
                    return "wem";
            }
        }

        private string GetOutputExtension(SoundFormat format)
        {
            switch (format)
            {
                case SoundFormat.WwiseOGG:
                    return "ogg";
                case SoundFormat.XMA:
                    return "wav";
                case SoundFormat.XWMA:
                    {
                        if (compressXwma.Checked)
                        {
                            // Get the file extension based upon the selected compression format
                            string extension = (string)xwmaCompression.SelectedItem;
                            return extension.Remove(extension.IndexOf(' ')).ToLowerInvariant();
                        }
                        else
                        {
                            return "wav";
                        }
                    }
                default:
                    return "bin";
            }
        }

        private int GetTotalFiles(SoundPack pack)
        {
            int total = 0;
            foreach (SoundPackFolder folder in pack.Folders)
                total += folder.Files.Count;
            return total;
        }

        private void LoadObjects(SoundPack pack, EndianReader reader, SoundScanner scanner, BackgroundWorker worker)
        {
            foreach (SoundPackFolder folder in pack.Folders)
            {
                foreach (SoundPackFile file in folder.Files)
                {
                    worker.ReportProgress(ObjectLoadProgressWeight * _filesProcessed / _totalFiles, "Reading file " + file.ID.ToString("X8") + ".wem...");

                    reader.SeekTo(file.Offset);
                    int magic = reader.ReadInt32();

                    switch (magic)
                    {
                        case 0x52494658: // RIFX - Embedded sound file
                            scanner.RegisterObject(file);
                            break;

                        case 0x424B4844: // BKHD - Sound bank
                            reader.SeekTo(file.Offset);
                            SoundBank bank = new SoundBank(reader, file.Size);
                            scanner.RegisterObjects(bank.Objects);
                            _soundbanks.Add(bank);
                            break;
                    }

                    _filesProcessed++;
                }
            }
        }

        private static void ExtractSound(SoundFileInfo info, RIFX rifx, string extractPath, bool compressXwma)
        {
            switch (info.Format)
            {
                case SoundFormat.XWMA:
                    if (!File.Exists("Helpers/xWMAEncode.exe"))
                        return;

                    if (compressXwma)
                        SoundExtraction.ExtractAndConvertXWMA(info.Reader, info.Offset, rifx, extractPath);
                    else
                        SoundExtraction.ExtractXWMAToWAV(info.Reader, info.Offset, rifx, extractPath);

                    break;

                case SoundFormat.XMA:
                    SoundExtraction.ExtractXMAToWAV(info.Reader, info.Offset, rifx, extractPath);
                    break;

                case SoundFormat.WwiseOGG:
                    SoundExtraction.ExtractWwiseToOGG(info.Reader, info.Offset, info.Size, extractPath);
                    break;
            }
        }

        private void ExtractFolder(TreeNode folder, string extractPath)
        {
            // Create a directory for the folder itself
            string folderPath = Path.Combine(extractPath, folder.Text);
            Directory.CreateDirectory(folderPath);

            // Recursively extract everything
            foreach (TreeNode node in folder.Nodes)
            {
                try
                {
                    if (node.Nodes.Count > 0)
                    {
                        ExtractFolder(node, folderPath);
                    }
                    else
                    {
                        SoundFileInfo info = GetSoundInfo(node);
                        if (info == null)
                            continue;

                        RIFX rifx = ReadRIFX(info);
                        string extension = GetOutputExtension(info.Format);
                        string filePath = Path.ChangeExtension(Path.Combine(folderPath, node.Text), extension);
                        ExtractSound(info, rifx, filePath, compressXwma.Checked);
                    }
                }
                catch (Exception ex)
                {
                    // FIXME: A catch-all like this is bad practice
                    Debug.WriteLine(ex);
                }
            }
        }
    }
}
