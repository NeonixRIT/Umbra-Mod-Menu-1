using System;
using System.Collections.Generic;
using UnityEngine;
using RoR2;


namespace UmbraMenu.Cheats.Player
{
    public class Movement
    {
        static Movement() { }
        private Movement() { }
        public static Movement instance = new();
        
        public bool flightToggle, alwaysSprintToggle;
        
        public void AlwaysSprint()
        {
            var isMoving = UmbraMenu.LocalNetworkUser.inputPlayer.GetAxis("MoveVertical") != 0f || UmbraMenu.LocalNetworkUser.inputPlayer.GetAxis("MoveHorizontal") != 0f;
            var localUser = LocalUserManager.GetFirstLocalUser();
            if (localUser == null || localUser.cachedMasterController || localUser.cachedMasterController.master) return;

            var controller = localUser.cachedMasterController;
            var body = controller.master.GetBody();
            if (!body || body.isSprinting || localUser.inputPlayer.GetButton("Sprint")) return;
            if (isMoving)
            {
                body.isSprinting = true;
            }
        }

        public void Flight()
        {
            try
            {
                if (Utility.GetCurrentCharacter().ToString() != "Loader")
                {
                    UmbraMenu.LocalPlayerBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
                }

                var inputBankTest = UmbraMenu.LocalPlayerBody.GetComponent<InputBankTest>();
                var forwardDirection = inputBankTest.moveVector.normalized;
                var aimDirection = inputBankTest.aimDirection.normalized;
                var upDirection = inputBankTest.moveVector.y + 1;
                var downDirection = inputBankTest.moveVector.y - 1;
                var isForward = Vector3.Dot(forwardDirection, aimDirection) > 0f;

                var isSprinting = alwaysSprintToggle ? UmbraMenu.LocalPlayerBody.isSprinting : UmbraMenu.LocalNetworkUser.inputPlayer.GetButton("Sprint");
                var isJumping = UmbraMenu.LocalNetworkUser.inputPlayer.GetButton("Jump");
                var isGoingDown = Input.GetKey(KeyCode.X);
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                var isStrafing = UmbraMenu.LocalNetworkUser.inputPlayer.GetAxis("MoveVertical") != 0f;

                if (isSprinting)
                {
                    if (!alwaysSprintToggle && !UmbraMenu.LocalNetworkUser.inputPlayer.GetButton("Sprint"))
                    {
                        UmbraMenu.LocalPlayerBody.isSprinting = false;
                    }

                    UmbraMenu.LocalPlayerBody.characterMotor.velocity = forwardDirection * 100f;
                    UmbraMenu.LocalPlayerBody.characterMotor.velocity.y = upDirection * 0.510005f;
                    if (isStrafing)
                    {
                        if (isForward)
                        {
                            UmbraMenu.LocalPlayerBody.characterMotor.velocity.y = aimDirection.y * 100f;
                        }
                        else
                        {
                            UmbraMenu.LocalPlayerBody.characterMotor.velocity.y = aimDirection.y * -100f;
                        }
                    }
                }
                else
                {
                    UmbraMenu.LocalPlayerBody.characterMotor.velocity = forwardDirection * 50;
                    UmbraMenu.LocalPlayerBody.characterMotor.velocity.y = upDirection * 0.510005f;
                    if (isStrafing)
                    {
                        if (isForward)
                        {
                            UmbraMenu.LocalPlayerBody.characterMotor.velocity.y = aimDirection.y * 50;
                        }
                        else
                        {
                            UmbraMenu.LocalPlayerBody.characterMotor.velocity.y = aimDirection.y * -50;
                        }
                    }
                }
                if (isJumping)
                {
                    UmbraMenu.LocalPlayerBody.characterMotor.velocity.y = upDirection * 100;
                }
                if (isGoingDown)
                {
                    UmbraMenu.LocalPlayerBody.characterMotor.velocity.y = downDirection * 100;
                }
            }
            catch (NullReferenceException)
            {
                //TODO Log error
            }
        }
    }
}
