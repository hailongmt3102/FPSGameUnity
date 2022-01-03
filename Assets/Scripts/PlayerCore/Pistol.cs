namespace PlayerCore
{
    class Pistol : Weapon
    {
        public Pistol() : base("pistol", 7, 7, 10, 100, 0) {
        }

        public override bool Fire()
        {
            if (currentBullet > 0) {
                currentBullet -= 1;
                return true;
            }
            return false;
        }

        public override int getDamage()
        {
            return this.damage;
        }

        public override float getRange()
        {
            return this.range;
        }

        public override void Reload()
        {
            currentBullet = numOfBullet;
        }
    }
}
