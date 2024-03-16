using Moq;
using OtusHW5.CommandsManager;

namespace OtusHW5.UnitTests.Command.CommandsManagerTests
{
    public class StartCommandsProccesor
    {
        [Fact]
        public async Task ShouldExcuteCommand_WhenRunning()
        {
            // Arrange

            var commandMock = new Mock<ICommand>();

            commandMock.Setup(_ => _.Execute());
            
            var commandsManager = new CommandsManager.CommandsManager();

            commandsManager.AddCommandToQueue(commandMock.Object);

            commandsManager.SoftStopCommandsProccesor();

            // Act

            await Task.Run(() => commandsManager.StartCommandsProccesor());

            // Assert

            commandMock.Verify(_ => _.Execute(), Times.Once);
        }
    }
}
