using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Sql;
using System.IO;
using System.Data;
using BOSPPC.Models;
using Newtonsoft.Json;
using System.Drawing;


namespace BOSPPC.Utils
{
	internal class Database
	{
		public static string DBAddress { get => Directory.GetCurrentDirectory() + "//Database//db1.json"; }
		public static Usuario RodadaOficial { get; set; }
		public static List<Usuario> Postulantes { get; set; }
		public static int BonusRodada { get; set; } 
		
		internal List<Usuario> InsertToDatabase(Usuario user)
		{
			if (!File.Exists(DBAddress)) File.Create(DBAddress).Close();

			if (ValidaRodadaOficial(user))
			{
				RodadaOficial = new Usuario();
				RodadaOficial = user;
			}

			var usuarios = File.ReadAllText(DBAddress);
			var objetos = JsonConvert.DeserializeObject<List<Usuario>>(usuarios);
			var userRanking = objetos?.FirstOrDefault(x => x.NomeUsuario == user.NomeUsuario);

			if (!ValidaRodadaOficial(user))
			{
				if (userRanking != null)
				{
					user.Rodadas.AddRange(userRanking.Rodadas);
					user.Rodadas = user.Rodadas.OrderBy(a => a.Numero_Rodada).ToList();
					user.Resultados = userRanking.Resultados;
					user.Pontos = userRanking.Pontos;
					user.Placares = userRanking.Placares;
					objetos.RemoveAll(x => x.NomeUsuario == userRanking.NomeUsuario);
				}

				if (objetos == null) objetos = new List<Usuario>();
				
				objetos.Add(RealizaAuditoriaDosPontos(user));
				Postulantes.Add(user);
			}

			return objetos;
		}

		private Usuario RealizaAuditoriaDosPontos(Usuario user)
		{
			if (RodadaOficial != null)
			{
				var oficial = RodadaOficial.Rodadas.FirstOrDefault();
				var jogosContabilizados = new List<PartF>();

				oficial.PartidasRodada.ForEach(x =>
				{
					var jogo = user.Rodadas.LastOrDefault().PartidasRodada.FirstOrDefault(a => a.TimeCasa == x.TimeCasa && a.TimeFora == x.TimeFora);

					if (jogo != null)
					{
						if (jogo.PlacarCasa == x.PlacarCasa && jogo.PlacarFora == x.PlacarFora)
						{
							//3 PONTOS
							jogo.PontoColetado = 3;
							user.Placares += 1;
						}
						else if (GanhadorPartida(x) == GanhadorPartida(jogo))
						{
							//1 PONTO
							jogo.PontoColetado = 1;
							user.Resultados += 1;
						}
						else
						{
							jogo.PontoColetado = 0;
						}

						user.Pontos += jogo.PontoColetado;

						jogosContabilizados.Add(jogo);
					}
				});

				user.Rodadas[user.Rodadas.Count - 1].PartidasRodada = jogosContabilizados;

				if (BonusRodada > 0)
				{
					user.Pontos += BonusRodada;
				}
			}

			return user;
		}

		private PartidaEstado GanhadorPartida(PartF partida)
		{
			if (partida.PlacarCasa > partida.PlacarFora) return PartidaEstado.CASA;
			else if (partida.PlacarFora > partida.PlacarCasa) return PartidaEstado.FORA;

			return PartidaEstado.EMPATOU;
		}

		private bool ValidaRodadaOficial(Usuario user)
		{
			if (user.NomeUsuario == "SISTEMA") return true;

			return false;
		}

		internal KeyValuePair<int, string> GerarRelatorio()
		{
			var arquivo = File.ReadAllText(DBAddress);

			var objetos = JsonConvert.DeserializeObject<List<Usuario>>(arquivo);

			List<Usuario> objetosRodada = new List<Usuario>();
			
			var partidasRodada = new List<PartF>();
			var rodadaAtual = RodadaOficial.Rodadas.Last().Numero_Rodada;
			
			objetosRodada = objetos.Where(x => x.Rodadas.Exists(a => a.Numero_Rodada == RodadaOficial.Rodadas.LastOrDefault().Numero_Rodada)).ToList();

			objetosRodada.ForEach(x =>
			{
				var partidas = x.Rodadas.FirstOrDefault(rod => rod.Numero_Rodada == rodadaAtual).PartidasRodada;
				partidasRodada.AddRange(partidas);
			});

			//var rdas = RodadaOficial.Rodadas.FirstOrDefault(x => x.Numero_Rodada == rodadaAtual).PartidasRodada;
			var partidasAgrupadas = partidasRodada.GroupBy(x => new { x.TimeCasa, x.TimeFora });
			var pontosColetadosPorPartida = partidasAgrupadas.Select(x => new { 
																				partida = x.Key.TimeCasa + " X " + x.Key.TimeFora, pontos = x.Sum(rd => rd.PontoColetado), 
																				placares = x.Where(a => a.PontoColetado == 3).Sum(a => a.PontoColetado - 2),
																				resultados = x.Where(a => a.PontoColetado == 1).Sum(a => a.PontoColetado),
																				}).OrderByDescending(x => x.pontos).ToList();

			var pontosTotalRodada = objetosRodada.Select(x => x.Rodadas.LastOrDefault().PartidasRodada.Sum(a => a.PontoColetado)).Sum();
			var media = (double)pontosTotalRodada / objetosRodada.Count;

			var mediaPlacares = (double)pontosColetadosPorPartida.Sum(x => x.placares) / pontosColetadosPorPartida.Count;
			var mediaResultados = (double)pontosColetadosPorPartida.Sum(x => x.resultados) / pontosColetadosPorPartida.Count;

			var informacoesCampeonato = objetos.Select(x => new { x.Pontos, usuario = x.NomeUsuario,  x.Placares, x.Resultados }).OrderByDescending(x => x.Pontos).ToList();
			
			var pontuadoresRodada = objetosRodada.Select(x => new { 
																				placares = x.Rodadas.LastOrDefault().PartidasRodada.Where(a => a.PontoColetado == 3).Count(), 
																				resultados = x.Rodadas.LastOrDefault().PartidasRodada.Where(a => a.PontoColetado == 1).Count(),
																				pontos = x.Rodadas.LastOrDefault().PartidasRodada.Sum(a => a.PontoColetado), 
																				usuario = x.NomeUsuario }).OrderByDescending(x => x.pontos).ToList();
			
			var porPlacar = objetos.Select(x => new { x.Placares, usuario = x.NomeUsuario }).OrderByDescending(x => x.Placares).ToList();
			var porResultados = objetos.Select(x => new { x.Resultados, usuario = x.NomeUsuario }).OrderByDescending(x => x.Resultados).ToList();


			StringBuilder stg = new StringBuilder();

			stg.AppendLine(" RELATÓRIO OFICIAL " + RodadaOficial.Rodadas.LastOrDefault().Numero_Rodada);
			stg.Append("\n");
			stg.AppendLine(" MEDIA: " + media.ToString("0.0"));
			stg.AppendLine(" MEDIA PLACARES POR JOGO: " + mediaPlacares.ToString("0.0"));
			stg.AppendLine(" MEDIA RESULTADOS POR JOGO: " + mediaResultados.ToString("0.0"));
			stg.AppendLine(" PONTOS DISTRIBUÍDOS POR JOGO:");
			stg.AppendLine("[spoiler]");
			pontosColetadosPorPartida.ForEach(x => 
			{
				stg.AppendLine($"A partida {x.partida} gerou {x.pontos} pontos! (placares:{x.placares} - resultados:{x.resultados})");
			});
			stg.AppendLine("[/spoiler]");

			stg.Append("\n");
			stg.AppendLine("PONTUADORES DA RODADA");
			stg.Append("\n");

			if (BonusRodada > 0)
			{
				stg.Append("\n");
				stg.AppendLine($"Obs: Adicionado(s) {BonusRodada} ponto(s) para quem postou");
				stg.Append("\n");
			}

			stg.AppendLine("[spoiler]");
			pontuadoresRodada.ForEach(x => 
			{
				stg.AppendLine($"{x.usuario} - {x.pontos + BonusRodada} - {x.placares} - {x.resultados}");
			});
			stg.AppendLine("[/spoiler]");
			stg.Append("\n");
			stg.AppendLine("RANKING");
			stg.Append("\n");
			stg.AppendLine("(PONTUAÇÃO - PLACARES - RESULTADOS)");
			stg.Append("\n");
			stg.Append("\n");

			for (int i = 0; i < informacoesCampeonato.ToArray().Length; i++)
			{
				stg.AppendLine($" {i+1} - {informacoesCampeonato[i].usuario} - {informacoesCampeonato[i].Pontos} - {informacoesCampeonato[i].Placares} - {informacoesCampeonato[i].Resultados} ");
			}

			stg.Append("\n");
			stg.Append("\n");

			stg.AppendLine("Placares");

			stg.AppendLine("[spoiler]");
			porPlacar.ForEach(x =>
			{
				stg.AppendLine(x.usuario + " - " + x.Placares);
			});
			stg.AppendLine("[/spoiler]");

			stg.Append("\n");
			stg.Append("\n");

			stg.AppendLine("Resultados");
			stg.AppendLine("[spoiler]");
			porResultados.ForEach(x =>
			{
				stg.AppendLine(x.usuario + " - " + x.Resultados);
			});
			stg.AppendLine("[/spoiler]");

			return new KeyValuePair<int, string>(RodadaOficial.Rodadas.FirstOrDefault().Numero_Rodada, stg.ToString());
		}

		internal enum PartidaEstado
		{
			CASA,
			FORA,
			EMPATOU
		}
	}


}
