namespace SupermarketReceipt
{
    public enum LoyaltyTier
    {
        Bronze,
        Silver,
        Gold
    }

    public static class LoyaltyProgram
    {
        public static int GetTierThreshold(LoyaltyTier tier)
        {
            return tier switch
            {
                LoyaltyTier.Bronze => 0,
                LoyaltyTier.Silver => 500,
                LoyaltyTier.Gold => 1000,
                _ => 0
            };
        }

        public static double CalculatePoints(Receipt receipt, LoyaltyTier tier)
        {
            double totalPoints = 0;

            foreach (var item in receipt.GetItems())
            {
                var itemPrice = item.TotalPrice;
                var basePoints = itemPrice; // 1 point per €1

                if (item.Product.Category == ProductCategory.Produce)
                {
                    // Apply tier multipliers for produce
                    if (tier == LoyaltyTier.Gold)
                    {
                        totalPoints += basePoints * 2.0; // 2x points for Gold
                    }
                    else if (tier == LoyaltyTier.Silver)
                    {
                        totalPoints += basePoints * 1.5; // 1.5x points for Silver
                    }
                    else
                    {
                        totalPoints += basePoints; // Bronze gets base points
                    }
                }
                else
                {
                    totalPoints += basePoints; // Non-produce gets base points
                }
            }

            return totalPoints;
        }
    }
}
