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

        private static double GetProduceMultiplier(LoyaltyTier tier)
        {
            return tier switch
            {
                LoyaltyTier.Gold => 2.0,
                LoyaltyTier.Silver => 1.5,
                _ => 1.0
            };
        }

        public static double CalculatePoints(Receipt receipt, LoyaltyTier tier)
        {
            double totalPoints = 0;
            double produceMultiplier = GetProduceMultiplier(tier);

            foreach (var item in receipt.GetItems())
            {
                var basePoints = item.TotalPrice; // 1 point per €1

                if (item.Product.Category == ProductCategory.Produce)
                {
                    totalPoints += basePoints * produceMultiplier;
                }
                else
                {
                    totalPoints += basePoints;
                }
            }

            return totalPoints;
        }
    }
}