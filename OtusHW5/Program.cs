using OtusHW5;
using OtusHW5.CommandsManager;

var commandsManager = new CommandsManager();

Task.Run(() => commandsManager.StartCommandsProccesor());
Task.Run(() => commandsManager.StartCommandsProccesor());
Task.Run(() => commandsManager.StartCommandsProccesor());

for (var i = 0; i < 10; i++)
{ 
    var command = new CommandImmitator();
    commandsManager.AddCommandToQueue(command);
}

commandsManager.SoftStopCommandsProccesor();

Console.ReadKey();