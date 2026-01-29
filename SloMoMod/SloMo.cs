// Mod
using MelonLoader;
// Unity Engine
using UnityEngine;

// Info for Melon Loader
[assembly: MelonInfo(typeof(SloMoMod.SloMo), "SloMo", "1.0.0", "jcau8")]
[assembly: MelonGame("Megagon Industries", "Lonely Mountains: Downhill")]

namespace SloMoMod
{
    public class SloMo : MelonMod
    {
        // Status for slo-mo mode
        private bool sloMoActive = false;
        // Speed when in slo-mo mode
        private const float sloMoSpeed = 0.3f;
        // Variable to save the original time scale to
        private float baseTimeScale = Time.timeScale;

        // Get the input from controller or keyboard
        public bool SloMoInput()
        {
            // Check whether right bumper or O key are pressed
            if (Input.GetKeyDown(KeyCode.Joystick1Button5) | Input.GetKeyDown(KeyCode.O))
            {
                return true;
            }
            return false;
        }

        // Change the game's time scale
        public void ActivateSloMo(float sloMoSpeed)
        {
            // Set the current time scale to the original time scale
            // This is useful if the slo-mo mode is already active
            Time.timeScale = baseTimeScale;

            // Flip the state of the bool controlling slo-mo mode
            sloMoActive = !sloMoActive;
            if (sloMoActive)
            {
                // Change the game's time scale to the slo-mo speed
                Time.timeScale = sloMoSpeed;
            }
            return;
        }

        public override void OnUpdate()
        {
            if (SloMoInput())
            {
                ActivateSloMo(sloMoSpeed);
            }
        }
    }
}