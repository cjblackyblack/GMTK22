using UnityEngine;
using System;
using System.Collections.Generic;
public enum EnemyTypes { DAD, WIZARD, NINJA, BOMBER }

[Serializable]
public class EnemyConfig
{
    public EnemyTypes type;
    public GameObject prefab;
    public int maxNumber;
    public int currentNumber;

    public int spawnAmount;
    public int spawnDC;
    public int minRound;
    public float maxAmountScale;
    public float spawnAmountScale;

    public float spawnTimer;

    public float maxTimer;
}
[DefaultExecutionOrder(200)]
public class EnemyManager : MonoBehaviour
{


    public static EnemyManager enemyManager => FindObjectOfType<EnemyManager>();
    public EnemyConfig[] enemies;
    bool enemyDictInit = false;
    Dictionary<EnemyTypes, EnemyConfig> _enemyDict;
    public Dictionary<EnemyTypes, EnemyConfig> enemyDict
    {
        get
        {
            if (!enemyDictInit)
            {
                enemyDictInit = true;
                _enemyDict = new Dictionary<EnemyTypes, EnemyConfig>();
                foreach (EnemyConfig c in enemies)
                {
                    _enemyDict.Add(c.type, c);
                }

            }
            return _enemyDict;
        }
    }
    public int globalEnemyMax;
    public float globalEnemySpawnDelay;
    public int roundsPerDifficultyIncrease;
    public int effectiveMax(EnemyTypes type)
    {
        return enemyDict[type].maxNumber + Mathf.FloorToInt(enemyDict[type].maxAmountScale * Mathf.FloorToInt(GameManager.current.round / roundsPerDifficultyIncrease < 1 ? 1 : roundsPerDifficultyIncrease));
    }



    public int effectiveAmount(EnemyTypes type)
    {
        return enemyDict[type].spawnAmount + Mathf.FloorToInt(enemyDict[type].spawnAmountScale * Mathf.FloorToInt(GameManager.current.round / (roundsPerDifficultyIncrease < 1 ? 1 : roundsPerDifficultyIncrease)));
    }
    public Transform wizardPoints;
    public PointConfig[] ninjaPoints;

    public Vector2 xBound, zBound;
    Vector3[] grassPositions;
    Vector3[] ninjaPositions;
    void Awake()
    {
        Transform[] ts = wizardPoints.GetComponentsInChildren<Transform>();
        grassPositions = new Vector3[ts.Length];
        for (int i = 0; i < grassPositions.Length; ++i)
        {
            grassPositions[i] = ts[i].position;
        }
    }

    void FixedUpdate()
    {
        if (!GameManager.current.started)
            return;
        int curTotalEnemies = 0;
        List<EnemyTypes> ready = new List<EnemyTypes>();
        float deltaTime = Time.fixedDeltaTime;
        foreach (EnemyConfig c in enemies)
        {
            EnemyConfig o = enemyDict[c.type];
            o.spawnTimer -= deltaTime;
            curTotalEnemies += o.currentNumber;
            if (o.spawnTimer < 0)
            {
                ready.Add(c.type);
                o.spawnTimer = o.maxTimer;
            }
        }

        foreach (EnemyTypes t in ready)
        {
            EnemyConfig o = enemyDict[t];

            if (StateMachine.RollDice > o.spawnDC &&
               o.currentNumber + effectiveAmount(t) < effectiveMax(t) &&
               curTotalEnemies + effectiveAmount(t) < globalEnemyMax)
            {
                SpawnPoint sp = SpawnPoint.GetRandomAwayFromPlayer(t);
                if (sp)
                {
                    for (int i = 0; i < effectiveAmount(t); ++i)
                    {
                        sp.SetEnemySpawn(globalEnemySpawnDelay);
                    }
                }
            }
        }
    }

    public Vector3 GetWizardPoint()
    {
        return grassPositions[UnityEngine.Random.Range(0, grassPositions.Length)];
    }

    public PointConfig GetNinjaPoint()
    {
        if (ninjaPoints.Length > 0)
        {
            return ninjaPoints[UnityEngine.Random.Range(0, ninjaPoints.Length)];
        }
        return null;
    }

}


[Serializable]
public class PointConfig
{
    public Transform point;
    public Vector2 NinjaMove, NinjaShoot;
    public Vector2 BomberSpawnDir;
}