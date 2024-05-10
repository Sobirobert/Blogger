using Infrastructure.Identity;

namespace UnitTests.Services
{
    public class UserServiceTest
    {
        [Fact]
        public void IsUserEmailConfirmed_WhenEmailConfirmedIsTrue_ReturnsTrue()
        {
            // Arrange
            ApplicationUser user = new()
            {
                UserName = "maciej",
                EmailConfirmed = true
            };

            UserService service = new();
            // Act

            bool isEmailConfirmed = service.IsUserEmailConfirmed(user);

            // Assert
            Assert.True(isEmailConfirmed);
        }

        [Fact]
        public void IsUserEmailConfirmed_WhenEmailConfirmedIsFalse_ReturnsFalse()
        {
            // Arrange
            ApplicationUser user = new()
            {
                UserName = "maciej",
                EmailConfirmed = false
            };

            UserService service = new();
            // Act

            bool isEmailConfirmed = service.IsUserEmailConfirmed(user);

            // Assert
            Assert.False(isEmailConfirmed);
        }
    }
}
