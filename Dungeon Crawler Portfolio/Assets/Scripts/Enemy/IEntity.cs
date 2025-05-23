using UnityEngine;

public interface IEntity
{
    public bool IsDead();
    public void OnDead();
    public void Heal(int amount);


}
