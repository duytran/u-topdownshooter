using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    // Gun type
    public enum GunType {Semi, Burst, Auto};
    public GunType gunType;

    // The spaw object
    public Transform spawn;

    public void Shoot () {

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
    }

    public void ShootContinuous () {
        // If gun type is auto
        if ( gunType == GunType.Auto ) {
            Shoot ();
        }
    }
}
