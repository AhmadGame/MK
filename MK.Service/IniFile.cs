using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace MK.Service
{
    public class IniFile
    {
        private readonly string _fileName;
        [DllImport("kernel32")]
        static extern long WritePrivateProfileString(string section, string key, string value, string filePath);

        [DllImport("kernel32")]
        static extern int GetPrivateProfileString(string section, string key, string defaultValue, StringBuilder retVal, int size, string filePath);

        public IniFile(string fileName = null)
        {
            if (String.IsNullOrWhiteSpace(fileName))
            {
                fileName = Path.Combine(Directory.GetCurrentDirectory(), "questions.ini");
            }
            _fileName = fileName;
        }

        public string Read(string section, string key)
        {
            var retVal = new StringBuilder(255);
            GetPrivateProfileString(section, key, "", retVal, 255, _fileName);
            return retVal.ToString();
        }

        public void Write(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, _fileName);
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