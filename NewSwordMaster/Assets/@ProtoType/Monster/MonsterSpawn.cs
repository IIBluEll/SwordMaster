using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class MonsterSpawn : MonoBehaviour
{
   public GameObject monsterObject;
   public Monster monster;
   public List<string> monsterNames;
   public List<Sprite> monsterSprites;
   
   public Dictionary<string, Sprite[]> monsterAnim = new Dictionary<string, Sprite[]>();

   public int currentLevel = 1;
   public int killCount = 0;
   public bool isBossActive = false;

   public Button monsterKillBtn;
   
   private async void Awake()
   {
      monsterKillBtn.onClick.AddListener(KillMonster);
      
      monster = monsterObject.GetComponent<Monster>();
      monster.monsterDie += SpawnMonster;
      // 몬스터 CSV에서 불러오기
      monsterNames = MonsterName_CSVLoad.LoadMonsterName("Assets/03_Resources/CSV/Monster_Names.csv");
      await LoadMonsterAnimation();
      
      SpawnMonster();
   }

   public void KillMonster()
   {
      monster.TakeDamage(10);
   }
   
   private async UniTask LoadMonsterAnimation()
   {
      foreach (var monsterSprite in monsterSprites)
      {
         string spriteKey = $"{monsterSprite.name}";
         AsyncOperationHandle<Sprite[]> handle = Addressables.LoadAssetAsync<Sprite[]>(spriteKey);

         await handle.Task;

         if (handle.Status == AsyncOperationStatus.Succeeded)
         {
            monsterAnim[monsterSprite.name] = handle.Result;
         }
         else
         {
            Debug.LogError($"Failed to load Addressable Asset: {spriteKey}");
         }
      }
   }

   public void SpawnMonster()
   {
      killCount++;
      if (killCount % 3 == 0)
      {
         currentLevel++;
      }
      string monsterName = GetRandomMonsterName();
      string spriteName = GetRandomSpriteKey();

      if (!monsterAnim.ContainsKey(spriteName))
      {
         Debug.LogError($"No sprite animation found for {spriteName}");
         return;
      }

      if (monster != null)
      {
         monster.SetUpMonster(monsterName, spriteName, monsterAnim[spriteName], CalculateHealth(currentLevel));
      }
   }


   private string GetRandomMonsterName()
   {
      return monsterNames[UnityEngine.Random.Range(0, monsterNames.Count)];
   }

   private string GetRandomSpriteKey()
   {
      return monsterSprites[UnityEngine.Random.Range(0, monsterSprites.Count)].name;
   }

   private float CalculateHealth(int level)
   {
      return 1 + (level);
   }
}
