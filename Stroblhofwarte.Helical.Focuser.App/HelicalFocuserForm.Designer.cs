
namespace Stroblhofwarte.Helical.Focuser.App
{
    partial class HelicalFocuserForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelicalFocuserForm));
            this.buttonASCOMChooser = new System.Windows.Forms.Button();
            this.labelASCOMDevice = new System.Windows.Forms.Label();
            this.buttonASCOMConnect = new System.Windows.Forms.Button();
            this.buttonRight = new System.Windows.Forms.Button();
            this.labelPosition = new System.Windows.Forms.Label();
            this.timerUpdate = new System.Windows.Forms.Timer(this.components);
            this.buttonLeft = new System.Windows.Forms.Button();
            this.buttonLeftSlow = new System.Windows.Forms.Button();
            this.buttonRightSlow = new System.Windows.Forms.Button();
            this.textBoxMoveTo = new System.Windows.Forms.TextBox();
            this.buttonMove = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonASCOMChooser
            // 
            this.buttonASCOMChooser.BackColor = System.Drawing.Color.DarkBlue;
            this.buttonASCOMChooser.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkBlue;
            this.buttonASCOMChooser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonASCOMChooser.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonASCOMChooser.ForeColor = System.Drawing.Color.Silver;
            this.buttonASCOMChooser.Location = new System.Drawing.Point(578, 44);
            this.buttonASCOMChooser.Name = "buttonASCOMChooser";
            this.buttonASCOMChooser.Size = new System.Drawing.Size(40, 34);
            this.buttonASCOMChooser.TabIndex = 0;
            this.buttonASCOMChooser.Text = "...";
            this.buttonASCOMChooser.UseVisualStyleBackColor = false;
            this.buttonASCOMChooser.Click += new System.EventHandler(this.buttonASCOMChooser_Click);
            // 
            // labelASCOMDevice
            // 
            this.labelASCOMDevice.AutoSize = true;
            this.labelASCOMDevice.BackColor = System.Drawing.Color.Transparent;
            this.labelASCOMDevice.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelASCOMDevice.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.labelASCOMDevice.Location = new System.Drawing.Point(177, 46);
            this.labelASCOMDevice.Name = "labelASCOMDevice";
            this.labelASCOMDevice.Size = new System.Drawing.Size(205, 24);
            this.labelASCOMDevice.TabIndex = 1;
            this.labelASCOMDevice.Text = "AAAAAAAAAAAAAAA";
            // 
            // buttonASCOMConnect
            // 
            this.buttonASCOMConnect.BackColor = System.Drawing.Color.DarkBlue;
            this.buttonASCOMConnect.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkBlue;
            this.buttonASCOMConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonASCOMConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonASCOMConnect.ForeColor = System.Drawing.Color.Silver;
            this.buttonASCOMConnect.Location = new System.Drawing.Point(624, 44);
            this.buttonASCOMConnect.Name = "buttonASCOMConnect";
            this.buttonASCOMConnect.Size = new System.Drawing.Size(75, 34);
            this.buttonASCOMConnect.TabIndex = 2;
            this.buttonASCOMConnect.Text = "Connect";
            this.buttonASCOMConnect.UseVisualStyleBackColor = false;
            this.buttonASCOMConnect.Click += new System.EventHandler(this.buttonASCOMConnect_Click);
            // 
            // buttonRight
            // 
            this.buttonRight.BackColor = System.Drawing.Color.DarkBlue;
            this.buttonRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRight.ForeColor = System.Drawing.Color.Silver;
            this.buttonRight.Location = new System.Drawing.Point(624, 334);
            this.buttonRight.Name = "buttonRight";
            this.buttonRight.Size = new System.Drawing.Size(75, 31);
            this.buttonRight.TabIndex = 3;
            this.buttonRight.Text = ">>";
            this.buttonRight.UseVisualStyleBackColor = false;
            this.buttonRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonRight_MouseDown);
            this.buttonRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonRight_MouseUp);
            // 
            // labelPosition
            // 
            this.labelPosition.AutoSize = true;
            this.labelPosition.BackColor = System.Drawing.Color.Transparent;
            this.labelPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPosition.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.labelPosition.Location = new System.Drawing.Point(514, 169);
            this.labelPosition.Name = "labelPosition";
            this.labelPosition.Size = new System.Drawing.Size(84, 24);
            this.labelPosition.TabIndex = 4;
            this.labelPosition.Text = "Position";
            // 
            // timerUpdate
            // 
            this.timerUpdate.Enabled = true;
            this.timerUpdate.Interval = 300;
            this.timerUpdate.Tick += new System.EventHandler(this.timerUpdate_Tick);
            // 
            // buttonLeft
            // 
            this.buttonLeft.BackColor = System.Drawing.Color.DarkBlue;
            this.buttonLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLeft.ForeColor = System.Drawing.Color.Silver;
            this.buttonLeft.Location = new System.Drawing.Point(543, 334);
            this.buttonLeft.Name = "buttonLeft";
            this.buttonLeft.Size = new System.Drawing.Size(75, 31);
            this.buttonLeft.TabIndex = 5;
            this.buttonLeft.Text = "<<";
            this.buttonLeft.UseVisualStyleBackColor = false;
            this.buttonLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonLeft_MouseDown);
            this.buttonLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonLeft_MouseUp);
            // 
            // buttonLeftSlow
            // 
            this.buttonLeftSlow.BackColor = System.Drawing.Color.DarkBlue;
            this.buttonLeftSlow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLeftSlow.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLeftSlow.ForeColor = System.Drawing.Color.Silver;
            this.buttonLeftSlow.Location = new System.Drawing.Point(543, 297);
            this.buttonLeftSlow.Name = "buttonLeftSlow";
            this.buttonLeftSlow.Size = new System.Drawing.Size(75, 31);
            this.buttonLeftSlow.TabIndex = 6;
            this.buttonLeftSlow.Text = "<";
            this.buttonLeftSlow.UseVisualStyleBackColor = false;
            this.buttonLeftSlow.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonLeftSlow_MouseDown);
            this.buttonLeftSlow.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonLeftSlow_MouseUp);
            // 
            // buttonRightSlow
            // 
            this.buttonRightSlow.BackColor = System.Drawing.Color.DarkBlue;
            this.buttonRightSlow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRightSlow.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRightSlow.ForeColor = System.Drawing.Color.Silver;
            this.buttonRightSlow.Location = new System.Drawing.Point(624, 297);
            this.buttonRightSlow.Name = "buttonRightSlow";
            this.buttonRightSlow.Size = new System.Drawing.Size(75, 31);
            this.buttonRightSlow.TabIndex = 7;
            this.buttonRightSlow.Text = ">";
            this.buttonRightSlow.UseVisualStyleBackColor = false;
            this.buttonRightSlow.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonRightSlow_MouseDown);
            this.buttonRightSlow.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonRightSlow_MouseUp);
            // 
            // textBoxMoveTo
            // 
            this.textBoxMoveTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMoveTo.Location = new System.Drawing.Point(518, 218);
            this.textBoxMoveTo.Name = "textBoxMoveTo";
            this.textBoxMoveTo.Size = new System.Drawing.Size(100, 26);
            this.textBoxMoveTo.TabIndex = 8;
            // 
            // buttonMove
            // 
            this.buttonMove.BackColor = System.Drawing.Color.DarkBlue;
            this.buttonMove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMove.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonMove.ForeColor = System.Drawing.Color.Silver;
            this.buttonMove.Location = new System.Drawing.Point(624, 214);
            this.buttonMove.Name = "buttonMove";
            this.buttonMove.Size = new System.Drawing.Size(75, 32);
            this.buttonMove.TabIndex = 9;
            this.buttonMove.Text = "move";
            this.buttonMove.UseVisualStyleBackColor = false;
            this.buttonMove.Click += new System.EventHandler(this.buttonMove_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.label1.Location = new System.Drawing.Point(14, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 24);
            this.label1.TabIndex = 10;
            this.label1.Text = "ASCOM Device:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.label2.Location = new System.Drawing.Point(415, 169);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 24);
            this.label2.TabIndex = 11;
            this.label2.Text = "Position:";
            // 
            // HelicalFocuserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 393);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonMove);
            this.Controls.Add(this.textBoxMoveTo);
            this.Controls.Add(this.buttonRightSlow);
            this.Controls.Add(this.buttonLeftSlow);
            this.Controls.Add(this.buttonLeft);
            this.Controls.Add(this.labelPosition);
            this.Controls.Add(this.buttonRight);
            this.Controls.Add(this.buttonASCOMConnect);
            this.Controls.Add(this.labelASCOMDevice);
            this.Controls.Add(this.buttonASCOMChooser);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HelicalFocuserForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Stroblhofwarte.Helical.Focuser";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonASCOMChooser;
        private System.Windows.Forms.Label labelASCOMDevice;
        private System.Windows.Forms.Button buttonASCOMConnect;
        private System.Windows.Forms.Button buttonRight;
        private System.Windows.Forms.Label labelPosition;
        private System.Windows.Forms.Timer timerUpdate;
        private System.Windows.Forms.Button buttonLeft;
        private System.Windows.Forms.Button buttonLeftSlow;
        private System.Windows.Forms.Button buttonRightSlow;
        private System.Windows.Forms.TextBox textBoxMoveTo;
        private System.Windows.Forms.Button buttonMove;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

