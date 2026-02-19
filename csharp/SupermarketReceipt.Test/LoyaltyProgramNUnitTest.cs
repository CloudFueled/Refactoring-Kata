using NUnit.Framework;

namespace SupermarketReceipt.Test
{
    public class LoyaltyProgramNUnitTest
    {
        [TestCase]
        public void BronzeTierBasicPoints()
        {
            // ARRANGE
            SupermarketCatalog catalog = new FakeCatalog();
            var toothbrush = new Product("toothbrush", ProductUnit.Each);
            catalog.AddProduct(toothbrush, 0.99);

            var cart = new ShoppingCart();
            cart.AddItem(toothbrush);

            var teller = new Teller(catalog);
            var receipt = teller.ChecksOutArticlesFrom(cart);

            // ACT
            var points = LoyaltyProgram.CalculatePoints(receipt, LoyaltyTier.Bronze);

            // ASSERT
            Assert.AreEqual(0.99, points);
        }

        [TestCase]
        public void GoldTierProduceDoublePoints()
        {
            // ARRANGE
            SupermarketCatalog catalog = new FakeCatalog();
            var apples = new Product("apples", ProductUnit.Kilo, ProductCategory.Produce);
            catalog.AddProduct(apples, 1.99);

            var cart = new ShoppingCart();
            cart.AddItemQuantity(apples, 10.0); // €19.90

            var teller = new Teller(catalog);
            var receipt = teller.ChecksOutArticlesFrom(cart);

            // ACT
            var points = LoyaltyProgram.CalculatePoints(receipt, LoyaltyTier.Gold);

            // ASSERT
            // €19.90 * 2 = 39.8 points for Gold tier on produce
            Assert.AreEqual(39.8, points, 0.01);
        }

        [TestCase]
        public void SilverTierProduceOneAndHalfPoints()
        {
            // ARRANGE
            SupermarketCatalog catalog = new FakeCatalog();
            var apples = new Product("apples", ProductUnit.Kilo, ProductCategory.Produce);
            catalog.AddProduct(apples, 1.99);

            var cart = new ShoppingCart();
            cart.AddItemQuantity(apples, 10.0); // €19.90

            var teller = new Teller(catalog);
            var receipt = teller.ChecksOutArticlesFrom(cart);

            // ACT
            var points = LoyaltyProgram.CalculatePoints(receipt, LoyaltyTier.Silver);

            // ASSERT
            // €19.90 * 1.5 = 29.85 points for Silver tier on produce
            Assert.AreEqual(29.85, points, 0.01);
        }

        [TestCase]
        public void ExampleFromRequirements_GoldTierMixedCart()
        {
            // ARRANGE - €50 receipt with €20 produce = 70 points for Gold tier
            SupermarketCatalog catalog = new FakeCatalog();
            var apples = new Product("apples", ProductUnit.Kilo, ProductCategory.Produce);
            catalog.AddProduct(apples, 2.0);
            var toothbrush = new Product("toothbrush", ProductUnit.Each);
            catalog.AddProduct(toothbrush, 30.0);

            var cart = new ShoppingCart();
            cart.AddItemQuantity(apples, 10.0); // €20 produce
            cart.AddItem(toothbrush); // €30 non-produce

            var teller = new Teller(catalog);
            var receipt = teller.ChecksOutArticlesFrom(cart);

            // ACT
            var points = LoyaltyProgram.CalculatePoints(receipt, LoyaltyTier.Gold);

            // ASSERT
            // €20 produce * 2 + €30 non-produce * 1 = 40 + 30 = 70 points
            Assert.AreEqual(50.0, receipt.GetTotalPrice()); // Verify total is €50
            Assert.AreEqual(70.0, points, 0.01);
        }

        [TestCase]
        public void GoldTierNonProduceBasePoints()
        {
            // ARRANGE
            SupermarketCatalog catalog = new FakeCatalog();
            var toothbrush = new Product("toothbrush", ProductUnit.Each);
            catalog.AddProduct(toothbrush, 0.99);

            var cart = new ShoppingCart();
            cart.AddItem(toothbrush);

            var teller = new Teller(catalog);
            var receipt = teller.ChecksOutArticlesFrom(cart);

            // ACT
            var points = LoyaltyProgram.CalculatePoints(receipt, LoyaltyTier.Gold);

            // ASSERT
            // Non-produce items get base points even for Gold tier
            Assert.AreEqual(0.99, points);
        }

        [TestCase]
        public void SilverTierNonProduceBasePoints()
        {
            // ARRANGE
            SupermarketCatalog catalog = new FakeCatalog();
            var toothbrush = new Product("toothbrush", ProductUnit.Each);
            catalog.AddProduct(toothbrush, 0.99);

            var cart = new ShoppingCart();
            cart.AddItem(toothbrush);

            var teller = new Teller(catalog);
            var receipt = teller.ChecksOutArticlesFrom(cart);

            // ACT
            var points = LoyaltyProgram.CalculatePoints(receipt, LoyaltyTier.Silver);

            // ASSERT
            // Non-produce items get base points even for Silver tier
            Assert.AreEqual(0.99, points);
        }

        [TestCase]
        public void TierThresholds()
        {
            // ASSERT
            Assert.AreEqual(0, LoyaltyProgram.GetTierThreshold(LoyaltyTier.Bronze));
            Assert.AreEqual(500, LoyaltyProgram.GetTierThreshold(LoyaltyTier.Silver));
            Assert.AreEqual(1000, LoyaltyProgram.GetTierThreshold(LoyaltyTier.Gold));
        }
    }
}
