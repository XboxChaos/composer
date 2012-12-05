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
        private EndianReader _packReader = null;
        private SoundPack _currentPack = null;
        private IDLookup _soundNames = null;
        private WwiseObjectCollection _wwiseObjects = new WwiseObjectCollection();

        public MainForm()
        {
            InitializeComponent();

            _defaultTitle = Text;
            _soundNames = INILookupLoader.LoadFromFile("soundnames.txt");
        }

        private void openPck_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open Sound Pack";
            ofd.Filter = "Sound Packs|*.pck|All Files|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (_packReader != null)
                    _packReader.Close();

                pckPath.Text = ofd.FileName;
                _packReader = new EndianReader(File.OpenRead(ofd.FileName), Endian.BigEndian);
                _currentPack = new SoundPack(_packReader);

                BuildFileTree();

                controls.Enabled = true;

                Text = _defaultTitle + " - " + Path.GetFileName(ofd.FileName);
            }
        }

        private void fileTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null && e.Node.Tag is SoundPackFolder)
            {
                extractFile.Text = "Extract Selected Folder...";
                extractFile.Enabled = true;
            }
            else if (e.Node != null && e.Node.Tag is SoundPackFile)
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

            SoundPackFolder folder = fileTree.SelectedNode.Tag as SoundPackFolder;
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
            }

            SoundPackFile file = fileTree.SelectedNode.Tag as SoundPackFile;
            if (file == null)
                return;

            try
            {
                // Just use the file's ID as its name for now
                string fileName = file.ID.ToString("X8");

                if (!convertFiles.Checked)
                {
                    // Extract the file's raw contents
                    string extractPath = AskForDestinationFile("Save Raw Data", "Binary Files|*.bin|All Files|*.*", fileName, "bin");
                    if (extractPath != null)
                        ExtractRaw(file, extractPath);
                    return;
                }
            
                // Read the RIFX data
                _packReader.SeekTo(file.Offset);
                RIFX rifx = new RIFX(_packReader);

                // Choose the output format based upon its codec
                switch (rifx.Codec)
                {
                    case 0x166:
                        {
                            string extractPath = AskForDestinationFile("Save WAV File", "WAV Files|*.wav", fileName, "wav");
                            if (extractPath == null)
                                return;
                            ExtractWAV(file, rifx, extractPath);
                        }
                        break;
                    case -1:
                        {
                            string extractPath = AskForDestinationFile("Save OGG File", "OGG Files|*.ogg", fileName, "ogg");
                            if (extractPath == null)
                                return;
                            ExtractOGG(file, extractPath);
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

            foreach (SoundPackFolder folder in _currentPack.Folders)
                ExtractFolder(folder, outPath);

            MessageBox.Show("All files extracted successfully!", _defaultTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        /// <summary>
        /// Builds and displays the file TreeView.
        /// </summary>
        private void BuildFileTree()
        {
            fileTree.Nodes.Clear();

            TreeViewBuilder builder = new TreeViewBuilder(fileTree.Nodes);
            foreach (SoundPackFolder folder in _currentPack.Folders)
            {
                string pathBase = folder.Name + '/';

                foreach (SoundPackFile file in folder.Files)
                {
                    string fileName = _soundNames.FindName(file.ID);
                    if (fileName == null)
                        fileName = file.ID.ToString("X8");
                    fileName = fileName.Trim('/', '\\');

                    builder.AddNode(pathBase + fileName, 1, file);
                }
            }
        }

        /// <summary>
        /// Extracts all of the files in a folder to a directory.
        /// </summary>
        /// <param name="folder">The SoundPackFolder to extract files from.</param>
        /// <param name="outPath">The path of the directory to store extracted files to.</param>
        private void ExtractFolder(SoundPackFolder folder, string outPath)
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
                        _packReader.SeekTo(file.Offset);
                        RIFX rifx = new RIFX(_packReader);

                        // Choose the output format based upon its codec
                        switch (rifx.Codec)
                        {
                            case 0x166: // XMA
                                ExtractWAV(file, rifx, pathBase + ".wav");
                                break;
                            case -1: // WWISE OGG
                                ExtractOGG(file, pathBase + ".ogg");
                                break;
                            /*default:
                                MessageBox.Show("Unsupported codec 0x" + ((ushort)rifx.Codec).ToString("X4"), _defaultTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;*/
                        }
                    }
                }
                catch (Exception ex)
                {
                    // TODO: Show a status message or something
                }
            }
        }

        /// <summary>
        /// Extracts the raw contents of a SoundPackFile to a file.
        /// </summary>
        /// <param name="file">The SoundPackFile to extract.</param>
        /// <param name="outPath">The path of the file to save to.</param>
        private void ExtractRaw(SoundPackFile file, string outPath)
        {
            using (EndianWriter output = new EndianWriter(File.OpenWrite(outPath), Endian.BigEndian))
            {
                // Just copy the data over to the output stream
                _packReader.SeekTo(file.Offset);
                StreamUtil.Copy(_packReader, output, file.Size);
            }
        }

        /// <summary>
        /// Extracts a SoundPackFile and converts it to a WAV.
        /// </summary>
        /// <param name="file">The SoundPackFile to extract. It must be in XMA format.</param>
        /// <param name="rifx">The RIFX data for the SoundPackFile.</param>
        /// <param name="outPath">The path of the file to save to.</param>
        private void ExtractWAV(SoundPackFile file, RIFX rifx, string outPath)
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
                    _packReader.SeekTo(file.Offset + rifx.DataOffset);
                    StreamUtil.Copy(_packReader, output, rifx.DataSize);
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
        /// Extracts a SoundPackFile and converts it to an OGG.
        /// </summary>
        /// <param name="file">The SoundPackFile to extract. It must be in WWISE OGG format.</param>
        /// <param name="outPath">The path of the file to save to.</param>
        private void ExtractOGG(SoundPackFile file, string outPath)
        {
            // Just extract the RIFX to a temporary file
            string tempFile = Path.GetTempFileName() + ".wav";

            try
            {
                using (EndianWriter output = new EndianWriter(File.OpenWrite(tempFile), Endian.BigEndian))
                {
                    _packReader.SeekTo(file.Offset);
                    StreamUtil.Copy(_packReader, output, file.Size);
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
