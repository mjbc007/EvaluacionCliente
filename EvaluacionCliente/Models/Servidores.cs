using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvaluacionCliente.Models
{
	public class Servidores
	{
		[PrimaryKey, AutoIncrement]
		public int id { get; set; }
		public string servidor { get; set; }
	}
}
