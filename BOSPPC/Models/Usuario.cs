using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOSPPC.Models
{
	public class Usuario
	{
		public string NomeUsuario { get; set; }
		public int Resultados { get; set; }
		public int Placares { get; set; }
		public int Pontos { get; set; }

		public List<Rodada> Rodadas { get; set; }
	}
}
