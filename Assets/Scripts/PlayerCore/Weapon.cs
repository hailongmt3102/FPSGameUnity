namespace PlayerCore
{
    abstract class Weapon
    {
        public float cooldownCounter;
        public float currentBullet;
        public string name;

        protected float cooldownTime;
        protected int numOfBullet;
        protected float damage;

        public Weapon(string name, int numOfBullet, int currentBullet, float damage, float cooldownCounter, float cooldownTime) {
            this.numOfBullet = numOfBullet;
            this.currentBullet = currentBullet;
            this.damage = damage;
            this.cooldownCounter = cooldownCounter;
            this.cooldownCounter = cooldownTime;
        }
        abstract public void SetCooldown();
        abstract public bool CheckCooldown();
        abstract public void DecreaseTime(float deltaTime);
        abstract public void Reload();
    }
}
