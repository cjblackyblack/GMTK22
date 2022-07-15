using UnityEngine;
public enum Alliance
{
    Ally,
    Enemy,
    Neutral,
}

public enum StateEnums
{
    Idle,
    Move,
    Action,
    Hurt,
    Dead
}

public enum PhysicalObjectTangibility 
{ 
    Normal, 
    Armor, 
    Guard, 
    Invincible, 
    Intangible 
}

public enum GambitTarget
{
    Ally,
    AllyExcludeSelf,
    Enemy,
    Neutral,
    Any,
    AnyExcludeAlly,
    AnyExcludeEnemy,
    AnyExcludeNeutral,
    AnyExcludeSelf,
    Self
}

[System.Serializable]
public class ObjectProperties
{
    public PhysicalObjectTangibility baseTangibility;
    public PhysicalObjectTangibility objectTangibility;

    public Alliance alliance;
    public Alliance baseAlliance;

}

[System.Serializable]
public class Stats //beginning of actively measured stats that exist on the  object initially determined by BaseStats but then modified by StatMods
{
    public float scaledTime;
    public int HP;
    public int maxHP;
    public float moveSpeed;
}

[System.Serializable]
public class StatMods //modifications applied to Stats
{
    public float maxHPMod;
    public float damageMod;
    public float knockbackMod;
    public float moveSpeedMod;
    public float timeMod;
    public float inputMod;
}

[System.Serializable]
public class BaseStats //Exists on the job, of the object
{
    public AnimationCurve maxHP;
}