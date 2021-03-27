using System;

[Serializable]
public abstract class Savable<T> where T : Savable<T>, new()
{
    protected static string SaveKey { get { return typeof(T).ToString(); } }
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                if (SaveExist())
                {
                    instance = Load();
                }
                else
                    instance = new T();
            }
            return instance;
        }
    }

    public static void Save()
    {
        SaveSystemBinary.Save(Instance, SaveKey);
    }
    public static bool SaveExist()
    {
        return SaveSystemBinary.SaveExists(SaveKey);
    }
    public static T Load() 
    {
        return SaveSystemBinary.Load<T>(SaveKey);
    }
    public static void DeleteSave()
    {
        SaveSystemBinary.DeleteSave(SaveKey);
        instance = null;
    }
}
