using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class EndOfLevel : MonoBehaviour {

    [SerializeField] private GameObject endOfLevelUI;

    [SerializeField] private int currentLevel;

    public static bool goalReached;

    private void Awake()
    {
        goalReached = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Guardian")) {
            endOfLevelUI.SetActive(true);
            goalReached = true;

            SaveLevelLockData();
        }
    }

    private void SaveLevelLockData()
    {
        if(File.Exists(Application.persistentDataPath + "/levelLockData.dat")) {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file;
            LevelLockData data;

            file = File.Open(Application.persistentDataPath + "/levelLockData.dat", FileMode.Open);
            data = (LevelLockData)binaryFormatter.Deserialize(file);
            file.Close();

            file = File.Create(Application.persistentDataPath + "/levelLockData.dat");

            if (data.currentLevelUnlocked <= currentLevel) {
                data.currentLevelUnlocked = currentLevel + 1;
            }

            binaryFormatter.Serialize(file, data);

            file.Close();
        }
    }
}
