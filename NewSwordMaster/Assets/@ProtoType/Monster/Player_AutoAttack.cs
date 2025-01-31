using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Player_AutoAttack : MonoBehaviour
{
    public int debugLevel = 1;
    [SerializeField] private SwordSO swordScriptableObject;
    [SerializeField] private SwordData currentSwordData;
    [SerializeField] private Monster monster;

    public SpriteRenderer spriteRenderer;
    private Animator playerAnim;
    private bool isAttack;

    private void Start()
    {
        playerAnim = GetComponent<Animator>();

        EquipSword(debugLevel);
        StartAttack();
    }


    public void EquipSword(int level)
    {
        currentSwordData = swordScriptableObject.GetSwordByLevel(level);

        spriteRenderer.sprite = currentSwordData.swordSprite;
        playerAnim.speed = currentSwordData.attackSpeed;
    }

    public async void StartAttack()
    {
        while (true)
        {
            var attackSpeed = (int)(1000f / currentSwordData.attackSpeed);
            
            Attack();
            await UniTask.Delay(attackSpeed);
        }
    }

    private void Attack()
    {
        if (currentSwordData == null || monster == null)
        {
            return;
        }

        PlayAttackAnim();
        //playerAnim.SetTrigger("isAttack");
    }

    private void PlayAttackAnim()
    {
        playerAnim.Play("ATTACK", 0 ,0);

        float attackDuration = 1f / currentSwordData.attackSpeed;
        playerAnim.speed = 1f / attackDuration;
    }

    public void GiveDamage()
    {
        monster.TakeDamage(currentSwordData.damage);
    }
}
