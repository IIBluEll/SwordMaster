using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwordEnhanceSystem : MonoBehaviour
{
   [SerializeField] private SwordData currentSword;
   [SerializeField] private SwordSO swordDataList;
   [SerializeField] private BlackSmithUpgradeMotion blackSmithUpgradeMotion;

   public TMP_Text swordName;
   public TMP_Text swordLevel;
   public TMP_Text swordUpgradeRate;
   
   public ParticleSystem swordChangeEffect;
   
   public Button enhanceButton;
   public SpriteRenderer spriteRenderer;
   
   public int playerGold = 1000;

   private bool isEnhancing = false;
   
   private void Awake()
   {
      enhanceButton.onClick.AddListener(OnClickEnhanceButton);

      currentSword = swordDataList.GetSwordByLevel(1);
      spriteRenderer.sprite = currentSword.swordSprite;
 
      ChangeText();
   }

   public void ChangeText()
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
      swordChangeEffect.Play();
      
      //확률 체크
      if (ReturnEnhanceRate(currentSword.upgradeRate))
      {
         SwordData nextSword = swordDataList.GetSwordByLevel(currentSword.nextSwordLevel);
         if (nextSword != null)
         {
            currentSword = nextSword;
            spriteRenderer.sprite = currentSword.swordSprite;
            Debug.Log($"강화 성공! 현재 검은 {currentSword.swordName_KR} 입니다.");
         }
      }
      else
      {
         Debug.Log("강화 실패!");
         SwordData level1Sword = swordDataList.GetSwordByLevel(1);
         currentSword = level1Sword;
         spriteRenderer.sprite = currentSword.swordSprite;
      }
      
      ChangeText();
      isEnhancing = false;
   }
   
   //확률 체크
   private bool ReturnEnhanceRate(float upgradeCost)
   {
      UnityEngine.Random.InitState((int)(DateTime.Now.Ticks));

      return UnityEngine.Random.Range(0, 100) <= upgradeCost;
   }
}
