using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnhanceSystem : MonoBehaviour
{
    [SerializeField] private List<SwordSO> swordSoList;
    [SerializeField] private SwordSO currentSwordSO;
    [SerializeField] private BlackSmithUpgradeMotion blackSmithUpgradeMotion;
    [SerializeField] private InventorySystem inventorySystem; // 인벤토리 시스템 

    private SwordData currentSword;
    public TMP_Text swordName;
    public TMP_Text swordLevel;
    public TMP_Text swordUpgradeRate;
    
    public Button enhanceButton;
    public Button addInventoryButton;
    public SpriteRenderer spriteRenderer;
    
    public int playerGold = 1000;
    private bool isEnhancing = false;

    private void Awake()
    {
        enhanceButton.onClick.AddListener(OnClickEnhanceButton);
        addInventoryButton.onClick.AddListener(AddToInventory);
        
        currentSwordSO = swordSoList[0];
        currentSword = currentSwordSO.swordData;

        spriteRenderer.sprite = currentSword.swordSprite;
        
        ChangeTxt();
    }

    private void ChangeTxt()
    {
        swordName.text = currentSword.swordName_EN;
        swordLevel.text = currentSword.swordLevel + "LV";
        swordUpgradeRate.text = currentSword.upgradeRate + "%";
    }

    public void OnClickEnhanceButton()
    {
        if (isEnhancing)
        {
            return;
        }
      
        if (playerGold < currentSword.upgradeCost)
        {
            Debug.LogError("플레이어가 가지고 있는 골드가 부족합니다.");
            return;
        }
        if (currentSword.nextSwordLevel <= 0)
        {
            Debug.Log("최종 단계입니다.");
            return;
        }

        playerGold -= currentSword.upgradeCost;

        isEnhancing = true;

        blackSmithUpgradeMotion.StartHammerSequence();
    }

    public void OnHammerSequenceComplete()
    {
        //확률 체크
        if (ReturnEnhanceRate(currentSword.upgradeRate))
        {
            var nextSword = swordSoList[currentSword.nextSwordLevel - 1];

            if (nextSword != null)
            {
                currentSwordSO = nextSword;
                currentSword = currentSwordSO.swordData;
                spriteRenderer.sprite = currentSword.swordSprite;
                Debug.Log($"강화 성공! 현재 검은 {currentSword.swordName_KR} 입니다.");
            }
        }
        else
        {
            Debug.Log("강화 실패!");
            currentSwordSO = swordSoList[0];
            currentSword = currentSwordSO.swordData;
            spriteRenderer.sprite = currentSword.swordSprite;
        }
        
        ChangeTxt();
        isEnhancing = false;
    }
    
    //확률 체크
    private bool ReturnEnhanceRate(float upgradeCost)
    {
        UnityEngine.Random.InitState((int)(DateTime.Now.Ticks));

        return UnityEngine.Random.Range(0, 100) <= upgradeCost;
    }
    
    public void AddToInventory()
    {
        if (inventorySystem != null)
        {
            inventorySystem.AddToInventory(currentSwordSO);
        }

        currentSwordSO = null;
        spriteRenderer.sprite = null;

        ResetToDefaultSword();
    }

    public void RemoveInventory(int num)
    {
        inventorySystem.RemoveFromInventory(num);
    }

    private void ResetToDefaultSword()
    {
        currentSwordSO = swordSoList[0]; // 레벨 1 검으로 변경
        currentSword = currentSwordSO.swordData;
        spriteRenderer.sprite = currentSword.swordSprite;
        ChangeTxt();
    }
}
