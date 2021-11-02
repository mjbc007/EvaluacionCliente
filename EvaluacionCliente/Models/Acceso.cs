using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvaluacionCliente.Models
{
	public class Acceso
	{
		[PrimaryKey, AutoIncrement]
		public int id { get; set; }
		public string usuario { get; set; }
		public string clave { get; set; }
	}
}
