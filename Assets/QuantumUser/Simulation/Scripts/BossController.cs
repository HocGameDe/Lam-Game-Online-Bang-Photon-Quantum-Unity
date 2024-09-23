namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class BossController : SystemMainThreadFilter<BossController.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public PhysicsBody2D* Body;
            public BossInfo* BossInfo;
            public Transform2D* Transform;
        }

        public override void Update(Frame frame, ref Filter filter)
        {
            filter.BossInfo->Time += frame.DeltaTime;
            if (filter.BossInfo->Time > filter.BossInfo->ChangeDirectionTime)
            {
                filter.BossInfo->ChangeDirectionTime = filter.BossInfo->Time + frame.Global->RngSession.Next(FP._2, FP._3);
                filter.BossInfo->Direction = new FPVector2(frame.Global->RngSession.Next(-FP._1, FP._1), frame.Global->RngSession.Next(-FP._1, FP._1)).Normalized;
            }

            filter.Body->Velocity = filter.BossInfo->Direction;

            if (filter.BossInfo->Time > filter.BossInfo->UseSkillTime)
            {
                filter.BossInfo->UseSkillTime = filter.BossInfo->Time + frame.Global->RngSession.Next(FP._4, FP._6);

                int BULLET_AMOUNT = 12;

                for (int i = 0; i < BULLET_AMOUNT; i++)
                {
                    var angle = FP.FromFloat_UNSAFE(360) / BULLET_AMOUNT * i;
                    var radiantAngle = angle * FP.Deg2Rad;

                    var spawnedBullet = frame.Create(filter.BossInfo->Bullet);
                    var bulletTransform = frame.Get<Transform2D>(spawnedBullet);
                    bulletTransform.Position = filter.Transform->Position;
                    var bulletInfo = frame.Get<BossBulletInfo>(spawnedBullet);
                    bulletInfo.Direction = new FPVector2(FPMath.Cos(radiantAngle), FPMath.Sin(radiantAngle));

                    frame.Set(spawnedBullet, bulletInfo);
                    frame.Set(spawnedBullet, bulletTransform);
                }
            }
        }
    }
}