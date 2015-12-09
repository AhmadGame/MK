using System;
using System.Collections.Generic;
using System.IO;
using MkImporter.Models;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;

namespace MkImporter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            switch (args[0])
            {
                case "import":
                    Import(args[1], args[2]);
                    break;
                case "create":
                    CreateDb();
                    break;
            }
            Console.WriteLine("");
            Console.WriteLine("Done!");
            Console.ReadKey();
        }

        private static void CreateDb()
        {
            throw new NotImplementedException();
        }

        private static void Import(string file, string language)
        {
            if (!File.Exists(file))
            {
                Console.WriteLine($"Cannot find file {file}");
            }

            try
            {
                // Open the text file using a stream reader.
                using (var sr = new StreamReader(file))
                {
                    // Read the stream to a string, and write the string to the console.
                    var line = sr.ReadToEnd();
                    var questions = JsonConvert.DeserializeObject<List<IoQuestion>>(line);
                    DoImport(questions, language);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void DoImport(List<IoQuestion> questions, string language)
        {
            using (
                var conn = new NpgsqlConnection("Host=localhost;Username=postgres;Password=BevHills90210;Database=mk"))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"INSERT INTO folder (language) VALUES (@language) RETURNING id";
                    cmd.Parameters.AddWithValue("@language", NpgsqlDbType.Text, language);
                    var folderId = cmd.ExecuteScalar();

                    var commandText = string.Empty;
                    var count = 0;
                    foreach (var ioQuestion in questions)
                    {
                        commandText +=
                            "INSERT INTO question (folderid, title, answer1, answer2, answer3, answer4, correct, explain1, explain2, explain3, explain4, image1, image2, image3, image4) " +
                            $"VALUES (@folderid{count}, @title{count}, @answer1{count}, @answer2{count}, @answer3{count}, @answer4{count}, @correct{count}, @explain1{count}, @explain2{count}, @explain3{count}, @explain4{count}, @image1{count}, @image2{count}, @image3{count}, @image4{count}); ";

                        cmd.Parameters.AddWithValue($"@folderid{count}", NpgsqlDbType.Bigint, folderId);
                        cmd.Parameters.AddWithValue($"@title{count}", NpgsqlDbType.Text, ioQuestion.Title);
                        cmd.Parameters.AddWithValue($"@answer1{count}", NpgsqlDbType.Text, ioQuestion.Answer1);
                        cmd.Parameters.AddWithValue($"@answer2{count}", NpgsqlDbType.Text, ioQuestion.Answer2);
                        cmd.Parameters.AddWithValue($"@answer3{count}", NpgsqlDbType.Text, ioQuestion.Answer3);
                        cmd.Parameters.AddWithValue($"@answer4{count}", NpgsqlDbType.Text, ioQuestion.Answer4);
                        cmd.Parameters.AddWithValue($"@correct{count}", NpgsqlDbType.Integer, ioQuestion.Correct);
                        cmd.Parameters.AddWithValue($"@explain1{count}", NpgsqlDbType.Text, ioQuestion.Explain1);
                        cmd.Parameters.AddWithValue($"@explain2{count}", NpgsqlDbType.Text, ioQuestion.Explain2);
                        cmd.Parameters.AddWithValue($"@explain3{count}", NpgsqlDbType.Text, ioQuestion.Explain3);
                        cmd.Parameters.AddWithValue($"@explain4{count}", NpgsqlDbType.Text, ioQuestion.Explain4);
                        cmd.Parameters.AddWithValue($"@image1{count}", NpgsqlDbType.Text, ioQuestion.Image1);
                        cmd.Parameters.AddWithValue($"@image2{count}", NpgsqlDbType.Text, ioQuestion.Image2);
                        cmd.Parameters.AddWithValue($"@image3{count}", NpgsqlDbType.Text, ioQuestion.Image3);
                        cmd.Parameters.AddWithValue($"@image4{count}", NpgsqlDbType.Text, ioQuestion.Image4);

                        count++;
                        Console.Write(".");
                    }
                    cmd.CommandText = commandText;
                    cmd.ExecuteNonQuery();

                    conn.Close();
                }
            }
        }
    }
}