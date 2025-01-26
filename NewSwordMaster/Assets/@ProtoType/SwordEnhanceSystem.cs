using System;
using UnityEngine;
using UnityEngine.UI;

public class SwordEnhanceSystem : MonoBehaviour
{
   [SerializeField] private SwordData currentSword;
   [SerializeField] private SwordSO swordDataList;

   public int playerGold = 1000;

   public Button enhanceButton;

   private void Awake()
   {
      enhanceButton.onClick.AddListener(OnClickEnhanceButton);

      currentSword = swordDataList.GetSwordByLevel(1);
   }

   public void OnClickEnhanceButton()
   {
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
      
      //확률 체크
      if (ReturnEnhanceRate(currentSword.upgradeRate))
      {
         SwordData nextSword = swordDataList.GetSwordByLevel(currentSword.nextSwordLevel);
         if (nextSword != null)
         {
            currentSword = nextSword;
            Debug.Log($"강화 성공! 현재 검은 {currentSword.swordName_KR} 입니다.");
         }
      }
      else
      {
         Debug.Log("강화 실패!");
         SwordData level1Sword = swordDataList.GetSwordByLevel(1);
         currentSword = level1Sword;
      }
   }
   
   //확률 체크
   private bool ReturnEnhanceRate(float upgradeCost)
   {
      UnityEngine.Random.InitState((int)(DateTime.Now.Ticks));

      return UnityEngine.Random.Range(0, 100) <= upgradeCost;
   }
}
