﻿
namespace BOSPPC
{
	partial class central1
	{
		/// <summary>
		/// Variável de designer necessária.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Limpar os recursos que estão sendo usados.
		/// </summary>
		/// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Código gerado pelo Windows Form Designer

		/// <summary>
		/// Método necessário para suporte ao Designer - não modifique 
		/// o conteúdo deste método com o editor de código.
		/// </summary>
		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.Entrance1 = new System.Windows.Forms.RichTextBox();
			this.relatorioBtn = new System.Windows.Forms.Button();
			this.removedbBtn = new System.Windows.Forms.Button();
			this.velocidadeConversaoOpt = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.ForeColor = System.Drawing.Color.Black;
			this.button1.Location = new System.Drawing.Point(12, 337);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(116, 26);
			this.button1.TabIndex = 0;
			this.button1.Text = "IMPORTAR";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// Entrance1
			// 
			this.Entrance1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Entrance1.Enabled = false;
			this.Entrance1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Entrance1.Location = new System.Drawing.Point(12, 12);
			this.Entrance1.Name = "Entrance1";
			this.Entrance1.ReadOnly = true;
			this.Entrance1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.Entrance1.Size = new System.Drawing.Size(267, 319);
			this.Entrance1.TabIndex = 1;
			this.Entrance1.Text = "";
			// 
			// relatorioBtn
			// 
			this.relatorioBtn.Location = new System.Drawing.Point(134, 337);
			this.relatorioBtn.Name = "relatorioBtn";
			this.relatorioBtn.Size = new System.Drawing.Size(145, 26);
			this.relatorioBtn.TabIndex = 2;
			this.relatorioBtn.Text = "GERAR RELATORIO";
			this.relatorioBtn.UseVisualStyleBackColor = true;
			this.relatorioBtn.Visible = false;
			this.relatorioBtn.Click += new System.EventHandler(this.relatorioBtn_Click);
			// 
			// removedbBtn
			// 
			this.removedbBtn.Location = new System.Drawing.Point(134, 369);
			this.removedbBtn.Name = "removedbBtn";
			this.removedbBtn.Size = new System.Drawing.Size(145, 26);
			this.removedbBtn.TabIndex = 3;
			this.removedbBtn.Text = "RESET DATABASE";
			this.removedbBtn.UseVisualStyleBackColor = true;
			this.removedbBtn.Click += new System.EventHandler(this.removedbBtn_Click);
			// 
			// velocidadeConversaoOpt
			// 
			this.velocidadeConversaoOpt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.velocidadeConversaoOpt.Location = new System.Drawing.Point(12, 371);
			this.velocidadeConversaoOpt.Name = "velocidadeConversaoOpt";
			this.velocidadeConversaoOpt.Size = new System.Drawing.Size(116, 21);
			this.velocidadeConversaoOpt.TabIndex = 4;
			this.velocidadeConversaoOpt.TabStop = false;
			this.velocidadeConversaoOpt.SelectedIndexChanged += new System.EventHandler(this.velocidadeConversaoOpt_SelectedIndexChanged);
			// 
			// central1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(291, 404);
			this.Controls.Add(this.removedbBtn);
			this.Controls.Add(this.relatorioBtn);
			this.Controls.Add(this.Entrance1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.velocidadeConversaoOpt);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "central1";
			this.Text = "BOSPPC";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.RichTextBox Entrance1;
		private System.Windows.Forms.Button relatorioBtn;
		private System.Windows.Forms.Button removedbBtn;
		private System.Windows.Forms.ComboBox velocidadeConversaoOpt;
	}
}

