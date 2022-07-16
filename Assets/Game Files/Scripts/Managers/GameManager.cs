using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager current;
	public List<TangibleObject> Grass;
	public int round;
	bool started;

	[Header("Screen Wipe")]
	public float wipeSpeed;
	float fadeTime;
	float wipe;
	public Material[] transitions;
	private void Start()
	{
		if (current != null && current != this)
		{ Destroy(this.gameObject); return; }

		current = this;
		Application.targetFrameRate = 60;
		StartCoroutine(StartManager());
		DontDestroyOnLoad(this.gameObject);
	}

	IEnumerator StartManager()
	{
		yield return new WaitForEndOfFrame();
		started = true;
	}

	private void Update()
	{
		if(Grass.Count == 0 && started)
		{
			RoundWin();

		}
		for (int i = Grass.Count -1; i >= 0; i--)
		{
			if (Grass[i] == null)
			{
				Grass.RemoveAt(i);
				continue;
			}
		}
	}

	public void RoundWin()
	{
		started = false;
		round++;
		LoadLevel(round);
	}

	public void GameOver(bool win)
	{
		started = false;
		round = 0;
	}

	public void Pause()
	{

	}

	public void LoadLevel(int scene)
	{
		StopAllCoroutines();
		StartCoroutine(LoadLevelCoroutine(scene));
	}

	public IEnumerator LoadLevelCoroutine(int scene)
	{
		Debug.Log($"loading {scene}");
		//eventSystem.gameObject.SetActive(false);
		StartCoroutine(WipeScreen(wipeSpeed, 0, 1));
		yield return new WaitUntil(() => (fadeTime == wipe));
		//eventSystem.gameObject.SetActive(true);
		Time.timeScale = 1;
		SceneManager.LoadScene(scene);
	}

	public void ReloadCurrentLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

	}

	private void OnLevelWasLoaded(int level)
	{
		StartCoroutine(WipeScreen(-wipeSpeed, 1, 0));
		started = true;
	}
	public IEnumerator WipeScreen(float rate, float start, float goal)
	{
		//simpleBlit.TransitionMaterial = transitions[Random.Range(0, transitions.Length - 1)];
		fadeTime = start;
		wipe = goal;


		while (fadeTime != goal)
		{
			foreach (Material mat in transitions)
				mat.SetFloat("_Cutoff", fadeTime);

			fadeTime += rate * 0.0125f;

			fadeTime = Mathf.Clamp01(fadeTime);
			yield return new WaitForEndOfFrame();

			foreach (Material mat in transitions)
				mat.SetFloat("_Cutoff", Mathf.RoundToInt(goal));
		}
	}
}