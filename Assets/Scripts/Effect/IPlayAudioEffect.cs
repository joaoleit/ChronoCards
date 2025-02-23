public interface IPlayAudioEffect
{
    AudioName GetAudioName();
}

public enum AudioName
{
    Damage,
    Heal,
    Mana
}