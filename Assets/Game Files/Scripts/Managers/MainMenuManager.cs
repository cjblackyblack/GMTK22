using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
	public void OnStartButton()
	{
		GameManager.current.round = 1;
		GameManager.current.LoadLevel(GameManager.current.round);
	}
}
