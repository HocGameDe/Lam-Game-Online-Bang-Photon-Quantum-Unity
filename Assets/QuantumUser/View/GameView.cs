namespace Quantum
{
    using TMPro;
    using UnityEngine;

    public unsafe class GameView : QuantumEntityViewComponent
    {
        [SerializeField] private TextMeshProUGUI txtWin;
        [SerializeField] private TextMeshProUGUI txtLose;

        private bool showGameWin = false;
        private bool showGameLose = false;

        private void Awake()
        {
            txtWin.gameObject.SetActive(false);
            txtLose.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (VerifiedFrame == null) return;

            if (VerifiedFrame.Global->CurrentGameState == GameState.Win && showGameWin == false)
            {
                txtWin.gameObject.SetActive(true);
            }

            if (VerifiedFrame.Global->CurrentGameState == GameState.Lose && showGameLose == false)
            {
                txtLose.gameObject.SetActive(true);
            }
        }
    }
}
