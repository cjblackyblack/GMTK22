using UnityEngine;

public static class StaticMethods
{
    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

	public static Vector3 ConvertVector2(this Vector2 vect2)
	{
		Vector3 vect2Convert = new Vector3((vect2.x), 0, (vect2.y));
		return vect2Convert;
	}

	public static Vector3 RotateVector(this Vector3 vect3, Vector2 _facingDir)
	{
		Quaternion rotation = Quaternion.Euler(0, 0, (Mathf.Atan2(_facingDir.y, _facingDir.x) * Mathf.Rad2Deg));
		Vector2 vect2 = new Vector2(vect3.x, vect3.z).Rotate(rotation.eulerAngles.z);
		Vector3 vect3Rotated = new Vector3((vect2.x), 0, (vect2.y));
		return vect3Rotated;
	}

	public static bool ValidTarget(TangibleObject selfObject, TangibleObject targetObject, GambitTarget targetAlliance)
	{

		if (targetObject.properties.objectTangibility == PhysicalObjectTangibility.Intangible)
			return false;

		switch (targetAlliance)
		{
			case GambitTarget.Ally:
				{
					if (selfObject.properties.alliance == targetObject.properties.baseAlliance)
						return true;
					return false;
				}
			case GambitTarget.AllyExcludeSelf:
				{
					if (selfObject.properties.alliance == targetObject.properties.baseAlliance && selfObject != targetObject)
						return true;
					return false;
				}
			case GambitTarget.Enemy:
				{
					if (selfObject.properties.alliance != targetObject.properties.baseAlliance)
						return true;
					return false;
				}
			case GambitTarget.Neutral:
				{
					return true;
				}
			case GambitTarget.Any:
				{
					return true;
				}
			case GambitTarget.AnyExcludeAlly:
				{
					if (targetObject.properties.baseAlliance != Alliance.Ally)
						return true;
					else
						return false;
				}
			case GambitTarget.AnyExcludeEnemy:
				{
					if (targetObject.properties.baseAlliance != Alliance.Enemy)
						return true;
					else
						return false;
				}
			case GambitTarget.AnyExcludeNeutral:
				{
					return false;
				}
			case GambitTarget.AnyExcludeSelf:
				{
					if (targetObject != selfObject)
						return true;
					else
						return false;
				}
			case GambitTarget.Self:
				{
					if (targetObject == selfObject)
						return true;
					else
						return false;
				}
		}
		return false;
	}
}