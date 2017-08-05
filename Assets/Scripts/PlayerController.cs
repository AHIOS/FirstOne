using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum MaterialState {gem, enemy, undefined};

public class PlayerController : MonoBehaviour {

	public float speed;

	public Rigidbody rb;
	private Renderer rend;

	private MaterialState currentMaterial;

	private int gemCount;
	private int enemyCount;

	public Text gemText;
	public Text enemyText;




	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		rend = GetComponent<Renderer>();
		currentMaterial = MaterialState.undefined;
		gemCount = 0;
     	enemyCount = 0;

		updateGemText();
		updateEnemiesText ();

		#if UNITY_IOS
			Screen.orientation = ScreenOrientation.LandscapeLeft;
		#endif

	}

	// Update is called once per frame
	void Update () {

	}

	// Physics calculation
	void FixedUpdate () {
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		#if UNITY_IOS
			moveHorizontal = Input.acceleration.x;
			moveVertical = Input.acceleration.y;
		#endif


		Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
		rb.AddForce(movement * speed);
	}

	void OnTriggerEnter	(Collider other){
		if (other.gameObject.CompareTag ("Pick Up")) {
			if (currentMaterial == MaterialState.gem) {
				other.gameObject.SetActive (false);
				gemCount++;
				updateGemText ();
			}
		} else if (other.gameObject.CompareTag ("Enemy")) {
			enemyCount++;
			updateEnemiesText ();
			other.gameObject.SetActive (false);
			gameObject.SetActive (false);
		} else if (other.gameObject.CompareTag ("Gem Wall")) {
			Debug.Log ("on trigger enter GEM wall");
			rend.material = other.GetComponent<Renderer> ().material;
			other.isTrigger = false;
			currentMaterial = MaterialState.gem;

		} else if (other.gameObject.CompareTag ("Enemy Wall")) {
			Debug.Log ("on trigger enter ENEMY wall");
			rend.material = other.GetComponent<Renderer> ().material;
			other.isTrigger = false;
			currentMaterial = MaterialState.enemy;
		}

//		Debug.Log (other.GetComponent<Renderer>().material.name);
//		Debug.Log (rend.material.name);
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.CompareTag ("Gem Wall")) {
			Debug.Log ("on trigger exit GEM wall");
			other.isTrigger = true;

		} else if (other.gameObject.CompareTag ("Enemy Wall")) {
			Debug.Log ("on trigger exit ENEMY wall");
			other.isTrigger = true;
		}
	
	}
	void updateGemText(){
		gemText.text = "Gems: " + gemCount.ToString();
	}

	void updateEnemiesText(){
		enemyText.text = "Enemies: " + enemyCount.ToString();
	}
}
