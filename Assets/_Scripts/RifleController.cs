using UnityEngine;

internal class RifleController : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem bulletsEmitter;

    public void Shoot()
    {
        bulletsEmitter.Play();
    }
}