namespace PlayerCore
{
    class PlayerSetting
    {
        private int hearth;
        private int speed;
        private int jumpForce;
        private float skillCoolDown;

        public PlayerSetting(int hearth, int speed, int jumpForce, int skillCoolDown)
        {
            this.hearth = hearth;
            this.speed = speed;
            this.jumpForce = jumpForce;
            this.skillCoolDown = skillCoolDown;
        }

        public int Hearth { get => hearth; }

        public int Speed { get => speed; }

        public int JumpForce { get => jumpForce; }

        public float SkillCoolDown { get => skillCoolDown; }
    }
}
