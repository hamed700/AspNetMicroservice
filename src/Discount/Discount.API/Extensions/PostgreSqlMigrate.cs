using Npgsql;

namespace Discount.API.Extensions;

public static class PostgreSqlMigrate
{
    public static void ConfigurePostgreSql(this IServiceCollection services, IConfiguration configuration, int retry = 0)
    {
        int retryAvailabel = retry;

        NpgsqlConnection _connection = new(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        _connection.Open();

        using var command = new NpgsqlCommand
        {
            Connection = _connection,
        };

        try
        {
            command.CommandText = "DROP TABLE IF EXISTS Coupon";
            command.ExecuteNonQuery();

            command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY, 
                                                                ProductName VARCHAR(24) NOT NULL,
                                                                Description TEXT,
                                                                Amount INT)";
            command.ExecuteNonQuery();

            command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('IPhone X', 'IPhone Discount', 150);";
            command.ExecuteNonQuery();

            command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Samsung 10', 'Samsung Discount', 100);";
            command.ExecuteNonQuery();
        }
        catch (NpgsqlException ex)
        {


            if (retryAvailabel < 50)
            {
                retryAvailabel++;
                System.Threading.Thread.Sleep(2000);
                ConfigurePostgreSql(services, configuration, retryAvailabel);
            }
        }
    }
}
