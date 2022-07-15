using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager current;
	private void Start()
	{
		if (current != null && current != this)
		{ Destroy(this.gameObject); return; }

		current = this;
		Application.targetFrameRate = 60;
		//Screen.SetResolution(480, 360, false);
	}
}