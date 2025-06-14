namespace QuanLyNhaHang
{
    partial class FormQuanLyBan
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
            txtMaSoBan = new TextBox();
            label1 = new Label();
            flpBanAn = new FlowLayoutPanel();
            cmbTrangThai = new ComboBox();
            label2 = new Label();
            btnThemBan = new Button();
            btnSuaBan = new Button();
            btnXoaBan = new Button();
            btnLamMoi = new Button();
            SuspendLayout();
            // 
            // txtMaSoBan
            // 
            txtMaSoBan.Location = new Point(99, 323);
            txtMaSoBan.Name = "txtMaSoBan";
            txtMaSoBan.Size = new Size(151, 27);
            txtMaSoBan.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(15, 330);
            label1.Name = "label1";
            label1.Size = new Size(78, 20);
            label1.TabIndex = 4;
            label1.Text = "Mã số bàn";
            // 
            // flpBanAn
            // 
            flpBanAn.AutoScroll = true;
            flpBanAn.Location = new Point(12, 12);
            flpBanAn.Name = "flpBanAn";
            flpBanAn.Size = new Size(771, 288);
            flpBanAn.TabIndex = 8;
            // 
            // cmbTrangThai
            // 
            cmbTrangThai.FormattingEnabled = true;
            cmbTrangThai.Location = new Point(99, 356);
            cmbTrangThai.Name = "cmbTrangThai";
            cmbTrangThai.Size = new Size(151, 28);
            cmbTrangThai.TabIndex = 9;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(15, 364);
            label2.Name = "label2";
            label2.Size = new Size(75, 20);
            label2.TabIndex = 10;
            label2.Text = "Trạng thái";
            // 
            // btnThemBan
            // 
            btnThemBan.Location = new Point(15, 401);
            btnThemBan.Name = "btnThemBan";
            btnThemBan.Size = new Size(94, 29);
            btnThemBan.TabIndex = 11;
            btnThemBan.Text = "Thêm bàn";
            btnThemBan.UseVisualStyleBackColor = true;
            // 
            // btnSuaBan
            // 
            btnSuaBan.Location = new Point(115, 401);
            btnSuaBan.Name = "btnSuaBan";
            btnSuaBan.Size = new Size(94, 29);
            btnSuaBan.TabIndex = 12;
            btnSuaBan.Text = "Sửa bàn";
            btnSuaBan.UseVisualStyleBackColor = true;
            // 
            // btnXoaBan
            // 
            btnXoaBan.Location = new Point(215, 401);
            btnXoaBan.Name = "btnXoaBan";
            btnXoaBan.Size = new Size(94, 29);
            btnXoaBan.TabIndex = 13;
            btnXoaBan.Text = "Xóa bàn ";
            btnXoaBan.UseVisualStyleBackColor = true;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Location = new Point(315, 401);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(94, 29);
            btnLamMoi.TabIndex = 14;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = true;
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // FormQuanLyBan
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(800, 450);
            Controls.Add(btnLamMoi);
            Controls.Add(btnXoaBan);
            Controls.Add(btnSuaBan);
            Controls.Add(btnThemBan);
            Controls.Add(label2);
            Controls.Add(cmbTrangThai);
            Controls.Add(flpBanAn);
            Controls.Add(txtMaSoBan);
            Controls.Add(label1);
            Name = "FormQuanLyBan";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FormQuanLyBan";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox txtMaSoBan;
        private Label label1;
        private FlowLayoutPanel flpBanAn;
        private ComboBox cmbTrangThai;
        private Label label2;
        private Button btnThemBan;
        private Button btnSuaBan;
        private Button btnXoaBan;
        private Button btnLamMoi;
    }
}