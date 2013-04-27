namespace Composer
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.openSoundbank = new System.Windows.Forms.Button();
            this.soundbankPath = new System.Windows.Forms.TextBox();
            this.fileTree = new System.Windows.Forms.TreeView();
            this.nodeImages = new System.Windows.Forms.ImageList(this.components);
            this.extractFile = new System.Windows.Forms.Button();
            this.extractAll = new System.Windows.Forms.Button();
            this.convertFiles = new System.Windows.Forms.CheckBox();
            this.controls = new System.Windows.Forms.Panel();
            this.stopSound = new System.Windows.Forms.Button();
            this.playSound = new System.Windows.Forms.Button();
            this.xwmaCompression = new System.Windows.Forms.ComboBox();
            this.compressXwma = new System.Windows.Forms.CheckBox();
            this.soundPosition = new System.Windows.Forms.TrackBar();
            this.volumeSlider = new System.Windows.Forms.TrackBar();
            this.openSoundstream = new System.Windows.Forms.Button();
            this.soundstreamPath = new System.Windows.Forms.TextBox();
            this.loadFromFolder = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.loadingControls = new System.Windows.Forms.Panel();
            this.soundTimer = new System.Windows.Forms.Timer(this.components);
            this.controls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.soundPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.volumeSlider)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.loadingControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // openSoundbank
            // 
            this.openSoundbank.Location = new System.Drawing.Point(0, 29);
            this.openSoundbank.Name = "openSoundbank";
            this.openSoundbank.Size = new System.Drawing.Size(149, 23);
            this.openSoundbank.TabIndex = 2;
            this.openSoundbank.Text = "Open soundbank.pck...";
            this.openSoundbank.UseVisualStyleBackColor = true;
            this.openSoundbank.Click += new System.EventHandler(this.openSoundbank_Click);
            // 
            // soundbankPath
            // 
            this.soundbankPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.soundbankPath.Location = new System.Drawing.Point(155, 31);
            this.soundbankPath.Name = "soundbankPath";
            this.soundbankPath.ReadOnly = true;
            this.soundbankPath.Size = new System.Drawing.Size(369, 20);
            this.soundbankPath.TabIndex = 3;
            // 
            // fileTree
            // 
            this.fileTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileTree.HideSelection = false;
            this.fileTree.ImageIndex = 0;
            this.fileTree.ImageList = this.nodeImages;
            this.fileTree.Location = new System.Drawing.Point(0, 0);
            this.fileTree.Name = "fileTree";
            this.fileTree.SelectedImageIndex = 0;
            this.fileTree.Size = new System.Drawing.Size(524, 199);
            this.fileTree.TabIndex = 4;
            this.fileTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.fileTree_AfterSelect);
            this.fileTree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.fileTree_NodeMouseDoubleClick);
            // 
            // nodeImages
            // 
            this.nodeImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("nodeImages.ImageStream")));
            this.nodeImages.TransparentColor = System.Drawing.Color.Transparent;
            this.nodeImages.Images.SetKeyName(0, "folder.png");
            this.nodeImages.Images.SetKeyName(1, "sound.png");
            // 
            // extractFile
            // 
            this.extractFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.extractFile.Enabled = false;
            this.extractFile.Location = new System.Drawing.Point(0, 234);
            this.extractFile.Name = "extractFile";
            this.extractFile.Size = new System.Drawing.Size(149, 23);
            this.extractFile.TabIndex = 5;
            this.extractFile.Text = "Extract Selected File...";
            this.extractFile.UseVisualStyleBackColor = true;
            this.extractFile.Click += new System.EventHandler(this.extractFile_Click);
            // 
            // extractAll
            // 
            this.extractAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.extractAll.Location = new System.Drawing.Point(155, 234);
            this.extractAll.Name = "extractAll";
            this.extractAll.Size = new System.Drawing.Size(149, 23);
            this.extractAll.TabIndex = 6;
            this.extractAll.Text = "Extract All Files...";
            this.extractAll.UseVisualStyleBackColor = true;
            this.extractAll.Click += new System.EventHandler(this.extractAll_Click);
            // 
            // convertFiles
            // 
            this.convertFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.convertFiles.AutoSize = true;
            this.convertFiles.Checked = true;
            this.convertFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.convertFiles.Location = new System.Drawing.Point(0, 263);
            this.convertFiles.Name = "convertFiles";
            this.convertFiles.Size = new System.Drawing.Size(157, 17);
            this.convertFiles.TabIndex = 7;
            this.convertFiles.Text = "Convert files after extraction";
            this.convertFiles.UseVisualStyleBackColor = true;
            this.convertFiles.CheckedChanged += new System.EventHandler(this.convertFiles_CheckedChanged);
            // 
            // controls
            // 
            this.controls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.controls.Controls.Add(this.stopSound);
            this.controls.Controls.Add(this.playSound);
            this.controls.Controls.Add(this.xwmaCompression);
            this.controls.Controls.Add(this.compressXwma);
            this.controls.Controls.Add(this.convertFiles);
            this.controls.Controls.Add(this.extractAll);
            this.controls.Controls.Add(this.extractFile);
            this.controls.Controls.Add(this.fileTree);
            this.controls.Controls.Add(this.soundPosition);
            this.controls.Controls.Add(this.volumeSlider);
            this.controls.Enabled = false;
            this.controls.Location = new System.Drawing.Point(12, 99);
            this.controls.Name = "controls";
            this.controls.Size = new System.Drawing.Size(524, 282);
            this.controls.TabIndex = 8;
            // 
            // stopSound
            // 
            this.stopSound.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.stopSound.Enabled = false;
            this.stopSound.Image = global::Composer.Properties.Resources.stop;
            this.stopSound.Location = new System.Drawing.Point(29, 205);
            this.stopSound.Name = "stopSound";
            this.stopSound.Size = new System.Drawing.Size(23, 23);
            this.stopSound.TabIndex = 11;
            this.stopSound.UseVisualStyleBackColor = true;
            this.stopSound.Click += new System.EventHandler(this.stopSound_Click);
            // 
            // playSound
            // 
            this.playSound.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.playSound.Enabled = false;
            this.playSound.Image = global::Composer.Properties.Resources.play;
            this.playSound.Location = new System.Drawing.Point(0, 205);
            this.playSound.Name = "playSound";
            this.playSound.Size = new System.Drawing.Size(23, 23);
            this.playSound.TabIndex = 10;
            this.playSound.UseVisualStyleBackColor = true;
            this.playSound.Click += new System.EventHandler(this.playSound_Click);
            // 
            // xwmaCompression
            // 
            this.xwmaCompression.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.xwmaCompression.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.xwmaCompression.FormattingEnabled = true;
            this.xwmaCompression.Items.AddRange(new object[] {
            "FLAC (lossless, medium size)",
            "MP3 (lossy, small size)"});
            this.xwmaCompression.Location = new System.Drawing.Point(300, 261);
            this.xwmaCompression.Name = "xwmaCompression";
            this.xwmaCompression.Size = new System.Drawing.Size(181, 21);
            this.xwmaCompression.TabIndex = 9;
            // 
            // compressXwma
            // 
            this.compressXwma.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.compressXwma.AutoSize = true;
            this.compressXwma.Checked = true;
            this.compressXwma.CheckState = System.Windows.Forms.CheckState.Checked;
            this.compressXwma.Location = new System.Drawing.Point(163, 263);
            this.compressXwma.Name = "compressXwma";
            this.compressXwma.Size = new System.Drawing.Size(131, 17);
            this.compressXwma.TabIndex = 8;
            this.compressXwma.Text = "Compress xWMA files:";
            this.compressXwma.UseVisualStyleBackColor = true;
            this.compressXwma.CheckedChanged += new System.EventHandler(this.compressXwma_CheckedChanged);
            // 
            // soundPosition
            // 
            this.soundPosition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.soundPosition.Enabled = false;
            this.soundPosition.Location = new System.Drawing.Point(58, 205);
            this.soundPosition.Name = "soundPosition";
            this.soundPosition.Size = new System.Drawing.Size(356, 45);
            this.soundPosition.TabIndex = 12;
            this.soundPosition.TickStyle = System.Windows.Forms.TickStyle.None;
            this.soundPosition.ValueChanged += new System.EventHandler(this.soundPosition_ValueChanged);
            // 
            // volumeSlider
            // 
            this.volumeSlider.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.volumeSlider.Location = new System.Drawing.Point(420, 205);
            this.volumeSlider.Maximum = 100;
            this.volumeSlider.Name = "volumeSlider";
            this.volumeSlider.Size = new System.Drawing.Size(104, 45);
            this.volumeSlider.TabIndex = 13;
            this.volumeSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.volumeSlider.Value = 100;
            this.volumeSlider.ValueChanged += new System.EventHandler(this.volumeSlider_ValueChanged);
            // 
            // openSoundstream
            // 
            this.openSoundstream.Location = new System.Drawing.Point(0, 58);
            this.openSoundstream.Name = "openSoundstream";
            this.openSoundstream.Size = new System.Drawing.Size(149, 23);
            this.openSoundstream.TabIndex = 9;
            this.openSoundstream.Text = "Open soundstream.pck...";
            this.openSoundstream.UseVisualStyleBackColor = true;
            this.openSoundstream.Click += new System.EventHandler(this.openSoundstream_Click);
            // 
            // soundstreamPath
            // 
            this.soundstreamPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.soundstreamPath.Location = new System.Drawing.Point(155, 60);
            this.soundstreamPath.Name = "soundstreamPath";
            this.soundstreamPath.ReadOnly = true;
            this.soundstreamPath.Size = new System.Drawing.Size(369, 20);
            this.soundstreamPath.TabIndex = 10;
            // 
            // loadFromFolder
            // 
            this.loadFromFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.loadFromFolder.Location = new System.Drawing.Point(0, 0);
            this.loadFromFolder.Name = "loadFromFolder";
            this.loadFromFolder.Size = new System.Drawing.Size(524, 23);
            this.loadFromFolder.TabIndex = 11;
            this.loadFromFolder.Text = "Load sound packs from folder...";
            this.loadFromFolder.UseVisualStyleBackColor = true;
            this.loadFromFolder.Click += new System.EventHandler(this.loadFromFolder_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.progressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 392);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(548, 22);
            this.statusStrip1.TabIndex = 12;
            this.statusStrip1.Text = "statusBar";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(381, 17);
            this.statusLabel.Spring = true;
            this.statusLabel.Text = "Ready";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // progressBar
            // 
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(150, 16);
            // 
            // loadingControls
            // 
            this.loadingControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.loadingControls.Controls.Add(this.loadFromFolder);
            this.loadingControls.Controls.Add(this.soundstreamPath);
            this.loadingControls.Controls.Add(this.openSoundstream);
            this.loadingControls.Controls.Add(this.soundbankPath);
            this.loadingControls.Controls.Add(this.openSoundbank);
            this.loadingControls.Location = new System.Drawing.Point(12, 12);
            this.loadingControls.Name = "loadingControls";
            this.loadingControls.Size = new System.Drawing.Size(524, 80);
            this.loadingControls.TabIndex = 13;
            // 
            // soundTimer
            // 
            this.soundTimer.Interval = 10;
            this.soundTimer.Tick += new System.EventHandler(this.soundTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 414);
            this.Controls.Add(this.loadingControls);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.controls);
            this.MinimumSize = new System.Drawing.Size(564, 405);
            this.Name = "MainForm";
            this.Text = "Composer - Halo 4 Audio Extractor";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.controls.ResumeLayout(false);
            this.controls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.soundPosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.volumeSlider)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.loadingControls.ResumeLayout(false);
            this.loadingControls.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button openSoundbank;
        private System.Windows.Forms.TextBox soundbankPath;
        private System.Windows.Forms.TreeView fileTree;
        private System.Windows.Forms.Button extractFile;
        private System.Windows.Forms.Button extractAll;
        private System.Windows.Forms.CheckBox convertFiles;
        private System.Windows.Forms.Panel controls;
        private System.Windows.Forms.ImageList nodeImages;
        private System.Windows.Forms.Button openSoundstream;
        private System.Windows.Forms.TextBox soundstreamPath;
        private System.Windows.Forms.Button loadFromFolder;
        private System.Windows.Forms.CheckBox compressXwma;
        private System.Windows.Forms.ComboBox xwmaCompression;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.Panel loadingControls;
        private System.Windows.Forms.Button stopSound;
        private System.Windows.Forms.Button playSound;
        private System.Windows.Forms.TrackBar soundPosition;
        private System.Windows.Forms.TrackBar volumeSlider;
        private System.Windows.Forms.Timer soundTimer;
    }
}

