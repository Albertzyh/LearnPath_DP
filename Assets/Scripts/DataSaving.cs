using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataSaving : MonoBehaviour
{
    public static DataSaving Instance { get; private set; }

    public int BestScore;
    public string UserName;
    public string BestName;

    private string dataPath;

    public InputField m_userNameInput;

    public void StartButton_Click()
    {
        if (m_userNameInput.text != string.Empty)
        {
            UserName = m_userNameInput.text;
        }
        else
        {
            if (UserName == string.Empty)
            {
                UserName = "XXX";
            }
        }
        SceneManager.LoadScene(1);
    }

    public void QuitButton_Click()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    private void Awake()
    {
        dataPath = Application.persistentDataPath + "/UserData.json";
        if (Instance != null)
        {
            Destroy(Instance);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadFromJson();

        if(GameObject.Find("BestScore").gameObject != null)
        {
            GameObject.Find("BestScore").GetComponent<Text>().text = $"Best Score : {DataSaving.Instance.BestName} : {DataSaving.Instance.BestScore}";
        }
    }

    [Serializable]
    public class UserData
    {
        public string UserName;
        public int BestScore;
    }

    public void Save2Json()
    {
        UserData userData = new UserData()
        {
            UserName = BestName,
            BestScore = BestScore
        };

        string json = JsonUtility.ToJson(userData);

        File.WriteAllText(dataPath, json);
    }

    public void LoadFromJson()
    {
        if (File.Exists(dataPath))
        {
            UserData data;
            string json = File.ReadAllText(dataPath);
            data = JsonUtility.FromJson<UserData>(json);
            BestName = data.UserName;
            BestScore = (int)data.BestScore;
        }
    }
}
