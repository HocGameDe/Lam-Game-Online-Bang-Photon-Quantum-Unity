namespace Quantum
{
    using UnityEngine;

    public class BulletView : QuantumEntityViewComponent
    {
        public Quaternion rotationLeft;
        public Quaternion rotationRight;
        public Transform visual;

        private BulletInfo bulletInfo;

        private void Update()
        {
            if (VerifiedFrame.TryGet<BulletInfo>(_entityView.EntityRef, out BulletInfo bulletInfo))
            {
                if (bulletInfo.Facing == PlayerFacing.Left)
                {
                    visual.rotation = rotationLeft;
                }
                else
                {
                    visual.rotation = rotationRight;
                }
            }
        }
    }
}