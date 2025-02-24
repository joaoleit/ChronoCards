using System.Collections.Generic;
public class AreaDamageEffect : ICardEffect
{
  private int damage;

  public AreaDamageEffect(EffectData data)
  {
    damage = data.value;
  }
  public void ApplyEffect(Player player, Enemy enemy)
  {
    List<Enemy> enemies = BattleManager.Instance.enemies;
    int finalDamage = damage;

    // Apply damage modifiers before dealing damage
    foreach (var modifier in EffectManager.Instance.GetModifiers<IDamageModifier>())
    {
      finalDamage = modifier.ModifyDamage(finalDamage);
    }

    foreach (var _enemy in enemies)
    {
      if (_enemy != null && _enemy.health > 0)
        _enemy.TakeDamage(finalDamage);
    }
  }

  public bool ShouldTriggerOnEnemy() => false;

  public string GetDescription() => "Deal " + damage + " damage to all enemies.";
  public void UpgradeEffect() => damage += 1;
  public EffectData GetEffectData() => new EffectData { value = damage };
}
