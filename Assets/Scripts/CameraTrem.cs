using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class CameraTrem : MonoBehaviour
{
    public Vector2 intervaloTempo;
    public AudioClip som;

    private void Start()
    {
        StartCoroutine(Tremer());
    }
    public IEnumerator Tremer()
    {
        yield return new WaitForSeconds(Random.Range(intervaloTempo.x,intervaloTempo.y));
        CameraShaker.Instance.ShakeOnce(2f, 2f, 1f, 1f);

        GameObject.Find("Efeitos Sonoros").GetComponent<AudioSource>().PlayOneShot(som);
        StartCoroutine(Tremer());
    } 
}
