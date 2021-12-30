namespace PlayerCore
{
    abstract class Weapon
    {
        public int currentBullet;
        public string name;
        public bool firing;

        protected float cooldownTime;
        protected int numOfBullet;
        protected int damage;
        protected float range;

        public Weapon(string name, int numOfBullet, int currentBullet, int damage, float range, float cooldownTime, bool firing = false) {
            this.name = name;
            this.numOfBullet = numOfBullet;
            this.currentBullet = currentBullet;
            this.damage = damage;
            this.range = range;
            this.cooldownTime = cooldownTime;
            this.firing = firing;
        }
        abstract public void Reload();
        abstract public bool Fire();

        abstract public int getDamage();

        abstract public float getRange();
    }
}
