using FluentAssertions;
using Moq;
using OtusHW5.CommandsManager;

namespace OtusHW5.UnitTests.Command.CommandsManagerTests
{
    public class SoftStopCommandsProccesor
    {
        private const int CommandsCount = 10;

        [Fact]
        public async Task ShouldNotFinishAllTasks_WhenExecuted()
        {
            // Arrange

            var commandMock = new Mock<ICommand>();

            var commandsManager = new CommandsManager.CommandsManager();

            for (var i = 0; i < CommandsCount; i++)
            {

                commandsManager.AddCommandToQueue(commandMock.Object);
            }

            // Act

            commandsManager.SoftStopCommandsProccesor();

            await Task.Run(() => commandsManager.StartCommandsProccesor());

            // Assert

            commandsManager.CommandsCount.Should().Be(0);
        }
    }
}
