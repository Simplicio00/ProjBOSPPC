using BOSPPC.Extensions;
using BOSPPC.Models;
using BOSPPC.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinAPP.Utils
{
	internal class Converter
	{
		internal void AdicionaDataCampo(RichTextBox logSistema, string[] data, int velocidadeConversao)
		{
			var usuarios = new List<Usuario>();
			var usuario = new Usuario();
			var rda = new Rodada();
			var sequencia = 0;


			for (int i = 0; i < data.Length; i++)
			{
				if (string.IsNullOrEmpty(data[i].Trim())) continue;

				switch (data[i].Substring(0, 2))
				{
					//SISTEMA DE ABERTURA
					case "01":

						if (!string.IsNullOrEmpty(usuario.NomeUsuario))
						{
							usuario.Rodadas = new List<Rodada>();
							usuario.Rodadas.Add(rda);
							usuarios.Add(usuario);
							usuario = new Usuario();
						}

						sequencia = 0;
						var informacoes = data[i].Split('_');
						int.TryParse(informacoes[1].Trim(), out var rodada);
						usuario.NomeUsuario = "SISTEMA";
						logSistema.AppendNewText("VERIFICANDO", Color.Black, velocidadeConversao);
						if (rodada == 0)
						{
							logSistema.AppendNewText("OCORREU UM ERRO NA LEITURA DOS DADOS, ASSEGURE QUE O PADRÃO CORRETO DO ARQUIVO DE IMPORTAÇÃO ESTEJA NO FORMATO ESPERADO.", Color.Red, velocidadeConversao);
							logSistema.AppendNewText("(TIPO)_(RODADA)_NOME_(NOMEUSUARIO)", Color.Red, velocidadeConversao);
							break;
						}
						logSistema.AppendNewText("\n ABERTURA DE RODADA DETECTADA - Nº" + informacoes[1], Color.DarkOrange, velocidadeConversao);
						logSistema.AppendNewText("CONFIGURANDO NA BASE DE DADOS....", Color.DarkBlue, velocidadeConversao);
						usuario.NomeUsuario = "SISTEMA";
						rda = new Rodada() { Numero_Rodada = rodada, PartidasRodada = new List<PartF>() };
						sequencia++;
						continue;

					//USUARIO COMUM
					case "02":

						if (!string.IsNullOrEmpty(usuario.NomeUsuario))
						{
							usuario.Rodadas = new List<Rodada>();
							usuario.Rodadas.Add(rda);
							usuarios.Add(usuario);
							usuario = new Usuario();
						}

						sequencia = 0;
						var informacoesUsr = data[i].Split('_');

						int.TryParse(informacoesUsr[1].Trim(), out var rd);

						usuario.NomeUsuario = informacoesUsr.LastOrDefault();

						logSistema.AppendNewText("VERIFICANDO.....", Color.Black, velocidadeConversao);

						if (rd == 0)
						{
							logSistema.AppendNewText("OCORREU UM ERRO NA LEITURA DOS DADOS, ASSEGURE QUE O PADRÃO CORRETO DO ARQUIVO DE IMPORTAÇÃO ESTEJA NO FORMATO ESPERADO.", Color.Red, velocidadeConversao);
							logSistema.AppendNewText("(TIPO)_(RODADA)_NOME_(NOMEUSUARIO)", Color.Red, velocidadeConversao);
							break;
						}
						logSistema.AppendNewText("RODADA Nº" + informacoesUsr[1], Color.DarkBlue, velocidadeConversao);
						logSistema.AppendNewText("CONFIGURANDO NA BASE DE DADOS....", Color.DarkBlue, velocidadeConversao);
						usuario.NomeUsuario = informacoesUsr.LastOrDefault();
						rda = new Rodada() { Numero_Rodada = rd, PartidasRodada = new List<PartF>() };
						sequencia++;
						continue;
					//PREENCHENDO PALPITE
					case "FI":
						if (!string.IsNullOrEmpty(usuario.NomeUsuario))
						{
							usuario.Rodadas = new List<Rodada>();
							usuario.Rodadas.Add(rda);
							usuarios.Add(usuario);
							usuario = new Usuario();
						}
						break;

					default:
						sequencia++;
						var partida = PreencheRodada(data[i], sequencia, i, logSistema, usuario, rda, velocidadeConversao);
						if (partida != null)
							rda.PartidasRodada.Add(partida);
						else
							continue;

						continue;
				}
			}

			logSistema.AppendNewText("PRONTO PARA EXPORTAÇÃO", Color.DarkOrange);


			Database db1 = new Database();
			Database.Postulantes = new List<Usuario>();

			//logSistema.ScrollBars = RichTextBoxScrollBars.ForcedBoth;
			logSistema.Enabled = true;

			usuarios.ForEach(x =>
			{
				var objetos = db1.InsertToDatabase(x);
				var objAtualizado = JsonConvert.SerializeObject(objetos);

				File.WriteAllText(Database.DBAddress, objAtualizado);
			});
		}

		private static PartF PreencheRodada(string item, int sequencia, int indice, RichTextBox log, Usuario usuario, Rodada rodada, int velocidadeConversao)
		{

			log.AppendNewText($"LENDO VALORES, LINHA {indice + 1}, SEQUÊNCIA {sequencia}, RODADA {rodada.Numero_Rodada}, USUÁRIO {usuario.NomeUsuario}", Color.DarkBlue, velocidadeConversao);

			var valores = item.Split(' ').Where(x => !string.IsNullOrEmpty(x.Trim())).ToArray();

			if (valores.Length < 5)
			{
				log.AppendNewText($"CONVERSÃO ABORTADA...", Color.Red);
				log.AppendNewText($"OCORREU UM ERRO NA LEITURA DA INFORMAÇÃO.", Color.Red);
				log.AppendNewText($"NOME : {usuario.NomeUsuario}", Color.Red);
				log.AppendNewText($"LINHA: {indice}", Color.Red);
				log.AppendNewText($"TIPO : A QUANTIDADE DE ELEMENTOS NÃO SEGUE O PADRÃO RECONHECIDO PELO SISTEMA. \n" +
					$"VERIFIQUE SE O JOGO DO ARQUIVO DE ENTRADA SEGUE O PADRÃO \n " +
					$"AAA 0 X 0 AAA", Color.Red);

				return null;
			}

			int.TryParse(valores[1], out int golsCasa);
			int.TryParse(valores[3], out int golsFora);

			log.AppendNewText($"IMPORTADO COM SUCESSO, LINHA {indice}, SEQUÊNCIA {sequencia}, RODADA {rodada.Numero_Rodada}, USUÁRIO {usuario.NomeUsuario}", Color.DarkBlue, velocidadeConversao);

			return new PartF
			{
				Sequencia = sequencia,
				TimeCasa = valores[0],
				TimeFora = valores[4],
				PlacarCasa = golsCasa,
				PlacarFora = golsFora
			};
		}
	}
}
