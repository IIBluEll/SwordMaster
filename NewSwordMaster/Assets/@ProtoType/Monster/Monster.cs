using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Monster : MonoBehaviour
{
   public string monsterName;
   public float health;
   public SpriteRenderer spriteRenderer;
   private Sprite[] animationFrames;
   private float animationSpeed = 0.5f;

   public Action monsterDie;
   public void SetUpMonster(string name, string sprite, Sprite[] frames, float hp)
   {
      monsterName = name;
      animationFrames = frames;
      health = hp;
      
      spriteRenderer = GetComponent<SpriteRenderer>();

      if (animationFrames != null && animationFrames.Length > 0)
      {
         StartCoroutine(PlayAnimation());
      }
   }

   private IEnumerator PlayAnimation()
   {
      int index = 0;

      while (true)
      {
         if (animationFrames.Length > 0)
         {
            spriteRenderer.sprite = animationFrames[index];
            index = (index + 1) % animationFrames.Length;
         }

         yield return new WaitForSeconds(animationSpeed);
      }
   }

   public void TakeDamage(int damage)
   {
      health -= damage;

      if (health <= 0)
      {
         Die();
      }
   }

   private void Die()
   {
      Debug.Log($"{monsterName} has been defeated!");
      monsterDie?.Invoke();
   }
}
