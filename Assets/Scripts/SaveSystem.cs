using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveMatarial(GameManager gm) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/material.sun";
        FileStream stream = new FileStream(path, FileMode.Create);

        GameData data = new GameData(gm);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameData LoadMaterial() {
        string path = Application.persistentDataPath + "/material.sun";
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();

            return data;
        } else {
            Debug.Log("cannot save");
            return null;
        }
    }
}
