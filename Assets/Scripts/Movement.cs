using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    public float speed = 10f;
    float hori;
    float vert;
    Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        hori = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");
        anim.SetFloat("Horizontal", hori);
        anim.SetFloat("Vertical", vert);

        transform.Translate(new Vector3(hori * Time.deltaTime * speed, 0f, vert * Time.deltaTime * speed));
    }
}
