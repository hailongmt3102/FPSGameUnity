using UnityEngine;

namespace PlayerCore
{
    class CharacterAnimatorController
    {
        private Animator animator;
        private float speed, x_velocity, y_velocity, z_velocity;
        private bool landing, grounding;
        private int gun_type, gun_state;

        public CharacterAnimatorController(Animator animator,
                float speed = 0f,
                float x_velocity = 0f,
                float y_velocity = 0f,
                float z_velocity = 0f,
                bool landing = false,
                bool grounding = false,
                int gun_state = 0,
                int gun_type = 0
        )
        {
            this.animator = animator;
            this.speed = speed;
            this.x_velocity = x_velocity;
            this.y_velocity = y_velocity;
            this.z_velocity = z_velocity;
            this.landing = landing;
            this.grounding = grounding;
            this.gun_state = gun_state;
            this.gun_type = gun_type;
        }

        public void movement(float speed, float x_velocity, float y_velocity, float z_velocity, bool grounding) {
            if (this.speed != speed)
            {
                this.speed = speed;
                animator.SetFloat("speed", speed);
            }

            // calculate some input
            if (grounding)
            {
                // set grounding variable
                if (!this.grounding) {
                    this.grounding = true;
                    animator.SetBool("grounding", true);
                }
                // on the ground
                if (x_velocity == 0 && z_velocity == 0)
                {
                    // idle state
                    Idle();
                }
                else {
                    // walk state
                    Walk(x_velocity, z_velocity);
                }
            }
            else {
                // falling or jumping
                Jump(y_velocity);
            }
        }

        public void Jump(float y_velocity)
        {
            if (grounding)
            {
                grounding = false;
                animator.SetBool("grounding", false);
            }
            if (this.y_velocity != y_velocity)
            {
                // set y velocity
                this.y_velocity = y_velocity;
                animator.SetFloat("y_velocity", y_velocity);
            }
        }

        public void Walk(float x_velocity, float z_velocity) {
            if (this.x_velocity != x_velocity) {
                this.x_velocity = x_velocity;
                animator.SetFloat("x_velocity", x_velocity);
            }

            if (this.z_velocity != z_velocity)
            {
                this.z_velocity = z_velocity;
                animator.SetFloat("z_velocity", z_velocity);
            }
        }
        
        public void Landing()
        {
            if (!grounding) {
                grounding = true;
                animator.SetBool("grounding", true);
            }
            if (!landing) {
                landing = true;
                animator.SetBool("landing", true);
            }
        }

        public void DisableLanding() {
            if (landing)
            {
                landing = false;
                animator.SetBool("landing", false);
            }
        }

        public void Idle() {
            // check and set idle state
            if (x_velocity != 0) {
                x_velocity = 0;
                animator.SetFloat("x_velocity", 0f);
            }
            if (z_velocity != 0)
            {
                z_velocity = 0;
                animator.SetFloat("x_velocity", 0f);
            }
        }

        public void fireEvent(int guntype, int gunstate) {
            // pistol gun
            if (gun_type != guntype) {
                gun_type = guntype;
                animator.SetInteger("guntype", guntype);
            }
            if (gun_state != gunstate) {
                gun_state = gunstate;
                animator.SetInteger("gunstate", gunstate);
            }
        }

        public void EnableHandLayer() {
            if (animator.layerCount > 1)
                animator.SetLayerWeight(1, 1);
        }

        public void DisableHandLayer() {
            if (animator.layerCount > 1) 
                animator.SetLayerWeight(1, 0);
        }

        public void EnableFire() {
            
        }

        public void DisableFire() {
           
        }
    }
}
