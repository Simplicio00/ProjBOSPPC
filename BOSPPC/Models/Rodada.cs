using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOSPPC.Models
{
	public class Rodada
	{
		public int Numero_Rodada { get; set; }
		public List<PartF> PartidasRodada { get; set; }
	}
}
