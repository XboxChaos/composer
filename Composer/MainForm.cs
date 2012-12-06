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

        private class SoundFileInfo
        {
            public EndianReader Reader;
            public int Offset;
            public int Size;
            public uint ID;
        }

        public MainForm()
        {
            InitializeComponent();

            _defaultTitle = Text;
            _soundNames = INILookupLoader.LoadFromFile("soundnames.txt");
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
                string fileName = fileTree.SelectedNode.Text;

                if (!convertFiles.Checked)
                {
                    // Extract the file's raw contents
                    string extractPath = AskForDestinationFile("Save Raw Data", "Binary Files|*.bin|All Files|*.*", fileName, "bin");
                    if (extractPath != null)
                        ExtractRaw(info.Reader, info.Offset, info.Size, extractPath);
                    return;
                }
            
                // Read the RIFX data
                info.Reader.SeekTo(info.Offset);
                RIFX rifx = new RIFX(info.Reader);

                // Choose the output format based upon its codec
                switch (rifx.Codec)
                {
                    case 0x166:
                        {
                            string extractPath = AskForDestinationFile("Save WAV File", "WAV Files|*.wav", fileName, "wav");
                            if (extractPath == null)
                                return;
                            ExtractWAV(info.Reader, info.Offset, rifx, extractPath);
                        }
                        break;
                    case -1:
                        {
                            string extractPath = AskForDestinationFile("Save OGG File", "OGG Files|*.ogg", fileName, "ogg");
                            if (extractPath == null)
                                return;
                            ExtractOGG(info.Reader, info.Offset, info.Size, extractPath);
                        }
                        break;
                    default:
                        MessageBox.Show("Unsupported codec 0x" + ((ushort)rifx.Codec).ToString("X4"), _defaultTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
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
            sfd.FileName = baseName + "." + extension;
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
            LoadObjects(_soundbankPack, _soundbankReader, scanner);
            LoadObjects(_soundstreamPack, _soundstreamReader, scanner);

            // Clear the TreeView and scan everything
            // The event handlers attached above are responsible for adding nodes to the tree
            fileTree.Nodes.Clear();
            scanner.ScanAll();
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

            // Create information to tag the tree node with
            SoundFileInfo info = new SoundFileInfo
            {
                Reader = _soundbankReader,
                Offset = offset,
                Size = e.File.Size,
                ID = e.File.ID
            };

            // Add it to the tree
            AddSoundNode(e.SourceEvent, info);
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

            // Create information to tag the tree node with
            SoundFileInfo info = new SoundFileInfo
            {
                Reader = reader,
                Offset = e.File.Offset,
                Size = e.File.Size,
                ID = e.File.ID
            };

            // Add it to the tree
            AddSoundNode(e.SourceEvent, info);
        }

        private void AddSoundNode(SoundBankEvent sourceEvent, SoundFileInfo info)
        {
            // Determine the name of the sound based upon its source event
            string name = _soundNames.FindName(sourceEvent.ID);
            if (name == null)
                name = "unknown/" + info.ID.ToString("X8");

            TreeNode oldNode = _treeBuilder.GetNode(name);
            if (oldNode != null)
            {
                SoundFileInfo oldInfo = oldNode.Tag as SoundFileInfo;
                if (oldInfo != null)
                {
                    if (oldInfo.ID == info.ID)
                        return;

                    // A different node with the path already exists
                    // Untag it, make it a folder, and add it as a child for the old node instead
                    oldNode.Tag = null;
                    oldNode.ImageIndex = 0;
                    oldNode.SelectedImageIndex = 0;

                    TreeNode child = new TreeNode(oldInfo.ID.ToString("X8"), 1, 1);
                    child.Tag = oldInfo;
                    oldNode.Nodes.Add(child);
                }

                // Add us as a child of the node
                name += "/" + info.ID.ToString("X8");
            }

            // Add the sound to the tree
            _treeBuilder.AddNode(name, 1, info);
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
                            scanner.RegisterSoundBank(bank);
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

        /// <summary>
        /// Extracts the raw contents of a sound to a file.
        /// </summary>
        /// <param name="reader">The stream to read from.</param>
        /// <param name="offset">The offset of the data to extract.</param>
        /// <param name="size">The size of the data to extract.</param>
        /// <param name="outPath">The path of the file to save to.</param>
        private void ExtractRaw(IReader reader, int offset, int size, string outPath)
        {
            using (EndianWriter output = new EndianWriter(File.OpenWrite(outPath), Endian.BigEndian))
            {
                // Just copy the data over to the output stream
                reader.SeekTo(offset);
                StreamUtil.Copy(reader, output, size);
            }
        }

        /// <summary>
        /// Extracts a sound and converts it to a WAV.
        /// </summary>
        /// <param name="reader">The stream to read from.</param>
        /// <param name="offset">The offset of the data to extract.</param>
        /// <param name="rifx">The RIFX data for the SoundPackFile.</param>
        /// <param name="outPath">The path of the file to save to.</param>
        private void ExtractWAV(IReader reader, int offset, RIFX rifx, string outPath)
        {
            // Create a temporary file to write an XMA to
            string tempFile = Path.GetTempFileName() + ".xma";

            try
            {
                using (EndianWriter output = new EndianWriter(File.OpenWrite(tempFile), Endian.BigEndian))
                {
                    // Generate an XMA header
                    // Adapted from wwisexmabank
                    output.WriteInt32(0x52494646); // 'RIFF'
                    output.Endianness = Endian.LittleEndian;
                    output.WriteInt32(rifx.DataSize + 0x34);
                    output.Endianness = Endian.BigEndian;
                    output.WriteInt32(0x57415645); // 'WAVE'

                    // Generate the 'fmt ' chunk
                    output.WriteInt32(0x666D7420); // 'fmt '
                    output.Endianness = Endian.LittleEndian;
                    output.WriteInt32(0x20);
                    output.WriteInt16(0x165);
                    output.WriteInt16(16);
                    output.WriteInt16(0);
                    output.WriteInt16(0);
                    output.WriteInt16(1);
                    output.WriteByte(0);
                    output.WriteByte(3);
                    output.WriteInt32(0);
                    output.WriteInt32(rifx.SampleRate);
                    output.WriteInt32(0);
                    output.WriteInt32(0);
                    output.WriteByte(0);
                    output.WriteByte((byte)rifx.ChannelCount);
                    output.WriteInt16(0x0002);

                    // 'data' chunk
                    output.Endianness = Endian.BigEndian;
                    output.WriteInt32(0x64617461); // 'data'
                    output.Endianness = Endian.LittleEndian;
                    output.WriteInt32(rifx.DataSize);

                    // Copy the data chunk contents from the original RIFX
                    reader.SeekTo(offset + rifx.DataOffset);
                    StreamUtil.Copy(reader, output, rifx.DataSize);
                }

                // Convert it with towav
                RunProgramSilently("Helpers/towav.exe", Path.GetFileName(tempFile), Path.GetDirectoryName(tempFile));

                // Move the WAV to the destination directoryPath
                File.Move(Path.ChangeExtension(tempFile, "wav"), outPath);
            }
            finally
            {
                // Delete the temporary XMA file
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
        }

        /// <summary>
        /// Extracts a sound and converts it to an OGG.
        /// </summary>
        /// <param name="reader">The stream to read from.</param>
        /// <param name="offset">The offset of the data to extract.</param>
        /// <param name="size">The size of the data to extract.</param>
        /// <param name="outPath">The path of the file to save to.</param>
        private void ExtractOGG(IReader reader, int offset, int size, string outPath)
        {
            // Just extract the RIFX to a temporary file
            string tempFile = Path.GetTempFileName() + ".wav";

            try
            {
                using (EndianWriter output = new EndianWriter(File.OpenWrite(tempFile), Endian.BigEndian))
                {
                    reader.SeekTo(offset);
                    StreamUtil.Copy(reader, output, size);
                }

                // Run ww2ogg to convert the resulting RIFX to an OGG
                RunProgramSilently("Helpers/ww2ogg.exe",
                    string.Format("\"{0}\" -o \"{1}\" --pcb Helpers/packed_codebooks_aoTuV_603.bin", tempFile, outPath),
                    Path.GetDirectoryName(Application.ExecutablePath));

                // Run revorb to fix up the OGG
                RunProgramSilently("Helpers/revorb.exe", "\"" + outPath + "\"", Directory.GetCurrentDirectory());
            }
            finally
            {
                // Delete the old RIFX file
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
        }

        /// <summary>
        /// Silently executes a program and waits for it to finish.
        /// </summary>
        /// <param name="path">The path to the program to execute.</param>
        /// <param name="arguments">Command-line arguments to pass to the program.</param>
        /// <param name="workingDirectory">The working directory to run in the program in.</param>
        private void RunProgramSilently(string path, string arguments, string workingDirectory)
        {
            ProcessStartInfo info = new ProcessStartInfo(path, arguments);
            info.CreateNoWindow = true;
            //info.RedirectStandardOutput = true;
            info.UseShellExecute = false;
            info.WorkingDirectory = workingDirectory;

            Process proc = Process.Start(info);
            proc.WaitForExit();
        }
    }
}
