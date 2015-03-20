using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MK.Service.Web;

namespace MK.Service
{
    public class Service : IDisposable
    {
        private readonly WebHost _webHost;

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
            _webHost.Start(this);
        }

        public void SaveQuestion(Question question)
        {
            var index = (GetLastIndex() + 1).ToString(CultureInfo.InvariantCulture);
            var iniFile = new IniFile();
            foreach (var prop in question.GetType().GetProperties())
            {
                iniFile.Write(index, prop.Name, prop.GetValue(question).ToString());
            }

            iniFile.Write("index", "end", index);
        }

        private int GetLastIndex()
        {
            var iniFile = new IniFile();
            int latestIndex;
            return int.TryParse(iniFile.Read("index", "end"), out latestIndex) ? latestIndex : 0;
        }

        private int GetFirstIndex()
        {
            var iniFile = new IniFile();
            int firstIndex;
            return int.TryParse(iniFile.Read("index", "start"), out firstIndex) ? firstIndex : 0;
        }

        public List<Question> GetQuestions(int number)
        {
            var indeces = GetRandomQuestions(number);
            var questions = new List<Question>();
            var iniFile = new IniFile();
            foreach (var index in indeces)
            {
                var question = new Question {Id = index};
                foreach (var prop in question.GetType().GetProperties().Where(prop => prop.Name != "Id"))
                {
                    prop.SetValue(question, iniFile.Read(index.ToString(CultureInfo.InvariantCulture), prop.Name));
                }
                if (!String.IsNullOrWhiteSpace(question.question))
                {
                    questions.Add(question);
                }
            }

            return questions;
        }

        private IEnumerable<int> GetRandomQuestions(int number)
        {
            var indeces = new List<int>();
            var min = GetFirstIndex();
            var max = GetLastIndex();

            var random = new Random();
            for (int i = 0; i < number; i++)
            {
                int index;
                do
                {
                    index = random.Next(min, max);
                } while (indeces.Contains(index));

                indeces.Add(index);
            }

            return indeces;
        }
    }
}
