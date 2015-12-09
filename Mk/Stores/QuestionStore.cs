using System.Collections.Generic;
using Mk.Models;
using Npgsql;
using NpgsqlTypes;

namespace Mk.Stores
{
    public class QuestionStore : Store
    {
        public List<Question> GetByFolderId(long folderId)
        {
            var questions = new List<Question>();
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM question WHERE folderid = @folderid";
                    cmd.Parameters.AddWithValue("@folderid", folderId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            questions.Add(new Question
                            {
                                Id = reader.GetInt64(reader.GetOrdinal("id")),
                                Title = reader.GetString(reader.GetOrdinal("title")),
                                Answer1 = reader.GetString(reader.GetOrdinal("answer1")),
                                Answer2 = reader.GetString(reader.GetOrdinal("answer2")),
                                Answer3 = reader.GetString(reader.GetOrdinal("answer3")),
                                Answer4 = reader.GetString(reader.GetOrdinal("answer4")),
                                Explain1 = reader.GetString(reader.GetOrdinal("explain1")),
                                Explain2 = reader.GetString(reader.GetOrdinal("explain2")),
                                Explain3 = reader.GetString(reader.GetOrdinal("explain3")),
                                Explain4 = reader.GetString(reader.GetOrdinal("explain4")),
                                Image1 = reader.GetString(reader.GetOrdinal("image1")),
                                Image2 = reader.GetString(reader.GetOrdinal("image2")),
                                Image3 = reader.GetString(reader.GetOrdinal("image3")),
                                Image4 = reader.GetString(reader.GetOrdinal("image4")),
                                Correct = reader.GetInt32(reader.GetOrdinal("correct"))
                            });
                        }
                    }
                }
            }
            return questions;
        }

        public void Add(long folderId, Question question)
        {
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText =
                            "INSERT INTO question (folderid, title, answer1, answer2, answer3, answer4, correct, explain1, explain2, explain3, explain4, image1, image2, image3, image4) " +
                            "VALUES (@folderid, @title, @answer1, @answer2, @answer3, @answer4, @correct, @explain1, @explain2, @explain3, @explain4, @image1, @image2, @image3, @image4); ";

                    cmd.Parameters.AddWithValue("@folderid", NpgsqlDbType.Bigint, folderId);
                    cmd.Parameters.AddWithValue("@title", NpgsqlDbType.Text, question.Title);
                    cmd.Parameters.AddWithValue("@answer1", NpgsqlDbType.Text, question.Answer1);
                    cmd.Parameters.AddWithValue("@answer2", NpgsqlDbType.Text, question.Answer2);
                    cmd.Parameters.AddWithValue("@answer3", NpgsqlDbType.Text, question.Answer3);
                    cmd.Parameters.AddWithValue("@answer4", NpgsqlDbType.Text, question.Answer4);
                    cmd.Parameters.AddWithValue("@correct", NpgsqlDbType.Integer, question.Correct);
                    cmd.Parameters.AddWithValue("@explain1", NpgsqlDbType.Text, question.Explain1);
                    cmd.Parameters.AddWithValue("@explain2", NpgsqlDbType.Text, question.Explain2);
                    cmd.Parameters.AddWithValue("@explain3", NpgsqlDbType.Text, question.Explain3);
                    cmd.Parameters.AddWithValue("@explain4", NpgsqlDbType.Text, question.Explain4);
                    cmd.Parameters.AddWithValue("@image1", NpgsqlDbType.Text, question.Image1);
                    cmd.Parameters.AddWithValue("@image2", NpgsqlDbType.Text, question.Image2);
                    cmd.Parameters.AddWithValue("@image3", NpgsqlDbType.Text, question.Image3);
                    cmd.Parameters.AddWithValue("@image4", NpgsqlDbType.Text, question.Image4);

                    cmd.ExecuteNonQuery();

                }
            }
        }
    }
}
