using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipalManager : MonoBehaviour
{
    public void Jogar()
    {
        if (!string.IsNullOrEmpty("SampleScene"))
        {
            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            Debug.LogError("Erro: Nome da cena não foi definido! Defina um nome no Inspector.");
        }
    }

    public void SairJogo()
    {
        Debug.Log("Saindo do jogo...");
        Application.Quit();
    }
}
