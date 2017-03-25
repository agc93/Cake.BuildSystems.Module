using System;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Module.Shared;

namespace Cake.MyGet.Module
{
    public class MyGetBuildLog : ICakeLog
    {
        public MyGetBuildLog(IConsole console, Verbosity verbosity = Verbosity.Normal)
        {
            _cakeLogImplementation = new CakeBuildLog(console, verbosity);
        }
        private readonly ICakeLog _cakeLogImplementation;

        public void Write(Verbosity verbosity, LogLevel level, string format, params object[] args)
        {
            if (!string.IsNullOrWhiteSpace(System.Environment.GetEnvironmentVariable("TF_BUILD")))
            {
                switch (level)
                {
                    case LogLevel.Fatal:
                        _cakeLogImplementation.Write(Verbosity.Quiet, LogLevel.Information,
                            //"##myget[buildProblem description='{0}']", string.Format(format, args));
                            "##myget[message text='{0}' status='FAILURE']", string.Format(format, args));
                        break;
                    case LogLevel.Error:
                        _cakeLogImplementation.Write(Verbosity.Quiet, LogLevel.Information,
                            "##myget[message text='{0}' status='ERROR']", string.Format(format, args));
                        break;
                    case LogLevel.Warning:
                        _cakeLogImplementation.Write(Verbosity.Quiet, LogLevel.Information,
                            "##myget[message text='{0}' status='WARNING']", string.Format(format, args));
                        break;
                    case LogLevel.Information:
                        /*
                        _cakeLogImplementation.Write(Verbosity.Quiet, LogLevel.Information,
                            "##myget[message text='{0}' status='NORMAL']", string.Format(format, args));
                        break;
                         */
                    case LogLevel.Verbose:
                    case LogLevel.Debug:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(level), level, null);
                }
            }
            _cakeLogImplementation.Write(verbosity, level, format, args);
        }

        public Verbosity Verbosity
        {
            get { return _cakeLogImplementation.Verbosity; }
            set { _cakeLogImplementation.Verbosity = value; }
        }
    }
}