using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Cena : MonoBehaviour
{
    public void CarregarCena(string nome)
    {
        StartCoroutine(CarregarCenaCor(nome));
    }
    public IEnumerator CarregarCenaCor(string nome)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(nome);
    }
}
