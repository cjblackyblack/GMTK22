using UnityEngine;

[SerializeField]
public interface ITimeScaleable
{
	void SubscribeTime();
	void UnsubscribeTime();
	void SetTimeScale(float speed);
}

[SerializeField]
public interface IInteractable
{
	void OnInteract();
}
