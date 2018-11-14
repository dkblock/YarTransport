namespace RoutesCreator
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
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_RouteNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_TransportType = new System.Windows.Forms.ComboBox();
            this.Btn_Add = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Window;
            this.label2.Location = new System.Drawing.Point(31, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Тип транспорта";
            // 
            // textBox_RouteNumber
            // 
            this.textBox_RouteNumber.Location = new System.Drawing.Point(34, 66);
            this.textBox_RouteNumber.Name = "textBox_RouteNumber";
            this.textBox_RouteNumber.Size = new System.Drawing.Size(121, 20);
            this.textBox_RouteNumber.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Window;
            this.label1.Location = new System.Drawing.Point(5, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 13);
            this.label1.TabIndex = 17;
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
            this.comboBox_TransportType.Location = new System.Drawing.Point(34, 25);
            this.comboBox_TransportType.Name = "comboBox_TransportType";
            this.comboBox_TransportType.Size = new System.Drawing.Size(121, 21);
            this.comboBox_TransportType.TabIndex = 16;
            // 
            // Btn_Add
            // 
            this.Btn_Add.Location = new System.Drawing.Point(34, 92);
            this.Btn_Add.Name = "Btn_Add";
            this.Btn_Add.Size = new System.Drawing.Size(116, 23);
            this.Btn_Add.TabIndex = 15;
            this.Btn_Add.Text = "Новый маршрут";
            this.Btn_Add.UseVisualStyleBackColor = true;
            this.Btn_Add.Click += new System.EventHandler(this.Btn_Add_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(190, 154);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_RouteNumber);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_TransportType);
            this.Controls.Add(this.Btn_Add);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_RouteNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_TransportType;
        private System.Windows.Forms.Button Btn_Add;
    }
}

