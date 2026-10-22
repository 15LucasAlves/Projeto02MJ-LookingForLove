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
        public string idleStateName = "Idle"; // Nome do estado Idle no Animator

        private bool isLocked = false; // Bloqueia o input apenas durante o resultado da ronda

        private void Update()
        {
            if (isLocked) return;

            if (playerId == 1)
            {
                if (Input.GetKeyDown(KeyCode.W)) SubmitDirection(0);
                else if (Input.GetKeyDown(KeyCode.S)) SubmitDirection(1);
                else if (Input.GetKeyDown(KeyCode.A)) SubmitDirection(2);
                else if (Input.GetKeyDown(KeyCode.D)) SubmitDirection(3);
            }
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
            if (isLocked) return;

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
            isLocked = true; // Bloqueia novos inputs enquanto mostra a animação de resultado

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
            isLocked = false;

            if (playerAnimator != null)
            {
                // Limpa as Triggers de vitória/derrota
                playerAnimator.ResetTrigger(correctTrigger);
                playerAnimator.ResetTrigger(wrongTrigger);

                // Força o parâmetro para 0 (Idle)
                playerAnimator.SetInteger(lookDirectionParam, 0);

                // Força a transição direta para o estado Idle no Animator
                playerAnimator.Play(idleStateName, 0, 0f);
            }
        }
    }
}