
public class CrimeBroadcast : MonoBehaviour {

    public static Collider2D[] objInRange;
	
	private float radius = 10;

	public void Broadcast() {
		Vector2 pos = new Vector2(transform.position.x, transform.position.y);
		objInRange = Physics2D.OverlapCircleAll(pos, radius);
		for o in objInRange:
			crimeManager = o.GetComponent<CrimeManager>();
			if (crimeManager) {
				Vector3 direction = o.transform.position - transform.position;
				direction.Normalize();
				float strInput = Vector3.Dot(direction, transform.right);
				if (strInput > 0) {
					crimeManager.onCrimeHappen.Invoke(o.player.position, direction);
				}
			}
	}
 
}
