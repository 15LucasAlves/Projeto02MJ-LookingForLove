using UnityEngine;

namespace CardGame
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Identificação")]
        public int playerId = 1; // 1 para P1 (WASD), 2 para P2 (JKIL)
        public CardDirectionGame gameManager;

        [Header("Animações")]
        public Animator playerAnimator;

        public string lookDirectionParam = "LookDirection"; // 0: Idle, 1: Cima, 2: Baixo, 3: Esquerda, 4: Direita
        public string correctTrigger = "Win";
        public string wrongTrigger = "Lose";

        private bool hasChosen = false;

        private void Update()
        {
            if (hasChosen) return;

            // Jogador 1: WASD (0: Cima/W, 1: Baixo/S, 2: Esquerda/A, 3: Direita/D)
            if (playerId == 1)
            {
                if (Input.GetKeyDown(KeyCode.W)) SubmitDirection(0);
                else if (Input.GetKeyDown(KeyCode.S)) SubmitDirection(1);
                else if (Input.GetKeyDown(KeyCode.A)) SubmitDirection(2);
                else if (Input.GetKeyDown(KeyCode.D)) SubmitDirection(3);
            }
            // Jogador 2: JKIL (0: Cima/I, 1: Baixo/K, 2: Esquerda/J, 3: Direita/L)
            else if (playerId == 2)
            {
                if (Input.GetKeyDown(KeyCode.I)) SubmitDirection(0);
                else if (Input.GetKeyDown(KeyCode.K)) SubmitDirection(1);
                else if (Input.GetKeyDown(KeyCode.J)) SubmitDirection(2);
                else if (Input.GetKeyDown(KeyCode.L)) SubmitDirection(3);
            }
        }

        private void SubmitDirection(int dirIndex)
        {
            if (hasChosen) return;
            hasChosen = true;

            OnDirectionChosen(dirIndex);

            if (gameManager != null)
            {
                gameManager.RegisterPlayerInput(playerId, dirIndex);
            }
        }

        public void OnDirectionChosen(int dirIndex)
        {
            if (playerAnimator != null)
            {
                playerAnimator.SetInteger(lookDirectionParam, dirIndex + 1);
            }
        }

        public void TriggerResultAnimation(bool isCorrect)
        {
            if (playerAnimator != null)
            {
                if (isCorrect)
                    playerAnimator.SetTrigger(correctTrigger);
                else
                    playerAnimator.SetTrigger(wrongTrigger);
            }
        }

        public void ResetRoundState()
        {
            hasChosen = false;
            if (playerAnimator != null)
            {
                playerAnimator.SetInteger(lookDirectionParam, 0); // Volta ao Idle
            }
        }
    }
}