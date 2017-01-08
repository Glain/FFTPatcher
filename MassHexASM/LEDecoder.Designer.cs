/*
 * Created by SharpDevelop.
 * User: Glain
 * Date: 9/17/2011
 * Time: 14:13
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace LEDecoder
{
    partial class LEDecoder
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Disposes resources used by the form.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbl_InputFile = new System.Windows.Forms.Label();
            this.lbl_OutputFile = new System.Windows.Forms.Label();
            this.txt_InputFile = new System.Windows.Forms.TextBox();
            this.txt_OutputFile = new System.Windows.Forms.TextBox();
            this.btn_InputFile = new System.Windows.Forms.Button();
            this.btn_OutputFile = new System.Windows.Forms.Button();
            this.btn_Process = new System.Windows.Forms.Button();
            this.chk_LittleEndian = new System.Windows.Forms.CheckBox();
            this.pic_LED = new System.Windows.Forms.PictureBox();
            this.lbl_Overwrite = new System.Windows.Forms.Label();
            this.txt_StartingAddress = new System.Windows.Forms.TextBox();
            this.lbl_StartingAddress = new System.Windows.Forms.Label();
            this.chk_NameRegisters = new System.Windows.Forms.CheckBox();
            this.cb_Mode = new System.Windows.Forms.ComboBox();
            this.lbl_Mode = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pic_LED)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_InputFile
            // 
            this.lbl_InputFile.Location = new System.Drawing.Point(28, 52);
            this.lbl_InputFile.Name = "lbl_InputFile";
            this.lbl_InputFile.Size = new System.Drawing.Size(114, 22);
            this.lbl_InputFile.TabIndex = 0;
            this.lbl_InputFile.Text = "Input File:";
            // 
            // lbl_OutputFile
            // 
            this.lbl_OutputFile.Location = new System.Drawing.Point(28, 107);
            this.lbl_OutputFile.Name = "lbl_OutputFile";
            this.lbl_OutputFile.Size = new System.Drawing.Size(114, 22);
            this.lbl_OutputFile.TabIndex = 1;
            this.lbl_OutputFile.Text = "Output File:";
            // 
            // txt_InputFile
            // 
            this.txt_InputFile.Location = new System.Drawing.Point(120, 49);
            this.txt_InputFile.Name = "txt_InputFile";
            this.txt_InputFile.Size = new System.Drawing.Size(291, 20);
            this.txt_InputFile.TabIndex = 2;
            // 
            // txt_OutputFile
            // 
            this.txt_OutputFile.Location = new System.Drawing.Point(120, 109);
            this.txt_OutputFile.Name = "txt_OutputFile";
            this.txt_OutputFile.Size = new System.Drawing.Size(291, 20);
            this.txt_OutputFile.TabIndex = 3;
            // 
            // btn_InputFile
            // 
            this.btn_InputFile.Location = new System.Drawing.Point(424, 47);
            this.btn_InputFile.Name = "btn_InputFile";
            this.btn_InputFile.Size = new System.Drawing.Size(78, 22);
            this.btn_InputFile.TabIndex = 4;
            this.btn_InputFile.Text = "Browse...";
            this.btn_InputFile.UseVisualStyleBackColor = true;
            this.btn_InputFile.Click += new System.EventHandler(this.Btn_InputFileClick);
            // 
            // btn_OutputFile
            // 
            this.btn_OutputFile.Location = new System.Drawing.Point(424, 109);
            this.btn_OutputFile.Name = "btn_OutputFile";
            this.btn_OutputFile.Size = new System.Drawing.Size(78, 22);
            this.btn_OutputFile.TabIndex = 5;
            this.btn_OutputFile.Text = "Browse...";
            this.btn_OutputFile.UseVisualStyleBackColor = true;
            this.btn_OutputFile.Click += new System.EventHandler(this.Btn_OutputFileClick);
            // 
            // btn_Process
            // 
            this.btn_Process.Location = new System.Drawing.Point(467, 249);
            this.btn_Process.Name = "btn_Process";
            this.btn_Process.Size = new System.Drawing.Size(85, 30);
            this.btn_Process.TabIndex = 6;
            this.btn_Process.Text = "Process";
            this.btn_Process.UseVisualStyleBackColor = true;
            this.btn_Process.Click += new System.EventHandler(this.Btn_ProcessClick);
            // 
            // chk_LittleEndian
            // 
            this.chk_LittleEndian.Location = new System.Drawing.Point(467, 184);
            this.chk_LittleEndian.Name = "chk_LittleEndian";
            this.chk_LittleEndian.Size = new System.Drawing.Size(89, 27);
            this.chk_LittleEndian.TabIndex = 7;
            this.chk_LittleEndian.Text = "Little Endian";
            this.chk_LittleEndian.UseVisualStyleBackColor = true;
            // 
            // pic_LED
            // 
            this.pic_LED.Location = new System.Drawing.Point(424, 249);
            this.pic_LED.Name = "pic_LED";
            this.pic_LED.Size = new System.Drawing.Size(35, 30);
            this.pic_LED.TabIndex = 8;
            this.pic_LED.TabStop = false;
            this.pic_LED.Paint += new System.Windows.Forms.PaintEventHandler(this.pic_LEDPaint);
            // 
            // lbl_Overwrite
            // 
            this.lbl_Overwrite.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Overwrite.Location = new System.Drawing.Point(120, 132);
            this.lbl_Overwrite.Name = "lbl_Overwrite";
            this.lbl_Overwrite.Size = new System.Drawing.Size(233, 24);
            this.lbl_Overwrite.TabIndex = 9;
            this.lbl_Overwrite.Text = "(Output file will be overwritten)";
            // 
            // txt_StartingAddress
            // 
            this.txt_StartingAddress.Location = new System.Drawing.Point(120, 184);
            this.txt_StartingAddress.Name = "txt_StartingAddress";
            this.txt_StartingAddress.Size = new System.Drawing.Size(76, 20);
            this.txt_StartingAddress.TabIndex = 10;
            // 
            // lbl_StartingAddress
            // 
            this.lbl_StartingAddress.AutoSize = true;
            this.lbl_StartingAddress.Location = new System.Drawing.Point(28, 184);
            this.lbl_StartingAddress.Name = "lbl_StartingAddress";
            this.lbl_StartingAddress.Size = new System.Drawing.Size(87, 13);
            this.lbl_StartingAddress.TabIndex = 11;
            this.lbl_StartingAddress.Text = "Starting Address:";
            // 
            // chk_NameRegisters
            // 
            this.chk_NameRegisters.AutoSize = true;
            this.chk_NameRegisters.Location = new System.Drawing.Point(467, 215);
            this.chk_NameRegisters.Name = "chk_NameRegisters";
            this.chk_NameRegisters.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.chk_NameRegisters.Size = new System.Drawing.Size(101, 17);
            this.chk_NameRegisters.TabIndex = 12;
            this.chk_NameRegisters.Text = "Name Registers";
            this.chk_NameRegisters.UseVisualStyleBackColor = true;
            // 
            // cb_Mode
            // 
            this.cb_Mode.FormattingEnabled = true;
            this.cb_Mode.Location = new System.Drawing.Point(120, 215);
            this.cb_Mode.Name = "cb_Mode";
            this.cb_Mode.Size = new System.Drawing.Size(65, 21);
            this.cb_Mode.TabIndex = 13;
            this.cb_Mode.SelectedIndexChanged += new System.EventHandler(this.cb_Mode_SelectedIndexChanged);
            // 
            // lbl_Mode
            // 
            this.lbl_Mode.AutoSize = true;
            this.lbl_Mode.Location = new System.Drawing.Point(76, 219);
            this.lbl_Mode.Name = "lbl_Mode";
            this.lbl_Mode.Size = new System.Drawing.Size(37, 13);
            this.lbl_Mode.TabIndex = 14;
            this.lbl_Mode.Text = "Mode:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 308);
            this.Controls.Add(this.lbl_Mode);
            this.Controls.Add(this.cb_Mode);
            this.Controls.Add(this.chk_NameRegisters);
            this.Controls.Add(this.lbl_StartingAddress);
            this.Controls.Add(this.txt_StartingAddress);
            this.Controls.Add(this.lbl_Overwrite);
            this.Controls.Add(this.pic_LED);
            this.Controls.Add(this.chk_LittleEndian);
            this.Controls.Add(this.btn_Process);
            this.Controls.Add(this.btn_OutputFile);
            this.Controls.Add(this.btn_InputFile);
            this.Controls.Add(this.txt_OutputFile);
            this.Controls.Add(this.txt_InputFile);
            this.Controls.Add(this.lbl_OutputFile);
            this.Controls.Add(this.lbl_InputFile);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "MainForm";
            this.Text = "LEDecoder";
            ((System.ComponentModel.ISupportInitialize)(this.pic_LED)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.Label lbl_Overwrite;
        private System.Windows.Forms.PictureBox pic_LED;
        private System.Windows.Forms.CheckBox chk_LittleEndian;
        private System.Windows.Forms.Button btn_Process;
        private System.Windows.Forms.Button btn_OutputFile;
        private System.Windows.Forms.Button btn_InputFile;
        private System.Windows.Forms.TextBox txt_OutputFile;
        private System.Windows.Forms.TextBox txt_InputFile;
        private System.Windows.Forms.Label lbl_OutputFile;
        private System.Windows.Forms.Label lbl_InputFile;
        private System.Windows.Forms.TextBox txt_StartingAddress;
        private System.Windows.Forms.Label lbl_StartingAddress;
        private System.Windows.Forms.CheckBox chk_NameRegisters;
        private System.Windows.Forms.ComboBox cb_Mode;
        private System.Windows.Forms.Label lbl_Mode;
    }
}
