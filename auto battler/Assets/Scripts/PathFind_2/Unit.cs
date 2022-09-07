using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {


	[SerializeField]private Vector3 target;
	float speed = 3;
	Vector3[] path;
	int targetIndex;
	Attributes attributes;


	private void Awake()
	{
		attributes = GetComponent<Attributes>();
	}

	public Vector3 GetTarget()
	{
		return target;
	}

	public void SetTarget(Vector3 _target) 
	{
		this.target = _target;
        PathRequestManager.RequestPath(transform.position, target, OnPathFound);
    }

	public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
		if (pathSuccessful) {
			path = newPath;
			targetIndex = 0;
			StopCoroutine(nameof(FollowPath));
			StartCoroutine(nameof(FollowPath));
		}
	}

	public void EndMove()
	{
		StopCoroutine(nameof(FollowPath));
        gameObject.SendMessage("EndMovement", SendMessageOptions.RequireReceiver);
    }

	IEnumerator FollowPath() {
		if (path.Length < 1)
		{
            gameObject.SendMessage("EndMovement", SendMessageOptions.RequireReceiver);
            yield break;
        } 
		Vector3 currentWaypoint = path[0];
		while (true) {
			if (transform.position == currentWaypoint) {
				targetIndex ++;
				if (targetIndex >= path.Length) {
					gameObject.SendMessage("EndMovement", SendMessageOptions.RequireReceiver);
					yield break;
				}
				currentWaypoint = path[targetIndex];
			}
			attributes.SetFacing((currentWaypoint - transform.position).normalized);
			transform.position = Vector3.MoveTowards(transform.position,currentWaypoint,speed * Time.deltaTime);
			yield return null;

		}
	}

	public void OnDrawGizmos() {
		if (path != null) {
			for (int i = targetIndex; i < path.Length; i ++) {
				Gizmos.color = Color.black;
				Gizmos.DrawCube(path[i], new Vector3(0.2f, 0.2f, 0.2f));

				if (i == targetIndex) {
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else {
					Gizmos.DrawLine(path[i-1],path[i]);
				}
			}
		}
	}
}
