using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPowerUp : MonoBehaviour
{
	public bool used;
	public GameObject FX;
	private void Start()
	{
		GetComponent<MeshRenderer>().enabled = false;
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<PlayerObject>() && !used)
		{
			for(int i = 0; i < PlayerManager.current.StaticParty.Length; i++)
			{
				if(PlayerManager.current.StaticParty[i] == other.GetComponent<PlayerObject>())
				{
					used = true;
					Debug.Log($"found match");
					CharacterJob randomJob = GameManager.current.Jobs[Random.Range(0, GameManager.current.Jobs.Length)];
					Debug.Log($"rolling {randomJob.name}");
					while (other.GetComponent<PlayerObject>().job == randomJob)
					{
						Debug.Log($"bad roll, rolling {randomJob.name}");
						randomJob = GameManager.current.Jobs[Random.Range(0, GameManager.current.Jobs.Length)];
						continue;
					}
					GameManager.current.CurrentJobs[i] = randomJob;
					other.GetComponent<PlayerObject>().SetJob(randomJob, true);
					Instantiate(FX, other.transform.position, other.transform.rotation);
					StartCoroutine(DestroyBehaviour());
				}
			}
		}
	}

	IEnumerator DestroyBehaviour()
	{
		yield return new WaitForEndOfFrame();
		Destroy(this.gameObject);
	}
}