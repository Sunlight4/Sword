using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordy : MonoBehaviour {
	public float swordDistance = 1;
	Rigidbody2D rb;
	public float flingStrength = 1;
	public float defaultWidth = 0.5f;
	public float lineLength = 2;
	LineRenderer rendy;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		rendy = GetComponent<LineRenderer> ();
		rendy.SetPosition (0, transform.position);

	}
	void Hit(Vector2 direction, Collider2D col) {
		//First try - assume point is hard
		rb.velocity=(direction)*-flingStrength*col.GetComponent<HardSoft>().hardness;
	}
	// Update is called once per frame
	void Update () {
		transform.position -= (Vector2) rb.velocity / 2;
		Vector2 direction;
		Vector3 target;
		target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		target.z = transform.position.z;
		direction = (Vector2)(target - transform.position);
		RaycastHit2D hit = Physics2D.Raycast (transform.position, direction, swordDistance);
		if (hit.collider != null) {
			rendy.startWidth = rendy.endWidth = defaultWidth;
			rendy.SetPosition (1, -((Vector2) hit.point-(Vector2) transform.position).normalized*hit.collider.GetComponent<HardSoft>().hardness*lineLength);
			if (Input.GetMouseButtonDown (0)) {
				Hit (direction.normalized, hit.collider);
			}
		} else {
			rendy.startWidth = rendy.endWidth = 0f;
		}
	}
	void OnTriggerEnter2D(Collider2D col) {
		rb.velocity = new Vector3 (0, 0, 0);
		rb.gravityScale = 0;
	}
	void OnTriggerExit2D(Collider2D col) {
		rb.gravityScale = 1;
	}
}
