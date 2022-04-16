using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOSPPC.Models
{
	public class PartF
	{
		public DateTime Data { get; set; }
		public int PontoColetado { get; set; }
		public int Sequencia { get; set; }
		public int PlacarCasa { get; set; }
		public int PlacarFora { get; set; }
		public string TimeCasa { get; set; }
		public string TimeFora { get; set; }
	}
}
