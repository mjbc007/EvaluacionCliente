using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvaluacionCliente.Models
{
	public class Sucursal
	{
		[PrimaryKey, AutoIncrement]
		public int id { get; set; }
		public string sucursal { get; set; }
	}
}
