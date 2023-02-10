namespace EasyQuartzStore;

public class MySqlOptions
{
    public const string DefaultSchema = "quartz";

    /// <summary>
    /// Gets or sets the table name prefix to use when creating database objects.
    /// </summary>
    public string TableNamePrefix { get; set; } = DefaultSchema;

    /// <summary>
    /// Gets or sets the database's connection string that will be used to store database entities.
    /// </summary>
    public string ConnectionString { get; set; }
}