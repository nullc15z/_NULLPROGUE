using UnityEngine;
using System.Collections;

public class PlayerSCR : MonoBehaviour {
	
	public bool moveFlag;
	public bool sceneJumpFlag = false;

	public Vector2 move;
	public Vector2 direction;
	
	// Use this for initialization
	void Start () {
		iTween.Init (gameObject);
		direction = new Vector2 ( 0, -1 );
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
		Attack ();
	}

	void Move() {
		if (moveFlag) return;

		move = Vector2.zero;
		move.x = Input.GetAxisRaw ("Horizontal");
		move.y = Input.GetAxisRaw ("Vertical") ;

		if( Input.GetKey( "x" ) ) {
			if (move.x != 0 && move.y != 0) {
				HitCheck( "move" );
			}
		} else {
			if (move.x != 0 || move.y != 0) {
				HitCheck( "move" );
			}
		}
		SetMove(move);
	}

	void Attack() {
		if (moveFlag) return;

		if (Input.GetKey ("z")) {
			HitCheck ( "attack" );
			SetAttack();
		}

	}
	
	void HitCheck( string pattern ) {
		moveFlag = true;
		
		RaycastHit2D hit = new RaycastHit2D();
		Vector2 tmpStartPoint = this.gameObject.transform.position;
		float tmpRange = Mathf.Sqrt( Mathf.Abs(direction.x) + Mathf.Abs(direction.y) );
		
		
		switch( pattern ) {
		case "move":
			direction  = move;
			hit = Physics2D.Raycast (tmpStartPoint, direction, tmpRange);
			if ( hit ) {
				if( hit.transform.gameObject.name == "Floor_Step" ) {
					sceneJumpFlag = true;
				}
				if( hit.transform.gameObject.tag == "Wall" ) {
					moveFlag = false;
				}
				if( hit.transform.gameObject.tag == "Wall_Hidden" ) {
					moveFlag = false;
				}
			}
			break;
		case "attack":
			hit = Physics2D.Raycast (tmpStartPoint, direction, tmpRange);
			if (hit) {
				if (hit.transform.gameObject.tag == "Wall_Hidden") {
					Wall_HiddenSCR wall_hiddenSCR = hit.transform.gameObject.GetComponent<Wall_HiddenSCR>();
					wall_hiddenSCR.Hit ( direction );
				}
			}
			break;
		}
	}

	void SetAttack() {
		if( !moveFlag ) return;

		GameObject tmpObj;
		Vector3 tmpPos = gameObject.transform.position + (Vector3)direction;
		tmpObj = (GameObject)Instantiate(
			Resources.Load( "Prefabs/Effect/Effect_Attack" ),
			tmpPos,
			Quaternion.identity);
		Destroy( tmpObj, 0.250f );

		Vector3 targetScale = new Vector3 ( 1.5f,1.5f,1.0f);
		Hashtable hash = new Hashtable();
		hash.Add( "scale", targetScale );
		hash.Add( "time", 0.5f );
		hash.Add( "oncomplete", "OnCompleteCallback");
		hash.Add( "oncompleteparams", "AttackComplete");
		hash.Add( "oncompletetarget", gameObject );
		iTween.ScaleFrom (gameObject, hash );
		moveFlag = true;
	}

	void SetMove( Vector2 move ) {
		if (!moveFlag) return;

		Vector3 targetPos = new Vector3 (
			gameObject.transform.position.x + move.x,
			gameObject.transform.position.y + move.y,
			0.0f);
		Hashtable hash = new Hashtable();
		hash.Add( "position", targetPos );
		hash.Add( "time", 0.5f );
		hash.Add( "oncomplete", "OnCompleteCallback");
		hash.Add( "oncompleteparams", "MoveComplete");
		hash.Add( "oncompletetarget", gameObject );
		iTween.MoveTo (gameObject, hash );
		moveFlag = true;
	}
	
	// iTween Complete
	void OnCompleteCallback( string message )
	{
		moveFlag = false;

		if (sceneJumpFlag) {
			switch( Application.loadedLevelName ) {
			case "_Town":
				Application.LoadLevel( "_Dungeon" );
				break;
			case "_Dungeon":
				Application.LoadLevel( "_Town" );
				break;
			}
		}
	}
}
