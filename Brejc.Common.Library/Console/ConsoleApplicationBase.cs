using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Brejc.Common.Console
{
    /// <summary>
    /// A base abstract class which can be used to implement a console application.
    /// </summary>
    public abstract class ConsoleApplicationBase
    {
        /// <summary>
        /// Gets the console command line arguments.
        /// </summary>
        /// <value>The console command line arguments.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage ("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        protected string[] Args
        {
            get { return args; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleApplicationBase"/> class with the command line arguments specified.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        protected ConsoleApplicationBase (string[] args)
        {
            this.args = args;
        }

        /// <summary>
        /// Runs the console application.
        /// </summary>
        [SuppressMessage ("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void Run ()
        {
            try
            {
                ShowBanner();

                IList<IConsoleApplicationCommand> commands = ParseArguments ();

                if (commands == null)
                {
                    ShowHelp();
                    Environment.Exit (1);
                }
                else
                {
                    foreach (IConsoleApplicationCommand command in commands)
                        command.Execute ();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler (ex);
            }
        }

        /// <summary>
        /// Parses command line arguments and returns a list of resulting console commands to be executed.
        /// An abstract method which needs to be implemented by the inheriting class.
        /// </summary>
        /// <returns>A list of resulting console commands to be executed.</returns>
        public abstract IList<IConsoleApplicationCommand> ParseArguments ();

        /// <summary>
        /// Writes the application banner text to the console output stream.
        /// An abstract method which needs to be implemented by the inheriting class.
        /// </summary>
        public abstract void ShowBanner ();

        /// <summary>
        /// Writes the application help text to the console output stream.
        /// An abstract method which needs to be implemented by the inheriting class.
        /// </summary>
        public abstract void ShowHelp ();

        /// <summary>
        /// Handles an exception which occurred during execution of the <see cref="Run"/> method of this console application.
        /// </summary>
        /// <param name="ex">Instance of the thrown exception.</param>
        public virtual void ExceptionHandler (Exception ex)
        {
#if DEBUG
            System.Console.Error.WriteLine (ex.ToString());
#endif
            System.Console.Error.WriteLine ();
            System.Console.Error.WriteLine ("ERROR: {0}", ex);
            Environment.Exit (-1);
        }

        private readonly string[] args;
    }
}
