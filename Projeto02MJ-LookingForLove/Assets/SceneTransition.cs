using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    [Header("Configurações")]
    [Tooltip("Arraste aqui o GameObject que contém o Animator do FadeOut.")]
    public GameObject fadeObject;

    [Tooltip("Nome do parâmetro Trigger no Animator que dispara o fade.")]
    public string fadeTriggerName = "FadeOut";

    [Tooltip("Duração exata da animação de fadeout em segundos.")]
    public float fadeDuration = 1.0f;

    // Método chamado pelo botão no OnClick
    public void FadeAndLoadScene(string sceneName)
    {
        StartCoroutine(TransitionRoutine(sceneName));
    }

    private IEnumerator TransitionRoutine(string sceneName)
    {
        // Ativa o objeto do fade caso esteja desativado
        if (fadeObject != null)
        {
            fadeObject.SetActive(true);
            Animator anim = fadeObject.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger(fadeTriggerName);
            }
        }

        // Espera o tempo da animação terminar
        yield return new WaitForSeconds(fadeDuration);

        // Carrega a nova cena
        SceneManager.LoadScene(sceneName);
    }
}
