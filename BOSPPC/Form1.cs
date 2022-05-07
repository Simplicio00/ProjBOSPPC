using BOSPPC.Extensions;
using BOSPPC.Models;
using BOSPPC.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinAPP.Utils;

namespace BOSPPC
{
	public partial class central1 : Form
	{
		public central1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			try
			{
				var dinfo = Directory.GetCurrentDirectory() + "//Database//db1.json";

				if (!File.Exists(dinfo))
				{
					Directory.CreateDirectory(Directory.GetCurrentDirectory() + "//Database");
					File.Create(dinfo);
				}

				var stg = new StringBuilder();
				var obj = File.ReadAllText(Database.DBAddress);
				var rodadas = JsonConvert.DeserializeObject<List<Usuario>>(obj).OrderByDescending(x => x.Rodadas.Count).First().Rodadas;

				foreach (var rodada in rodadas)
				{
					stg.AppendLine("Rodada cadastrada - " + rodada.Numero_Rodada);
				}

				Entrance1.Text = stg.ToString();
			}
			catch (Exception)
			{
				Entrance1.Text = "Nenhuma rodada cadastrada foi encontrada";
			}

		}

		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
				Entrance1.ReadOnly = true;
				Entrance1.Enabled = false;

				Entrance1.Clear();

				OpenFileDialog opf = new OpenFileDialog();
				var cvt = new Converter();
				var dba = new Database();

				if (opf.ShowDialog() == DialogResult.OK)
				{
					opf.Filter = "txt files (*.txt)|*.txt|All Files(*.*)|*-*";
					var arquivo = opf.FileName.Split('\\').LastOrDefault();

					var formatoArquivo = arquivo.Split('.').LastOrDefault().ToString();

					if (!Validations.FileIsValid(formatoArquivo))
					{
						MessageBox.Show("Somente arquivos no formato .TXT são permitidos.");
					}
					else
					{
						var resultado = File.ReadAllLines(opf.FileName);

						int.TryParse(ptAutomatico.Text, out var dec);

						Database.BonusRodada = dec;

						cvt.AdicionaDataCampo(Entrance1, resultado);
					}
				}

				relatorioBtn.Visible = true;
			}
			catch (Exception ex)
			{
				Entrance1.AppendNewText(ex.Message, Color.Red);
			}
		}

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{

		}

		private void relatorioBtn_Click(object sender, EventArgs e)
		{
			try
			{
				var dba = new Database();

				Entrance1.ReadOnly = true;
				Entrance1.Enabled = false;

				var informacoes = dba.GerarRelatorio();

				Entrance1.Clear();

				Entrance1.AppendNewText($"Rodada {informacoes.Key} gerada com sucesso!", Color.Blue);

				Task.Delay(2000).Wait();

				Entrance1.ReadOnly = false;
				Entrance1.Enabled = true;

				Entrance1.Clear();
				Entrance1.AppendText(informacoes.Value);

			}
			catch (Exception ex)
			{
				Entrance1.AppendNewText(ex.Message, Color.Red);
			}

		}

		private void removedbBtn_Click(object sender, EventArgs e)
		{
			try
			{
				Entrance1.ReadOnly = true;
				Entrance1.Enabled = false;

				Entrance1.Clear();
				File.Delete(Database.DBAddress);

				Entrance1.AppendNewText("Database foi excluido.", Color.Red);
				Entrance1.AppendNewText("Caso for iniciar o campeonato, coloque as unidades em sequência.", Color.Red);
			}
			catch (Exception ex)
			{
				Entrance1.AppendNewText(ex.Message, Color.Red);
			}

		}

		private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
			{
				e.Handled = true;
			}
			else if (ptAutomatico.Text.Trim().Length >= 1)
			{
				e.Handled = true;
			}

			if (e.KeyChar == '\b')
			{
				e.Handled = false;
			}

		}
	}
}
