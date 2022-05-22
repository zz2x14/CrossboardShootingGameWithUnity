using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] private GameObject deathVFX;

    [Header("ÑªÁ¿ÏµÍ³")]
    [SerializeField] protected float maxHealth;
    protected float curHealth;
    [SerializeField] private StatusBar healthBar;
    [SerializeField] private bool isShowHealthBar = true;

    protected virtual void OnEnable()
    {
        curHealth = maxHealth;

        if (isShowHealthBar)
        {
            ShowHealthBar();
        }
        else
        {
            HideHealthBar();
        }
    }

    public virtual void TakenDamage(float damageValue)
    {
        if (curHealth <= 0) return;

        curHealth = Mathf.Clamp(curHealth - damageValue, 0, curHealth - damageValue);

        if (isShowHealthBar && gameObject.activeSelf)
        {
            healthBar.UpdateFillAmount(curHealth, maxHealth);
        }
        
        if(curHealth == 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        curHealth = 0;
        PoolManager.Instance.Release(deathVFX, transform.position);
        gameObject.SetActive(false); 
    }

    protected virtual void RestoreHealth(float value)
    {
        if (curHealth == maxHealth) return;

        curHealth = Mathf.Clamp(curHealth + value, 0 , maxHealth);

        if (isShowHealthBar)
        {
            healthBar.UpdateFillAmount(curHealth, maxHealth);
        }
    }

    protected IEnumerator HealthRegenerateCor(WaitForSeconds regenerateInterval, float regeneratePercent)
    {
        while(curHealth < maxHealth)
        {
            yield return regenerateInterval;
            RestoreHealth(maxHealth * regeneratePercent);
        }
    }
   
    protected IEnumerator TakenDamageOverTimeCor(WaitForSeconds takenDamageInterval,float damageValue)
    {
        while (curHealth > 0f)
        {
            yield return takenDamageInterval;
            TakenDamage(damageValue);
        }
    }

    private void ShowHealthBar()
    {
        healthBar.InitializeFillAmount(curHealth, maxHealth);
        healthBar.gameObject.SetActive(true);
    }
    private void HideHealthBar()
    {
        healthBar.gameObject.SetActive(false);
    }

}
