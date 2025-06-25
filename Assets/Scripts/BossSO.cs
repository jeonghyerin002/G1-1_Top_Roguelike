using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossType
{
     TestScene, Round1, Round2, Round3, Round4, Round5
}
[CreateAssetMenu]
public class BossSO : ScriptableObject
{
   

    public BossType bossType;
}
