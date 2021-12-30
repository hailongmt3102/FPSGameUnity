using UnityEngine;

namespace PlayerCore
{
    class CharacterAnimatorController
    {
        private Animator animator;
        private float speed, x_velocity, y_velocity, z_velocity;
        private bool landing, grounding;

        public CharacterAnimatorController(Animator animator,
                float speed = 0f,
                float x_velocity = 0f,
                float y_velocity = 0f,
                float z_velocity = 0f,
                bool landing = false,
                bool grounding = false
        )
        {
            this.animator = animator;
            this.speed = speed;
            this.x_velocity = x_velocity;
            this.y_velocity = y_velocity;
            this.z_velocity = z_velocity;
            this.landing = landing;
            this.grounding = grounding;
        }

        public void move(float speed, float x_velocity, float y_velocity, float z_velocity, bool grounding) {
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
            float center = 0f;
            if (this.x_velocity != x_velocity) {
                center = Mathf.Lerp(this.x_velocity, x_velocity, .2f);
                this.x_velocity = center;
                animator.SetFloat("x_velocity", center);
            }

            if (this.z_velocity != z_velocity)
            {
                center = Mathf.Lerp(this.z_velocity, z_velocity, .2f);
                this.z_velocity = center;
                animator.SetFloat("z_velocity", center);
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

        public void Fire() {
            animator.SetBool("firing", true);
        }

        public void StopFiring() {
            animator.SetBool("firing", false);
        }

        public void Reload()
        {
            animator.SetTrigger("reloading");
        }
    }
}
