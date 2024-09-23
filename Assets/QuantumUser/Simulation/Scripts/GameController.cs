namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class GameController : SystemSignalsOnly, ISignalOnPlayerDead, ISignalOnBossDead
    {
        public void OnBossDead(Frame frame)
        {
            if (frame.Global->CurrentGameState == GameState.Lose || frame.Global->CurrentGameState == GameState.Win) return;

            frame.Global->CurrentGameState = GameState.Win;
        }

        public void OnPlayerDead(Frame frame)
        {
            if (frame.Global->CurrentGameState == GameState.Lose || frame.Global->CurrentGameState == GameState.Win) return;

            bool allPlayerDead = true;
            var players = frame.GetComponentIterator<PlayerInfo>();

            foreach(var player in players)
            {
                if (player.Component.CurrentHealth > 0)
                {
                    allPlayerDead = false;
                    break;
                }
            }

            if (allPlayerDead)
            {
                frame.Global->CurrentGameState = GameState.Lose;
            }
        }
    }
}