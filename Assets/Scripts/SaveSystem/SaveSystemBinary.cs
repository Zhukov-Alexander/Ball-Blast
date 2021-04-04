using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystemBinary
{
    private static readonly string path = Application.persistentDataPath + "\\savings";
    public static byte[] SerializeState<T>(T objectToSave)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream())
        {
            formatter.Serialize(ms, objectToSave);
            return ms.GetBuffer();
        }
    }
    public static T DeserializeState<T>(byte[] data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream(data))
        {
            return (T)formatter.Deserialize(ms);
        }
    }

    public static void Save<T>(T objectToSave, string key)
    {
        Directory.CreateDirectory(path);
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream fileStream = new FileStream(path + key + ".txt", FileMode.Create))
        {
            formatter.Serialize(fileStream, objectToSave);
        }
    }

    public static T Load<T>(string key)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(path + key + ".txt", FileMode.Open);
        T returnValue = (T)formatter.Deserialize(fileStream);
        fileStream.Dispose();
        return returnValue;
    }

    public static bool SaveExists(string key)
    {
        string pathh = path + key + ".txt";
        return File.Exists(pathh);
    }

    public static void DeleteSave(string key)
    {
        string pathh = path + key + ".txt";
        File.Delete(pathh);
    }

    public static void DeleteAllSaveFiles()
    {
        DirectoryInfo directory = new DirectoryInfo(path);
        if (directory.Exists)
        {
            directory.Delete(true);
        }
    }
}