using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


[Serializable]
public class PlayerData
{
    public List<string> collectedItems = new List<string>();
    public int OpenedMemo = 0;
}
public class GameDataManager : MonoBehaviour
{
    public static GameDataManager instance;
    public PlayerData playerData;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); //중복 방지
        }

        //string filePath = Application.persistentDataPath + "/player_data.json";
        //if (System.IO.File.Exists(filePath))
        //{
            //System.IO.File.Delete(filePath);
           //Debug.Log("기존 데이터 초기화");
        //}
        //if (instance == null)
        //{
            //instance= this;
            //DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
            //Destroy(gameObject); //중복방지
        //}
    }
    public void SaveData (PlayerData playerData)
    {
        string filePath = Application.persistentDataPath + "/player_data.json";
        string json = JsonUtility.ToJson(playerData, true);
        System.IO.File.WriteAllText(filePath, json);
        Debug.Log("게임 데이터 저장됨 : " + json);

    }
    public PlayerData LoadData()
    {
        string filePath = Application.persistentDataPath + "/player_data.json";
        if (System.IO.File.Exists(filePath))
        {
            string json = System.IO.File.ReadAllText(filePath);
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("게임 데이터 로드 됨" + json);
            return playerData;
        }
        else
        {
            Debug.Log("저장된게임 데이터가 없습니다.");
            return new PlayerData();
        }
    }

    public void GameStart()
    {
        playerData = LoadData();
        if (playerData == null)
        {
            playerData = new PlayerData();
        }
            SceneManager.LoadScene("TestScene");
    }

    public void PlayerDead(int stageIndex)
    {
        PlayerData playerData = LoadData();
        if (playerData != null)
        {
            playerData.OpenedMemo = stageIndex;
            SaveData(playerData);
            Debug.Log("ASD");
        }
        SceneManager.LoadScene("DyingMessage");
    }

    // 개발 중 메인씬 테스트 씬 스킵을 위한 장치
    public void DevGameStart()
    {
        playerData = LoadData();
        if (playerData == null)
        {
            playerData = new PlayerData();
        }
    }

    private void Update()
    {
        if ( Input.GetKey(KeyCode.F1) )
            GameDataManager.instance.DevGameStart();
    }

}
