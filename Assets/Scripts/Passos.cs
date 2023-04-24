using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passos : MonoBehaviour
{
    [System.Serializable]
    public class SomPassos
    {
        public string tagchao;
        public AudioClip[] audios;
    }
    public AudioClip[] SomPassosPadrao;
    public SomPassos[] SomDosPassos;
    string tagAtual;
    public void TocarSomPasso(AudioSource audiosource)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up * -1, out hit, 0.2f))
        {
            tagAtual = hit.collider.tag;
        }
        if (tagAtual != "Untagged")
        {
            for (int i = 0; i < SomDosPassos.Length; i++)
            {
                if (tagAtual == SomDosPassos[i].tagchao)
                {   
                    int s = Random.Range(0, SomDosPassos[i].audios.Length);
                    audiosource.PlayOneShot(SomDosPassos[i].audios[s]);
                    return;
                }
            }
        }
            int x = Random.Range(0, SomPassosPadrao.Length);
            audiosource.PlayOneShot(SomPassosPadrao[x]);
    }

}

