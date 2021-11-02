using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvaluacionCliente.Models
{
	public class Dispositivo
	{
		[PrimaryKey, AutoIncrement]
		public int id { get; set; }
		public string nombre { get; set; }
	}
}
