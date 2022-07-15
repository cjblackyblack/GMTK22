using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random4WayTurret : MonoBehaviour
{
    [Header("Projectile")]
    public GameObject[] projectiles;
    [SerializeField]
    public int[] projectileTime;
    int currentTime;
    public int maxTime;
    public GambitTarget gambitTarget;

    private void FixedUpdate()
    {
        currentTime++;
        if (currentTime > maxTime)
            currentTime = 0;
        CreateProjectiles();
    }
    protected void CreateProjectiles()
    {
        for (int i = 0; i < projectiles.Length; i++)
        {
            if (currentTime == projectileTime[i])
            {
                ProjectileObject projectile = Instantiate(projectiles[i], transform.position, transform.rotation, projectiles[i].GetComponent<ProjectileObject>().local ? transform : null).GetComponent<ProjectileObject>();
                projectile.origin = null;
                projectile.inputDir = Random.Range(0, 2) > 0 ? Vector2.down * (Random.Range(0, 2) > 0 ? 1 : -1) : Vector2.right * (Random.Range(0, 2) > 0 ? 1 : -1);
                projectile.properties.baseAlliance = Alliance.Enemy;
                projectile.properties.alliance = Alliance.Enemy;
                projectile.targetAlliance = gambitTarget;
                //if (VFX.GetComponent<CustomVFX>())
                //{
                //    VFX.GetComponent<CustomVFX>().scaledTime = playerStateMachine.playerController.scaledTime;
                //    VFX.GetComponent<CustomVFX>().CreateVFX(attackDir, playerStateMachine.playerController.storedDir);
                //}
            }
        }
    }
}