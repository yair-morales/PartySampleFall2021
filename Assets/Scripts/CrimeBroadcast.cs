using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrimeBroadcast : MonoBehaviour {
	
    public static Collider2D[] objInRange;
	
	private float radius = 10;

	public void Broadcast() {
		Vector2 pos = new Vector2(transform.position.x, transform.position.y);
		objInRange = Physics2D.OverlapCircleAll(pos, radius);

		foreach (var o in objInRange) {
			CrimeManager crimeManager = o.GetComponent<CrimeManager>();
			if (crimeManager) {
				Vector3 direction = o.transform.position - transform.position;
				direction.Normalize();
				float strInput = Vector3.Dot(direction, transform.right);
				if (strInput > 0) {
					crimeManager.TriggerCrime(o.transform.position, direction);
				}
			}
		}
	}
	
}