using UnityEngine;

[ExecuteInEditMode]
public class TransitionManager : MonoBehaviour
{
	public Material transitionMaterial;
	public static TransitionManager current;


	private void Start()
	{
		if (current != this && current != null) 
		{ Destroy(this.gameObject); return; }

		current = this;
	}

	private void Update()
	{
		//this is hacky but it lets us see the transitions in editor mode without Unity Crying too much
		if(!Application.isPlaying)
			current = this;
	}
}
