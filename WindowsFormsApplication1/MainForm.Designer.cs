namespace TemperatureFetcher
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
            this.label2 = new System.Windows.Forms.Label();
            this.exportButton = new System.Windows.Forms.Button();
            this.dataViewer = new System.Windows.Forms.ListView();
            this.recordIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.typeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cityColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.countryColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.departColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.arrivColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.dureeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tMinDepColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tMaxDepColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tMinArrColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tMaxArrColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.minTempIntervalTextBox = new System.Windows.Forms.TextBox();
            this.maxTempIntervalTextBox = new System.Windows.Forms.TextBox();
            this.minTempIntervalLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.processButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.keyTextBox = new System.Windows.Forms.TextBox();
            this.fetchProgressBar = new System.Windows.Forms.ProgressBar();
            this.cancelFetchButton = new System.Windows.Forms.Button();
            this.waitingLabel = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFile_button = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.processedRowLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 26);
            this.label2.TabIndex = 14;
            // 
            // exportButton
            // 
            this.exportButton.Location = new System.Drawing.Point(40, 1010);
            this.exportButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(164, 61);
            this.exportButton.TabIndex = 2;
            this.exportButton.Text = "Export";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
            // 
            // dataViewer
            // 
            this.dataViewer.BackColor = System.Drawing.SystemColors.Window;
            this.dataViewer.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.recordIndex,
            this.typeColumn,
            this.cityColumn,
            this.countryColumn,
            this.departColumn,
            this.arrivColumn,
            this.dureeColumn,
            this.tMinDepColumn,
            this.tMaxDepColumn,
            this.tMinArrColumn,
            this.tMaxArrColumn});
            this.dataViewer.GridLines = true;
            this.dataViewer.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.dataViewer.Location = new System.Drawing.Point(1503, 293);
            this.dataViewer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataViewer.Name = "dataViewer";
            this.dataViewer.Size = new System.Drawing.Size(145, 130);
            this.dataViewer.TabIndex = 3;
            this.dataViewer.UseCompatibleStateImageBehavior = false;
            this.dataViewer.View = System.Windows.Forms.View.Details;
            this.dataViewer.Visible = false;
            // 
            // recordIndex
            // 
            this.recordIndex.Text = "N° étape";
            this.recordIndex.Width = 50;
            // 
            // typeColumn
            // 
            this.typeColumn.Text = "Type";
            // 
            // cityColumn
            // 
            this.cityColumn.Text = "Ville";
            // 
            // countryColumn
            // 
            this.countryColumn.Text = "Pays";
            // 
            // departColumn
            // 
            this.departColumn.Text = "Départ";
            this.departColumn.Width = 69;
            // 
            // arrivColumn
            // 
            this.arrivColumn.Text = "Arrivée";
            this.arrivColumn.Width = 50;
            // 
            // dureeColumn
            // 
            this.dureeColumn.Text = "Duree";
            // 
            // tMinDepColumn
            // 
            this.tMinDepColumn.Text = "T Moy. Min. départ";
            // 
            // tMaxDepColumn
            // 
            this.tMaxDepColumn.Text = "T Moy. Max. départ";
            // 
            // tMinArrColumn
            // 
            this.tMinArrColumn.Text = "T Moy. Min.  arrivée";
            // 
            // tMaxArrColumn
            // 
            this.tMaxArrColumn.Text = "T Moy. Max. arrivée";
            this.tMaxArrColumn.Width = 0;
            // 
            // minTempIntervalTextBox
            // 
            this.minTempIntervalTextBox.Location = new System.Drawing.Point(128, 2);
            this.minTempIntervalTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.minTempIntervalTextBox.Name = "minTempIntervalTextBox";
            this.minTempIntervalTextBox.Size = new System.Drawing.Size(100, 31);
            this.minTempIntervalTextBox.TabIndex = 5;
            // 
            // maxTempIntervalTextBox
            // 
            this.maxTempIntervalTextBox.Location = new System.Drawing.Point(128, 37);
            this.maxTempIntervalTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.maxTempIntervalTextBox.Name = "maxTempIntervalTextBox";
            this.maxTempIntervalTextBox.Size = new System.Drawing.Size(100, 31);
            this.maxTempIntervalTextBox.TabIndex = 6;
            // 
            // minTempIntervalLabel
            // 
            this.minTempIntervalLabel.AutoSize = true;
            this.minTempIntervalLabel.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.minTempIntervalLabel.Location = new System.Drawing.Point(3, 0);
            this.minTempIntervalLabel.Name = "minTempIntervalLabel";
            this.minTempIntervalLabel.Size = new System.Drawing.Size(47, 26);
            this.minTempIntervalLabel.TabIndex = 7;
            this.minTempIntervalLabel.Text = "Min";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.maxTempIntervalTextBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.minTempIntervalLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.minTempIntervalTextBox, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(28, 52);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(251, 71);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 26);
            this.label4.TabIndex = 12;
            this.label4.Text = "Max";
            // 
            // processButton
            // 
            this.processButton.Location = new System.Drawing.Point(40, 355);
            this.processButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.processButton.Name = "processButton";
            this.processButton.Size = new System.Drawing.Size(164, 58);
            this.processButton.TabIndex = 12;
            this.processButton.Text = "Process";
            this.processButton.UseVisualStyleBackColor = true;
            this.processButton.Click += new System.EventHandler(this.processButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Location = new System.Drawing.Point(1337, 36);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(351, 144);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Set required temperature interval";
            this.groupBox1.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1498, 202);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 26);
            this.label5.TabIndex = 16;
            this.label5.Text = "Key";
            this.label5.Visible = false;
            // 
            // keyTextBox
            // 
            this.keyTextBox.Location = new System.Drawing.Point(1554, 202);
            this.keyTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.keyTextBox.Name = "keyTextBox";
            this.keyTextBox.Size = new System.Drawing.Size(145, 31);
            this.keyTextBox.TabIndex = 17;
            this.keyTextBox.Visible = false;
            // 
            // fetchProgressBar
            // 
            this.fetchProgressBar.Location = new System.Drawing.Point(257, 1049);
            this.fetchProgressBar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.fetchProgressBar.Name = "fetchProgressBar";
            this.fetchProgressBar.Size = new System.Drawing.Size(1180, 22);
            this.fetchProgressBar.TabIndex = 18;
            this.fetchProgressBar.Visible = false;
            // 
            // cancelFetchButton
            // 
            this.cancelFetchButton.Location = new System.Drawing.Point(1463, 1037);
            this.cancelFetchButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cancelFetchButton.Name = "cancelFetchButton";
            this.cancelFetchButton.Size = new System.Drawing.Size(101, 46);
            this.cancelFetchButton.TabIndex = 19;
            this.cancelFetchButton.Text = "Cancel";
            this.cancelFetchButton.UseVisualStyleBackColor = true;
            this.cancelFetchButton.Visible = false;
            this.cancelFetchButton.Click += new System.EventHandler(this.cancelFetchButton_Click);
            // 
            // waitingLabel
            // 
            this.waitingLabel.AutoSize = true;
            this.waitingLabel.Location = new System.Drawing.Point(977, 14);
            this.waitingLabel.Name = "waitingLabel";
            this.waitingLabel.Size = new System.Drawing.Size(133, 26);
            this.waitingLabel.TabIndex = 20;
            this.waitingLabel.Text = "waitingLabel";
            this.waitingLabel.Visible = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(257, 61);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 33;
            this.dataGridView1.Size = new System.Drawing.Size(1307, 943);
            this.dataGridView1.TabIndex = 22;
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1700, 24);
            this.menuStrip1.TabIndex = 23;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(115, 39);
            this.optionsToolStripMenuItem.Text = "Options";
            this.optionsToolStripMenuItem.Visible = false;
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // openFile_button
            // 
            this.openFile_button.Location = new System.Drawing.Point(40, 100);
            this.openFile_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.openFile_button.Name = "openFile_button";
            this.openFile_button.Size = new System.Drawing.Size(164, 56);
            this.openFile_button.TabIndex = 0;
            this.openFile_button.Text = "Choose file";
            this.openFile_button.UseVisualStyleBackColor = true;
            this.openFile_button.Click += new System.EventHandler(this.openFileButton_Click);
            // 
            // processedRowLabel
            // 
            this.processedRowLabel.AutoSize = true;
            this.processedRowLabel.Location = new System.Drawing.Point(3, 14);
            this.processedRowLabel.Name = "processedRowLabel";
            this.processedRowLabel.Size = new System.Drawing.Size(209, 26);
            this.processedRowLabel.TabIndex = 24;
            this.processedRowLabel.Text = "processedRowLabel";
            this.processedRowLabel.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.processedRowLabel);
            this.panel1.Controls.Add(this.waitingLabel);
            this.panel1.Location = new System.Drawing.Point(257, 1098);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1307, 53);
            this.panel1.TabIndex = 25;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1700, 1305);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.cancelFetchButton);
            this.Controls.Add(this.fetchProgressBar);
            this.Controls.Add(this.keyTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.processButton);
            this.Controls.Add(this.dataViewer);
            this.Controls.Add(this.exportButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.openFile_button);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.Text = "TemperatureFetcher";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button openFile_button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.ListView dataViewer;
        private System.Windows.Forms.ColumnHeader departColumn;
        private System.Windows.Forms.TextBox minTempIntervalTextBox;
        private System.Windows.Forms.TextBox maxTempIntervalTextBox;
        private System.Windows.Forms.Label minTempIntervalLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button processButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ColumnHeader recordIndex;
        private System.Windows.Forms.ColumnHeader arrivColumn;
        private System.Windows.Forms.ColumnHeader typeColumn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox keyTextBox;
        private System.Windows.Forms.ProgressBar fetchProgressBar;
        private System.Windows.Forms.Button cancelFetchButton;
        private System.Windows.Forms.Label waitingLabel;
        private System.Windows.Forms.ColumnHeader dureeColumn;
        private System.Windows.Forms.ColumnHeader tMinDepColumn;
        private System.Windows.Forms.ColumnHeader tMaxDepColumn;
        private System.Windows.Forms.ColumnHeader cityColumn;
        private System.Windows.Forms.ColumnHeader countryColumn;
        private System.Windows.Forms.ColumnHeader tMinArrColumn;
        private System.Windows.Forms.ColumnHeader tMaxArrColumn;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label processedRowLabel;
        private System.Windows.Forms.Panel panel1;
    }
}

