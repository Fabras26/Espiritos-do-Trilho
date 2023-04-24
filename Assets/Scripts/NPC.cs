using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public bool dialogoDisponivel;

    public Transform cabeca;

    public bool giravel = true;

    private void FixedUpdate()
    {

        //transform.LookAt(GameObject.Find("Jogador").transform);
        if (giravel)
        {
            var lookPos = GameObject.Find("PlayerCameraRoot").transform.position - transform.position;



            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);

        }


        if (cabeca != null) cabeca.LookAt(GameObject.Find("PlayerCameraRoot").transform);

    }

    public void Dialogar()
    {
        if(dialogoDisponivel)
        {
            GetComponent<Dialogos>().IniciarDialogo();
        }
    }

    public void Disponivel(bool state)
    {
        dialogoDisponivel = state;
    }
}
