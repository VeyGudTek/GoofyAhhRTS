using System;
using System.IO;
using UnityEngine;

namespace Source.Shared.Services
{
    public static class DataAccessService
    {
        private const string FileType = ".json";
        private static string GetFilePath(string fileName) => Path.GetFullPath(fileName + FileType, Application.persistentDataPath);

        public static void WriteData(string fileName, object data)
        {
            string filePath = GetFilePath(fileName);
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                string serializedData = JsonUtility.ToJson(data, true);
                sw.Write(serializedData);
            }
        }

        public static T ReadData<T>(string fileName) where T : class
        {
            try
            {
                string filePath = GetFilePath(fileName);
                T data;
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string stringData = sr.ReadToEnd();
                    data = JsonUtility.FromJson<T>(stringData);
                }
                return data;
            }
            catch (Exception e)
            {
                if (e is FileNotFoundException)
                {
                    return Activator.CreateInstance<T>();
                }
                throw;
            }

        }
    }
}

