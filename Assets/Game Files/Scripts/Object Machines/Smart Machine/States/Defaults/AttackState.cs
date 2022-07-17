using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SmartState/AttackState")]
public class AttackState : SmartState
{
    [SerializeField]
    public string attackName;

    public int maxTime;

    public AnimationCurve strafeMultiplier;
    public float moveSpeed;
    [SerializeField]
    private AnimationCurve forwardMovement;
    [SerializeField]
    private AnimationCurve verticalMovement;
    [SerializeField]
    private AnimationCurve strafeMovement;

    public List<StatusEffect> statusEffects;

    [Header("Stats")]
    [SerializeField]
    protected Vector2Int iframes;
    [SerializeField]
    protected Vector2Int armorFrames;
    [SerializeField]
    protected Vector2Int guardFrames;
    [SerializeField]
    protected bool armorPierce;
    [SerializeField]
    protected bool ignoreProtections;


    [Header("VFX")]
    public GameObject[] hitParticles;
    [SerializeField]
    protected GameObject[] particleEffects;
    [SerializeField]
    public int[] particleTime;
    [SerializeField]
    protected Vector3[] particlePos;


    [Header("SFX")]
    [SerializeField]
    public int[] sfxTime;
    [SerializeField]
    protected AudioClip[] sfxClip;
    [SerializeField]
    protected float[] sfxVolume;
    [SerializeField]
    protected AudioClip[] hitSFX;


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
    [SerializeField]
    public float[] hitboxSize;

    [Header("Damage")]
    [Space(5)]
    [SerializeField]
    protected float[] hitboxDamage;
    public bool useMagic;
    public bool flatDamage;

    [SerializeField]
    [Header("Knockback")]
    protected Vector2[] hitboxKnockbackDir;
    [SerializeField]
    protected float[] knockbackStrength;

    [Header("Hitstun")]
    [Space(5)]
    [SerializeField]
    protected int[] hitboxStun;

    [SerializeField]
    protected float hitStopTime;

    [Header("Projectile")]
    public GameObject[] projectiles;
    [SerializeField]
    public int[] projectileTime;
    [SerializeField]
    protected Vector3[] projectilePos;
    public GambitTarget targetAlliance;

    public override void OnEnter(SmartObject smartObject)
    {
        base.OnEnter(smartObject);
      
        smartObject.anim.SetFloat("xDir", smartObject.facingDir.x);
        smartObject.anim.Play(attackName, 0, 0);
        smartObject.OnStateChange?.Invoke(smartObject, StateEnums.Action);
        smartObject.velocity *= 0;
        smartObject.SetFacingDir(false);
    }

    public override void OnFixedUpdate(SmartObject smartObject)
    {
        smartObject.velocity.x = (((smartObject._inputDir.normalized * forwardMovement.Evaluate(smartObject.currentTime)) + (smartObject._inputDir.normalized.Rotate(-90) * strafeMovement.Evaluate(smartObject.currentTime))).ConvertVector2()).x * moveSpeed * smartObject.stats.moveSpeed * smartObject.statMods.moveSpeedMod;
        smartObject.velocity.z = (((smartObject._inputDir.normalized * forwardMovement.Evaluate(smartObject.currentTime)) + (smartObject._inputDir.normalized.Rotate(-90) * strafeMovement.Evaluate(smartObject.currentTime))).ConvertVector2()).z * moveSpeed * smartObject.stats.moveSpeed * smartObject.statMods.moveSpeedMod;

        HandleState(smartObject);
        AttackID(smartObject);
        //CreateDamageFrames(smartObject);
        CreateHitboxes(smartObject);
        CreateProjectiles(smartObject);
        CreateVFX(smartObject);
        CreateSFX(smartObject);
    }


    public override void HandleState(SmartObject smartObject)
    {
        if (smartObject.currentTime > maxTime)
            smartObject.stateMachine.ChangeState(StateEnums.Idle);
    }

    public override void OnExit(SmartObject smartObject)
    {
        base.OnExit(smartObject);
        smartObject.properties.objectTangibility = smartObject.properties.baseTangibility;
    }

    protected void AttackID(SmartObject smartObject)
    {
        for (int i = 0; i < multihitTime.Length; i++) //changes hitID to allow another hit, based on times in multiHitTime[]
            if (smartObject.currentTime == multihitTime[i])
                smartObject.hitID = Random.Range(0, 10000);
    }


    protected void CreateDamageFrames(SmartObject smartObject)
    {
        if (smartObject.currentTime >= iframes.x && smartObject.currentTime <= iframes.y && iframes != Vector2.zero)
            smartObject.properties.objectTangibility = PhysicalObjectTangibility.Invincible;

        else if (smartObject.currentTime >= guardFrames.x && smartObject.currentTime <= guardFrames.y && guardFrames != Vector2.zero)
            smartObject.properties.objectTangibility = PhysicalObjectTangibility.Guard;

        else if (smartObject.currentTime >= armorFrames.x && smartObject.currentTime <= armorFrames.y && armorFrames != Vector2.zero)
            smartObject.properties.objectTangibility = PhysicalObjectTangibility.Armor;

        else
            smartObject.properties.objectTangibility = smartObject.properties.baseTangibility;
    }

    protected void CreateHitboxes(SmartObject smartObject)
    {
        for (int i = 0; i < hitboxTime.Length; i++) //hitbox placement + prefab instantiation
        {
            if (smartObject.currentTime >= hitboxTime[i].x && smartObject.currentTime <= hitboxTime[i].y)
            {
                //Debug.Log($" attack at { smartObject.RotateTileSelection(hitboxPos[i]) + new Vector3(smartObject.transform.position.x, 0, smartObject.transform.position.z)}");
                Collider[] collisions = Physics.OverlapSphere((hitboxPos[i].RotateVector(smartObject.facingDir)) + smartObject.transform.position, hitboxSize[i], smartObject.hittable); //creating hitboxes based on hitBoxPos adjusted by player rotation
                foreach (Collider hitObject in collisions)//anything hit
                {
                    if (hitObject.TryGetComponent(out TangibleObject _hitObj))
                    {
                        if (!StaticMethods.ValidTarget(smartObject, _hitObj, targetAlliance)) { return; }
                        

                        if (!smartObject.ValidHitID(_hitObj)) { return; }
                        {
                            DamageInstance damageInstance = new DamageInstance(smartObject, statusEffects, hitboxDamage[i], knockbackStrength[i], new Vector2(hitboxKnockbackDir[i].x, hitboxKnockbackDir[i].y * smartObject.facingDir.y), hitStopTime, armorPierce, hitboxStun[i], flatDamage, ignoreProtections, useMagic);
                            PhysicalObjectTangibility hitTang = _hitObj.TakeDamage(damageInstance);
                            switch (hitTang)
                            {
                                case PhysicalObjectTangibility.Normal:
                                    {
                                        foreach (GameObject particle in hitParticles)
                                        {
                                            GameObject _prefab = Instantiate(particle, _hitObj.transform.position, Quaternion.identity);
                                        }
                                    }
                                    break;
                                case PhysicalObjectTangibility.Armor:
                                    {
                                        foreach (GameObject particle in hitParticles)
                                        {
                                            GameObject _prefab = Instantiate(particle, _hitObj.transform.position, Quaternion.identity);
                                        }
                                    }
                                    break;
                                case PhysicalObjectTangibility.Guard:
                                    {
                                    }
                                    break;
                                case PhysicalObjectTangibility.Invincible:
                                    {
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }

    protected void CreateProjectiles(SmartObject smartObject)
    {
        for (int i = 0; i < projectiles.Length; i++)
        {
            if (smartObject.currentTime == projectileTime[i])
            {
                ProjectileObject projectile = Instantiate(projectiles[i], (projectilePos[i].RotateVector(smartObject.facingDir)) + smartObject.transform.position, smartObject.anim.transform.rotation, projectiles[i].GetComponent<ProjectileObject>().local ? smartObject.transform : null).GetComponent<ProjectileObject>();
                //if (projectile.GetComponent<ProjectileObject>().local)
                //    projectile.transform.parent = smartObject.transform;
                projectile.origin = smartObject;
                projectile.inputDir = smartObject.facingDir;
                projectile.properties.baseAlliance = smartObject.properties.baseAlliance;
                projectile.properties.alliance = smartObject.properties.alliance;
                projectile.targetAlliance = targetAlliance;
                //if (VFX.GetComponent<CustomVFX>())
                //{
                //    VFX.GetComponent<CustomVFX>().scaledTime = playerStateMachine.playerController.scaledTime;
                //    VFX.GetComponent<CustomVFX>().CreateVFX(attackDir, playerStateMachine.playerController.storedDir);
                //}
            }
        }
    }

    protected void CreateVFX(SmartObject smartObject)
    {
        for (int i = 0; i < particleEffects.Length; i++)
        {
            if (smartObject.currentTime == particleTime[i])
            {
                GameObject VFX = Instantiate(particleEffects[i], (particlePos[i].RotateVector(smartObject.facingDir)) + smartObject.transform.position, smartObject.transform.rotation, smartObject.transform);
                //if (VFX.GetComponent<CustomVFX>())
                //{
                //    VFX.GetComponent<CustomVFX>().scaledTime = playerStateMachine.playerController.scaledTime;
                //    VFX.GetComponent<CustomVFX>().CreateVFX(attackDir, playerStateMachine.playerController.storedDir);
                //}
            }
        }
    }

    protected void CreateSFX(SmartObject smartObject)
    {
        for (int i = 0; i < sfxTime.Length; i++)
        {
            if (smartObject.currentTime == sfxTime[i] && sfxClip[i] != null)
            {
                smartObject.audioSource.PlayOneShot(sfxClip[i], sfxVolume[i]);
            }
        }
    }
}