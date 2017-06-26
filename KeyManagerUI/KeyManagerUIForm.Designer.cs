namespace KeyManagerUI
{
    partial class KeyManagerUIForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KeyManagerUIForm));
            this.load_key = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.change_key = new System.Windows.Forms.Button();
            this.import_key = new System.Windows.Forms.Button();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.exit_button = new System.Windows.Forms.Button();
            this.load_store_btn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.new_key = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // load_key
            // 
            this.load_key.Location = new System.Drawing.Point(12, 12);
            this.load_key.Name = "load_key";
            this.load_key.Size = new System.Drawing.Size(125, 23);
            this.load_key.TabIndex = 0;
            this.load_key.Text = "Load Key";
            this.load_key.UseVisualStyleBackColor = true;
            this.load_key.Click += new System.EventHandler(this.load_key_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "KeyFIle|*.p7mkey";
            // 
            // change_key
            // 
            this.change_key.Location = new System.Drawing.Point(490, 167);
            this.change_key.Name = "change_key";
            this.change_key.Size = new System.Drawing.Size(75, 23);
            this.change_key.TabIndex = 4;
            this.change_key.Text = "Save key";
            this.change_key.UseVisualStyleBackColor = true;
            this.change_key.Click += new System.EventHandler(this.change_click);
            // 
            // import_key
            // 
            this.import_key.Location = new System.Drawing.Point(11, 21);
            this.import_key.Name = "import_key";
            this.import_key.Size = new System.Drawing.Size(125, 23);
            this.import_key.TabIndex = 1;
            this.import_key.Text = "from file";
            this.import_key.UseVisualStyleBackColor = true;
            this.import_key.Click += new System.EventHandler(this.import_key_Click);
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.Filter = "CertFile|*.cer";
            // 
            // listView1
            // 
            this.listView1.CheckBoxes = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listView1.Location = new System.Drawing.Point(167, 40);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(398, 121);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Certificates";
            this.columnHeader1.Width = 390;
            // 
            // exit_button
            // 
            this.exit_button.Location = new System.Drawing.Point(12, 167);
            this.exit_button.Name = "exit_button";
            this.exit_button.Size = new System.Drawing.Size(75, 23);
            this.exit_button.TabIndex = 5;
            this.exit_button.Text = "Exit";
            this.exit_button.UseVisualStyleBackColor = true;
            this.exit_button.Click += new System.EventHandler(this.exit_button_click);
            // 
            // load_store_btn
            // 
            this.load_store_btn.Location = new System.Drawing.Point(11, 49);
            this.load_store_btn.Name = "load_store_btn";
            this.load_store_btn.Size = new System.Drawing.Size(125, 23);
            this.load_store_btn.TabIndex = 2;
            this.load_store_btn.Text = "from store";
            this.load_store_btn.UseVisualStyleBackColor = true;
            this.load_store_btn.Click += new System.EventHandler(this.load_store_btn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.import_key);
            this.groupBox1.Controls.Add(this.load_store_btn);
            this.groupBox1.Location = new System.Drawing.Point(12, 81);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(142, 80);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Load certifcates";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.CreatePrompt = true;
            this.saveFileDialog1.Filter = "KeyFIle|*.p7mkey";
            // 
            // new_key
            // 
            this.new_key.Location = new System.Drawing.Point(12, 40);
            this.new_key.Name = "new_key";
            this.new_key.Size = new System.Drawing.Size(125, 23);
            this.new_key.TabIndex = 8;
            this.new_key.Text = "New Key";
            this.new_key.UseVisualStyleBackColor = true;
            this.new_key.Click += new System.EventHandler(this.new_key_Click);
            // 
            // KeyManagerUIForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 202);
            this.Controls.Add(this.new_key);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.exit_button);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.change_key);
            this.Controls.Add(this.load_key);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KeyManagerUIForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "KeePass KeyManager v1.2b";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyManagerUIForm_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button load_key;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button change_key;
        private System.Windows.Forms.Button import_key;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button exit_button;
        private System.Windows.Forms.Button load_store_btn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button new_key;
    }
}