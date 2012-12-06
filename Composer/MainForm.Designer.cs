namespace Composer
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

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
            this.openSoundstream = new System.Windows.Forms.Button();
            this.soundstreamPath = new System.Windows.Forms.TextBox();
            this.loadFromFolder = new System.Windows.Forms.Button();
            this.controls.SuspendLayout();
            this.SuspendLayout();
            // 
            // openSoundbank
            // 
            this.openSoundbank.Location = new System.Drawing.Point(12, 41);
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
            this.soundbankPath.Location = new System.Drawing.Point(167, 43);
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
            this.fileTree.Size = new System.Drawing.Size(524, 257);
            this.fileTree.TabIndex = 4;
            this.fileTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.fileTree_AfterSelect);
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
            this.extractFile.Location = new System.Drawing.Point(0, 263);
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
            this.extractAll.Location = new System.Drawing.Point(155, 263);
            this.extractAll.Name = "extractAll";
            this.extractAll.Size = new System.Drawing.Size(149, 23);
            this.extractAll.TabIndex = 6;
            this.extractAll.Text = "Extract All Files...";
            this.extractAll.UseVisualStyleBackColor = true;
            this.extractAll.Click += new System.EventHandler(this.extractAll_Click);
            // 
            // convertFiles
            // 
            this.convertFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.convertFiles.AutoSize = true;
            this.convertFiles.Checked = true;
            this.convertFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.convertFiles.Location = new System.Drawing.Point(367, 267);
            this.convertFiles.Margin = new System.Windows.Forms.Padding(0);
            this.convertFiles.Name = "convertFiles";
            this.convertFiles.Size = new System.Drawing.Size(157, 17);
            this.convertFiles.TabIndex = 7;
            this.convertFiles.Text = "Convert files after extraction";
            this.convertFiles.UseVisualStyleBackColor = true;
            // 
            // controls
            // 
            this.controls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.controls.Controls.Add(this.convertFiles);
            this.controls.Controls.Add(this.extractAll);
            this.controls.Controls.Add(this.extractFile);
            this.controls.Controls.Add(this.fileTree);
            this.controls.Enabled = false;
            this.controls.Location = new System.Drawing.Point(12, 99);
            this.controls.Name = "controls";
            this.controls.Size = new System.Drawing.Size(524, 286);
            this.controls.TabIndex = 8;
            // 
            // openSoundstream
            // 
            this.openSoundstream.Location = new System.Drawing.Point(12, 70);
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
            this.soundstreamPath.Location = new System.Drawing.Point(167, 72);
            this.soundstreamPath.Name = "soundstreamPath";
            this.soundstreamPath.ReadOnly = true;
            this.soundstreamPath.Size = new System.Drawing.Size(369, 20);
            this.soundstreamPath.TabIndex = 10;
            // 
            // loadFromFolder
            // 
            this.loadFromFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.loadFromFolder.Location = new System.Drawing.Point(12, 12);
            this.loadFromFolder.Name = "loadFromFolder";
            this.loadFromFolder.Size = new System.Drawing.Size(524, 23);
            this.loadFromFolder.TabIndex = 11;
            this.loadFromFolder.Text = "Load sound packs from folder...";
            this.loadFromFolder.UseVisualStyleBackColor = true;
            this.loadFromFolder.Click += new System.EventHandler(this.loadFromFolder_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 397);
            this.Controls.Add(this.loadFromFolder);
            this.Controls.Add(this.soundstreamPath);
            this.Controls.Add(this.openSoundstream);
            this.Controls.Add(this.soundbankPath);
            this.Controls.Add(this.controls);
            this.Controls.Add(this.openSoundbank);
            this.MinimumSize = new System.Drawing.Size(564, 405);
            this.Name = "MainForm";
            this.Text = "Composer";
            this.controls.ResumeLayout(false);
            this.controls.PerformLayout();
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
    }
}

