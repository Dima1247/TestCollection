using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace TestApp.Tests {

  public class SqlTest : ITest {

    ///<inheritdoc/>
    public async Task StartAsync() {
      await Case3();
    }

    private static async Task<int> Case1() {
      Console.WriteLine("Case 1:");
      await using var connection = (DbConnection)new SqlConnection(@"Server=(LocalDb)\MSSQLLocalDB;Database=LearningStationDb;Trusted_Connection=True;");
      await using var command = connection.CreateCommand();
      command.CommandType = CommandType.StoredProcedure;
      command.CommandText = "dbo.some_proc";

      var idParam = new SqlParameter("Id", SqlDbType.Int) {
        Direction = ParameterDirection.Output
      };
      var idParam2 = command.Parameters.Add(idParam);
      command.Parameters.Add(new SqlParameter("MediaId", 1));

      await connection.OpenAsync();
      await command.ExecuteNonQueryAsync();

      return idParam2;
    }

    private static async Task<int> Case2() {
      Console.WriteLine("Case 2:");
      await using var connection = (DbConnection)new SqlConnection(@"Server=(LocalDb)\MSSQLLocalDB;Database=LearningStationDb;Trusted_Connection=True;");
      await using var command = connection.CreateCommand();
      command.CommandType = CommandType.StoredProcedure;
      command.CommandText = "dbo.some_proc";

      var idParam = new SqlParameter("Id", SqlDbType.Int) {
        Direction = ParameterDirection.Output
      };
      command.Parameters.Add(idParam);
      command.Parameters.Add(new SqlParameter("MediaId", 1));

      await connection.OpenAsync();
      await command.ExecuteNonQueryAsync();

      return (int)idParam.Value;
    }

    private static async Task<int> Case3() {
      Console.WriteLine("Case 3:");
      await using var connection = new SqlConnection(@"Server=(LocalDb)\MSSQLLocalDB;Database=LearningStationDb;Trusted_Connection=True;");
      await using var command = connection.CreateCommand();
      command.CommandType = CommandType.Text;
      command.CommandText = "SELECT * FROM tblMedia";

      await connection.OpenAsync();
      await using var dataReader = await command.ExecuteReaderAsync();

      while(await dataReader.ReadAsync()) {
        try {
          Console.WriteLine("Usual string cast:");
          var str = dataReader["Name2"].ToString().ToLower();
        }
        catch (Exception ex) {
          Console.WriteLine(ex.Message);
        }

        try {
          Console.WriteLine("Extended string cast:");
          var str = GetValueOrDefault<string>(dataReader, "Name2").ToLower();
        }
        catch (Exception ex) {
          Console.WriteLine(ex.Message);
        }

        var val = (int)dataReader["Id"];
      }

      return default;
    }

    public static T GetValueOrDefault<T>(IDataReader dataReader, string columnName, bool throwException = false) {
      T result = default;

      try {
        var columnOrdinal = dataReader.GetOrdinal(columnName);

        result = dataReader.IsDBNull(columnOrdinal) ? default : (T)dataReader.GetValue(columnOrdinal);
      }
      catch {
        if (throwException) {
          throw;
        }
      }

      return result;
    }
  }
}
