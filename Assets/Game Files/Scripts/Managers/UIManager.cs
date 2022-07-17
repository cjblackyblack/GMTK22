using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
	public static UIManager current;

	[Header("Refs")]

	public RectTransform FormationVisParent;
	public RectTransform[] FormationVis;
	public TextMeshProUGUI scoreText;
	public TextMeshProUGUI roundTimerText;

	[Header("Scoring")]
	private int displayScore;
	private int lateScore;
	[SerializeField]
	private int lateScoreIncrement;

	[Header("Timing")]
	public float RoundTime;
	public float RoundTimer;

	[Header("HP")]
	public PartyElement[] PartyElements;
	public Sprite FullHeart;
	public Sprite HalfHeart;

	public Canvas[] GameCanvases;

	public void UpdateGameCanvasRefs()
	{
		foreach(Canvas canvas in GameCanvases)
		{
			canvas.renderMode = RenderMode.ScreenSpaceCamera;
			canvas.worldCamera = Camera.main;
		}
	}

	private void Start()
	{
		if (current != null && current != this)
		{ Destroy(this.gameObject); return; }

		current = this;
	}

	public void ResetFormationVis()
	{
		FormationVis = new RectTransform[3];
		for(int i = 0; i < FormationVis.Length; i++)
		{
			FormationVis[i] = FormationVisParent.GetChild(i).GetComponent<RectTransform>();
		}

		for (int i = 0; i < FormationVis.Length; i++)
		{
			FormationVis[i].localPosition = new Vector3(PlayerManager.current.PlayerFormations[0].playerPositions[FormationVis[i].transform.GetSiblingIndex()].LocalOffset.normalized.x * FormationVis[i].sizeDelta.x, PlayerManager.current.PlayerFormations[0].playerPositions[FormationVis[i].transform.GetSiblingIndex()].LocalOffset.normalized.z * FormationVis[i].sizeDelta.y);
			FormationVis[i].localRotation = Quaternion.Euler(new Vector3(0, 0, (Mathf.Atan2(PlayerManager.current.PlayerFormations[0].playerPositions[FormationVis[i].transform.GetSiblingIndex()].FacingDir.y, PlayerManager.current.PlayerFormations[0].playerPositions[FormationVis[i].transform.GetSiblingIndex()].FacingDir.x) * Mathf.Rad2Deg - 90)));
		}
	}

	private void Update()
	{
		if (GameManager.current.round == 0)
			return;

		for(int i = 0; i < FormationVis.Length; i++)
		{
			FormationVis[i].localPosition = Vector3.Lerp(FormationVis[i].localPosition, new Vector3( PlayerManager.current.PlayerFormations[PlayerManager.current.currentFormation].playerPositions[i].LocalOffset.normalized.x * FormationVis[i].sizeDelta.x, PlayerManager.current.PlayerFormations[PlayerManager.current.currentFormation].playerPositions[i].LocalOffset.normalized.z * FormationVis[i].sizeDelta.y), 0.1f);
			FormationVis[i].localRotation = Quaternion.Slerp(FormationVis[i].localRotation, Quaternion.Euler( new Vector3(0, 0, (Mathf.Atan2(PlayerManager.current.PlayerFormations[PlayerManager.current.currentFormation].playerPositions[i].FacingDir.y, PlayerManager.current.PlayerFormations[PlayerManager.current.currentFormation].playerPositions[i].FacingDir.x) * Mathf.Rad2Deg - 90))), 0.1f);
		}

		if(lateScore < displayScore)
		{
			lateScore += lateScoreIncrement;

		}

		scoreText.text = $"{lateScore:D7}";

		if (GameManager.current.started)
		{
			if (RoundTimer > 0)
			{
				roundTimerText.text = $"{(RoundTimer / RoundTime) * 100:00}";
				RoundTimer -= (RoundTimer / RoundTime > 0.1f ? Time.deltaTime : (RoundTimer / RoundTime < 0.01f ? Time.deltaTime * 0.25f : Time.deltaTime * 0.65f));
			}
			else
			{
				GameManager.current.GameOver(false);
			}
		}
		else
		{
			if (RoundTimer != RoundTime)
				RoundTimer = RoundTime;
		}
	}

	public void UpdateDisplayScore(int _displayScore)
	{
		displayScore = _displayScore;
		if (_displayScore == 0)
			lateScore = 0; 
	}

	public void SetAllGameCanvasesActive(bool enabled)
	{
		foreach (Canvas canvas in GameCanvases)
			canvas.transform.GetChild(0).gameObject.SetActive(enabled);
	}
}