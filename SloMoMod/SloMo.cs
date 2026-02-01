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
        // Variable to track time elapsed during the transtition
        private float elapsedTime = 0f;
        // Length of the transition
        private float easeDuration = 1.3f;
        // Bool for knowing when action key is pressed
        private bool inputRegistered = false;

        public float EaseGradient(float time)
        {
            // Normalising time
            float x = Mathf.Clamp01(time);
            // Returning the gradient for the transition
            return Mathf.Max(0f, Mathf.Pow(Mathf.Min(Mathf.Cos(Mathf.PI * x / 2f), 1 - Mathf.Abs(x)), 0.5f));
        }

        public float EaseInOut(float baseTimeScale, float sloMoSpeed, float elapsedTime, bool reverse)
        {
            // Creating a normalised transition timer
            float time = Mathf.Clamp01(elapsedTime / easeDuration);

            // Mapping the gradient and accentuating it
            float gradient = 1f - Mathf.Pow((1f - EaseGradient(time)), 2f);

            if (reverse)
            {
                // Exit transition instead of entry transition
                return Mathf.Lerp(sloMoSpeed, baseTimeScale, gradient);
            }
            // Entry transition
            return Mathf.Lerp(baseTimeScale, sloMoSpeed, gradient);

        }

        // Get the input from controller or keyboard
        public bool SloMoInput()
        {
            // Check whether right bumper or O key are pressed
            if (Input.GetKeyDown(KeyCode.Joystick1Button5) || Input.GetKeyDown(KeyCode.Joystick2Button5) || Input.GetKeyDown(KeyCode.Joystick3Button5) || Input.GetKeyDown(KeyCode.Joystick4Button5) || Input.GetKeyDown(KeyCode.O))
            {
                return true;
            }
            return false;
        }

        // Main mod function
        public void main()
        {
            // Check if the RB or O inputs are pressed
            if (SloMoInput())
            {
                // Switch the state of slo-mo mode
                sloMoActive = !sloMoActive;
                // Reset elapsed time
                elapsedTime = 0f;
                inputRegistered = true;
            }

            // Activate Slo-mo mode
            if (sloMoActive)
            {
                // Update elapsed time by real time
                elapsedTime += Time.unscaledDeltaTime;
                // Transition the game's time scale to the slo-mo speed
                Time.timeScale = EaseInOut(baseTimeScale, sloMoSpeed, elapsedTime, false);

                // The transition has finished, keep constant speed
                if (elapsedTime >= easeDuration)
                {
                    Time.timeScale = sloMoSpeed;
                }
            }
            else if (!sloMoActive && inputRegistered)
            {
                // The slo-mo mode is already active, use the transition and turn it off
                // Update elapsed time by real time
                elapsedTime += Time.unscaledDeltaTime;
                // Transition the slo-mo time scale back to the game's original time scale
                Time.timeScale = EaseInOut(baseTimeScale, sloMoSpeed, elapsedTime, true);

                // The transition has finished, keep constant speed
                if (elapsedTime >= easeDuration)
                {
                    Time.timeScale = baseTimeScale;
                }
            }
        }

        public override void OnUpdate()
        {
            // Run the main function
            main();
        }
    }
}