using UnityEngine;

namespace Agame.Run.Combat.Tutorials
{
    /// <summary>
    /// Points a tapping hand at a grounded rock until the player flips a rock with the mouse, once per save
    /// (<see cref="RunData.FinishedFirstFlipTutorial"/>). The hand hides while no rock is on the ground.
    /// Goes on its own object in the Run scene; it moves its own transform onto the rock and animates <see cref="hand"/>.
    /// </summary>
    public class FirstFlipTutorial : ExtendedMonoBehaviourRun
    {
        [SerializeField, Tooltip("Shown and hidden, and tapped toward the rock. Put the fingertip at its origin.")]
        private Transform hand;
        [SerializeField, Tooltip("Where the hand rests, from the rock's position.")]
        private Vector2 handOffset = new Vector2(0.05f, -0.15f);

        [Header("Tap animation")]
        [SerializeField, Min(0.01f), Tooltip("Seconds per tap.")]
        private float tapDuration = 1f;
        [SerializeField, Tooltip("How far the hand moves toward the rock at the deepest point of a tap.")]
        private float tapDistance = 0.08f;
        [SerializeField, Tooltip("The hand's scale at the deepest point of a tap, relative to its normal scale.")]
        private float tapScale = 0.85f;
        [SerializeField, Tooltip("How deep the tap is over one tap (0 to 1): 0 at rest, 1 at the deepest point.")]
        private AnimationCurve tapCurve = new AnimationCurve(
            new Keyframe(0f, 0f),
            new Keyframe(0.15f, 1f),
            new Keyframe(0.35f, 0f),
            new Keyframe(1f, 0f));

        private Rock targetRock;
        private Vector3 normalHandScale;
        private float tapTime;

        protected void Start()
        {
            ///
            if (RunData.FinishedFirstFlipTutorial)
            {
                gameObject.SetActive(false);
                return;
            }

            ///
            normalHandScale = hand.localScale;
            hand.gameObject.SetActive(false);
        }

        protected void Update()
        {
            ///
            if (HasFlippedAnyRockWithMouse())
            {
                Finish();
                return;
            }

            ///
            if (targetRock == null || !IsOnGround(targetRock))
            {
                targetRock = FindRockOnGround();
            }

            ///
            var showing = targetRock != null;
            if (hand.gameObject.activeSelf != showing)
            {
                hand.gameObject.SetActive(showing);
                tapTime = 0f;
            }
            if (!showing)
            {
                return;
            }

            ///
            var th = transformHandle;
            th.position = (Vector2)targetRock.transform.position;
            UpdateTapping();
        }

        private void UpdateTapping()
        {
            ///
            tapTime = (tapTime + Time.deltaTime) % tapDuration;
            var depth = tapCurve.Evaluate(tapTime / tapDuration);

            ///
            hand.localPosition = Vector2.MoveTowards(handOffset, Vector2.zero, tapDistance * depth);
            hand.localScale = normalHandScale * Mathf.Lerp(1f, tapScale, depth);
        }

        private bool HasFlippedAnyRockWithMouse()
        {
            foreach (var rock in RunEntry.rockInstanceManager.ActiveRocks)
            {
                if (rock.Flippable.IsFlipping && rock.Flippable.LastFlipSource == FlipSource.Mouse)
                {
                    return true;
                }
            }
            return false;
        }

        private Rock FindRockOnGround()
        {
            foreach (var rock in RunEntry.rockInstanceManager.ActiveRocks)
            {
                if (IsOnGround(rock))
                {
                    return rock;
                }
            }
            return null;
        }

        private static bool IsOnGround(Rock rock)
        {
            return rock.isActiveAndEnabled && !rock.Flippable.IsFlipping;
        }

        private void Finish()
        {
            ///
            RunData.FinishFirstFlipTutorial();
            entry.playerDataSaver.SetSaveThisFrame();

            ///
            gameObject.SetActive(false);
        }
    }

}
