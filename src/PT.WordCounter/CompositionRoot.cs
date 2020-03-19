using System;
using System.Text;
using Autofac;
using PT.WordCounter.Logic;
using PT.WordCounter.Support;
using PT.WordCounter.Contracts;
using PT.WordCounter.FileProvider;
using PT.WordCounter.ConsoleProvider;
using PT.WordCounter.DatabaseProvider;

namespace PT.WordCounter
{
    internal static class CompositionRoot
    {
        private static ContainerBuilder Default()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Processor>();

            return builder;
        }

        public static IContainer CreateContainer(ICommand command)
        {
            var builder = Default();
            if (command is FileCommand fileCommand)
            {
                return CreateContainerInternal(fileCommand, builder);
            }
            else if (command is ConsoleCommand consoleCommand)
            {
                return CreateContainerInternal(consoleCommand, builder);
            }
            else if (command is DatabaseCommand databaseCommand)
            {
                return CreateContainerInternal(databaseCommand, builder);
            }
            else
            {
                throw new Exception("Unsupported command");
            }
        }

        private static IContainer CreateContainerInternal(FileCommand command, ContainerBuilder builder)
        {
            var encoding = Constants.EncodingWin1251;
            if (command.UTF8)
            {
                encoding = Encoding.UTF8;
            }

            var options = new TextFileProviderOptions(command.SourceFile, command.TargetFile, 4 * 1024, encoding);
            builder.RegisterInstance(options);
            builder.RegisterType<TextFileProvider>().As<IDataProvider>();

            return builder.Build();
        }

        private static IContainer CreateContainerInternal(ConsoleCommand command, ContainerBuilder builder)
        {
            var provider = new StdOutProvider(command.Text);
            builder.RegisterInstance(provider).As<IDataProvider>();
            return builder.Build();
        }

        private static IContainer CreateContainerInternal(DatabaseCommand command, ContainerBuilder builder)
        {
            var options = new DatabaseProviderOptions(command.ConnectionString, command.Table, command.Column);
            builder.RegisterInstance(options);
            builder.RegisterType<DatabaseProvider.DatabaseProvider>().As<IDataProvider>();
            return builder.Build();
        }
    }
}
