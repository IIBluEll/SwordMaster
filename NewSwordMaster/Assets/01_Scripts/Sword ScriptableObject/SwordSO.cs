using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SwordData
{
    public int swordLevel;
    public string swordName_KR;
    public string swordName_EN;

    public int nextSwordLevel;
    public int upgradeCost;
    public float upgradeRate;

    public float damage;
    public float attackSpeed;

    public Sprite swordSprite;
}

[CreateAssetMenu(fileName = "SwordDataList", menuName = "Data/SwordDataList", order = 1)]
public class SwordSO : ScriptableObject
{
    public List<SwordData> SwordDatas;

    public SwordData GetSwordByLevel(int level)
    {
        return SwordDatas.Find(sword => sword.swordLevel == level);
    }
}
