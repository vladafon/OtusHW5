using OtusHW5.CommandsManager;

namespace OtusHW5
{
    internal class CommandImmitator : ICommand
    {
        public void Execute()
        {
            var random = new Random();

            var executeTimeMs = random.Next(300, 5000);

            var failProbability = random.Next(0, 100);

            var isFailLoad = failProbability > 50;

            if (isFailLoad)
            {
                Thread.Sleep(100);

                throw new Exception("Команда не выполнилась.");
            }

            Thread.Sleep(executeTimeMs);
        }
    }
}
