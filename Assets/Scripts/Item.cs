using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{

    public bool inspecionavel;

    public bool coletavel;
    public Sprite imgInventario;
    public int idInventario;

    public AudioClip audioClip;
    public AudioClip audiocolect;


    [TextArea]
    public string Text;
}
