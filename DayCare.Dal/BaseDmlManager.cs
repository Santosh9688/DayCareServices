using Azure.Core.GeoJson;
using FastMember;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DayCare.Dal
{
    public class BaseDmlManager
    {
        protected readonly string _connectionString;
        internal BaseDmlManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<T>> RetrieveAsync<T>(Dictionary<string, object> parameters, string storedProcName)
            where T : class, new()
        {
            var data = new List<T>();
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = storedProcName;
                    command.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(new SqlParameter(parameter.Key, parameter.Value ?? DBNull.Value));
                        }
                    }
                    await connection.OpenAsync();
                    var dataReader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                    if (dataReader.HasRows)
                    {
                        while (await dataReader.ReadAsync())
                        {
                                var t = new T();
                                t = ConvertToObject<T>(dataReader);
                                data.Add(t);
                        }
                    }

                }
            }
            return data;
        }
        public async Task CreateAsync<T>(T entity, string storedProcName) where T : class, new()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = storedProcName;
                    command.CommandType = CommandType.StoredProcedure;
                    var entityProps = typeof(T).GetProperties(System.Reflection.BindingFlags.Public
                                                       | System.Reflection.BindingFlags.Instance
                                                       | System.Reflection.BindingFlags.DeclaredOnly);
                    if (entityProps != null)
                    {
                        var idenPropInfo = entityProps.FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(Identity)))
                                ?? throw new Exception($"Identity column is not set for {typeof(T).Name}.");
                        foreach (var propertyInfo in entityProps)
                        {
                            if (propertyInfo.Name != idenPropInfo.Name && 
                                propertyInfo.Name != "CreateDateTime" && 
                                propertyInfo.Name != "UpdateBy" && 
                                propertyInfo.Name != "UpdateDateTime")
                            {
                                command.Parameters.Add(new SqlParameter(propertyInfo.Name, propertyInfo.GetValue(entity) ?? DBNull.Value));
                            }
                        }
                    }
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task UpdateAsync<T>(T entity, string storedProcName) where T : class, new()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = storedProcName;
                    command.CommandType = CommandType.StoredProcedure;
                    var entityProps = typeof(T).GetProperties(System.Reflection.BindingFlags.Public
                                                       | System.Reflection.BindingFlags.Instance
                                                       | System.Reflection.BindingFlags.DeclaredOnly);
                    if (entityProps != null)
                    {
                        foreach (var propertyInfo in entityProps)
                        {
                            if (propertyInfo.Name != "CreateBy" &&
                                propertyInfo.Name != "CreateDateTime" &&
                                propertyInfo.Name != "UpdateDateTime")
                            {
                                command.Parameters.Add(new SqlParameter(propertyInfo.Name, propertyInfo.GetValue(entity) ?? DBNull.Value));
                            }
                        }
                    }
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task DeleteAsync<T>(T entity, string storedProcName) where T : class, new()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = storedProcName;
                    command.CommandType = CommandType.StoredProcedure;
                    var entityProps = typeof(T).GetProperties(System.Reflection.BindingFlags.Public
                                                       | System.Reflection.BindingFlags.Instance
                                                       | System.Reflection.BindingFlags.DeclaredOnly);
                    if (entityProps != null)
                    {
                        var idenPropInfo = entityProps.FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(Identity)))
                                ?? throw new Exception($"Identity column is not set for {typeof(T).Name}.");
                        command.Parameters.Add(new SqlParameter(idenPropInfo.Name, idenPropInfo.GetValue(entity) ?? DBNull.Value));
                    }
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        private static T ConvertToObject<T>(SqlDataReader rd) where T : class, new()
        {
            Type type = typeof(T);
            var accessor = TypeAccessor.Create(type);
            var members = accessor.GetMembers();
            var t = new T();

            for (int i = 0; i < rd.FieldCount; i++)
            {
                if (!rd.IsDBNull(i))
                {
                    string fieldName = rd.GetName(i);

                    if (members.Any(m => string.Equals(m.Name, fieldName, StringComparison.OrdinalIgnoreCase)))
                    {
                        accessor[t, fieldName] = rd.GetValue(i);
                    }
                }
            }

            return t;
        }
    }
}