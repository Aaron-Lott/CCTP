using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public class SaveAndLoadManager : MonoBehaviour
{
    private GameDataController gameDataController;

    void Awake()
    {
        //Delete();
        gameDataController = GetComponent<GameDataController>();
        LoadData();
    }

    private string FilePath
    {
        get { return Application.persistentDataPath + "/GameData.dat"; }
    }

    public void SaveData()
    {
        FileStream file = new FileStream(FilePath, FileMode.OpenOrCreate);

        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(file, gameDataController.gameData);
        }
        catch (SerializationException exp)
        {
            Debug.Log("There was a problem with Serializing the data: " + exp.Data);
        }
        finally
        {
            file.Close();
        }
    }

    public void LoadData()
    {
        if (File.Exists(FilePath))
        {
            FileStream file = new FileStream(FilePath, FileMode.Open);

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                gameDataController.gameData = formatter.Deserialize(file) as GameData;

            }
            catch (SerializationException exp)
            {
                Debug.Log("Error with de-serializing the data: " + exp.Data);
            }
            finally
            {
                file.Close();
            }
        }
    }

    public void Delete()
    {
        try
        {
            File.Delete(FilePath);
        }
        catch (SerializationException exp)
        {
            Debug.Log("Error File cant be deleted: " + exp.Data);
        }
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    private void OnDestroy()
    {
        SaveData();
    }
}

