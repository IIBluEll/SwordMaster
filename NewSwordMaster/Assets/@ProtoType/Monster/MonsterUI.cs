using System;
using TMPro;
using UnityEngine;

public class MonsterUI : MonoBehaviour
{
    public Monster monsterInfo;

    public TMP_Text monsterName;
    public TMP_Text monsterHP;

    private float monsterAllHP;
    
    private void Awake()
    {
        monsterInfo.monsterUIEvent += SettingMonsterUI;
        monsterInfo.monsterHitUIEvent += ChangeMonsterHP;
    }

    private void SettingMonsterUI(string name, float hp)
    {
        monsterAllHP = hp;
        monsterName.text = name;
        monsterHP.text = $"{hp}/{monsterAllHP}";
    }

    private void ChangeMonsterHP(float reduceHP)
    {
        monsterHP.text = $"{reduceHP}/{monsterAllHP}";
    }
}
