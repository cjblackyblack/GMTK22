using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager current;
	public int score;

	[Header("Round Data")]
	public List<TangibleObject> Grass;
	public int round;
	public bool started;
	public bool GameWon;

	[Header("Job Data")]
	public CharacterJob[] Jobs;
	public CharacterJob[] CurrentJobs = new CharacterJob[3];


	[Header("Audio")]
	public AudioSource BGMSource;
	public AudioClip MenuTheme;
	public AudioClip GameTheme;
	public AudioClip BossTheme;
	public float audioFadeSpeed;
	public float audioFadeTime;
	public float audioFadeGoal;

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
		for(int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
		{
			if(SceneManager.GetSceneByBuildIndex(i) == SceneManager.GetActiveScene())
				LoadLevelBehaviour(SceneManager.GetActiveScene().buildIndex);
		}
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

	public void IncrementScore(int delta)
	{
		score += delta;
		UIManager.current.UpdateDisplayScore(score);
	}
	public void RoundWin()
	{
		started = false;
		PlayerManager.current.playerController.started = false;
		if(PlayerManager.current.StaticParty[0] != null)
		for (int i = 0; i < PlayerManager.current.StaticParty.Length; i++)
		{
			PlayerManager.current.storedHP[i] = PlayerManager.current.StaticParty[i].stats.HP;
		}
		GameWon = (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 2);
		round++;
		LoadLevel(round);
	}

	public void GameOver(bool win)
	{
		started = false;
		round = 0;
		UIManager.current.UpdateDisplayScore(score);

		LoadLevel(SceneManager.sceneCountInBuildSettings - 1);
	}

	public void Pause()
	{

	}

	public void LoadLevel(int scene)
	{
		//StopAllCoroutines();
		StartCoroutine(LoadLevelCoroutine(scene));
	}

	public IEnumerator LoadLevelCoroutine(int scene)
	{
		Debug.Log($"loading {scene}");
		//eventSystem.gameObject.SetActive(false);

		StartCoroutine(WipeScreen(wipeSpeed, 0, 1));
		if (scene == 1 || scene == SceneManager.sceneCountInBuildSettings - 1)
		{
			StartCoroutine(FadeMusic(-audioFadeSpeed, 1, 0));
		}
		yield return new WaitUntil(() => (fadeTime == wipe));
		//eventSystem.gameObject.SetActive(true);
		Time.timeScale = 1;
		SceneManager.LoadScene(scene);
		UIManager.current.SetAllGameCanvasesActive(false);
		yield return new WaitForSeconds(0.1f);
		LoadLevelBehaviour(scene);
	}

	public void LoadLevelBehaviour(int scene)
	{
		StartCoroutine(LoadLevelBehaviourCoroutine(scene));

		IEnumerator LoadLevelBehaviourCoroutine(int scene)
		{
			yield return new WaitForEndOfFrame();
			UIManager.current.UpdateGameCanvasRefs();
			if (SceneManager.GetActiveScene().name == "End") //hack because we don't yet know what number the final scene is
			{
				UIManager.current.SetAllGameCanvasesActive(false);
			}
			else
			{
				switch (scene)
				{
					case 0:
						{
							
							score = 0;
							PlayerManager.current.SetFormation(0);
							UIManager.current.ResetFormationVis();
							UIManager.current.SetAllGameCanvasesActive(false);


							for (int i = 0; i < 3; i++)
							{
								CurrentJobs[i] = Jobs[Random.Range(0, Jobs.Length)];
							}

							for (int i = 0; i < PlayerManager.current.Party.Length; i++)
							{
								PlayerManager.current.Party[i].SetJob(CurrentJobs[i], true);
							}
							break;
						}
					default:
						{
							PlayerManager.current.SetFormation(0);
							UIManager.current.ResetFormationVis();
							UIManager.current.SetAllGameCanvasesActive(true);

							for (int i = 0; i < PlayerManager.current.Party.Length; i++)
							{
								PlayerManager.current.Party[i].SetJob(CurrentJobs[i], false);
							}
							break;
						}
				}
			}

			yield return new WaitForSecondsRealtime(0.25f);
			StartCoroutine(WipeScreen(-wipeSpeed, 1, 0));

			//later behaviours here
			Debug.Log("starting");
			if (SceneManager.GetActiveScene().name == "End") //hack because we don't yet know what number the final scene is
			{
				StartCoroutine(FadeMusic(audioFadeSpeed, 0, 1));
				BGMSource.clip = MenuTheme;
				BGMSource.Play();
			}
			else
				switch (scene)
				{
					case 0:
						{
							if (!BGMSource.isPlaying) 
							{
								BGMSource.clip = MenuTheme;
								BGMSource.Play();
								StartCoroutine(FadeMusic(audioFadeSpeed, 0, 1));
								Debug.Log("fading in special on 0");
							} 
							PlayerManager.current.playerController.started = true;
							started = true;
							break;
						}
					case 1:
						{

							PlayerManager.current.playerController.started = true;
							started = true;
							if (BGMSource.clip != GameTheme)
							{
								BGMSource.clip = GameTheme;
								BGMSource.Play();
								StartCoroutine(FadeMusic(audioFadeSpeed, 0, 1));
							}
							break;
						}
					default:
						{
							PlayerManager.current.playerController.started = true;
							started = true;
							if (BGMSource.clip != GameTheme)
							{
								BGMSource.clip = GameTheme;
								BGMSource.Play();
								StartCoroutine(FadeMusic(audioFadeSpeed, 0, 1));
							}
							break;
						}
				}
		}
		

	}
	public void ReloadCurrentLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

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

	public IEnumerator FadeMusic(float rate, float start, float goal)
	{
		audioFadeTime = start;
		audioFadeGoal = goal;

		while (audioFadeTime != goal)
		{
			//Debug.Log($"Ticking by { rate * 0.0125f}");
			BGMSource.volume = audioFadeTime;

			audioFadeTime += rate * 0.0125f;

			audioFadeTime = Mathf.Clamp01(audioFadeTime);
			yield return new WaitForEndOfFrame();

			if (rate > 1 && audioFadeTime > goal)
				audioFadeTime = goal;

			if (rate < 1 && audioFadeTime < goal)
				audioFadeTime = goal;

		}

		BGMSource.volume = goal;
	}
}