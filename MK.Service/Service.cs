using System;
using System.Collections.Generic;
using System.IO;
using MK.Service.Web;
using Newtonsoft.Json;

namespace MK.Service
{
    public class Service : IDisposable
    {
        private const string JsonFilePath = "questions.json";
        private readonly WebHost _webHost;
        private int _added;
        private List<Question> _questions;

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
            string json = JsonConvert.SerializeObject(_questions);
            if (File.Exists(JsonFilePath))
            {
                File.Copy(JsonFilePath, JsonFilePath + ".bak", true);
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
                string json = r.ReadToEnd();
                _questions = JsonConvert.DeserializeObject<List<Question>>(json);
            }
        }

        public void SaveQuestion(Question question)
        {
            //Func<Question, Question> fixPathsFunc = q =>
            //{
            //    q.image1 = Path.GetFileName(q.image1);
            //    q.image2 = Path.GetFileName(q.image2);
            //    q.image3 = Path.GetFileName(q.image3);
            //    q.image4 = Path.GetFileName(q.image4);
            //    return q;
            //};

            //_questions.Add(fixPathsFunc(question));

            _questions.Add(new Func<Question, Question>(q =>
            {
                q.image1 = Path.GetFileName(q.image1);
                q.image2 = Path.GetFileName(q.image2);
                q.image3 = Path.GetFileName(q.image3);
                q.image4 = Path.GetFileName(q.image4);
                return q;
            })(question));
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