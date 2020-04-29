using Common;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DBConnector
{
    public class DBConnectorPage : DBConnector
    {
        public DBConnectorPage() : base() { }
        public async Task<DBPage> Get(int Id)
        {
            try
            {
                DBPage page = new DBPage();
                await connection.OpenAsync();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("SELECT Id, IdFichier, Ordre,Element");
                stringBuilder.AppendLine("FROM Page ");
                stringBuilder.AppendLine($"WHERE Id = {Id}");
                var cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = stringBuilder.ToString();
                var result = await cmd.ExecuteReaderAsync();
                if (result.HasRows)
                {
                    if (await result.ReadAsync())
                    {
                        page = ExtractDataFromDataReader(result);
                    }
                }

                return page;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }
        public async Task<IEnumerable<DBPage>> GetAllByFichier(int IdFichier)
        {
            List<DBPage> pages = new List<DBPage>();
            try
            {
                await connection.OpenAsync();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("SELECT Id, IdFichier, Ordre,Element");
                stringBuilder.AppendLine("FROM Page ");
                stringBuilder.AppendLine($"WHERE IdFichier = {IdFichier}");
                stringBuilder.AppendLine($"ORDER BY Ordre");
                var cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = stringBuilder.ToString();
                var result = await cmd.ExecuteReaderAsync();
                if (result.HasRows)
                {
                    while (await result.ReadAsync())
                    {
                        pages.Add(ExtractDataFromDataReader(result));
                    }
                }

                return pages;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }
        public async Task Create(DBPage newValue)
        {
            try
            {
                await connection.OpenAsync();
                StringBuilder stringBuilder = new StringBuilder();
                var cmd = new MySqlCommand();
                cmd.Connection = connection;
                stringBuilder.AppendLine("Insert INTO Page(IdFichier, Ordre,Element) ");
                stringBuilder.AppendLine($"Values({newValue.IDFichier},{newValue.Ordre},@ele) ");
                MySqlParameter Imgparam = new MySqlParameter("@ele", MySqlDbType.LongBlob);
                Imgparam.Value = newValue.Element;
                cmd.Parameters.Add(Imgparam);
                cmd.CommandText = stringBuilder.ToString();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }
        public async Task Update(DBPage newValue)
        {
            try
            {
                await connection.OpenAsync();
                var cmd = new MySqlCommand();
                cmd.Connection = connection;
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("Update Page ");
                stringBuilder.AppendLine($"SET IdFichier = {newValue.IDFichier} ");
                stringBuilder.AppendLine($",Ordre = {newValue.Ordre} ");
                stringBuilder.AppendLine($",Element = @ele ");
                stringBuilder.AppendLine($"WHERE Id = {newValue.ID}");
                MySqlParameter Imgparam = new MySqlParameter("@ele", MySqlDbType.LongBlob);
                Imgparam.Value = newValue.Element;
                cmd.Parameters.Add(Imgparam);
                cmd.CommandText = stringBuilder.ToString();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }
        public async Task<bool> Delete(DBPage valueToDelete)
        {
            try
            {
                await connection.OpenAsync();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("DELETE FROM Page ");
                stringBuilder.AppendLine($"WHERE ID ={valueToDelete.ID}");
                var cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = stringBuilder.ToString();
                int result = await cmd.ExecuteNonQueryAsync();
                return Convert.ToBoolean(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }
        private static DBPage ExtractDataFromDataReader(System.Data.Common.DbDataReader result)
        {
            DBPage dbPage = new DBPage();
            dbPage.ID = result.GetInt32(0);
            dbPage.IDFichier = result.GetInt32(1);
            dbPage.Ordre = result.GetInt32(2);
            if (!string.IsNullOrEmpty(result.GetValue(3).ToString()) && !string.IsNullOrWhiteSpace(result.GetValue(3).ToString()))
                dbPage.Element = ReadToEnd(result.GetStream(3));
            return dbPage;
        }
    }
}
