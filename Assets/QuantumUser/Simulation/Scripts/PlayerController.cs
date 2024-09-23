namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class PlayerController : SystemMainThreadFilter<PlayerController.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public PhysicsBody2D* Body;
            public Transform2D* Transform;
            public PlayerInfo* PlayerInfo;
        }

        public override void Update(Frame frame, ref Filter filter)
        {
            var input = frame.GetPlayerInput(filter.PlayerInfo->PlayerRef);
            filter.Body->Velocity = input->Direction * filter.PlayerInfo->Speed;

            if (input->Direction.X > 0)
            {
                filter.PlayerInfo->Facing = PlayerFacing.Right;
            } else if (input->Direction.X < 0)
            {
                filter.PlayerInfo->Facing = PlayerFacing.Left;
            }

            if (input->SpawnBullet)
            {
                var spawnedBullet = frame.Create(filter.PlayerInfo->Bullet);
                var transform = frame.Get<Transform2D>(spawnedBullet);
                transform.Position = filter.Transform->Position;
                var bulletInfo = frame.Get<BulletInfo>(spawnedBullet);
                bulletInfo.Owner = filter.Entity;
                bulletInfo.Facing = filter.PlayerInfo->Facing;
                frame.Set(spawnedBullet, transform);
                frame.Set(spawnedBullet, bulletInfo);
            }
        }
    }
}