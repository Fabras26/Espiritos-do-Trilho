using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventario : MonoBehaviour
{
    [SerializeField]
    private GameObject painel;
    public GameObject prefab;
   

    public void AddItem(int idItem, Sprite imgItem)
    {
        var residuo = Instantiate(prefab, painel.transform);
        residuo.GetComponent<Image>().sprite = imgItem;
        residuo.GetComponent<ItemNoInventario>().Id = idItem;
    }
    public void Adicionar(Item item)
    {
        AddItem(item.idInventario, item.imgInventario);
    }
    public void UseItem(int IdItem)
    {
        ItemNoInventario[] obj = FindObjectsOfType<ItemNoInventario>();

        for(int i = 0; i < obj.Length; i++)
        {
            if(obj[i].Id == IdItem)
            {
                Destroy(obj[i].gameObject);
                break;
            }
        }

        
    }
    public bool ChecarItem(int IdItem)
    {
        ItemNoInventario[] obj = FindObjectsOfType<ItemNoInventario>();

        for (int i = 0; i < obj.Length; i++)
        {
            if (obj[i].Id == IdItem)
            {
                return true;
            }
        }
        return false;
    }
}
