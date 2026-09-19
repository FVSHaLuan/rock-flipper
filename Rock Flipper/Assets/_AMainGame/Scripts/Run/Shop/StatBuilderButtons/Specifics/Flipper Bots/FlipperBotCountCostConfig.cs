namespace Agame.Run.Shop
{
    public class FlipperBotCountCostConfig : TieredCapScalingCostConfig
    {
        protected override int GetPriceLevelSinceLastCapIncrease(int currentLevel, out int priceSet)
        {
            return BuildStats.GetFlipperBotCountPriceLevelSinceLastCapIncrease(currentLevel, out priceSet);
        }
    }
}
