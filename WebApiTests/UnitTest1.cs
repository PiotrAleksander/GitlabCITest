using WebApiLincora.Helpers;
using Xunit;

namespace WebApiTests
{
    public class UnitTest1
    {
        public static TopicsMatchingHelper helper = new TopicsMatchingHelper();

        [Fact]
        public void SameTopicsMatchingTest()
        {
            Assert.True(helper.TopicMatch("dev/+/+/events/+",
                "dev/+/+/events/+"));
        }

        [Fact]
        public void DifferentTopicsMatchingTest()
        {
            Assert.False(helper.TopicMatch("dev/+/+/commands/+",
                "dev/+/+/events/+"));
        }

        [Theory]
        [InlineData("dev/{variable}/replies/{variable}/{variable}")]
        [InlineData("dev/{variable}/events/{variable}")]
        [InlineData("drv/{variable}/{variable}/events/{variable}")]
        [InlineData("drv/{variable}/{variable}/commands/{variable}")]
        [InlineData("dev/{variable}/commands/{variable}")]
        [InlineData("sys/{variable}/replies/{variable}/{variable}")]
        [InlineData("sys/{variable}/events/{variable}")]
        [InlineData("sys/{variable}/commands/{variable}")]
        
        public void StandardTopicsAllowanceTest(string value)
        {
            var tokens = value.Split('/');
            Assert.True(helper.IsTopicAllowed(value, 2));
        }

        [Theory]
        [InlineData("dev/{variable}/commands/+")]
        [InlineData("dev/+/events/+")]
        [InlineData("sys/+/commands/+")]
        [InlineData("dev/+/replies/+/+")]
        [InlineData("drv/{model]/+/replies/+/+")]
        [InlineData("drv/{variable}/+/events/+")]
        [InlineData("drv/{variable}/+/replies/+/+")]
        [InlineData("sys/+/events/+")]
        [InlineData("sys/+commands/+")]
        [InlineData("sys/+/replies/+/+")]
        public void StandardSubscribeTopicsAllowanceTest(string value)
        {
            var tokens = value.Split('/');
            Assert.True(helper.IsTopicAllowed(value, 1));
        }

        [Theory]
        [InlineData("dev/{variable}/{variable}/commands/+")]
        [InlineData("error/{variable}/{variable}/commands/+")]
        [InlineData("error/+/+/events/+")]
        [InlineData("error/+/+/replies/+")]
        [InlineData("error/+/commands/+")]
        [InlineData("error/{variable}/+/events/+")]
        [InlineData("error/{variable}/+/commands/+")]
        [InlineData("error/{variable}/+/replies/+/+")]
        [InlineData("error/+/replies/+/+")]
        [InlineData("error/+/events/+")]
        [InlineData("dev/#/events/+")]
        public void InvalidSubscribeTopicsAllowanceTest(string value)
        {
            var tokens = value.Split('/');
            Assert.False(helper.IsTopicAllowed(value, 1));
        }

        [Theory]
        [InlineData("error/{variable}/{variable}/events/{variable}")]
        [InlineData("error/{variable}/{variable}/replies/{variable}/{variable}")]
        [InlineData("error/{variable}/{variable}/events/{variable}")]
        [InlineData("error/{variable}/{variable}/commands/{variable}")]
        [InlineData("error/{variable}/{variable}/replies/{variable}/{variable}")]
        [InlineData("error/{variable}/events/{variable}")]
        [InlineData("error/{variable}/replies/{variable}/{variable}")]
        [InlineData("error/{variable}/{variable}/commands/{variable}")]
        [InlineData("error/{variable}/commands/{variable}")]
        public void InvalidPublishTopicsAllowanceTest(string value)
        {
            var tokens = value.Split('/');
            Assert.False(helper.IsTopicAllowed(value, 2));
        }

        [Theory]
        [InlineData("dev/ac_emu:542982025331250/events/telemetry")]
        public void ValidNonstandardPublishTopicAllowanceTest(string value)
        {
            var tokens = value.Split('/');
            Assert.True(helper.IsTopicAllowed(value, 2));
        }

        [Theory]
        [InlineData("dev/+/replies/#")]
        [InlineData("sys/+/commands/#")]
        [InlineData("dev/+/events/+")]
        [InlineData("dev/ac_emu:542982025331250/commands/#")]
        public void ValidNonstandardSubscribeTopicAllowanceTest(string value)
        {
            var tokens = value.Split('/');
            Assert.True(helper.IsTopicAllowed(value, 1));
        }
    }
}
