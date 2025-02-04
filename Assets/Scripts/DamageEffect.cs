using UnityEngine;

public class DamageEffect : ICardEffect
{
	private int damage;

	public DamageEffect(int damage)
	{
		this.damage = damage;
	}

	public void ApplyEffect(Player player, Enemy enemy)
	{
		enemy.TakeDamage(damage);
	}

	public bool ShouldTriggerOnEnemy()
	{
		return true;
	}
}