namespace Quantum
{
    using UnityEngine;

    public class BossView : QuantumEntityViewComponent
    {
        public Quaternion rotationLeft;
        public Quaternion rotationRight;
        public Transform visual;
        private PhysicsBody2D body;
        private BossInfo bossInfo;
        private HealthBar healthBar;

        private void Awake()
        {
            healthBar = GetComponentInChildren<HealthBar>();
        }

        private void Update()
        {
            if (VerifiedFrame == null) return;
            if (VerifiedFrame.TryGet<PhysicsBody2D>(_entityView.EntityRef, out body))
            {
                if (body.Velocity.X < 0)
                {
                    visual.rotation = rotationLeft;
                } else if (body.Velocity.X > 0)
                {
                    visual.rotation = rotationRight;
                }
            }

            if (VerifiedFrame.TryGet<BossInfo>(_entityView.EntityRef, out bossInfo))
            {
                if (bossInfo.Health == 0)
                {
                    healthBar.SetValue(0);
                }
                else
                {
                    healthBar.SetValue((bossInfo.CurrentHealth / bossInfo.Health).AsFloat);
                }
            }
        }
    }
}