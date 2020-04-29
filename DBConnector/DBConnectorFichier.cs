using Common;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace DBConnector
{
    public class DBConnectorFichier : DBConnector
    {
        public DBConnectorFichier() : base(){ }
        public async Task<bool> Exist(DBFichier dBFichier)
        {
            try
            {
                await connection.OpenAsync();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("SELECT Id, Name, Ordre, Image, IDParent, Collection ");
                stringBuilder.AppendLine("FROM Fichier ");
                stringBuilder.AppendLine($"WHERE Name = '{dBFichier.Name}' ");
                stringBuilder.AppendLine($"AND IDParent = {dBFichier.ParentID}");

                var cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = stringBuilder.ToString();
                var result = await cmd.ExecuteReaderAsync();
                if (result.HasRows)
                {
                    return true;
                }
                return false;
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
        public async Task<DBFichier> Get(DBFichier dBFichier)
        {
            try
            {
                await connection.OpenAsync();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("SELECT Id, Name, Ordre, Image, IDParent, Collection ");
                stringBuilder.AppendLine("FROM Fichier ");
                stringBuilder.AppendLine($"WHERE Name = @name ");
                stringBuilder.AppendLine($"AND IDParent = @parent ");

                MySqlParameter Nameparam = new MySqlParameter("@name", MySqlDbType.Text);
                var cmd = new MySqlCommand();
                Nameparam.Value = dBFichier.Name;
                cmd.Parameters.Add(Nameparam);

                MySqlParameter Parentparam = new MySqlParameter("@parent", MySqlDbType.Int32);
                Parentparam.Value = dBFichier.ParentID;
                cmd.Parameters.Add(Parentparam);

                cmd.Connection = connection;
                cmd.CommandText = stringBuilder.ToString();
                var result = await cmd.ExecuteReaderAsync();
                if (result.HasRows)
                {
                    if (await result.ReadAsync())
                    {
                        dBFichier = ExtractDataFromDataReader(result);
                    }
                }

                return dBFichier;
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
        public async Task<DBFichier> Get(int Id)
        {
            try
            {
                DBFichier dBFichier = new DBFichier();
                await connection.OpenAsync();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("SELECT Id, Name, Ordre, Image, IDParent, Collection ");
                stringBuilder.AppendLine("FROM Fichier ");
                stringBuilder.AppendLine($"WHERE Id = {Id}");

                var cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = stringBuilder.ToString();
                var result = await cmd.ExecuteReaderAsync();
                if (result.HasRows)
                {
                    if (await result.ReadAsync())
                    {
                        dBFichier = ExtractDataFromDataReader(result);
                    }
                }

                return dBFichier;
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
        public async Task<IEnumerable<DBFichier>> GetAllWithoutParent()
        {
            List<DBFichier> fichiers = new List<DBFichier>();
            try
            {
                await connection.OpenAsync();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("SELECT Id, Name, Ordre, Image, IDParent, Collection ");
                stringBuilder.AppendLine("FROM Fichier ");
                stringBuilder.AppendLine("WHERE IDParent is null ");
                var cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = stringBuilder.ToString();
                var result = await cmd.ExecuteReaderAsync();
                if (result.HasRows)
                {
                    while (await result.ReadAsync())
                    {
                        fichiers.Add(ExtractDataFromDataReader(result));
                    }
                }
                return fichiers;
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
        public async Task<IEnumerable<DBFichier>> GetAll()
        {
            List<DBFichier> fichiers = new List<DBFichier>();
            try
            {
                await connection.OpenAsync();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("SELECT Id, Name, Ordre, Image, IDParent, Collection ");
                stringBuilder.AppendLine("FROM Fichier ");
                var cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = stringBuilder.ToString();
                var result = await cmd.ExecuteReaderAsync();               
                if (result.HasRows)
                {
                    while(await result.ReadAsync())
                    {
                        fichiers.Add(ExtractDataFromDataReader(result));
                    }
                }
                return fichiers;
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
        public async Task<IEnumerable<DBFichier>> GetAllByParent(Nullable<int> parentID)
        {
            List<DBFichier> fichiers = new List<DBFichier>();
            try
            {
                await connection.OpenAsync();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("SELECT Id, Name, Ordre, Image, IDParent, Collection ");
                stringBuilder.AppendLine("FROM Fichier ");
                stringBuilder.AppendLine($"WHERE IDParent = {parentID}");
                var cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = stringBuilder.ToString();
                var result = await cmd.ExecuteReaderAsync();
                if (result.HasRows)
                {
                    while (await result.ReadAsync())
                    {
                        fichiers.Add(ExtractDataFromDataReader(result));
                    }
                }
                return fichiers;
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
        public async Task Create(DBFichier newValue)
        {
            try
            {
                DBFichier dBFichier = new DBFichier();
                await connection.OpenAsync();
                StringBuilder stringBuilder = new StringBuilder();
                if (newValue.ParentID.HasValue)
                {
                    stringBuilder.AppendLine("Insert INTO Fichier( Name, Ordre, Image, Collection , IDParent) ");
                    stringBuilder.AppendLine($"Values(@name,@ordre,@rawImage,@collec,@parent) ");
                }
                else
                {
                    stringBuilder.AppendLine("Insert INTO Fichier(Name, Ordre, Image, Collection ");
                    stringBuilder.AppendLine($"Values('{newValue.Name}',{newValue.Order},@rawImage,'{newValue.Collection}') ");

                }
                var cmd = new MySqlCommand();
                cmd.Connection = connection;
                MySqlParameter Nameparam = new MySqlParameter("@name", MySqlDbType.Text);
                Nameparam.Value = newValue.Name;
                cmd.Parameters.Add(Nameparam);
                MySqlParameter Collecparam = new MySqlParameter("@collec", MySqlDbType.Text);
                Collecparam.Value = newValue.Collection;
                cmd.Parameters.Add(Collecparam);
                MySqlParameter Ordreparam = new MySqlParameter("@ordre", MySqlDbType.Int32);
                Ordreparam.Value = newValue.Order;
                cmd.Parameters.Add(Ordreparam);
                MySqlParameter Parentparam = new MySqlParameter("@parent", MySqlDbType.Int32);
                Parentparam.Value = newValue.ParentID;
                cmd.Parameters.Add(Parentparam);
                MySqlParameter Imgparam = new MySqlParameter("@rawImage", MySqlDbType.LongBlob);
                Imgparam.Value = newValue.Image;
                cmd.Parameters.Add(Imgparam);
                cmd.CommandText = stringBuilder.ToString();
                await cmd.ExecuteNonQueryAsync();

                
                Notifications.consoleNotifs.Add(new ConsoleNotif($"Inserting {newValue.Name}",Color.Green));
            }
            catch (Exception ex)
            {
                Notifications.consoleNotifs.Add(new ConsoleNotif($"Error Inserting {newValue.Name}", Color.Red));
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }
        public async Task Update(DBFichier newValue)
        {
            try
            {
                DBFichier dBFichier = new DBFichier();
                await connection.OpenAsync();
                var cmd = new MySqlCommand();
                cmd.Connection = connection;
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("Update Fichier ");
                stringBuilder.AppendLine($"SET Name = '{newValue.Name}' ");
                stringBuilder.AppendLine($",Ordre = {newValue.Order} ");
                stringBuilder.AppendLine($",Image = @rawImage ");
                stringBuilder.AppendLine($",ParentID = {newValue.ParentID} ");
                stringBuilder.AppendLine($",Collection = '{newValue.Collection}' ");
                stringBuilder.AppendLine($"WHERE Id = {newValue.ID}");

                MySqlParameter Imgparam = new MySqlParameter("@rawImage", MySqlDbType.LongBlob);
                Imgparam.Value = newValue.Image;
                cmd.Parameters.Add(Imgparam);

                cmd.CommandText = stringBuilder.ToString();
                await cmd.ExecuteNonQueryAsync();
                
                Notifications.consoleNotifs.Add(new ConsoleNotif($"Updating id:{newValue.ID} name:{newValue.Name}", Color.Green));
            }
            catch (Exception ex)
            {
                //
                Notifications.consoleNotifs.Add(new ConsoleNotif($"Error Updating id:{newValue.ID} name:{newValue.Name}", Color.Red));
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }
        public async Task<bool> Delete(DBFichier valueToDelete)
        {
            try
            {
                await connection.OpenAsync();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("DELETE FROM Fichier ");
                stringBuilder.AppendLine($"WHERE ID ={valueToDelete.ID}");
                var cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = stringBuilder.ToString();
                int result = await cmd.ExecuteNonQueryAsync();
                Notifications.consoleNotifs.Add(new ConsoleNotif($"Deleting id:{valueToDelete.ID} name:{valueToDelete.Name}", Color.Green));
                return Convert.ToBoolean(result);
            }
            catch (Exception ex)
            {
                //
                Notifications.consoleNotifs.Add(new ConsoleNotif($"Error Deleting id:{valueToDelete.ID} name:{valueToDelete.Name}", Color.Red));
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }
        private static DBFichier ExtractDataFromDataReader(System.Data.Common.DbDataReader result)
        {
            DBFichier dBFichier = new DBFichier();
            dBFichier.ID = result.GetInt32(0);
            dBFichier.Name = result.GetString(1);
            if (!string.IsNullOrEmpty(result.GetValue(2).ToString()) && !string.IsNullOrWhiteSpace(result.GetValue(2).ToString()))
                dBFichier.Order = result.GetInt32(2);            
            if (!string.IsNullOrEmpty(result.GetValue(3).ToString()) && !string.IsNullOrWhiteSpace(result.GetValue(3).ToString()))
                dBFichier.Image = ReadToEnd(result.GetStream(3));            
            if (!string.IsNullOrEmpty(result.GetValue(4).ToString()) && !string.IsNullOrWhiteSpace(result.GetValue(4).ToString()))
                dBFichier.ParentID = result.GetInt32(4);
            if (!string.IsNullOrEmpty(result.GetValue(5).ToString()) && !string.IsNullOrWhiteSpace(result.GetValue(5).ToString()))
                dBFichier.Collection = result.GetString(5);
            return dBFichier;
        }
    }
}
