namespace Orb
{
    //public static partial class GUI
    //{
        public partial class ServerConsole
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
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerConsole));
                this.ConsoleInput = new System.Windows.Forms.TextBox();
                this.ConsoleOutput = new System.Windows.Forms.RichTextBox();
                this.ConsoleUserList = new System.Windows.Forms.RichTextBox();
                this.SuspendLayout();
                // 
                // ConsoleInput
                // 
                this.ConsoleInput.BackColor = System.Drawing.Color.Black;
                this.ConsoleInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                this.ConsoleInput.Dock = System.Windows.Forms.DockStyle.Bottom;
                this.ConsoleInput.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.ConsoleInput.ForeColor = System.Drawing.Color.White;
                this.ConsoleInput.Location = new System.Drawing.Point(0, 514);
                this.ConsoleInput.Margin = new System.Windows.Forms.Padding(0);
                this.ConsoleInput.Name = "ConsoleInput";
                this.ConsoleInput.Size = new System.Drawing.Size(1095, 20);
                this.ConsoleInput.TabIndex = 1;
                this.ConsoleInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SubmitLine);
                this.ConsoleInput.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.ArrowKeyOverrides);
                // 
                // ConsoleOutput
                // 
                this.ConsoleOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                            | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                this.ConsoleOutput.BackColor = System.Drawing.Color.Black;
                this.ConsoleOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
                this.ConsoleOutput.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.ConsoleOutput.ForeColor = System.Drawing.Color.White;
                this.ConsoleOutput.Location = new System.Drawing.Point(0, 0);
                this.ConsoleOutput.Margin = new System.Windows.Forms.Padding(0);
                this.ConsoleOutput.Name = "ConsoleOutput";
                this.ConsoleOutput.ReadOnly = true;
                this.ConsoleOutput.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
                this.ConsoleOutput.Size = new System.Drawing.Size(953, 514);
                this.ConsoleOutput.TabIndex = 2;
                this.ConsoleOutput.TabStop = false;
                this.ConsoleOutput.Text = "";
                this.ConsoleOutput.TextChanged += new System.EventHandler(this.TextChangedActions);
                // 
                // ConsoleUserList
                // 
                this.ConsoleUserList.BackColor = System.Drawing.Color.Black;
                this.ConsoleUserList.BorderStyle = System.Windows.Forms.BorderStyle.None;
                this.ConsoleUserList.Dock = System.Windows.Forms.DockStyle.Right;
                this.ConsoleUserList.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.ConsoleUserList.ForeColor = System.Drawing.Color.White;
                this.ConsoleUserList.Location = new System.Drawing.Point(953, 0);
                this.ConsoleUserList.Margin = new System.Windows.Forms.Padding(0);
                this.ConsoleUserList.Name = "ConsoleUserList";
                this.ConsoleUserList.ReadOnly = true;
                this.ConsoleUserList.Size = new System.Drawing.Size(142, 514);
                this.ConsoleUserList.TabIndex = 3;
                this.ConsoleUserList.TabStop = false;
                this.ConsoleUserList.Text = "";
                // 
                // ServerConsole
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.BackColor = System.Drawing.Color.White;
                this.ClientSize = new System.Drawing.Size(1095, 534);
                this.Controls.Add(this.ConsoleUserList);
                this.Controls.Add(this.ConsoleOutput);
                this.Controls.Add(this.ConsoleInput);
                this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
                this.Name = "ServerConsole";
                this.Text = "Orb for YSFlight - Console";
                this.Load += new System.EventHandler(this.ResumeMainThread);
                this.ResumeLayout(false);
                this.PerformLayout();

            }

            #endregion

            public System.Windows.Forms.TextBox ConsoleInput;
            public System.Windows.Forms.RichTextBox ConsoleOutput;
            public System.Windows.Forms.RichTextBox ConsoleUserList;

        }
    //}
}

