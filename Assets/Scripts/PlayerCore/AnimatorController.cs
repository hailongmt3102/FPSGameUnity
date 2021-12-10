using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace PlayerCore
{
    class AnimatorController
    {
        private Animator animator;
        private bool jumping;
        private bool walking;
        private RigLayer fireHandle;

        private bool firing = false;

        public AnimatorController(Animator animator, bool jumping, bool walking, RigLayer fireHandle) {
            this.animator = animator;
            this.jumping = jumping;
            this.walking = walking;
            this.fireHandle = fireHandle;
            fireHandle.active = false;
        }

        public void Jump()
        {
            if (jumping) return;
            jumping = true;
            animator.SetBool("jumping", true);
            animator.SetBool("walking", false);
        }

        public void Walk() {
            if (walking) return;

            // if jumping, change to false
            if (jumping) {
                jumping = false;
                animator.SetBool("jumping", false);
            }
            // set wakling status
            walking = true;
            animator.SetBool("walking", true);
        }

        public void Idle() {
            if (walking) {
                walking = false;
                animator.SetBool("walking", false);
            }
            if (jumping) {
                jumping = false;
                animator.SetBool("jumping", false);
            }
        }

        public void EnableFire() {
            if (firing) return;
            firing = true;
            fireHandle.active = true;
        }

        public void DisableFire() {
            if (!firing) return;
            firing = false;
            fireHandle.active = false;
        }
    }
}
