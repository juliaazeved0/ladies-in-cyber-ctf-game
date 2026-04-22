using UnityEngine;

public class BossFadeManager : MonoBehaviour
{
    void Awake()
    {
        // 1. LIMPEZA: Procura o painel que veio da cena anterior (o "intruso")
        // O nome deve ser o nome que o objeto tinha na cena do mapa
        GameObject antigoFade = GameObject.Find("PanelFade"); 
        
        if (antigoFade != null && antigoFade != this.gameObject)
        {
            Destroy(antigoFade);
        }
    }

    // 2. O MÉTODO DO EVENTO: Adicione este no último frame da sua animação
    public void FinalizarFadeBoss()
    {
        // Desativa o próprio painel onde o script está colado
        gameObject.SetActive(false);
        
        Debug.Log("Luzes acesas! O painel foi desativado e os cliques liberados.");
    }
}