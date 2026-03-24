using UnityEngine;

[CreateAssetMenu(fileName = "NewBonusData", menuName = "Scriptable Objects/BonusData")]
public class BonusData : ScriptableObject
{
    public enum BonusType { Health, SpeedBoost }

    public BonusType bonusType;
    public int healthAmount;
    public float speedMultiplier;
    public float duration;
    public Color color;
}