using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ChainLightningEffect : MonoBehaviour, ICardEffect
{
    private int baseDamage;
    private int bounces;
    private float delayBetweenBounces = 0.5f;

    public ChainLightningEffect(EffectData data)
    {
        baseDamage = data.value;
        bounces = data.duration;
    }

    public async void ApplyEffect(Player player, Enemy enemy)
    {
        Enemy currentTarget = enemy;
        for (int i = 0; i < bounces; i++)
        {
            if (currentTarget == null) break;

            int finalDamage = baseDamage;
            foreach (var modifier in EffectManager.Instance.GetModifiers<IDamageModifier>())
            {
                finalDamage = modifier.ModifyDamage(finalDamage);
            }

            currentTarget.TakeDamage(finalDamage);
            currentTarget = FindNextTarget(currentTarget);

            // Wait using async/await
            await Task.Delay((int)(delayBetweenBounces * 1000));
        }
    }

    private Enemy FindNextTarget(Enemy current)
    {
        var enemies = BattleManager.Instance.enemies;
        var possibleTargets = new List<Enemy>();
        foreach (var _enemy in enemies)
        {
            if (_enemy.health > 0)
                possibleTargets.Add(_enemy);
        }

        return possibleTargets.Count > 0 ?
            possibleTargets[Random.Range(0, possibleTargets.Count)] :
            null;
    }

    public bool ShouldTriggerOnEnemy() => true;

    public string GetDescription() => $"Deal {baseDamage} damage, chain {bounces} times.";

    public void UpgradeEffect()
    {
        baseDamage += 1;
        bounces += 1;
    }

    public EffectData GetEffectData() => new EffectData { duration = bounces, value = baseDamage };
    public ICardEffect Clone() => new ChainLightningEffect(GetEffectData());
}