using System.Collections.Concurrent;

namespace OtusHW5.CommandsManager
{
    public class CommandsManager
    {
        public int CommandsCount { get => _commands.Count; }

        private ConcurrentQueue<ICommand> _commands = new ConcurrentQueue<ICommand>();

        private ManualResetEvent _onNewCommandInQueueEvent = new ManualResetEvent(false);

        private bool isHardStopped = false;

        private bool isSoftStopped = false;

        private Barrier _barrier = new Barrier(0);

        public void AddCommandToQueue(ICommand command)
        {
            _commands.Enqueue(command);
            _onNewCommandInQueueEvent.Set();

            Console.WriteLine($"Команда {command} добавлена в очередь.");
        }

        public void StartCommandsProccesor()
        {
            _barrier.AddParticipant();

            while (true)
            {
                if (isHardStopped)
                {
                    Console.WriteLine("Инициирована принудительная жесткая остановка обработчика.");
                    _barrier.SignalAndWait();
                    isHardStopped = false;
                    isSoftStopped = false;
                    break;
                }

                if (isSoftStopped && _commands.IsEmpty)
                {
                    Console.WriteLine("Инициирована принудительная мягкая остановка обработчика.");
                    _barrier.SignalAndWait();
                    isSoftStopped = false;
                    isHardStopped = false;
                    break;
                }

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

            _barrier.RemoveParticipant();
            Console.WriteLine($"Обработчик прекратил работу. Задач в очереди {_commands.Count}.");
        }

        public void HardStopCommandsProccesor()
        {
            isHardStopped = true;
            _onNewCommandInQueueEvent.Set();

            Console.WriteLine($"Вызван метод жесткой остановки.");
        }

        public void SoftStopCommandsProccesor()
        {
            isSoftStopped = true;
            _onNewCommandInQueueEvent.Set();

            Console.WriteLine($"Вызван метод мягкой остановки.");
        }
    }
}
