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
        public Service()
        {
            _webHost = new WebHost();            
        }
        public void Dispose()
        {
            _webHost.Dispose();
        }

        public void Start()
        {
            LoadJson();
            _webHost.Start(this);
        }

        public void LoadJson()
        {
            using (var r = new StreamReader("questions.json"))
            {
                string json = r.ReadToEnd();
                _questions = JsonConvert.DeserializeObject<List<Question>>(json);
            }
        }

        public void SaveQuestion(Question question)
        {
            _questions.Add(question);
        }

        public List<Question> GetQuestions(int number)
        {
            var questions = new List<Question>();
            var random = new Random();
            for (int i = 0; i < number; i++)
            {
                Question q;
                do
                {
                    int index = random.Next(_questions.Count);
                    q = _questions[index];
                } while (questions.Contains(q));

                questions.Add(q);
            }
            return questions;
        }
    }
}
