using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ImagesRepository
{
    public class ImageRepository
    {
        private string _connectionString;
        public ImageRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int Add(string title, string password, string imagePath)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Uploads (Title, Path, Password, Views) VALUES (@title, @path,@Password,0)SELECT SCOPE_IDENTITY()";
            cmd.Parameters.AddWithValue("@title", title);
            cmd.Parameters.AddWithValue("@path", imagePath);
            cmd.Parameters.AddWithValue("@Password", password);
            conn.Open();
            int ID= (int)(decimal)cmd.ExecuteScalar();
            return ID;
        }

        
        public string GetImagePasswordById(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Password FROM Uploads WHERE id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            return (string)cmd.ExecuteScalar();
           
        }
        public UploadedImage GetimageById(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Path, Title, views FROM  uploads where id=@id";
            conn.Open();
            cmd.Parameters.AddWithValue("@id", id);
            var reader = cmd.ExecuteReader();
            reader.Read();
            return new UploadedImage
            {
                ImagePath = (string)reader["Path"],
                Views = (int)reader["views"],
                Title = (string)reader["Title"]
            };


        }
        public void IncrementView(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"Update uploads set Views=Views+1 Where Id=@id";

            cmd.Parameters.AddWithValue("@id", id);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
