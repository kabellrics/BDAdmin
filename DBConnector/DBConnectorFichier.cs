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
        public async Task<DBFichier> Get(int Id)
        {
            try
            {
                DBFichier dBFichier = new DBFichier();
                await connection.OpenAsync();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("SELECT Id, Name, Ordre, Data, Extension, Année, Image,Imgextension, IDParent, IdCollection ");
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
                stringBuilder.AppendLine("SELECT Id, Name, Ordre, Data, Extension, Année, Image, Imgextension, IDParent, IdCollection ");
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
                stringBuilder.AppendLine("SELECT Id, Name, Ordre, Data, Extension, Année, Image, Imgextension, IDParent, IdCollection ");
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
                stringBuilder.AppendLine("SELECT Id, Name, Ordre, Data, Extension, Année, Image, Imgextension, IDParent, IdCollection ");
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
                    stringBuilder.AppendLine("Insert INTO Fichier(Name, Ordre, Data, Extension, Année, Image, Imgextension, IDParent, IdCollection) ");
                    stringBuilder.AppendLine($"Values('{newValue.Name}',{newValue.Order},@rawData,'{newValue.Extension}',{newValue.Year},@rawImage,{newValue.ImgExtension},{newValue.ParentID},{newValue.CollectionID}) ");
                }
                else
                {
                    stringBuilder.AppendLine("Insert INTO Fichier(Name, Ordre, Data, Extension, Année, Image)Imgextension,  IdCollection ");
                    stringBuilder.AppendLine($"Values('{newValue.Name}',{newValue.Order},@rawData,'{newValue.Extension}',{newValue.Year},@rawImage,{newValue.ImgExtension},{newValue.CollectionID}) ");

                }
                var cmd = new MySqlCommand();
                cmd.Connection = connection;
                MySqlParameter Imgparam = new MySqlParameter("@rawImage", MySqlDbType.LongBlob);
                Imgparam.Value = newValue.Image;
                cmd.Parameters.Add(Imgparam);
                MySqlParameter Dataparam = new MySqlParameter("@rawData", MySqlDbType.LongBlob);
                Dataparam.Value = newValue.Image;
                cmd.Parameters.Add(Dataparam);
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
                stringBuilder.AppendLine($",Extension = '{newValue.Extension}' ");
                stringBuilder.AppendLine($",Year = {newValue.Year} ");
                stringBuilder.AppendLine($",Image = @rawImage ");
                stringBuilder.AppendLine($",Imgextension = {newValue.ImgExtension}");
                stringBuilder.AppendLine($",Data = @rawData ");
                stringBuilder.AppendLine($",ParentID = {newValue.ParentID} ");
                stringBuilder.AppendLine($",IDCollection = {newValue.CollectionID} ");
                stringBuilder.AppendLine($"WHERE Id = {newValue.ID}");

                MySqlParameter Imgparam = new MySqlParameter("@rawImage", MySqlDbType.LongBlob);
                Imgparam.Value = newValue.Image;
                cmd.Parameters.Add(Imgparam);
                MySqlParameter Dataparam = new MySqlParameter("@rawData", MySqlDbType.LongBlob);
                Dataparam.Value = newValue.Image;
                cmd.Parameters.Add(Dataparam);

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
                dBFichier.Data = ReadToEnd(result.GetStream(3));
            if (!string.IsNullOrEmpty(result.GetValue(4).ToString()) && !string.IsNullOrWhiteSpace(result.GetValue(4).ToString()))
                dBFichier.Extension = result.GetString(4);
            if (!string.IsNullOrEmpty(result.GetValue(5).ToString()) && !string.IsNullOrWhiteSpace(result.GetValue(5).ToString()))
                dBFichier.Year = result.GetInt32(5);
            if (!string.IsNullOrEmpty(result.GetValue(6).ToString()) && !string.IsNullOrWhiteSpace(result.GetValue(6).ToString()))
                dBFichier.Image = ReadToEnd(result.GetStream(6));
            dBFichier.ImgExtension = result.GetString(7);
            if (!string.IsNullOrEmpty(result.GetValue(8).ToString()) && !string.IsNullOrWhiteSpace(result.GetValue(8).ToString()))
                dBFichier.ParentID = result.GetInt32(8);
            if (!string.IsNullOrEmpty(result.GetValue(9).ToString()) && !string.IsNullOrWhiteSpace(result.GetValue(9).ToString()))
                dBFichier.CollectionID = result.GetInt32(9);
            return dBFichier;
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
