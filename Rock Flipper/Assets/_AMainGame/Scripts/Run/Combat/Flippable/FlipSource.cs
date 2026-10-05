namespace Agame.Run.Combat
{
    /// <summary>
    /// What started a flip
    /// </summary>
    public enum FlipSource
    {
        Mouse = 0,
        FlipperBot = 1,
        /// <summary>
        /// Flip from a Bouncy rock bouncing
        /// </summary>
        Bounce = 2,
        /// <summary>
        /// Flip from a Restless rock flipping itself
        /// </summary>
        SelfFlip = 3,
        /// <summary>
        /// Flip of a new rock entering the playfield (purchased, spawned or replacing a broken rock)
        /// </summary>
        NewRock = 4,
        /// <summary>
        /// Flip from a Shockwave rock's shockwave
        /// </summary>
        Shockwave = 5,
    }
}
