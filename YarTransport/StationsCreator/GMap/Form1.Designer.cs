namespace GMap
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.gMapControl1 = new GMap.NET.WindowsForms.GMapControl();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_RouteNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_TransportType = new System.Windows.Forms.ComboBox();
            this.Btn_EndRoute = new System.Windows.Forms.Button();
            this.Btn_StartRoute = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // gMapControl1
            // 
            this.gMapControl1.Bearing = 0F;
            this.gMapControl1.CanDragMap = true;
            this.gMapControl1.GrayScaleMode = false;
            this.gMapControl1.LevelsKeepInMemmory = 5;
            this.gMapControl1.Location = new System.Drawing.Point(12, 12);
            this.gMapControl1.MarkersEnabled = true;
            this.gMapControl1.MaxZoom = 2;
            this.gMapControl1.MinZoom = 2;
            this.gMapControl1.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gMapControl1.Name = "gMapControl1";
            this.gMapControl1.NegativeMode = false;
            this.gMapControl1.PolygonsEnabled = true;
            this.gMapControl1.RetryLoadTile = 0;
            this.gMapControl1.RoutesEnabled = true;
            this.gMapControl1.ShowTileGridLines = false;
            this.gMapControl1.Size = new System.Drawing.Size(150, 150);
            this.gMapControl1.TabIndex = 0;
            this.gMapControl1.Zoom = 0D;
            this.gMapControl1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gMapControl1_KeyDown);
            this.gMapControl1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.gMapControl1_MouseDoubleClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Window;
            this.label2.Location = new System.Drawing.Point(1056, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Тип транспорта";
            // 
            // textBox_RouteNumber
            // 
            this.textBox_RouteNumber.Location = new System.Drawing.Point(1059, 184);
            this.textBox_RouteNumber.Name = "textBox_RouteNumber";
            this.textBox_RouteNumber.Size = new System.Drawing.Size(121, 20);
            this.textBox_RouteNumber.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Window;
            this.label1.Location = new System.Drawing.Point(1030, 167);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "№ маршрута (пиши вручную)";
            // 
            // comboBox_TransportType
            // 
            this.comboBox_TransportType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_TransportType.FormattingEnabled = true;
            this.comboBox_TransportType.Items.AddRange(new object[] {
            "Автобус",
            "Троллейбус",
            "Трамвай",
            "Маршрутка"});
            this.comboBox_TransportType.Location = new System.Drawing.Point(1059, 143);
            this.comboBox_TransportType.Name = "comboBox_TransportType";
            this.comboBox_TransportType.Size = new System.Drawing.Size(121, 21);
            this.comboBox_TransportType.TabIndex = 11;
            // 
            // Btn_EndRoute
            // 
            this.Btn_EndRoute.Location = new System.Drawing.Point(1059, 239);
            this.Btn_EndRoute.Name = "Btn_EndRoute";
            this.Btn_EndRoute.Size = new System.Drawing.Size(116, 23);
            this.Btn_EndRoute.TabIndex = 10;
            this.Btn_EndRoute.Text = "Закончить маршрут";
            this.Btn_EndRoute.UseVisualStyleBackColor = true;
            this.Btn_EndRoute.Click += new System.EventHandler(this.Btn_EndRoute_Click);
            // 
            // Btn_StartRoute
            // 
            this.Btn_StartRoute.Location = new System.Drawing.Point(1059, 210);
            this.Btn_StartRoute.Name = "Btn_StartRoute";
            this.Btn_StartRoute.Size = new System.Drawing.Size(116, 23);
            this.Btn_StartRoute.TabIndex = 9;
            this.Btn_StartRoute.Text = "Новый маршрут";
            this.Btn_StartRoute.UseVisualStyleBackColor = true;
            this.Btn_StartRoute.Click += new System.EventHandler(this.Btn_StartRoute_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(1001, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(171, 108);
            this.richTextBox1.TabIndex = 8;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 639);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_RouteNumber);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_TransportType);
            this.Controls.Add(this.Btn_EndRoute);
            this.Controls.Add(this.Btn_StartRoute);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.gMapControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NET.WindowsForms.GMapControl gMapControl1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_RouteNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_TransportType;
        private System.Windows.Forms.Button Btn_EndRoute;
        private System.Windows.Forms.Button Btn_StartRoute;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}

