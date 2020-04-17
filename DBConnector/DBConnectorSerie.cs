using Common;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConnector
{
    public class DBConnectorSerie: DBConnector
    {
        public DBConnectorSerie() : base(){ }
        public async Task<DBSerie> Get(int Id)
        {
            try
            {
                DBSerie series = new DBSerie();
                await connection.OpenAsync();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("SELECT Id, Name, Image,Extension, IDParent ");
                stringBuilder.AppendLine("FROM Serie ");
                stringBuilder.AppendLine($"WHERE Id = {Id}");
                var cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = stringBuilder.ToString();
                var result = await cmd.ExecuteReaderAsync();
                if (result.HasRows)
                {
                    if (await result.ReadAsync())
                    {
                        series = ExtractDataFromDataReader(result);
                    }
                }

                return series;
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
        public async Task<IEnumerable<DBSerie>> GetAllWithoutParent()
        {
            List<DBSerie> series = new List<DBSerie>();
            try
            {
                await connection.OpenAsync();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("SELECT Id, Name, Image,Extension, IDParent ");
                stringBuilder.AppendLine("FROM Serie ");
                stringBuilder.AppendLine("WHERE IDParent is null ");
                var cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = stringBuilder.ToString();
                var result = await cmd.ExecuteReaderAsync();
                if (result.HasRows)
                {
                    while (await result.ReadAsync())
                    {
                        series.Add(ExtractDataFromDataReader(result));
                    }
                }
                return series;
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
        public async Task<IEnumerable<DBSerie>> GetAll()
        {
            List<DBSerie> series = new List<DBSerie>();
            try
            {
                await connection.OpenAsync();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("SELECT Id, Name, Image,Extension, IdParent ");
                stringBuilder.AppendLine("FROM Serie ");
                var cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = stringBuilder.ToString();
                var result = await cmd.ExecuteReaderAsync();
                if (result.HasRows)
                {
                    while (await result.ReadAsync())
                    {
                        series.Add(ExtractDataFromDataReader(result));
                    }
                }
                return series;
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
        public async Task<IEnumerable<int>> GetAllDescendant(DBSerie serie)
        {
            List<int> descendants = new List<int>();
            try
            {
                await connection.OpenAsync();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine($"select id, name, IdParent ");
                stringBuilder.AppendLine($"from (select * from Serie order by IdParent, id) Serie, ");
                stringBuilder.AppendLine($"(select @pv :={serie.ID}) initialisation ");
                stringBuilder.AppendLine($"where find_in_set(IdParent,@pv) ");
                stringBuilder.AppendLine($"and length(@pv := concat(@pv, ',',id)) ");
                var cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = stringBuilder.ToString();
                var result = await cmd.ExecuteReaderAsync();
                if (result.HasRows)
                {
                    while (await result.ReadAsync())
                    {
                        descendants.Add(result.GetInt32(0));
                    }
                }
                return descendants;
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
        public async Task<DBSerie> FirstAncestor(DBSerie node)
        {
            DBSerie serie = new DBSerie();
            if (node.ParentID.HasValue == false)
            { return node; }
            else
            {
                var ancestorIDs = await GetAllAncestor(node);
                List<DBSerie> ancestors = new List<DBSerie>();
                foreach(int id in ancestorIDs)
                {
                    ancestors.Add(await Get(id));
                }
                return ancestors.FirstOrDefault(x => x.ParentID.HasValue == false);
            }
        }
        public async Task<IEnumerable<int>> GetAllAncestor(DBSerie serie)
        {
            List<int> ancestor = new List<int>();
            try
            {
                await connection.OpenAsync();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine($"SELECT @r as _id, ");
                stringBuilder.AppendLine($"( ");
                stringBuilder.AppendLine($"SELECT @r := IdParent ");
                stringBuilder.AppendLine($"FROM Serie ");
                stringBuilder.AppendLine($"WHERE id= _id");
                stringBuilder.AppendLine($") ");
                stringBuilder.AppendLine($"as parent, ");
                stringBuilder.AppendLine($"@l := @l+1 as lvl ");
                stringBuilder.AppendLine($"FROM( ");
                stringBuilder.AppendLine($"SELECT @r := {serie.ID}, ");
                stringBuilder.AppendLine($"@l :=0, ");
                stringBuilder.AppendLine($"@cl :=0) vars, ");
                stringBuilder.AppendLine($"Serie h ");
                stringBuilder.AppendLine($"WHERE @r <> 0");
                var cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = stringBuilder.ToString();
                var result = await cmd.ExecuteReaderAsync();
                if (result.HasRows)
                {
                    while (await result.ReadAsync())
                    {
                        ancestor.Add(int.Parse(result.GetString(0)));
                    }
                }
                return ancestor;
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
        public async Task<IEnumerable<DBSerie>> GetAllByParent(Nullable<int> parentID)
        {
            List<DBSerie> series = new List<DBSerie>();
            try
            {
                await connection.OpenAsync();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("SELECT Id, Name, Image,Extension,  IdParent ");
                stringBuilder.AppendLine("FROM Serie ");
                stringBuilder.AppendLine($"WHERE IDParent = {parentID}");
                var cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = stringBuilder.ToString();
                var result = await cmd.ExecuteReaderAsync();
                if (result.HasRows)
                {
                    while (await result.ReadAsync())
                    {
                        series.Add(ExtractDataFromDataReader(result));
                    }
                }
                return series;
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
        public async Task Create(DBSerie newValue)
        {
            try
            {
                DBSerie dbSerie = new DBSerie();
                await connection.OpenAsync();
                StringBuilder stringBuilder = new StringBuilder();
                var cmd = new MySqlCommand();
                cmd.Connection = connection;
                if (newValue.ParentID.HasValue)
                {
                    stringBuilder.AppendLine("Insert INTO Serie(Name, Image,Extension,  IdParent) ");
                    stringBuilder.AppendLine($"Values(@name,@rawImage,@exten,@parentid) ");
                    MySqlParameter Parentparam = new MySqlParameter("@parentid", MySqlDbType.Int32);
                    Parentparam.Value = newValue.ParentID;
                    cmd.Parameters.Add(Parentparam);
                }
                else
                {
                    stringBuilder.AppendLine("Insert INTO Serie(Name, Image,Extension ) ");
                    stringBuilder.AppendLine($"Values(@name,@rawImage,@exten) ");
                }
                MySqlParameter nameparam = new MySqlParameter("@name", MySqlDbType.Text);
                nameparam.Value = newValue.Name;
                cmd.Parameters.Add(nameparam);
                MySqlParameter extenparam = new MySqlParameter("@exten", MySqlDbType.Text);
                extenparam.Value = newValue.Extension;
                cmd.Parameters.Add(extenparam);
                MySqlParameter Imgparam = new MySqlParameter("@rawImage", MySqlDbType.LongBlob);
                Imgparam.Value = newValue.Image;
                cmd.Parameters.Add(Imgparam);
                
                cmd.CommandText = stringBuilder.ToString();
                
                Notifications.consoleNotifs.Add(new ConsoleNotif($"Inserting {newValue.Name}", Color.Green));
                await cmd.ExecuteNonQueryAsync();
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
        public async Task Update(DBSerie newValue)
        {            
            try
            {
                DBSerie dbSerie = new DBSerie();
                
                await connection.OpenAsync();
                var cmd = new MySqlCommand();
                cmd.Connection = connection;
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("Update Serie ");
                stringBuilder.AppendLine($"SET Name = @name ");
                stringBuilder.AppendLine($",Image = @rawImage ");
                stringBuilder.AppendLine($",Extension = @exten ");
                stringBuilder.AppendLine($",IdParent = @parentid ");
                stringBuilder.AppendLine($"WHERE Id = @id");

                MySqlParameter IDparam = new MySqlParameter("@id", MySqlDbType.Int32);
                IDparam.Value = newValue.ID;
                cmd.Parameters.Add(IDparam);
                MySqlParameter Parentparam = new MySqlParameter("@parentid", MySqlDbType.Int32);
                Parentparam.Value = newValue.ParentID;
                cmd.Parameters.Add(Parentparam);
                MySqlParameter nameparam = new MySqlParameter("@name", MySqlDbType.Text);
                nameparam.Value = newValue.Name;
                cmd.Parameters.Add(nameparam);
                MySqlParameter extenparam = new MySqlParameter("@exten", MySqlDbType.Text);
                extenparam.Value = newValue.Extension;
                cmd.Parameters.Add(extenparam);
                MySqlParameter Imgparam = new MySqlParameter("@rawImage", MySqlDbType.LongBlob);
                Imgparam.Value = newValue.Image;
                cmd.Parameters.Add(Imgparam);

                cmd.CommandText = stringBuilder.ToString();
                await cmd.ExecuteNonQueryAsync();
                
                Notifications.consoleNotifs.Add(new ConsoleNotif($"Updating id:{newValue.ID} name:{newValue.Name}", Color.Green));
            }
            catch (Exception ex)
            {
                Notifications.consoleNotifs.Add(new ConsoleNotif($"Error Updating id:{newValue.ID} name:{newValue.Name}", Color.Red));
                //throw ex;
            }
            finally
            {
                connection.Close();
            }
        }
        public async Task<bool> Delete(DBSerie valueToDelete)
        {
            try
            {
                await connection.OpenAsync();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("DELETE FROM Serie ");
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
                Notifications.consoleNotifs.Add(new ConsoleNotif($"Error Deleting id:{valueToDelete.ID} name:{valueToDelete.Name}", Color.Red));
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }
        private static DBSerie ExtractDataFromDataReader(System.Data.Common.DbDataReader result)
        {
            DBSerie dbSerie = new DBSerie();
            dbSerie.ID = result.GetInt32(0);
            dbSerie.Name = result.GetString(1);
            if (!string.IsNullOrEmpty(result.GetValue(2).ToString()) && !string.IsNullOrWhiteSpace(result.GetValue(2).ToString()))
                dbSerie.Image = ReadToEnd(result.GetStream(2));
            dbSerie.Extension = result.GetString(3);
            if (!string.IsNullOrEmpty(result.GetValue(4).ToString()) && !string.IsNullOrWhiteSpace(result.GetValue(4).ToString()))
                dbSerie.ParentID = result.GetInt32(4);
            return dbSerie;
        }
        public static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
    }
}
