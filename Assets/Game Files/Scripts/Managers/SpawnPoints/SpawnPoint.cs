using UnityEngine;
using System;
using System.Collections.Generic;
public enum SpawnPointTypes {
    ONAWAKE,
    CONTINUING
}
public class SpawnPoint : MonoBehaviour {

public static Dictionary<EnemyTypes,List<SpawnPoint>> _spawnPoints = null;
    public static Dictionary<EnemyTypes,List<SpawnPoint>> spawnPoints {
        get{
            if(_spawnPoints == null){
                _spawnPoints = new Dictionary<EnemyTypes, List<SpawnPoint>>();

            }
            return _spawnPoints;
        }
    }

    Transform _tform = null;
    public Transform tform {
        get{
            if(!_tform)
                _tform = transform;
            return _tform;
        }
    }

    public static SpawnPoint GetRandomAwayFromPlayer(EnemyTypes enemyType){
        Vector3 totalPos = Vector3.zero;
        foreach(SmartObject so in PlayerManager.current.Party){
            totalPos += so.tform.position;

        }
        totalPos /= PlayerManager.current.Party.Length;
        totalPos.y=1;
        List<SpawnPoint> set = EnemyManager.enemyManager.spDict[enemyType];
        if(set.Count == 0){
            return null;
        }
        int worst = 0;
        float worstd = Vector3.Distance(set[0].tform.position, totalPos);
        for(int i = 1; i < set.Count; ++i){
            
            float cd = Vector3.Distance(set[i].tform.position, totalPos);
            if(cd < worstd){
                worst = i;
                worstd = cd;
            }
            
        }
        if(set.Count > 1)
        set.RemoveAt(worst);
        return set[UnityEngine.Random.Range(0,set.Count)];
    }


    void Start(){
        /*if(type == SpawnPointTypes.ONAWAKE){
            Instantiate(EnemyManager.enemyManager.enemyDict[enemyType].prefab, tform.position, Quaternion.identity);
        }
        else{
        if(!spawnPoints.ContainsKey(enemyType)){
            spawnPoints.Add(enemyType,new List<SpawnPoint>());
        }
        spawnPoints[enemyType].Add(this);
        }*/
    }

    public bool spawnOnStart;
    public SpawnPointTypes type;

    public EnemyTypes enemyType;
    int spawnChains = 0;
    float delay;
    public void SetEnemySpawn(float delay){
        this.delay=delay;
if(spawnChains==0){
        Invoke("SpawnEnemy", delay);
        }
        spawnChains++;
    }

    void SpawnEnemy(){
        Instantiate(EnemyManager.enemyManager.enemyDict[enemyType].prefab, tform.position, Quaternion.identity);
        if(spawnChains > 0){
            spawnChains--;
            Invoke("SpawnEnemy", delay);
        }
    }
}