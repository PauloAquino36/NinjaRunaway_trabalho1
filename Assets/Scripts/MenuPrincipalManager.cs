using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipalManager : MonoBehaviour
{
    [SerializeField] private string nomeDoLevelDeJogo = "SampleScene";
    [SerializeField] private GameObject painelMenuInicial;
    [SerializeField] private GameObject painelOpcoes;

    public void Jogar()
    {
        if (!string.IsNullOrEmpty(nomeDoLevelDeJogo))
        {
            SceneManager.LoadScene(nomeDoLevelDeJogo);
        }
        else
        {
            Debug.LogError("Erro: Nome da cena não foi definido! Defina um nome no Inspector.");
        }
    }

    public void AbrirOpcoes()
    {
        painelMenuInicial.SetActive(false);
        painelOpcoes.SetActive(true);
    }

    public void FecharOpcoes()
    {
        painelMenuInicial.SetActive(true);
        painelOpcoes.SetActive(false);
    }

    public void SairJogo()
    {
        Debug.Log("Saindo do jogo...");
        Application.Quit();
    }
}
