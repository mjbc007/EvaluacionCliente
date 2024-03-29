﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvaluacionCliente.Models
{
	public class Evaluacion
	{
		[PrimaryKey, AutoIncrement]
		public int id { get; set; }
		public int evaluacion { get; set; }
		public DateTime? fecha_evaluacion { get; set; }
		public string device_name { get; set; }
		public string sucursal { get; set; }
	}
}
