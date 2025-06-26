using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoManager : MonoBehaviour
{
    public List<GameObject> MemoList;
    // Start is called before the first frame update
    void Start()
    {
        GameDataManager.instance.playerData = GameDataManager.instance.LoadData();

        for (int i = 0; i < GameDataManager.instance.playerData.OpenedMemo; i++)
        {
            MemoList[i].gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
