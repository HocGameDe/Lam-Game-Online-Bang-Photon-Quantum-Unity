namespace Quantum
{
    using Photon.Deterministic;
    using Spine.Unity;
    using TMPro;
    using UnityEngine;

    public enum AnimationName
    {
        Idle, Walk, Attack, Dead
    }

    public unsafe class PlayerView : QuantumEntityViewComponent
    {
        public const string ANIM_IDLE = "Idle";
        public const string ANIM_WALK = "Walk";
        public const string ANIM_ATTACK = "Attack";
        public const string ANIM_DEAD = "Dead";

        public Quaternion rotationLeft;
        public Quaternion rotationRight;

        private Animator animator;
        private PhysicsBody2D body;
        private AnimationName currentAnim;
        private PlayerInfo playerInfo;

        public bool spawnBullet = false;
        private HealthBar healthBar;
        private TextMeshPro txtName;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            healthBar = GetComponentInChildren<HealthBar>();
            txtName = GetComponentInChildren<TextMeshPro>();
            txtName.text = "";

            QuantumCallback.Subscribe(this, (CallbackPollInput callback) => PollInput(callback));
        }

        private void Update()
        {
            currentAnim = GetCurrentAnimation();
            body = VerifiedFrame.Get<PhysicsBody2D>(_entityView.EntityRef);
            if (body.Velocity != FPVector2.Zero && currentAnim != AnimationName.Attack && currentAnim != AnimationName.Dead)
            {
                animator.Play(ANIM_WALK);
            }

            playerInfo = VerifiedFrame.Get<PlayerInfo>(_entityView.EntityRef);
            
            if (txtName.text == "")
            {
                var playerData = VerifiedFrame.GetPlayerData(playerInfo.PlayerRef);
                txtName.text = playerData.PlayerNickname;
            }

            if (playerInfo.CurrentHealth <= 0)
            {
                animator.Play(ANIM_DEAD);
                healthBar.gameObject.SetActive(false);
                return;
            }

            var input = VerifiedFrame.GetPlayerInput(playerInfo.PlayerRef);
            if (input->Attack.WasPressed)
            {
                animator.Play(ANIM_ATTACK);
            }

            if (body.Velocity.X > 0)
            {
                animator.transform.rotation = rotationRight;
            } else if (body.Velocity.X < 0)
            {
                animator.transform.rotation = rotationLeft;
            }

            if (playerInfo.Health == 0)
            {
                healthBar.SetValue(0);
            }
            else
            {
                healthBar.SetValue((playerInfo.CurrentHealth / playerInfo.Health).AsFloat);
            }

            if (QuantumRunner.DefaultGame.PlayerIsLocal(playerInfo.PlayerRef))
            {
                Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
            }
        }

        private AnimationName GetCurrentAnimation()
        {
            var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName(ANIM_ATTACK)) return AnimationName.Attack;
            else if (stateInfo.IsName(ANIM_WALK)) return AnimationName.Walk;
            else if (stateInfo.IsName(ANIM_DEAD)) return AnimationName.Dead;
            return AnimationName.Idle;
        }

        public void PollInput(CallbackPollInput callback)
        {
            if (QuantumRunner.DefaultGame.PlayerIsLocal(playerInfo.PlayerRef) == false)
            {
                return;
            }
            Quantum.Input input = new Quantum.Input();
            FP x, y;
            GetPlayerInputDirection(out x, out y);
            input.Direction = new FPVector2(x, y);
            input.Attack = UnityEngine.Input.GetKey(KeyCode.Space);
            input.SpawnBullet = spawnBullet;
            if (spawnBullet == true) spawnBullet = false;
            callback.SetInput(input, DeterministicInputFlags.Repeatable);
        }

        private void GetPlayerInputDirection(out FP x, out FP y)
        {
            x = 0;
            y = 0;
            //nếu đang tấn công thì đứng yên
            if (currentAnim == AnimationName.Attack) return;
            if (currentAnim == AnimationName.Dead) return;
            if (UnityEngine.Input.GetKey(KeyCode.RightArrow))
            {
                x = 1;
            }
            else if (UnityEngine.Input.GetKey(KeyCode.LeftArrow))
            {
                x = -1;
            }
            if (UnityEngine.Input.GetKey(KeyCode.UpArrow))
            {
                y = 1;
            }
            else if (UnityEngine.Input.GetKey(KeyCode.DownArrow))
            {
                y = -1;
            }
        }
    }
}