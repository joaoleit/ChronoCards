using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ThornsModifier : ICardEffect, IModifier, IPlayerDamagedListener, ITurnListener
{
    private float reflectPercentage;
    private int duration;
    private ThornsModifier modifier;
    private float delayBeforeHit = .5f;


    public ThornsModifier(EffectData data)
    {
        reflectPercentage = data.value / 100f;
        duration = data.duration;
    }

    public void ApplyEffect(Player player, Enemy enemy)
    {
        modifier = new ThornsModifier(new EffectData { value = (int)(reflectPercentage * 100f), duration = duration });
        GameEvents.Instance.OnModifierAdded.Invoke(modifier);
    }

    public bool ShouldTriggerOnEnemy() => false;

    public string GetDescription() => $"Reflect {reflectPercentage * 100}% of damage taken. Lasts {duration} turn{(duration > 1 ? "s" : "")}.";

    public void UpgradeEffect()
    {
        reflectPercentage += 0.01f;
        duration += 1;
    }

    public async void OnPlayerDamaged(int damageAmount)
    {
        // Wait using async/await
        await Task.Delay((int)(delayBeforeHit * 1000));

        int reflectedDamage = Mathf.RoundToInt(damageAmount * reflectPercentage);
        List<Enemy> enemies = BattleManager.Instance.enemies;

        foreach (var _enemy in enemies)
        {
            if (_enemy != null && _enemy.health > 0)
                _enemy.TakeDamage(reflectedDamage);
        }
    }

    public void OnTurnStart()
    {
        duration--;
    }

    public bool IsExpired() => duration <= 0;

    public EffectData GetEffectData() => new EffectData { value = (int)(reflectPercentage * 100f), duration = duration };
    public ICardEffect Clone() => new ThornsModifier(GetEffectData());
}
