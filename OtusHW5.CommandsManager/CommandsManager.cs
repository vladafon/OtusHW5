using System.Collections.Concurrent;

namespace OtusHW5.CommandsManager
{
    public class CommandsManager
    {
        private ConcurrentQueue<ICommand> _commands = new ConcurrentQueue<ICommand>();

        private ManualResetEvent _onNewCommandInQueueEvent = new ManualResetEvent(false);

        public void AddCommandToQueue(ICommand command)
        {
            _commands.Enqueue(command);
            _onNewCommandInQueueEvent.Set();

            Console.WriteLine($"Команда {command} добавлена в очередь.");
        }

        public void StartCommandsProccesor()
        {
            while (true)
            {
                if (_commands.IsEmpty)
                {
                    _onNewCommandInQueueEvent.Reset();
                }

                Console.WriteLine($"Обработчик ждет команду. В очереди {_commands.Count} элементов");
                _onNewCommandInQueueEvent.WaitOne();

                if (!_commands.TryDequeue(out var command))
                {
                    Console.WriteLine("Отсутствует команда в очереди на выполнение.");
                    continue;
                }

                Console.WriteLine($"Поступила команда {command} на выполение.");

                try
                {
                    Console.WriteLine($"Начинаем выполнение команды {command}.");
                    command.Execute();
                    Console.WriteLine($"Команда {command} успешно отработала.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
