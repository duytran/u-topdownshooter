using UnityEngine;
using System.Collections;

public class Shell : MonoBehaviour {

    private float lifeTime = 5f;
    private Material mat;
    private Color origialCol;
    private float fadePercent;
    private float deathTime;
    private bool fading;

	// Use this for initialization
	void Start () {
        deathTime = Time.time + lifeTime;
        mat = GetComponent<Renderer>().material;
        origialCol = mat.color;
        StartCoroutine("Fade");
	}

	IEnumerator Fade () {
        while (true) {
            yield return new WaitForSeconds (0.2f);

            if ( fading ) {
                fadePercent += Time.deltaTime;
                mat.color = Color.Lerp(origialCol, Color.clear, fadePercent);

                if ( fadePercent >= 1 ) {
                    Destroy(gameObject);
                }
            }
            else {
                if ( Time.time > deathTime ) {
                    fading = true;
                }
            }
        }
    }

    void OnTriggerEnter (Collider c) {
        if ( c.tag == "Ground" ) {
            GetComponent<Rigidbody>().Sleep();
        }
    }
}
