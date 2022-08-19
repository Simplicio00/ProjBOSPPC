using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOSPPC.Utils
{
	[TestClass]
	public class UnifiedTest
	{
		[TestMethod]
		public void TestDatabase()
		{
			Database dba = new Database();

			dba.GerarRelatorio();
		}
	}
}
