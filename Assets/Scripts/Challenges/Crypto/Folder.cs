using UnityEngine;

/// <summary>
/// Representa uma pasta dentro do sistema de arquivos simulado do jogo.
/// </summary>
public class Folder : MonoBehaviour
{
    [Header("Folder Content")]
    [Tooltip("GameObject com o conteudo desta pasta (arquivos, subpastas, etc.), exibido ao abri-la.")]
    public GameObject contentFolder;

    [Tooltip("Nome de exibicao da pasta.")]
    public string folderName;
}