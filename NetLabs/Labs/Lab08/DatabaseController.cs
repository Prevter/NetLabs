using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Data;

namespace NetLabs.Labs.Lab08;

public static class DatabaseController
{
    public static SqlConnection? Connection;
    public static string Username = "", Password = "";

    public static bool IsConnected => Connection?.State == ConnectionState.Open;

    public static void Connect(string username, string password)
    {
        var connectionString = $"Data Source={App.Settings.Lab8.Server};Initial Catalog={App.Settings.Lab8.InitialCatalog};TrustServerCertificate=true;Persist Security Info=False";

        if (!string.IsNullOrEmpty(username))
            connectionString += $";User ID={username}";
        if (!string.IsNullOrEmpty(password))
            connectionString += $";Password={password}";
        
        Connection = new SqlConnection(connectionString);
        Connection.Open();
        Username = username;
        Password = password;
        CreateTables();
    }

    public static void CreateTables()
    {
        string sql = """
                     IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Sportsmen' and xtype='U')
                         CREATE TABLE Sportsmen (
                             Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
                             FirstName NVARCHAR(50) NOT NULL,
                             LastName NVARCHAR(50) NOT NULL,
                             MiddleName NVARCHAR(50) NOT NULL,
                             BirthYear INT NOT NULL,
                             IsMale BIT NOT NULL,
                             SportName NVARCHAR(50) NOT NULL,
                             Achievements NVARCHAR(1000) NOT NULL,
                             Prizes NVARCHAR(1000) NOT NULL
                         );
                     """;

        using SqlCommand command = new(sql, Connection);
        command.ExecuteNonQuery();
    }

    public static IEnumerable<Sportsman> GetSportsmen()
    {
        var command = new SqlCommand("SELECT * FROM Sportsmen", Connection);
        var reader = command.ExecuteReader();

        while (reader.Read())
        {
            yield return new(
                Id: reader.GetInt32(0),
                FirstName: reader.GetString(1),
                LastName: reader.GetString(2),
                MiddleName: reader.GetString(3),
                Year: reader.GetInt32(4),
                IsMale: reader.GetBoolean(5),
                SportName: reader.GetString(6),
                Achievements: reader.GetString(7),
                Prizes: reader.GetString(8));
        }

        reader.Close();
    }
    
    public static void AddSportsman(Sportsman sportsman)
    {
        var command = new SqlCommand("INSERT INTO Sportsmen (FirstName, LastName, MiddleName, BirthYear, IsMale, SportName, Achievements, Prizes) VALUES (@FirstName, @LastName, @MiddleName, @BirthYear, @IsMale, @SportName, @Achievements, @Prizes)", Connection);
        command.Parameters.AddWithValue("@FirstName", sportsman.FirstName);
        command.Parameters.AddWithValue("@LastName", sportsman.LastName);
        command.Parameters.AddWithValue("@MiddleName", sportsman.MiddleName);
        command.Parameters.AddWithValue("@BirthYear", sportsman.Year);
        command.Parameters.AddWithValue("@IsMale", sportsman.IsMale);
        command.Parameters.AddWithValue("@SportName", sportsman.SportName);
        command.Parameters.AddWithValue("@Achievements", sportsman.Achievements);
        command.Parameters.AddWithValue("@Prizes", sportsman.Prizes);
        command.ExecuteNonQuery();
    }
    
    public static void UpdateSportsman(Sportsman sportsman)
    {
        var command = new SqlCommand("UPDATE Sportsmen SET FirstName = @FirstName, LastName = @LastName, MiddleName = @MiddleName, BirthYear = @BirthYear, IsMale = @IsMale, SportName = @SportName, Achievements = @Achievements, Prizes = @Prizes WHERE Id = @Id", Connection);
        command.Parameters.AddWithValue("@Id", sportsman.Id);
        command.Parameters.AddWithValue("@FirstName", sportsman.FirstName);
        command.Parameters.AddWithValue("@LastName", sportsman.LastName);
        command.Parameters.AddWithValue("@MiddleName", sportsman.MiddleName);
        command.Parameters.AddWithValue("@BirthYear", sportsman.Year);
        command.Parameters.AddWithValue("@IsMale", sportsman.IsMale);
        command.Parameters.AddWithValue("@SportName", sportsman.SportName);
        command.Parameters.AddWithValue("@Achievements", sportsman.Achievements);
        command.Parameters.AddWithValue("@Prizes", sportsman.Prizes);
        command.ExecuteNonQuery();
    }
    
    public static void DeleteSportsman(Sportsman sportsman)
    {
        var command = new SqlCommand("DELETE FROM Sportsmen WHERE Id = @Id", Connection);
        command.Parameters.AddWithValue("@Id", sportsman.Id);
        command.ExecuteNonQuery();
    }
    
    public static void Disconnect()
    {
        Connection?.Close();
        Connection = null;
    }
}

public sealed record LabSettings(string Server, string InitialCatalog);