using System;
using System.Collections.Generic;
using System.IO;
using MK.Service.Web;
using Newtonsoft.Json;

namespace MK.Service
{
    public class Service : IDisposable
    {
        private readonly WebHost _webHost;
        private List<Question> _questions;
        private int _added;
        private const string JsonFilePath = "questions.json";

        public Service()
        {
            _webHost = new WebHost();            
        }
        public void Dispose()
        {
            SaveJson();
            _webHost.Dispose();
        }

        private void SaveJson()
        {
            var json = JsonConvert.SerializeObject(_questions);
            if (File.Exists(JsonFilePath))
            {
                File.Copy(JsonFilePath, JsonFilePath + ".bak", overwrite: true);
                File.Delete(JsonFilePath);
            }

            File.WriteAllText(JsonFilePath, json);
        }

        public void Start()
        {
            LoadJson();
            _webHost.Start(this);
        }

        public void LoadJson()
        {
            using (var r = new StreamReader(JsonFilePath))
            {
                var json = r.ReadToEnd();
                _questions = JsonConvert.DeserializeObject<List<Question>>(json);
            }
        }

        public void SaveQuestion(Question question)
        {
            _questions.Add(question);
            _added++;

            if (_added >= 10)
            {
                SaveJson();
            }
        }

        public List<Question> GetQuestions(int number)
        {
            var questions = new List<Question>();
            var random = new Random();
            for (int i = 0; i < number; i++)
            {
                Question question;
                do
                {
                    int index = random.Next(_questions.Count);
                    question = _questions[index];
                } while (questions.Contains(question));

                questions.Add(question);
            }
            return questions;
        }
    }
}
