using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-100)]
public class EndMenuManager : MonoBehaviour
{
	public Canvas GameWin;
	public Canvas GameOver;
	public Button button;
	public Button Wbutton;

	private void Start()
	{
		StartCoroutine(StartManager());
		IEnumerator StartManager()
		{
			button.gameObject.SetActive(true);
			Wbutton.gameObject.SetActive(true);
			yield return new WaitForEndOfFrame();
			if (GameManager.current.GameWon)
			{
				GameWin.transform.GetChild(0).gameObject.SetActive(true);
				GameOver.transform.GetChild(0).gameObject.SetActive(false);
			}
			else
			{
				GameOver.transform.GetChild(0).gameObject.SetActive(true);
				GameWin.transform.GetChild(0).gameObject.SetActive(false);
			}
		}

	}
	public void OnQuitButton()
	{
		GameManager.current.LoadLevel(0);
		button.gameObject.SetActive(false);
		Wbutton.gameObject.SetActive(false);
	}


}
