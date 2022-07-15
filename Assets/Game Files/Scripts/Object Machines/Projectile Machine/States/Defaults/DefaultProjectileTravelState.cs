using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ProjectileState/TravelState")]
public class DefaultProjectileTravelState : ProjectileState
{
	public ProjectileState exitState;

    public List<StatusEffect> statusEffects;

    [SerializeField]
    private AnimationCurve forwardMovement;
    [SerializeField]
    private AnimationCurve verticalMovement;
    [SerializeField]
    private AnimationCurve strafeMovement;

    [Header("Stats")]
    [SerializeField]
    private Vector2Int iframes;
    [SerializeField]
    private Vector2Int armorFrames;
    [SerializeField]
    private Vector2Int guardFrames;
    [SerializeField]
    private bool armorPierce;
    [SerializeField]
    private bool ignoreProtections;


    [Header("VFX")]
    public GameObject[] hitParticles;
    [SerializeField]
    private GameObject[] particleEffects;
    [SerializeField]
    public int[] particleTime;
    [SerializeField]
    private Vector3[] particlePos;


    [Header("SFX")]
    [SerializeField]
    public int[] sfxTime;
    [SerializeField]
    private AudioClip[] sfxClip;
    [SerializeField]
    private float[] sfxVolume;
    [SerializeField]
    private AudioClip[] hitSFX;


    [Header("Time")] //backwards because unity;
    [Space(5)]
    [Header("   Hitbox")]
    [SerializeField]
    public Vector2[] hitboxTime;
    [SerializeField]
    public int[] multihitTime;

    [Header("Range")]
    [Space(5)]
    [SerializeField]
    public Vector3[] hitboxPos;
    public float[] hitboxSize;

    [Header("Damage")]
    [Space(5)]
    [SerializeField]
    private float[] hitboxDamage;
    public bool useMagic;
    public bool flatDamage;

    [SerializeField]
    [Header("Knockback")]
    private Vector2[] hitboxKnockbackDir;
    [SerializeField]
    private float[] knockbackStrength;

    [Header("Hitstun")]
    [Space(5)]
    [SerializeField]
    private int[] hitboxStun;

    [SerializeField]
    private float hitStopTime;

    public override void OnEnter(ProjectileObject projectileObject)
	{
		projectileObject.activeTime = 0;
	}

	public override void OnUpdate(ProjectileObject projectileObject)
	{

	}

	public override void OnFixedUpdate(ProjectileObject projectileObject)
	{
		HandleState(projectileObject);
        AttackID(projectileObject);
        Vector2 dir = projectileObject.inputDir.normalized;
        projectileObject.velocity = ((dir * forwardMovement.Evaluate(projectileObject.currentTime)) + (dir.Rotate(-90) * strafeMovement.Evaluate(projectileObject.currentTime))).ConvertVector2() + new Vector3(0, verticalMovement.Evaluate(projectileObject.currentTime));
        CreateHitboxes(projectileObject);
    }

	public override void OnExit(ProjectileObject projectileObject)
	{

	}

	public override void HandleState(ProjectileObject projectileObject)
	{
		if (projectileObject.currentTime > maxTime)
			projectileObject.stateMachine.ChangeState(exitState);
	}

    void AttackID(ProjectileObject projectileObject)
    {
        for (int i = 0; i < multihitTime.Length; i++) //changes hitID to allow another hit, based on times in multiHitTime[]
            if (projectileObject.currentTime == multihitTime[i])
            {
                //Debug.Log("settingID");
                projectileObject.hitID = Random.Range(0, 10000);
            }
    }

    void CreateHitboxes(ProjectileObject projectileObject)
    {
        for (int i = 0; i < hitboxTime.Length; i++) //hitbox placement + prefab instantiation
        {
            if (projectileObject.currentTime >= hitboxTime[i].x && projectileObject.currentTime <= hitboxTime[i].y)
            {
                //Debug.Log($" attack at { smartObject.RotateTileSelection(hitboxPos[i]) + new Vector3(smartObject.transform.position.x, 0, smartObject.transform.position.z)}");
                Collider[] collisions = Physics.OverlapSphere((hitboxPos[i].RotateVector(projectileObject.inputDir)) + projectileObject.transform.position, hitboxSize[i], projectileObject.hittable); //creating hitboxes based on hitBoxPos adjusted by player rotation
                foreach (Collider hitObject in collisions)//anything hit
                {
                    if (hitObject.GetComponent<TangibleObject>())
                    {
                        TangibleObject _hitObj = hitObject.GetComponent<TangibleObject>();
                        if (StaticMethods.ValidTarget(projectileObject, _hitObj, projectileObject.targetAlliance) && projectileObject.ValidHitID(_hitObj))
                        {
                            DamageInstance damageInstance = new DamageInstance(projectileObject, statusEffects, hitboxDamage[i], knockbackStrength[i], new Vector2(hitboxKnockbackDir[i].x * projectileObject.inputDir.x, hitboxKnockbackDir[i].y * projectileObject.inputDir.y), hitStopTime, armorPierce, hitboxStun[i], flatDamage, ignoreProtections, useMagic);
                            PhysicalObjectTangibility hitTang = _hitObj.TakeDamage(damageInstance);
                            switch (hitTang)
                            {
                                case PhysicalObjectTangibility.Normal:
                                    {
                                        foreach (GameObject particle in hitParticles)
                                        {
                                            GameObject _prefab = Instantiate(particle, _hitObj.transform.position, Quaternion.identity);
                                            projectileObject.hitCount--;
                                        }
                                    }
                                    break;
                                case PhysicalObjectTangibility.Armor:
                                    {
                                        foreach (GameObject particle in hitParticles)
                                        {
                                            GameObject _prefab = Instantiate(particle, _hitObj.transform.position, Quaternion.identity);
                                            projectileObject.hitCount--;
                                        }
                                    }
                                    break;
                                case PhysicalObjectTangibility.Guard:
                                    {
                                        projectileObject.hitCount--;
                                    }
                                    break;
                                case PhysicalObjectTangibility.Invincible:
                                    {
                                        projectileObject.hitCount--;
                                    }
                                    break;
                                case PhysicalObjectTangibility.Intangible:
									{
                                        break;
									}
                            }

                            if (projectileObject.hitCount <= 0)
                                projectileObject.stateMachine.ChangeState(exitState);

                        }
                    }
                }
            }
        }
    }

}