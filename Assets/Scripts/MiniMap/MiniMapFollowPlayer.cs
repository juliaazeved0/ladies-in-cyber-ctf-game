using UnityEngine;

/// <summary>
/// Controla a posicao da camera do minimapa para acompanhar a posicao da jogadora.
/// </summary>
public class MiniMapFollowPlayer : MonoBehaviour
{
    [Header("Target Tracking")]
    [Tooltip("O Transform do alvo que o minimapa deve seguir (Player).")]
    [SerializeField] private Transform target;

    //Chamado no frame final para garantir que o minimapa atualize apos a movimentacao da jogadora
    void LateUpdate()
    {
        if(target == null) //Tratamento de erro
        {
            GameObject playerOnMap = GameObject.FindGameObjectWithTag("Player");
            
            if(playerOnMap != null)
            {
                target = playerOnMap.transform;
            }
            else
            {
                //Se a player ainda nao existe na cena, interrompe a execucao para evitar NullReferenceException
                return;
            }
        }

        //Atualiza a posicao X e Y acompanhando a player, mantendo o Z fixo da camera do minimapa
        transform.position = new Vector3(
            target.position.x,
            target.position.y,
            transform.position.z
        );
    }
}