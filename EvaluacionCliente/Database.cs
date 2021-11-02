using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EvaluacionCliente.Models;

namespace EvaluacionCliente
{
	public class Database
	{
		readonly SQLiteAsyncConnection _database;

		public Database(string dbPath)
		{
			_database = new SQLiteAsyncConnection(dbPath);
			_database.CreateTableAsync<Evaluacion>().Wait();
			_database.CreateTableAsync<Acceso>().Wait();
			_database.CreateTableAsync<Dispositivo>().Wait();
			_database.CreateTableAsync<Sucursal>().Wait();
		}

		public Task<int> GuardarEvaluacion(Evaluacion evaluacion)
		{
			return _database.InsertAsync(evaluacion);
		}

		public Task<List<Evaluacion>> ObtenerEvaluaciones()
		{
			return _database.Table<Evaluacion>().ToListAsync();
		}

		public Task<int> EliminarEvaluaciones()
		{
			return _database.DeleteAllAsync<Evaluacion>();
		}

		//Codigo Acceso
		public Task<int> GuardarAcceso(Acceso acceso)
		{
			return _database.InsertAsync(acceso);
		}

		public Task<List<Acceso>> ObtenerAccesos()
		{
			return _database.Table<Acceso>().ToListAsync();
		}

		public Task<List<Acceso>> VerificarAcceso(string usuario, string clave)
		{
			var quer = from tab in _database.Table<Acceso>()
					   where tab.usuario == usuario && tab.clave == clave
					   select tab;
			return quer.ToListAsync();
		}

		//Codigo Dispositivo
		public Task<int> GuardarDispositivo(Dispositivo dispositivo)
		{
			return _database.InsertAsync(dispositivo);
		}

		public Task<List<Dispositivo>> ObtenerDispositivo()
		{
			return _database.Table<Dispositivo>().ToListAsync();
		}

		public Task<int> ActualizarDispositivo(Dispositivo dispositivo)
		{
			return _database.UpdateAsync(dispositivo);
		}


		//Código Sucursal
		public Task<int> GuardarSucursal(Sucursal sucursal)
		{
			return _database.InsertAsync(sucursal);
		}

		public Task<List<Sucursal>> ObtenerSucursal()
		{
			return _database.Table<Sucursal>().ToListAsync();
		}

		public Task<int> ActualizarSucursal(Sucursal sucursal)
		{
			return _database.UpdateAsync(sucursal);
		}
	}
}
