namespace PlayerCore
{
    class Pistol : Weapon
    {
        public Pistol() : base("pistol", 7, 7, 10, 1, 0) { 
        }

        public override bool CheckCooldown()
        {
            return cooldownCounter <= 0 ? true : false;
        }

        public override void DecreaseTime(float deltaTime)
        {
            cooldownCounter -= deltaTime;
        }

        public override void Reload()
        {
            currentBullet = numOfBullet;
        }

        public override void SetCooldown()
        {
            cooldownCounter = cooldownTime;
        }
    }
}
