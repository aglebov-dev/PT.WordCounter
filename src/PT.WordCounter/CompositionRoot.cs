using System;
using System.Text;
using System.Threading;
using Autofac;
using PT.WordCounter.Logic;
using PT.WordCounter.Support;
using PT.WordCounter.Contracts;
using PT.WordCounter.FileProvider;
using PT.WordCounter.ConsoleProvider;
using PT.WordCounter.DatabaseProvider;
using Microsoft.EntityFrameworkCore;
using PT.WordCounter.DatabaseProvider.DataAccess;

namespace PT.WordCounter
{
    internal static class CompositionRoot
    {
        private static ContainerBuilder Default(CancellationToken token)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Processor>();
            builder.Register(x => token);

            return builder;
        }

        public static IContainer CreateContainer(ICommand command, CancellationToken token)
        {
            var builder = Default(token);
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

            builder
                .RegisterType<TextFileReader>()
                .As<IReader>()
                .SingleInstance();

            builder
              .RegisterType<TextFileWriter>()
              .As<IWriter>()
              .SingleInstance();

            return builder.Build();
        }

        private static IContainer CreateContainerInternal(ConsoleCommand command, ContainerBuilder builder)
        {
            builder
               .RegisterType<ConsoleReader>()
               .WithParameter(
                    (info, context) => info.ParameterType == typeof(string),
                    (info, context) => command.Text
                )
               .As<IReader>()
               .SingleInstance();

            builder
              .RegisterType<ConsoleWriter>()
              .As<IWriter>()
              .SingleInstance();

            return builder.Build();
        }

        private static IContainer CreateContainerInternal(DatabaseCommand command, ContainerBuilder builder)
        {  
            var options = new DatabaseProviderOptions(command.ConnectionString, command.Table, command.Column);
            var dbOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseNpgsql(options.ConnectionStrings);
          
            builder.RegisterInstance(options);
            builder.RegisterInstance(dbOptions.Options);

            builder
                .RegisterType<DatabaseReader>()
                .As<IReader>()
                .SingleInstance();

            builder
                .RegisterType<DatabaseWriter>()
                .As<IWriter>()
                .SingleInstance();

            builder
                .RegisterType<DatabaseContext>()
                .AsSelf()
                .SingleInstance();

            return builder.Build();
        }
    }
}
