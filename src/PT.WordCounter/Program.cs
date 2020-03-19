using System;
using System.Text;
using System.Threading;
using Autofac;
using CommandLine;
using PT.WordCounter.Support;
using PT.WordCounter.Logic;

namespace PT.WordCounter
{
    class Program
    {
        private static CancellationTokenSource _cts;

        static int Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            _cts = new CancellationTokenSource();
            Console.CancelKeyPress += Canceled;

            return Parser.Default
                .ParseArguments<FileCommand, ConsoleCommand, DatabaseCommand>(args)
                .MapResult(
                    (FileCommand command) => RunCommand(command, _cts.Token),
                    (ConsoleCommand command) => RunCommand(command, _cts.Token),
                    (DatabaseCommand command) => RunCommand(command, _cts.Token), 
                    errs => -1
                );
        }

        private static int RunCommand(ICommand command, CancellationToken token)
        {
            var container = CompositionRoot.CreateContainer(command);
            var process = container.Resolve<Processor>();

            try
            {
                process.StartAsync(token).Wait();
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return -1;
            }
        }

        private static void Canceled(object sender, ConsoleCancelEventArgs e)
        {
            if (e.SpecialKey == ConsoleSpecialKey.ControlC || e.SpecialKey == ConsoleSpecialKey.ControlBreak)
            {
                Console.CancelKeyPress -= Canceled;
                _cts.Cancel();
            }
        }
    }
}
