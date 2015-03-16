using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
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

        public void SaveQuestion(QuestionObject question)
        {
            var index = GetNewIndex();
            var iniFile = new IniFile();
            foreach (var prop in question.GetType().GetProperties())
            {
                iniFile.Write(index, prop.Name, prop.GetValue(question).ToString());
            }

            iniFile.Write("index", "index", index);
        }

        private string GetNewIndex()
        {
            var iniFile = new IniFile();
            var latestIndex = iniFile.Read("index", "index");
            int nextIndex;
            if (int.TryParse(latestIndex, out nextIndex))
            {
                nextIndex++;
                return nextIndex.ToString(CultureInfo.InvariantCulture);
            }
            
            return "";
        }

        public List<QuestionObject> GetQuestions(int number)
        {
            var questions = new List<QuestionObject>();
            var iniFile = new IniFile();
            for (int i = 0; i < number; i++)
            {
                var question = new QuestionObject();
                foreach (var prop in question.GetType().GetProperties())
                {
                    prop.SetValue(question, iniFile.Read(i.ToString(CultureInfo.InvariantCulture), prop.Name));
                }
                questions.Add(question);
            }

            return questions;
        }
    }

    public class QuestionObject
    {
        // ReSharper disable InconsistentNaming
        public string question { get; set; }
        public string answer1 { get; set; }
        public string answer2 { get; set; }
        public string answer3 { get; set; }
        public string answer4 { get; set; }
        public int correct { get; set; }
        public string explain1 { get; set; }
        public string explain2 { get; set; }
        public string explain3 { get; set; }
        public string explain4 { get; set; }
        public string image1 { get; set; }
        public string image2 { get; set; }
        public string image3 { get; set; }
        public string image4 { get; set; }
        public string page_reference { get; set; }
        // ReSharper restore InconsistentNaming

    }   

    public class IniFile   // revision 10
    {
        private readonly string _path;
        [DllImport("kernel32")]
        static extern long WritePrivateProfileString(string section, string key, string value, string filePath);

        [DllImport("kernel32")]
        static extern int GetPrivateProfileString(string section, string key, string defaultValue, StringBuilder retVal, int size, string filePath);

        public IniFile(string iniPath = null)
        {
            if (String.IsNullOrWhiteSpace(iniPath))
            {
                iniPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            }
            _path = new FileInfo(iniPath + ".ini").FullName;
        }

        public string Read(string section, string key)
        {
            var retVal = new StringBuilder(255);
            GetPrivateProfileString(section, key, "", retVal, 255, _path);
            return retVal.ToString();
        }

        public void Write(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, _path);
        }

        public void DeleteKey(string section, string key)
        {
            Write(section, key, null);
        }

        public void DeleteSection(string section)
        {
            Write(section, null, null);
        }

        public bool KeyExists(string section, string key)
        {
            return Read(section, key).Length > 0;
        }
    }
}
