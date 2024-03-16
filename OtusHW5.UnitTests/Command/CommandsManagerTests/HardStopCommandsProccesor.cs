using FluentAssertions;
using Moq;
using OtusHW5.CommandsManager;

namespace OtusHW5.UnitTests.Command.CommandsManagerTests
{
    public class HardStopCommandsProccesor
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

            commandsManager.HardStopCommandsProccesor();

            await Task.Run(() => commandsManager.StartCommandsProccesor());

            // Assert

            commandsManager.CommandsCount.Should().Be(CommandsCount);
        }
    }
}
