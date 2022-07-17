using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(-100)]
public class EndMenuManager : MonoBehaviour
{
	public Canvas GameWin;
	public Canvas GameOver;
	private void Start()
	{
		StartCoroutine(StartManager());
		IEnumerator StartManager()
		{
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
	}
}
