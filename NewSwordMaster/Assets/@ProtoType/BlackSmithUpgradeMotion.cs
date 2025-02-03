using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class BlackSmithUpgradeMotion : MonoBehaviour
{
    public Animator animator;
    public GameObject hitEffectPrefab;
    public GameObject changeEffectPrefab;
    public EnhanceSystem swordEnhanceSystem;

    public ParticleSystem hitEffectParticle;
    public ParticleSystem changeEffectParticle;
    
    private int hammerCount = 0;
    private int hammerTarget = 3;

    public void StartHammerSequence()
    {
        hammerCount = 0;
        animator.SetTrigger("isAttack");
    }

    public void OnHammerImpact()
    {
        if (hitEffectPrefab != null)
        {
            hitEffectParticle.Play();
        }
        
        //TODO : 사운드 및 카메라 효과
    }

    public void OnHammerAnimationEnd()
    {
        hammerCount++;
        hitEffectParticle.Stop();
        
        if (hammerCount < hammerTarget)
        {
            animator.SetTrigger("isAttack");
        }
        else
        {
            swordEnhanceSystem.OnHammerSequenceComplete();

            if (changeEffectParticle != null)
            {
                changeEffectParticle.Play();
            }
        }
    }
}
