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
                if (!convertFiles.Checked)
                {
                    // Just extract the file's raw contents
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Title = "Save Raw Data";
                    sfd.Filter = "Binary Files|*.bin|All Files|*.*";
                    sfd.DefaultExt = "bin";
                    sfd.FileName = file.ID.ToString("X8") + ".bin";
                    if (sfd.ShowDialog() == DialogResult.OK)
                        ExtractRaw(file, sfd.FileName);
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
                            SaveFileDialog sfd = new SaveFileDialog();
                            sfd.Title = "Save WAV File";
                            sfd.Filter = "WAV Files|*.wav";
                            sfd.DefaultExt = "wav";
                            sfd.FileName = file.ID.ToString("X8") + ".wav";
                            if (sfd.ShowDialog() == DialogResult.Cancel)
                                return;
                            ExtractWAV(file, rifx, sfd.FileName);
                        }
                        break;
                    case -1:
                        {
                            SaveFileDialog sfd = new SaveFileDialog();
                            sfd.Title = "Save OGG File";
                            sfd.Filter = "OGG Files|*.ogg";
                            sfd.DefaultExt = "ogg";
                            sfd.FileName = file.ID.ToString("X8") + ".ogg";
                            if (sfd.ShowDialog() == DialogResult.Cancel)
                                return;
                            ExtractOGG(file, sfd.FileName);
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

        private string AskForExtractionFolder()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select the folder to store extracted files to.";
            fbd.ShowNewFolderButton = true;
            if (fbd.ShowDialog() == DialogResult.Cancel)
                return null;
            return fbd.SelectedPath;
        }

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

        private void ExtractRaw(SoundPackFile file, string outPath)
        {
            using (EndianWriter output = new EndianWriter(File.OpenWrite(outPath), Endian.BigEndian))
            {
                // Just copy the data over to the output stream
                _packReader.SeekTo(file.Offset);
                StreamUtil.Copy(_packReader, output, file.Size);
            }
        }

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
