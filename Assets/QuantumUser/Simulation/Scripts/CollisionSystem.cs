namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class CollisionSystem : SystemSignalsOnly, ISignalOnCollisionEnter2D
    {
        public void OnCollisionEnter2D(Frame frame, CollisionInfo2D info)
        {
            if (frame.TryGet<BulletInfo>(info.Entity, out var bulletInfo))
            {
                info.IgnoreCollision = true;

                if (frame.TryGet<BossInfo>(info.Other, out var bossInfo)) 
                {
                    bossInfo.CurrentHealth -= 1;
                    frame.Set(info.Other, bossInfo);
                    frame.Destroy(info.Entity);

                    if (bossInfo.CurrentHealth <= 0)
                    {
                        frame.Signals.OnBossDead();
                        frame.Destroy(info.Other);
                    }
                }
            }

            if (frame.TryGet<BossBulletInfo>(info.Entity, out var bossBullet))
            {
                info.IgnoreCollision = true;

                if (frame.TryGet<PlayerInfo>(info.Other, out var playerInfo))
                {
                    playerInfo.CurrentHealth -= 1;
                    frame.Set(info.Other, playerInfo);
                    frame.Destroy(info.Entity);

                    if (playerInfo.CurrentHealth <= 0)
                    {
                        frame.Signals.OnPlayerDead();
                    }
                }
            }
        }
    }
}