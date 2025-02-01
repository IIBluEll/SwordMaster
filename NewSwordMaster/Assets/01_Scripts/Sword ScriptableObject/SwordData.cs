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