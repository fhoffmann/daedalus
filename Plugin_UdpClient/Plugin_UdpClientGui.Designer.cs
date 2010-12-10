namespace Plugin_UdpClient
{
    partial class Plugin_UdpClientGui
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
            this.textBox_msg = new System.Windows.Forms.TextBox();
            this.textBox_srv = new System.Windows.Forms.TextBox();
            this.textBox_port = new System.Windows.Forms.TextBox();
            this.button_send = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox_msg
            // 
            this.textBox_msg.Location = new System.Drawing.Point(12, 11);
            this.textBox_msg.Name = "textBox_msg";
            this.textBox_msg.Size = new System.Drawing.Size(248, 20);
            this.textBox_msg.TabIndex = 0;
            this.textBox_msg.Text = "Eine Nachricht";
            // 
            // textBox_srv
            // 
            this.textBox_srv.Location = new System.Drawing.Point(12, 37);
            this.textBox_srv.Name = "textBox_srv";
            this.textBox_srv.Size = new System.Drawing.Size(182, 20);
            this.textBox_srv.TabIndex = 1;
            this.textBox_srv.Text = "127.0.0.1";
            // 
            // textBox_port
            // 
            this.textBox_port.Location = new System.Drawing.Point(200, 37);
            this.textBox_port.Name = "textBox_port";
            this.textBox_port.Size = new System.Drawing.Size(60, 20);
            this.textBox_port.TabIndex = 1;
            this.textBox_port.Text = "514";
            // 
            // button_send
            // 
            this.button_send.Location = new System.Drawing.Point(185, 63);
            this.button_send.Name = "button_send";
            this.button_send.Size = new System.Drawing.Size(75, 23);
            this.button_send.TabIndex = 2;
            this.button_send.Text = "Senden";
            this.button_send.UseVisualStyleBackColor = true;
            this.button_send.Click += new System.EventHandler(this.button_send_Click);
            // 
            // Plugin_UdpClientGui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(271, 97);
            this.Controls.Add(this.button_send);
            this.Controls.Add(this.textBox_port);
            this.Controls.Add(this.textBox_srv);
            this.Controls.Add(this.textBox_msg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Plugin_UdpClientGui";
            this.Text = "Plugin_UdpClientGui";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_msg;
        private System.Windows.Forms.TextBox textBox_srv;
        private System.Windows.Forms.TextBox textBox_port;
        private System.Windows.Forms.Button button_send;
    }
}