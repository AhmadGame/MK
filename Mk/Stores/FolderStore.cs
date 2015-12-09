using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Mk.Models;
using Npgsql;
using NpgsqlTypes;

namespace Mk.Stores
{
    public class FolderStore : Store
    {
        public List<Folder> All()
        {
            var folders = new List<Folder>();
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM folder";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            folders.Add(new Folder
                            {
                                Id = reader.GetInt64(reader.GetOrdinal("id")),
                                Language = Util.LanguageFromCode(reader.GetString(reader.GetOrdinal("language"))),
                                Name = reader.GetString(reader.GetOrdinal("name"))
                            });
                        }
                    }
                }
            }

            return folders;
        }

        public List<Folder> GetByLanguage(string language)
        {
            var folders = new List<Folder>();
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM folder WHERE language = @language";
                    cmd.Parameters.AddWithValue("@language", Util.CodeFromLanguage(language));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            folders.Add(new Folder
                            {
                                Id = reader.GetInt64(reader.GetOrdinal("id")),
                                Language = Util.LanguageFromCode(reader.GetString(reader.GetOrdinal("language"))),
                                Name = reader.GetString(reader.GetOrdinal("name"))
                            });
                        }
                    }
                }
            }

            return folders;
        }

        public List<Folder> GetWithQuestionCountByLanguage(string language)
        {
            var folders = new List<Folder>();
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT fid, fname, Count(qid) as count " +
                                      "FROM (SELECT f.name as fname, q.id as qid, f.id as fid, f.language as lang " +
                                            "FROM folder f INNER JOIN question q " +
                                            "ON q.folderid = f.id) as x " +
                                      "WHERE lang = @language " +
                                      "GROUP BY fname, x.fid";
                    cmd.Parameters.AddWithValue("@language", Util.CodeFromLanguage(language));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            folders.Add(new Folder
                            {
                                Id = reader.GetInt64(reader.GetOrdinal("fid")),
                                QuestionCount = reader.GetInt64(reader.GetOrdinal("count")),
                                Name = reader.GetString(reader.GetOrdinal("fname"))
                            });
                        }
                    }
                }
            }

            return folders;
        }

        public long GetFolderToAddQuestion(string language)
        {
            var folders = GetWithQuestionCountByLanguage(language);
            var nonFullFolder = folders.FirstOrDefault(f => f.QuestionCount < 70);
            if (nonFullFolder != null)
            {
                return nonFullFolder.Id;
            }

            folders.Sort((f, f1) => string.Compare(f.Name, f1.Name, StringComparison.Ordinal));
            return Add(new Folder {Language = language, Name = GetName(folders.LastOrDefault())});
        }

        private string GetName(Folder folder)
        {
            if (folder == null)
            {
                return "Folder 1";
            }
            int i;
            return int.TryParse(Regex.Match(folder.Name, @"\d+").Value, out i) ? $"Folder {i}" : "Folder 1000";
        }

        private long Add(Folder folder)
        {
            long folderId;
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"INSERT INTO folder (name, language) VALUES (@name, @language) RETURNING id";
                    cmd.Parameters.AddWithValue("@name", NpgsqlDbType.Text, folder.Name);
                    cmd.Parameters.AddWithValue("@language", NpgsqlDbType.Text, folder.Language);
                    folderId = (long) cmd.ExecuteScalar();
                }
            }
            return folderId;
        }
    }
}