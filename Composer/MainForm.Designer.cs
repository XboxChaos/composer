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
            this.openPck = new System.Windows.Forms.Button();
            this.pckPath = new System.Windows.Forms.TextBox();
            this.fileTree = new System.Windows.Forms.TreeView();
            this.extractFile = new System.Windows.Forms.Button();
            this.extractAll = new System.Windows.Forms.Button();
            this.convertFiles = new System.Windows.Forms.CheckBox();
            this.controls = new System.Windows.Forms.Panel();
            this.nodeImages = new System.Windows.Forms.ImageList(this.components);
            this.controls.SuspendLayout();
            this.SuspendLayout();
            // 
            // openPck
            // 
            this.openPck.Location = new System.Drawing.Point(12, 12);
            this.openPck.Name = "openPck";
            this.openPck.Size = new System.Drawing.Size(124, 23);
            this.openPck.TabIndex = 2;
            this.openPck.Text = "Open .pck File...";
            this.openPck.UseVisualStyleBackColor = true;
            this.openPck.Click += new System.EventHandler(this.openPck_Click);
            // 
            // pckPath
            // 
            this.pckPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pckPath.Location = new System.Drawing.Point(142, 14);
            this.pckPath.Name = "pckPath";
            this.pckPath.ReadOnly = true;
            this.pckPath.Size = new System.Drawing.Size(394, 20);
            this.pckPath.TabIndex = 3;
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
            this.fileTree.Size = new System.Drawing.Size(524, 285);
            this.fileTree.TabIndex = 4;
            this.fileTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.fileTree_AfterSelect);
            // 
            // extractFile
            // 
            this.extractFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.extractFile.Enabled = false;
            this.extractFile.Location = new System.Drawing.Point(0, 291);
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
            this.extractAll.Location = new System.Drawing.Point(155, 291);
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
            this.convertFiles.Location = new System.Drawing.Point(367, 295);
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
            this.controls.Location = new System.Drawing.Point(12, 41);
            this.controls.Name = "controls";
            this.controls.Size = new System.Drawing.Size(524, 314);
            this.controls.TabIndex = 8;
            // 
            // nodeImages
            // 
            this.nodeImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("nodeImages.ImageStream")));
            this.nodeImages.TransparentColor = System.Drawing.Color.Transparent;
            this.nodeImages.Images.SetKeyName(0, "folder.png");
            this.nodeImages.Images.SetKeyName(1, "sound.png");
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 367);
            this.Controls.Add(this.pckPath);
            this.Controls.Add(this.controls);
            this.Controls.Add(this.openPck);
            this.MinimumSize = new System.Drawing.Size(564, 405);
            this.Name = "Form1";
            this.Text = "Halo 4 Audio Extractor";
            this.controls.ResumeLayout(false);
            this.controls.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button openPck;
        private System.Windows.Forms.TextBox pckPath;
        private System.Windows.Forms.TreeView fileTree;
        private System.Windows.Forms.Button extractFile;
        private System.Windows.Forms.Button extractAll;
        private System.Windows.Forms.CheckBox convertFiles;
        private System.Windows.Forms.Panel controls;
        private System.Windows.Forms.ImageList nodeImages;
    }
}

