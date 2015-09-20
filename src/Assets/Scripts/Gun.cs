using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AudioSource))]
public class Gun : MonoBehaviour {

    // Gun type
    public enum GunType {Semi, Burst, Auto};
    public GunType gunType;
    // Round per minute
    public float rpm;



    // System
    private float secondsBetweenShots;
    private float nextPossibleShootTime;

    // Components
    public Transform spawn;
    public Transform shellEjectionPoint;
    public Rigidbody shell;
    private AudioSource audio;
    private LineRenderer tracer;

    void Start () {
        secondsBetweenShots = 60/rpm;
        audio = GetComponent<AudioSource>();

        if (GetComponent<LineRenderer> ()) {
            tracer = GetComponent<LineRenderer> ();
        }
    }

    public void Shoot () {

        if ( CanShoot () ) {
            // Create a ray with spawn position and get forward drection
            Ray ray = new Ray (spawn.position, spawn.forward);
            RaycastHit hit;

            // Default shoot distance is 20;
            float shootDistance = 20;

            // Physic raycast with ray object and shoot distance
            if (Physics.Raycast(ray, out hit, shootDistance)) {
                // If hit available mark shoot distance by hit.distance
                shootDistance = hit.distance;
            }

            // Debug
            Debug.DrawRay(ray.origin, ray.direction * shootDistance, Color.red, 1);

            nextPossibleShootTime = Time.time + secondsBetweenShots;
            audio.Play ();

            if ( tracer ) {
                StartCoroutine("RenderTracer", ray.direction * shootDistance);
            }

            Rigidbody newShell = Instantiate(shell, shellEjectionPoint.position, Quaternion.identity)as Rigidbody;
            newShell.AddForce(shellEjectionPoint.forward * Random.Range(150f, 200f) + spawn.forward* Random.Range(-10f, 10f));
        }
    }

    public void ShootContinuous () {
        // If gun type is auto
        if ( gunType == GunType.Auto ) {
            Shoot ();
        }
    }

    private bool CanShoot () {
        bool canShoot = true;

        if ( Time.time < nextPossibleShootTime ) {
            canShoot = false;
        }
        return canShoot;
    }

    IEnumerator RenderTracer (Vector3 hitPoint) {
        tracer.enabled = true;
        tracer.SetPosition (0, spawn.position);
        tracer.SetPosition (1, spawn.position + hitPoint);
        yield return null;
        tracer.enabled = false;
    }
}
