using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class StarterDeckCreator : MonoBehaviour
{
    private static readonly List<CardConfig> AreaDamageCards = new List<CardConfig>
    {
        new CardConfig("Firestorm", 2,
            new AreaDamageEffect(new EffectData { value = 3 }), new Color(1, 0.4f, 0)),
        new CardConfig("Quake", 3,
            new AreaDamageEffect(new EffectData { value = 5 }), new Color(0.6f, 0.3f, 0.1f)),
        new CardConfig("Arctic Blast", 2,
            new AreaDamageEffect(new EffectData { value = 4 }), Color.cyan),
        new CardConfig("Thunderous Onslaught", 4,
            new AreaDamageEffect(new EffectData { value = 6 }), new Color(1, 0.9f, 0)),
        new CardConfig("Plaguewind", 1,
            new AreaDamageEffect(new EffectData { value = 1 }), new Color(0.2f, 0.7f, 0.2f)),
        new CardConfig("Tidal Wrath", 5,
            new AreaDamageEffect(new EffectData { value = 7 }), new Color(0, 0.4f, 0.8f)),
        new CardConfig("Celestial Rain", 6,
            new AreaDamageEffect(new EffectData { value = 8 }), new Color(0.5f, 0, 0)),
        new CardConfig("Solar Inferno", 5,
            new AreaDamageEffect(new EffectData { value = 6 }), new Color(1, 0.6f, 0.2f)),
        new CardConfig("Chaos Nova", 1,
            new AreaDamageEffect(new EffectData { value = 2 }), new Color(0.8f, 0, 0.8f)),
        new CardConfig("Void Collapse", 7,
            new AreaDamageEffect(new EffectData { value = 9 }), new Color(0.4f, 0, 0.5f)),
    };

    private static readonly List<CardConfig> DamageCards = new List<CardConfig>
    {
        new CardConfig("Strike", 1,
            new DamageEffect(new EffectData { value = 5 }), Color.red),
        new CardConfig("Fire Bolt", 2,
            new DamageEffect(new EffectData { value = 7 }), new Color(1, 0.5f, 0)),
        new CardConfig("Ice Shard", 1,
            new DamageEffect(new EffectData { value = 4 }), Color.cyan),
        new CardConfig("Thunder Clap", 2,
            new DamageEffect(new EffectData { value = 6 }), Color.yellow),
        new CardConfig("Poison Dart", 1,
            new DamageEffect(new EffectData { value = 3 }), Color.green),
        new CardConfig("Shadow Strike", 3,
            new DamageEffect(new EffectData { value = 8 }), Color.black),
        new CardConfig("Magma Blast", 2,
            new DamageEffect(new EffectData { value = 6 }), Color.red),
        new CardConfig("Frost Nova", 1,
            new DamageEffect(new EffectData { value = 4 }), Color.blue),
        new CardConfig("Lightning Bolt", 3,
            new DamageEffect(new EffectData { value = 7 }), Color.yellow),
        new CardConfig("Venom Strike", 2,
            new DamageEffect(new EffectData { value = 5 }), Color.green),
    };

    private static readonly List<CardConfig> HealCards = new List<CardConfig>
    {
        new CardConfig("Healing Touch", 1,
            new HealEffect(new EffectData { value = 5 }), Color.magenta),
        new CardConfig("Renew", 1,
            new HealEffect(new EffectData { value = 3 }), new Color(1, 0.8f, 0.9f)),
        new CardConfig("Greater Heal", 3,
            new HealEffect(new EffectData { value = 8 }), Color.white),
        new CardConfig("Bandage", 1,
            new HealEffect(new EffectData { value = 4 }), new Color(1, 0.2f, 0.2f)),
        new CardConfig("Vitality Boost", 2,
            new HealEffect(new EffectData { value = 6 }), new Color(0.5f, 1, 0.5f)),
        new CardConfig("Mend Wounds", 3,
            new HealEffect(new EffectData { value = 7 }), new Color(0.8f, 0.2f, 0.8f)),
        new CardConfig("Recovery Wave", 0,
            new HealEffect(new EffectData { value = 2 }), new Color(0.9f, 0.6f, 0.9f)),
        new CardConfig("Soul Link", 1,
            new HealEffect(new EffectData { value = 4 }), new Color(0.3f, 0.7f, 1)),
        new CardConfig("Cleansing Light", 2,
            new HealEffect(new EffectData { value = 5 }), new Color(1, 1, 0.7f)),
        new CardConfig("Nature's Blessing", 0,
            new HealEffect(new EffectData { value = 3 }), new Color(0.4f, 0.9f, 0.4f)),
    };

    private static readonly List<CardConfig> ManaCards = new List<CardConfig>
    {
        new CardConfig("Mana Crystal", 0,
            new ManaEffect(new EffectData { value = 2 }), Color.blue),
        new CardConfig("Arcane Intellect", 1,
            new ManaEffect(new EffectData { value = 3 }), new Color(0, 0.5f, 1)),
        new CardConfig("Energy Surge", 2,
            new ManaEffect(new EffectData { value = 4 }), Color.cyan),
        new CardConfig("Mystic Charge", 0,
            new ManaEffect(new EffectData { value = 1 }), new Color(0.7f, 0, 1)),
        new CardConfig("Power Flow", 1,
            new ManaEffect(new EffectData { value = 2 }), new Color(0, 1, 1)),
        new CardConfig("Essence Tap", 2,
            new ManaEffect(new EffectData { value = 3 }), new Color(0.2f, 0.2f, 0.8f)),
        new CardConfig("Aether Pulse", 3,
            new ManaEffect(new EffectData { value = 5 }), new Color(0.4f, 0, 0.9f)),
        new CardConfig("Mana Well", 0,
            new ManaEffect(new EffectData { value = 2 }), new Color(0, 0.8f, 0.8f)),
        new CardConfig("Siphon Power", 2,
            new ManaEffect(new EffectData { value = 4 }), new Color(0.5f, 0, 0.5f)),
        new CardConfig("Celestial Infusion", 1,
            new ManaEffect(new EffectData { value = 3 }), new Color(0.9f, 0.9f, 1)),
    };

    private static readonly List<CardConfig> DrawCards = new List<CardConfig>
    {
        new CardConfig("Quick Draw", 1,
            new DrawEffect(new EffectData { value = 1 }), Color.yellow),
        new CardConfig("Guidance", 2,
            new DrawEffect(new EffectData { value = 2 }), Color.green),
        new CardConfig("Cascade", 3,
            new DrawEffect(new EffectData { value = 3 }), Color.blue),
        new CardConfig("Revelation", 4,
            new DrawEffect(new EffectData { value = 4 }), Color.magenta),
        new CardConfig("Crimson Surge", 5,
            new DrawEffect(new EffectData { value = 5 }), Color.red),
        new CardConfig("Tactical Pull", 0,
            new DrawEffect(new EffectData { value = 1 }), new Color(0.8f, 0.8f, 0)),
        new CardConfig("Arcane Insight", 1,
            new DrawEffect(new EffectData { value = 2 }), new Color(0.3f, 0.6f, 1)),
        new CardConfig("Strategic Planning", 2,
            new DrawEffect(new EffectData { value = 3 }), new Color(0.7f, 0.4f, 0.9f)),
        new CardConfig("Lucky Dip", 0,
            new DrawEffect(new EffectData { value = 1 }), new Color(1, 0.7f, 0.7f)),
        new CardConfig("Master's Gambit", 1,
            new DrawEffect(new EffectData { value = 2 }), new Color(0.2f, 0.8f, 0.2f)),
    };

    private static readonly List<CardConfig> BonusDamageCards = new List<CardConfig>
    {
        new CardConfig("Preparation", 1,
            new BonusDamageModifier(new EffectData { value = 3, duration = 1 }), new Color(0.5f, 0.5f, 1)),
        new CardConfig("Empower", 4,
            new BonusDamageModifier(new EffectData { value = 5, duration = 2 }), new Color(1, 0, 1)),
        new CardConfig("Power Strike", 2,
            new BonusDamageModifier(new EffectData { value = 2, duration = 2 }), new Color(0.8f, 0.2f, 0.2f)),
        new CardConfig("Fury", 1,
            new BonusDamageModifier(new EffectData { value = 4, duration = 1 }), new Color(1, 0.3f, 0)),
        new CardConfig("Rage", 3,
            new BonusDamageModifier(new EffectData { value = 2, duration = 3 }), new Color(0.4f, 0.8f, 1)),
        new CardConfig("Imbue", 4,
            new BonusDamageModifier(new EffectData { value = 6, duration = 1 }), new Color(0.9f, 0.9f, 0.2f)),
    };

    private static readonly List<CardConfig> HealPerCardCards = new List<CardConfig>
    {
        new CardConfig("Fortify", 2,
            new HealPerCardModifier(new EffectData { value = 2, duration = 1 }), Color.gray),
        new CardConfig("Medic's Aid", 1,
            new HealPerCardModifier(new EffectData { value = 1, duration = 2 }), new Color(0.9f, 0.9f, 0.9f)),
        new CardConfig("Sustaining Light", 3,
            new HealPerCardModifier(new EffectData { value = 3, duration = 1 }), new Color(1, 0.8f, 0.9f)),
        new CardConfig("Chain Heal", 3,
            new HealPerCardModifier(new EffectData { value = 2, duration = 2 }), new Color(0.5f, 1, 0.5f)),
        new CardConfig("Compassion", 1,
            new HealPerCardModifier(new EffectData { value = 4, duration = 1 }), new Color(1, 0.6f, 0.7f)),
        new CardConfig("Renewing Touch", 2,
            new HealPerCardModifier(new EffectData { value = 2, duration = 1 }), new Color(0.8f, 0.3f, 0.8f)),
        new CardConfig("Vital Circuit", 3,
            new HealPerCardModifier(new EffectData { value = 1, duration = 1 }), new Color(0.4f, 0.9f, 0.4f)),
        new CardConfig("Circle of Life", 2,
            new HealPerCardModifier(new EffectData { value = 3, duration = 1 }), new Color(0.7f, 1, 0.7f)),
    };

    private static readonly List<CardConfig> DamagePerCardCards = new List<CardConfig>
    {
        new CardConfig("Overload", 2,
            new DamagePerCardModifier(new EffectData { value = 2, duration = 1 }), Color.black),
        new CardConfig("Chain Lightning", 4,
            new DamagePerCardModifier(new EffectData { value = 1, duration = 2}), new Color(0.8f, 0.8f, 0)),
        new CardConfig("Inferno", 3,
            new DamagePerCardModifier(new EffectData { value = 3, duration = 1 }), new Color(1, 0.4f, 0)),
        new CardConfig("Blood Pact", 4,
            new DamagePerCardModifier(new EffectData { value = 4, duration = 1 }), new Color(0.6f, 0, 0)),
        new CardConfig("Shrapnel Burst", 4,
            new DamagePerCardModifier(new EffectData { value = 2, duration = 2 }), new Color(0.5f, 0.5f, 0.5f)),
        new CardConfig("Echo Strike", 6,
            new DamagePerCardModifier(new EffectData { value = 3, duration = 2 }), new Color(0.3f, 0.3f, 1)),
        new CardConfig("Necrotic Flow", 1,
            new DamagePerCardModifier(new EffectData { value = 1, duration = 1 }), new Color(0.2f, 0.2f, 0.2f)),
        new CardConfig("Apocalypse", 6,
            new DamagePerCardModifier(new EffectData { value = 1, duration = 6 }), new Color(0.5f, 0, 0)),
    };

    private static readonly List<CardConfig> DoubleDamageCards = new List<CardConfig>
    {
        new CardConfig("Double Strike", 2,
            new DoubleDamageModifier(new EffectData { duration = 1 }), new Color(1, 0.8f, 0)),
        new CardConfig("Amplify", 4,
            new DoubleDamageModifier(new EffectData { duration = 3 }), new Color(0, 0.7f, 1)),
        new CardConfig("Vengeance", 4,
            new DoubleDamageModifier(new EffectData { duration = 1 }), new Color(0.6f, 0, 0))
    };

    private static List<CardConfig> BloodRitualCards = new List<CardConfig>
    {
        new CardConfig("Life Tap", 0,
            new BloodRitualEffect(new EffectData { value = 1 }), new Color(0.8f, 0.1f, 0.1f)),
        new CardConfig("Blood Pact", 0,
            new BloodRitualEffect(new EffectData { value = 2 }), new Color(0.7f, 0, 0)),
        new CardConfig("Vital Exchange", 0,
            new BloodRitualEffect(new EffectData { value = 3 }), new Color(0.6f, 0, 0)),
        new CardConfig("Hemorrhage", 0,
            new BloodRitualEffect(new EffectData { value = 4 }), new Color(0.5f, 0, 0)),
        new CardConfig("Sanguine Sacrifice", 0,
            new BloodRitualEffect(new EffectData { value = 5 }), new Color(0.9f, 0, 0)),
    };

    private static readonly List<CardConfig> ChainLightningCards = new List<CardConfig>
    {
        new CardConfig("Static Jump", 3,
            new ChainLightningEffect(new EffectData { value = 2, duration = 2 }), new Color(0.4f, 0.7f, 1f)),
        new CardConfig("Arc Surge", 3,
            new ChainLightningEffect(new EffectData { value = 3, duration = 2 }), new Color(0.2f, 0.5f, 1f)),
        new CardConfig("Voltaic Leap", 2,
            new ChainLightningEffect(new EffectData { value = 1, duration = 2 }), new Color(0.6f, 0.8f, 1f)),
        new CardConfig("Thunder Web", 3,
            new ChainLightningEffect(new EffectData { value = 4, duration = 2 }), new Color(1f, 0.9f, 0.3f)),
        new CardConfig("Plasma Cascade", 4,
            new ChainLightningEffect(new EffectData { value = 3, duration = 3 }), new Color(0.8f, 0.4f, 1f)),
        new CardConfig("Ion Storm", 3,
            new ChainLightningEffect(new EffectData { value = 5, duration = 2 }), new Color(0.9f, 0.6f, 0.1f)),
        new CardConfig("Tesla Coil", 5,
            new ChainLightningEffect(new EffectData { value = 4, duration = 4 }), new Color(0.1f, 0.8f, 0.9f)),
        new CardConfig("Electro Net", 4,
            new ChainLightningEffect(new EffectData { value = 6, duration = 3 }), new Color(1f, 0.7f, 0f)),
        new CardConfig("Superconductor", 5,
            new ChainLightningEffect(new EffectData { value = 7, duration = 2 }), new Color(0.7f, 0f, 1f)),
        new CardConfig("Ragnarok Surge", 6,
            new ChainLightningEffect(new EffectData { value = 8, duration = 3 }), new Color(0.9f, 0.9f, 0.1f))
    };

    private static readonly List<CardConfig> PoisonCards = new List<CardConfig>
    {
        new CardConfig("Venomous Bite", 1,
            new PoisonModifier(new EffectData { value = 3, duration = 2 }), new Color(0.2f, 0.7f, 0.1f)),
        new CardConfig("Toxic Cloud", 2,
            new PoisonModifier(new EffectData { value = 2, duration = 3 }), new Color(0.4f, 0.8f, 0.2f)),
        new CardConfig("Plaguebearer's Touch", 2,
            new PoisonModifier(new EffectData { value = 5, duration = 1 }), new Color(0.6f, 0.1f, 0.6f)),
        new CardConfig("Noxious Fumes", 1,
            new PoisonModifier(new EffectData { value = 1, duration = 4 }), new Color(0.3f, 0.9f, 0.3f)),
        new CardConfig("Neurotoxin", 3,
            new PoisonModifier(new EffectData { value = 4, duration = 2 }), new Color(0.1f, 0.8f, 0.8f)),
        new CardConfig("Pandemic Spread", 2,
            new PoisonModifier(new EffectData { value = 1, duration = 5 }), new Color(0.7f, 0.9f, 0.4f)),
        new CardConfig("Viper's Kiss", 3,
            new PoisonModifier(new EffectData { value = 3, duration = 3 }), new Color(0.0f, 0.6f, 0.0f)),
        new CardConfig("Rotting Bite", 2,
            new PoisonModifier(new EffectData { value = 2, duration = 4 }), new Color(0.4f, 0.3f, 0.1f)),
        new CardConfig("Septic Shock", 4,
            new PoisonModifier(new EffectData { value = 5, duration = 2 }), new Color(0.8f, 0.0f, 0.8f)),
        new CardConfig("Necrotic Venom", 5,
            new PoisonModifier(new EffectData { value = 3, duration = 5 }), new Color(0.1f, 0.1f, 0.1f))
    };

    private static readonly List<CardConfig> RangeDamageCards = new List<CardConfig>
    {
        new CardConfig("Wild Shot", 1,
            new RangeDamage(new EffectData { value = 2 }), new Color(1f, 0.6f, 0f)),
        new CardConfig("Scatterblast", 2,
            new RangeDamage(new EffectData { value = 3 }), new Color(1f, 0.8f, 0.2f)),
        new CardConfig("Chaos Bolt", 3,
            new RangeDamage(new EffectData { value = 4 }), new Color(0.8f, 0.1f, 0.8f)),
        new CardConfig("Buckshot Spray", 0,
            new RangeDamage(new EffectData { value = 2 }), new Color(1f, 0.4f, 0.1f)),
        new CardConfig("Hailstorm Strike", 3,
            new RangeDamage(new EffectData { value = 5 }), new Color(0.2f, 0.7f, 1f)),
        new CardConfig("Ricochet Fire", 1,
            new RangeDamage(new EffectData { value = 3 }), new Color(1f, 0.9f, 0.4f)),
        new CardConfig("Volatile Barrage", 2,
            new RangeDamage(new EffectData { value = 4 }), new Color(0.9f, 0.2f, 0.2f)),
        new CardConfig("Spread Cannon", 4,
            new RangeDamage(new EffectData { value = 6 }), new Color(0.7f, 0f, 0f)),
        new CardConfig("RNG Salvo", 3,
            new RangeDamage(new EffectData { value = 5 }), new Color(0.4f, 0.4f, 1f)),
        new CardConfig("Apocalypse Rain", 5,
            new RangeDamage(new EffectData { value = 7 }), new Color(0.5f, 0f, 0.5f))
    };

    private static readonly List<CardConfig> ShieldCards = new List<CardConfig>
    {
        new CardConfig("Ironclad Defense", 1,
            new ShieldEffect(new EffectData { value = 6 }), new Color(0.4f, 0.6f, 0.8f)),
        new CardConfig("Aegis Barrier", 2,
            new ShieldEffect(new EffectData { value = 9 }), new Color(0.2f, 0.5f, 1f)),
        new CardConfig("Bastion Protocol", 1,
            new ShieldEffect(new EffectData { value = 6 }), new Color(0.7f, 0.8f, 0.9f)),
        new CardConfig("Fortify Walls", 3,
            new ShieldEffect(new EffectData { value = 12 }), new Color(0.3f, 0.4f, 0.6f)),
        new CardConfig("Titanium Barrier", 2,
            new ShieldEffect(new EffectData { value = 6 }), new Color(0.8f, 0.8f, 0.8f)),
        new CardConfig("Reactive Plating", 4,
            new ShieldEffect(new EffectData { value = 10 }), new Color(0.9f, 0.6f, 0.3f)),
        new CardConfig("Phalanx Formation", 3,
            new ShieldEffect(new EffectData { value = 7 }), new Color(0.5f, 0.7f, 1f)),
        new CardConfig("Energy Dome", 2,
            new ShieldEffect(new EffectData { value = 4 }), new Color(0.1f, 0.9f, 0.9f)),
        new CardConfig("Bulwark", 5,
            new ShieldEffect(new EffectData { value = 15 }), new Color(0.6f, 0.3f, 0.8f)),
        new CardConfig("Impervious Guard", 4,
            new ShieldEffect(new EffectData { value = 9 }), new Color(0.9f, 0.9f, 0.9f))
    };

    private static readonly List<CardConfig> ThornsCards = new List<CardConfig>
{
        new CardConfig("Thorn Armor", 2,
            new ThornsModifier(new EffectData { value = 20, duration = 3 }), new Color(0.2f, 0.6f, 0.2f)),
        new CardConfig("Spike Shield", 1,
            new ThornsModifier(new EffectData { value = 25, duration = 2 }), new Color(0.5f, 0.8f, 0.2f)),
        new CardConfig("Retaliation Aura", 3,
            new ThornsModifier(new EffectData { value = 15, duration = 4 }), new Color(0.2f, 0.8f, 0.8f)),
        new CardConfig("Bramble Guard", 2,
            new ThornsModifier(new EffectData { value = 30, duration = 2 }), new Color(0.3f, 0.5f, 0.2f)),
        new CardConfig("Reflective Carapace", 3,
            new ThornsModifier(new EffectData { value = 40, duration = 1 }), new Color(0.7f, 0.7f, 0.7f)),
        new CardConfig("Razor Feedback", 1,
            new ThornsModifier(new EffectData { value = 10, duration = 5 }), new Color(0.8f, 1f, 0.8f)),
        new CardConfig("Pain Mirror", 4,
            new ThornsModifier(new EffectData { value = 50, duration = 1 }), new Color(0.8f, 0.2f, 0.2f)),
        new CardConfig("Barbed Surge", 4,
            new ThornsModifier(new EffectData { value = 35, duration = 3 }), new Color(0.4f, 0.9f, 0.4f)),
        new CardConfig("Vengeance Ward", 3,
            new ThornsModifier(new EffectData { value = 25, duration = 4 }), new Color(0.1f, 0.5f, 0.5f)),
        new CardConfig("Hedgehog's Defense", 5,
            new ThornsModifier(new EffectData { value = 60, duration = 1 }), new Color(0.5f, 0.3f, 0.1f))
};

    private static readonly List<CardConfig> AllDamageCards = new List<CardConfig>()
        .Concat(AreaDamageCards)
        .Concat(DamageCards)
        .Concat(ChainLightningCards)
        .Concat(PoisonCards)
        .Concat(RangeDamageCards)
        .Concat(ThornsCards)
        .ToList();

    private static readonly List<CardConfig> AllHelperCards = new List<CardConfig>()
        .Concat(HealCards)
        .Concat(ManaCards)
        .Concat(DrawCards)
        .Concat(ShieldCards)
        .ToList();

    private static readonly List<CardConfig> AllExtraCards = new List<CardConfig>()
        .Concat(BonusDamageCards)
        .Concat(HealPerCardCards)
        .Concat(DamagePerCardCards)
        .Concat(DoubleDamageCards)
        .Concat(BloodRitualCards)
        .ToList();

    private static readonly List<CardConfig> AllCards = new List<CardConfig>()
        .Concat(AllDamageCards)
        .Concat(AllHelperCards)
        .Concat(AllExtraCards)
        .ToList();

    private struct CardConfig
    {
        public string name;
        public int manaCost;
        public ICardEffect effect;
        public Color color;

        public CardConfig(string name, int manaCost, ICardEffect effect, Color color)
        {
            this.name = name;
            this.manaCost = manaCost;
            this.effect = effect;
            this.color = color;
        }
    }

    public static void CreateStarterDeck()
    {
        List<CardConfig> starterCards = new List<CardConfig>();
        Dictionary<List<CardConfig>, int> starterCreator = new Dictionary<List<CardConfig>, int>()
        {
            { AllDamageCards, 10 },
            { AllHelperCards, 5 },
            { AllExtraCards, 5 },
            { AllCards, 5 }
        };
        Debug.Log(AllCards.Count);
        foreach (var item in starterCreator)
        {
            for (int i = 0; i < item.Value; i++)
            {
                List<CardConfig> configs = item.Key;
                var config = configs[Random.Range(0, configs.Count)];
                starterCards.Add(config);
            }
        }

        foreach (var config in starterCards)
        {
            // Create card instance
            Card card = CardFactory.CreateCard(
                config.name,
                config.manaCost,
                new List<ICardEffect> { config.effect },
                config.color
            );

            // Add to deck
            DeckManager.Instance.AddCardToDeck(card);
        }
    }

    public static List<Card> getRandomCards(int number)
    {
        List<Card> cards = new List<Card>();
        for (int i = 0; i < number; i++)
        {
            int index = Random.Range(0, AllCards.Count);
            var config = AllCards[index];

            Card card = CardFactory.CreateCard(
                config.name,
                config.manaCost,
                new List<ICardEffect> { config.effect },
                config.color
            );

            cards.Add(card);
        }

        return cards;
    }
}