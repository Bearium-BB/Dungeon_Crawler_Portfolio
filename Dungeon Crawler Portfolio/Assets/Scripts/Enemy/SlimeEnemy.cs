using UnityEngine;

public class SlimeEnemy : MonoBehaviour, IDamageable,IEntity
{
    public EntityStats stats;

    float currentHealth = 100;

    public void Start()
    {
        currentHealth = stats.health;
        DoDamage(30,30);

        Heal(200);
    }

    public void DoDamage(float physicalDamage,float magicDamage)
    {
        physicalDamage -= stats.physicalResistance;

        magicDamage -= stats.magicResistance;

        physicalDamage -= (physicalDamage * stats.physicalResistancePercentage);

        magicDamage -= (magicDamage * stats.magicResistancePercentage);

        physicalDamage = Mathf.Max(physicalDamage, 0);

        magicDamage = Mathf.Max(magicDamage, 0);

        Debug.Log(physicalDamage);
        Debug.Log(magicDamage);

        if (IsDead())
        {
            OnDead();
        }
    }

    public bool IsDead()
    {

        if (currentHealth <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnDead()
    {
        Debug.Log("Boom");
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth  = Mathf.Min(currentHealth, stats.health);
        Debug.Log(currentHealth);

    }
}
