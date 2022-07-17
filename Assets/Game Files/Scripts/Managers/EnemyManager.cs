using UnityEngine;
using System;
using System.Collections.Generic;
public enum EnemyTypes {DAD, WIZARD, NINJA, BOMBER}

[Serializable]
public class EnemyConfig {
    public EnemyTypes type;
    public GameObject prefab;
    public int maxNumber;
    public int currentNumber;
    public float amountScale;
}
public class EnemyManager : MonoBehaviour {

    public static EnemyManager enemyManager => FindObjectOfType<EnemyManager>();
    public EnemyConfig[] enemies;
    public Dictionary<EnemyTypes, EnemyConfig> enemyDict;
    

    public int effectiveMax(EnemyTypes type) {
        return enemyDict[type].maxNumber + Mathf.FloorToInt(enemyDict[type].amountScale*Mathf.FloorToInt(GameManager.current.round/3));
    }
    public Transform wizardPoints;
    public PointConfig[] ninjaPoints;
    public Vector2 xBound, zBound;
    Vector3[] grassPositions;
    Vector3[] ninjaPositions;
    void Awake(){
        Transform[] ts = wizardPoints.GetComponentsInChildren<Transform>();
        grassPositions = new Vector3[ts.Length];
        for(int i = 0; i < grassPositions.Length; ++i){
            grassPositions[i] = ts[i].position;
        }
        enemyDict=new Dictionary<EnemyTypes, EnemyConfig>();
        foreach(EnemyConfig c in enemies){
            enemyDict.Add(c.type, c);
        }
    }

    public Vector3 GetWizardPoint(){
        return grassPositions[UnityEngine.Random.Range(0,grassPositions.Length)];
    }

    public PointConfig GetNinjaPoint(){
            if(ninjaPoints.Length > 0){
                return ninjaPoints[UnityEngine.Random.Range(0,ninjaPoints.Length)];
            }
        return null;
    }

}


    [Serializable]
    public class PointConfig {
        public Transform point;
        public Vector2 NinjaMove, NinjaShoot;
        public Vector2 BomberSpawnDir;
    }