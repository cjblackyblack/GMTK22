using System.Collections.Generic;
using UnityEngine;


public class TangibleObject : MonoBehaviour, ITimeScaleable
{
    public SpriteRenderer spriteRenderer => GetComponentInChildren<SpriteRenderer>();
    public Animator anim => GetComponentInChildren<Animator>();
    public AudioSource audioSource => GetComponent<AudioSource>();

    [HideInInspector]
    public int hitID;
    [HideInInspector]
    public List<int> hurtID;

    public ObjectProperties properties; 
    public Stats stats;
    public StatMods statMods;

    private void Start()
	{
        InitHurtID();
    if (GetComponent<SpriteRenderer>())
      transform.position += new Vector3(Random.Range(-0.05f, 0.05f), 0, Random.Range(-0.2f, 0.2f));
	}

	public void InitHurtID()
	{
        hurtID = new List<int>(10);
	}

    public virtual PhysicalObjectTangibility TakeDamage(DamageInstance damageInstance)
	{
        if (damageInstance.hitStun > 0)
        {
            switch (properties.objectTangibility)
            {
                case PhysicalObjectTangibility.Normal:
                    {
                        stats.HP -= Mathf.FloorToInt(damageInstance.damage);
                    }
                    break;
                case PhysicalObjectTangibility.Armor:
                    {
                        stats.HP -= Mathf.FloorToInt(damageInstance.damage);
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
                case PhysicalObjectTangibility.Intangible:
                    {

                    }
                    break;
            }
        }
        else
            stats.HP -= Mathf.FloorToInt(damageInstance.damage);
    if (stats.HP <= 0)
      Destroy(this.gameObject);
        return properties.objectTangibility;
	}

    public void SetTimeScale(float speed)
    {
        //throw new System.NotImplementedException();
    }

    public void SubscribeTime()
	{
		//throw new System.NotImplementedException();
	}

	public void UnsubscribeTime()
	{
		//throw new System.NotImplementedException();
	}

    public bool ValidHitID(TangibleObject _hitObj)//make hitbox class that tracks what it's hit, honestly storing these is dumb and I'm surprised I haven't seen the edge case yet
	{
        if (_hitObj.hurtID.Count > 0)
        {
            for (int j = _hitObj.hurtID.Count - 1; j >= 0; j--)
            {
                if (j > 9)
                {
                    _hitObj.hurtID.RemoveAt(j);
                    continue;
                }

                if (hitID == _hitObj.hurtID[j])
                    return false;
            }
        }
		else
		{
            _hitObj.hurtID.Add(hitID);
            return true;
		}

        _hitObj.hurtID.Insert(0, hitID);
        return true;
    }
}