using UnityEngine;
using System.Collections;

public class Wall_HiddenSCR : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Hit( Vector2 direction ) {
		if ( direction.x == 0 || direction.y == 0 ) {
			Destroy (gameObject);

			RaycastHit2D hit = new RaycastHit2D();
			float tmpRange = Mathf.Sqrt( Mathf.Abs(direction.x) + Mathf.Abs(direction.y) );
			hit = Physics2D.Raycast ( this.gameObject.transform.position, direction, tmpRange);
			if (hit) {
				if (hit.transform.gameObject.tag == "Wall_Hidden") {
					Wall_HiddenSCR wall_hiddenSCR = hit.transform.gameObject.GetComponent<Wall_HiddenSCR>();
					wall_hiddenSCR.Hit ( direction );
				}
			}
		}
	}

}
