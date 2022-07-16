using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyElement : MonoBehaviour
{
	public Transform healthbar;
	public void UpdateHealth(int newHPTotal)
	{
		int display = Mathf.CeilToInt(newHPTotal * 0.5f);
		int displayRemainder = newHPTotal % 2;

		foreach (Transform healthIcon in healthbar)
		{
			healthIcon.GetChild(0).gameObject.SetActive(healthIcon.GetSiblingIndex() <= display - 1);
			healthIcon.GetChild(0).GetComponentInChildren<Image>().sprite = healthIcon.GetSiblingIndex() == display - 1 && displayRemainder > 0 ? UIManager.current.HalfHeart : UIManager.current.FullHeart;
		}
	}

	public void SetMaxHealth(int maxHP)
	{
		int display = Mathf.CeilToInt(maxHP * 0.5f);
		foreach (Transform healthIcon in healthbar)
		{
			healthIcon.gameObject.SetActive(healthIcon.GetSiblingIndex() <= display - 1);
		}
	}
}
