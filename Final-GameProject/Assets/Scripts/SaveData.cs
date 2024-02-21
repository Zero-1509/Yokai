using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveData{

    public static void Save(BasicScript PlayerData)
    {
        BinaryFormatter ConvertToBinary = new BinaryFormatter();
        string path = Application.persistentDataPath +"/Error.404";
        FileStream DataAll = new FileStream(path, FileMode.Create);

        SavingData data = new SavingData(PlayerData);
        ConvertToBinary.Serialize(DataAll,data);
        DataAll.Close();

    }
    public static SavingData Load()
    {
        string path = Application.persistentDataPath + "/Error.404";
        if(File.Exists(path))
        {
            BinaryFormatter ConvertToBinary = new BinaryFormatter();
            FileStream DataAll = new FileStream(path, FileMode.Open);

            SavingData data = ConvertToBinary.Deserialize(DataAll) as SavingData;
            DataAll.Close();
            return data;
        }
        else
        {
            Debug.LogError("File Not Found!!!");
            return null;
        }
    }

}
