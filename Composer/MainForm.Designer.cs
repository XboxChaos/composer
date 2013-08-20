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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.soundTimer = new System.Windows.Forms.Timer(this.components);
            this.loadingControls = new System.Windows.Forms.GroupBox();
            this.importPacks = new System.Windows.Forms.Button();
            this.unloadPacks = new System.Windows.Forms.Button();
            this.browsePack = new System.Windows.Forms.Button();
            this.packList = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.controls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.soundPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.volumeSlider)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.loadingControls.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileTree
            // 
            this.fileTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileTree.HideSelection = false;
            this.fileTree.ImageIndex = 0;
            this.fileTree.ImageList = this.nodeImages;
            this.fileTree.Location = new System.Drawing.Point(6, 19);
            this.fileTree.Name = "fileTree";
            this.fileTree.SelectedImageIndex = 0;
            this.fileTree.Size = new System.Drawing.Size(469, 241);
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
            this.extractFile.Location = new System.Drawing.Point(0, 301);
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
            this.extractAll.Location = new System.Drawing.Point(155, 301);
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
            this.convertFiles.Location = new System.Drawing.Point(0, 330);
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
            this.controls.Controls.Add(this.groupBox1);
            this.controls.Controls.Add(this.stopSound);
            this.controls.Controls.Add(this.playSound);
            this.controls.Controls.Add(this.xwmaCompression);
            this.controls.Controls.Add(this.compressXwma);
            this.controls.Controls.Add(this.convertFiles);
            this.controls.Controls.Add(this.extractAll);
            this.controls.Controls.Add(this.extractFile);
            this.controls.Controls.Add(this.soundPosition);
            this.controls.Controls.Add(this.volumeSlider);
            this.controls.Enabled = false;
            this.controls.Location = new System.Drawing.Point(193, 12);
            this.controls.Name = "controls";
            this.controls.Size = new System.Drawing.Size(481, 349);
            this.controls.TabIndex = 8;
            // 
            // stopSound
            // 
            this.stopSound.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.stopSound.Enabled = false;
            this.stopSound.Image = global::Composer.Properties.Resources.stop;
            this.stopSound.Location = new System.Drawing.Point(29, 272);
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
            this.playSound.Location = new System.Drawing.Point(0, 272);
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
            this.xwmaCompression.Location = new System.Drawing.Point(300, 328);
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
            this.compressXwma.Location = new System.Drawing.Point(163, 330);
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
            this.soundPosition.Location = new System.Drawing.Point(58, 272);
            this.soundPosition.Name = "soundPosition";
            this.soundPosition.Size = new System.Drawing.Size(313, 45);
            this.soundPosition.TabIndex = 12;
            this.soundPosition.TickStyle = System.Windows.Forms.TickStyle.None;
            this.soundPosition.Scroll += new System.EventHandler(this.soundPosition_Scroll);
            // 
            // volumeSlider
            // 
            this.volumeSlider.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.volumeSlider.Location = new System.Drawing.Point(377, 272);
            this.volumeSlider.Maximum = 100;
            this.volumeSlider.Name = "volumeSlider";
            this.volumeSlider.Size = new System.Drawing.Size(104, 45);
            this.volumeSlider.TabIndex = 13;
            this.volumeSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.volumeSlider.Value = 100;
            this.volumeSlider.ValueChanged += new System.EventHandler(this.volumeSlider_ValueChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.progressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 372);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(686, 22);
            this.statusStrip1.TabIndex = 12;
            this.statusStrip1.Text = "statusBar";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(629, 17);
            this.statusLabel.Spring = true;
            this.statusLabel.Text = "Ready";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // progressBar
            // 
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(150, 16);
            // 
            // soundTimer
            // 
            this.soundTimer.Interval = 10;
            this.soundTimer.Tick += new System.EventHandler(this.soundTimer_Tick);
            // 
            // loadingControls
            // 
            this.loadingControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.loadingControls.Controls.Add(this.importPacks);
            this.loadingControls.Controls.Add(this.unloadPacks);
            this.loadingControls.Controls.Add(this.browsePack);
            this.loadingControls.Controls.Add(this.packList);
            this.loadingControls.Location = new System.Drawing.Point(12, 12);
            this.loadingControls.Name = "loadingControls";
            this.loadingControls.Size = new System.Drawing.Size(175, 349);
            this.loadingControls.TabIndex = 13;
            this.loadingControls.TabStop = false;
            this.loadingControls.Text = "Sound Packs";
            // 
            // importPacks
            // 
            this.importPacks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.importPacks.Location = new System.Drawing.Point(6, 320);
            this.importPacks.Name = "importPacks";
            this.importPacks.Size = new System.Drawing.Size(163, 23);
            this.importPacks.TabIndex = 3;
            this.importPacks.Text = "Import From Folder...";
            this.importPacks.UseVisualStyleBackColor = true;
            this.importPacks.Click += new System.EventHandler(this.importPacks_Click);
            // 
            // unloadPacks
            // 
            this.unloadPacks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.unloadPacks.Location = new System.Drawing.Point(91, 291);
            this.unloadPacks.Name = "unloadPacks";
            this.unloadPacks.Size = new System.Drawing.Size(78, 23);
            this.unloadPacks.TabIndex = 2;
            this.unloadPacks.Text = "Unload All";
            this.unloadPacks.UseVisualStyleBackColor = true;
            this.unloadPacks.Click += new System.EventHandler(this.unloadPacks_Click);
            // 
            // browsePack
            // 
            this.browsePack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.browsePack.Location = new System.Drawing.Point(6, 291);
            this.browsePack.Name = "browsePack";
            this.browsePack.Size = new System.Drawing.Size(79, 23);
            this.browsePack.TabIndex = 1;
            this.browsePack.Text = "Browse...";
            this.browsePack.UseVisualStyleBackColor = true;
            this.browsePack.Click += new System.EventHandler(this.browsePack_Click);
            // 
            // packList
            // 
            this.packList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.packList.FormattingEnabled = true;
            this.packList.IntegralHeight = false;
            this.packList.Location = new System.Drawing.Point(6, 19);
            this.packList.Name = "packList";
            this.packList.Size = new System.Drawing.Size(163, 266);
            this.packList.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.fileTree);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(481, 266);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Discovered Sounds";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 394);
            this.Controls.Add(this.loadingControls);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.controls);
            this.MinimumSize = new System.Drawing.Size(702, 236);
            this.Name = "MainForm";
            this.Text = "Composer - Halo 4 Audio Extractor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.controls.ResumeLayout(false);
            this.controls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.soundPosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.volumeSlider)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.loadingControls.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView fileTree;
        private System.Windows.Forms.Button extractFile;
        private System.Windows.Forms.Button extractAll;
        private System.Windows.Forms.CheckBox convertFiles;
        private System.Windows.Forms.Panel controls;
        private System.Windows.Forms.ImageList nodeImages;
        private System.Windows.Forms.CheckBox compressXwma;
        private System.Windows.Forms.ComboBox xwmaCompression;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.Button stopSound;
        private System.Windows.Forms.Button playSound;
        private System.Windows.Forms.TrackBar soundPosition;
        private System.Windows.Forms.TrackBar volumeSlider;
        private System.Windows.Forms.Timer soundTimer;
        private System.Windows.Forms.GroupBox loadingControls;
        private System.Windows.Forms.Button importPacks;
        private System.Windows.Forms.Button unloadPacks;
        private System.Windows.Forms.Button browsePack;
        private System.Windows.Forms.ListBox packList;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

