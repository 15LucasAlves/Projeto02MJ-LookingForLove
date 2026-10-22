using CardGame;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CardDirectionGame : MonoBehaviour
{
    [Header("UI - Rondas e Temporizador")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI roundText;
    public float roundDuration = 5.0f;
    public float roundInterval = 1.5f;

    [Header("UI - Painel de Fim de Jogo")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI winnerText;
    public TextMeshProUGUI p1ScoreText;
    public TextMeshProUGUI p2ScoreText;
    public string menuSceneName = "MainMenu";

    [Header("UI - Direções (Cima, Baixo, Esquerda, Direita)")]
    public RawImage[] directionRawImages = new RawImage[4]; // 0: Cima, 1: Baixo, 2: Esquerda, 3: Direita

    [Header("Texturas dos Naipes")]
    public Texture copasTexture;
    public Texture espadasTexture;
    public Texture pausTexture;
    public Texture ourosTexture;

    [Header("Jogadores")]
    public PlayerController player1;
    public PlayerController player2;

    [Header("Intro Animation")]
    public Animator introBarAnimator;
    public string introAnimationStateName = "IntroBarAnim";

    private int currentRound = 0;
    private const int maxRounds = 10;
    private int correctDirectionIndex = -1;

    private float timer;
    private bool isRoundActive = false;

    private bool p1Answered = false;
    private bool p2Answered = false;
    private int p1Choice = -1;
    private int p2Choice = -1;
    private float p1ResponseTime = 0f;
    private float p2ResponseTime = 0f;

    private int p1Score = 0;
    private int p2Score = 0;

    private void Start()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        StartCoroutine(StartGameSequence());
    }

    private IEnumerator StartGameSequence()
    {
        if (introBarAnimator != null)
        {
            yield return new WaitForEndOfFrame();
            while (introBarAnimator.GetCurrentAnimatorStateInfo(0).IsName(introAnimationStateName) &&
                   introBarAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                yield return null;
            }
        }

        StartNewRound();
    }

    private void StartNewRound()
    {
        currentRound++;

        if (currentRound > maxRounds)
        {
            EndGame();
            return;
        }

        if (roundText != null)
            roundText.text = $"Ronda {currentRound} / {maxRounds}";

        p1Answered = false;
        p2Answered = false;
        p1Choice = -1;
        p2Choice = -1;

        if (player1 != null) player1.ResetRoundState();
        if (player2 != null) player2.ResetRoundState();

        SetupCardLayout();

        timer = roundDuration;
        isRoundActive = true;
    }

    private void Update()
    {
        if (!isRoundActive) return;

        timer -= Time.deltaTime;

        if (timerText != null)
            timerText.text = Mathf.Max(0, timer).ToString("F2");

        // A ronda AGORA só termina quando o temporizador chega a 0
        if (timer <= 0)
        {
            StartCoroutine(EvaluateRound());
        }
    }

    private void SetupCardLayout()
    {
        foreach (var img in directionRawImages)
        {
            if (img != null) img.gameObject.SetActive(false);
        }

        int activeCount = currentRound == 1 ? 1 : (currentRound == 2 ? 2 : (currentRound == 3 ? 3 : 4));
        correctDirectionIndex = Random.Range(0, 4);

        List<Texture> availableTextures = new List<Texture> { espadasTexture, pausTexture, ourosTexture };
        ShuffleList(availableTextures);

        List<int> availableIndices = new List<int> { 0, 1, 2, 3 };

        ConfigureRawImage(correctDirectionIndex, copasTexture, GetRandomColor());
        availableIndices.Remove(correctDirectionIndex);

        int extraSymbols = activeCount - 1;
        for (int i = 0; i < extraSymbols; i++)
        {
            if (availableIndices.Count == 0 || availableTextures.Count == 0) break;

            int randIdx = Random.Range(0, availableIndices.Count);
            int chosenDir = availableIndices[randIdx];
            availableIndices.RemoveAt(randIdx);

            Texture tex = availableTextures[0];
            availableTextures.RemoveAt(0);

            ConfigureRawImage(chosenDir, tex, GetRandomColor());
        }
    }

    public void RegisterPlayerInput(int playerId, int directionIndex)
    {
        if (!isRoundActive) return;

        // Permite alterar a escolha até ao fim do tempo da ronda
        if (playerId == 1)
        {
            p1Answered = true;
            p1Choice = directionIndex;
            p1ResponseTime = roundDuration - timer; // Regista o tempo da escolha mais recente
            if (player1 != null) player1.OnDirectionChosen(directionIndex);
        }
        else if (playerId == 2)
        {
            p2Answered = true;
            p2Choice = directionIndex;
            p2ResponseTime = roundDuration - timer;
            if (player2 != null) player2.OnDirectionChosen(directionIndex);
        }
    }

    private IEnumerator EvaluateRound()
    {
        isRoundActive = false;

        bool p1Correct = (p1Choice == correctDirectionIndex);
        bool p2Correct = (p2Choice == correctDirectionIndex);

        if (player1 != null) player1.TriggerResultAnimation(p1Correct);
        if (player2 != null) player2.TriggerResultAnimation(p2Correct);

        if (p1Correct && p2Correct)
        {
            if (p1ResponseTime < p2ResponseTime)
            {
                p1Score += 100;
                p2Score += 50;
            }
            else
            {
                p1Score += 50;
                p2Score += 100;
            }
        }
        else if (p1Correct)
        {
            p1Score += 100;
        }
        else if (p2Correct)
        {
            p2Score += 100;
        }

        yield return new WaitForSeconds(2.0f);
        yield return new WaitForSeconds(roundInterval);

        StartNewRound();
    }

    private void EndGame()
    {
        isRoundActive = false;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);

        if (p1ScoreText != null) p1ScoreText.text = $"P1 Pontos: {p1Score}";
        if (p2ScoreText != null) p2ScoreText.text = $"P2 Pontos: {p2Score}";

        if (winnerText != null)
        {
            if (p1Score > p2Score) winnerText.text = "Jogador 1 Venceu!";
            else if (p2Score > p1Score) winnerText.text = "Jogador 2 Venceu!";
            else winnerText.text = "Empate!";
        }
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }

    private void ConfigureRawImage(int index, Texture texture, Color color)
    {
        if (directionRawImages[index] != null)
        {
            directionRawImages[index].texture = texture;
            directionRawImages[index].color = color;
            directionRawImages[index].gameObject.SetActive(true);
        }
    }

    private Color GetRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value, 1f);
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int r = Random.Range(i, list.Count);
            list[i] = list[r];
            list[r] = temp;
        }
    }
}