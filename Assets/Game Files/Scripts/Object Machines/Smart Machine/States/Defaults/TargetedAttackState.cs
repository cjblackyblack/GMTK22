using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SmartState/AttackStateTargeted")]
public class TargetedAttackState : AttackState
{
    public override void OnFixedUpdate(SmartObject smartObject)
    {
        HandleState(smartObject);
        AttackID(smartObject);
        CreateDamageFrames(smartObject);
        CreateHitboxes(smartObject, smartObject.targetPos);
        CreateProjectiles(smartObject);
        CreateVFX(smartObject);
        CreateSFX(smartObject);
    }

    public void CreateHitboxes(SmartObject smartObject, Vector3 target)
    {
        for (int i = 0; i < hitboxTime.Length; i++) //hitbox placement + prefab instantiation
        {
            if (smartObject.currentTime >= hitboxTime[i].x && smartObject.currentTime <= hitboxTime[i].y)
            {
                //Debug.Log($" attack at { smartObject.RotateTileSelection(hitboxPos[i]) + new Vector3(smartObject.transform.position.x, 0, smartObject.transform.position.z)}");
                Collider[] collisions = Physics.OverlapSphere((hitboxPos[i].RotateVector(target - smartObject.transform.position).normalized) + target, hitboxSize[i], smartObject.hittable); //creating hitboxes based on hitBoxPos adjusted by player rotation
                foreach (Collider hitObject in collisions)//anything hit
                {
                    if (hitObject.TryGetComponent(out TangibleObject _hitObj))
                    {
                        if (StaticMethods.ValidTarget(smartObject, _hitObj, targetAlliance) && smartObject.ValidHitID(_hitObj))
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

    protected void CreateProjectiles(SmartObject smartObject, TangibleObject target)
    {
        for (int i = 0; i < projectiles.Length; i++)
        {
            if (smartObject.currentTime == projectileTime[i])
            {
                ProjectileObject projectile = Instantiate(projectiles[i], (projectilePos[i].RotateVector(target.transform.position - smartObject.transform.position).normalized) + target.transform.position, smartObject.transform.rotation, projectiles[i].GetComponent<ProjectileObject>().local ? smartObject.transform : null).GetComponent<ProjectileObject>();
                //if (projectile.GetComponent<ProjectileObject>().local)
                //    projectile.transform.parent = smartObject.transform;
                projectile.inputDir = smartObject._inputDir;
                projectile.properties.baseAlliance = smartObject.properties.baseAlliance;
                projectile.properties.alliance = smartObject.properties.alliance;
                //if (VFX.GetComponent<CustomVFX>())
                //{
                //    VFX.GetComponent<CustomVFX>().scaledTime = playerStateMachine.playerController.scaledTime;
                //    VFX.GetComponent<CustomVFX>().CreateVFX(attackDir, playerStateMachine.playerController.storedDir);
                //}
            }
        }
    }
}