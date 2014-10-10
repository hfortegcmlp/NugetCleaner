using System;
using System.Configuration;
using System.ServiceProcess;
using System.Threading;
using DirectoryScanner;
using log4net;

namespace NugetCleaner
{
    public partial class Service1 : ServiceBase
    {
        private Scanner _scanner;
        private ILog _logger;
        private Timer _timer;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _logger = LogManager.GetLogger("NugetCleaner");
            _logger.Debug("Service Started");
            var appSettings = ConfigurationManager.AppSettings;
            string source = appSettings["NugetSource"];
            string destination = appSettings["NugetDestination"];

            if (source == null || destination == null)
            {
                throw new Exception("NugetSource and NugetDestination must be defined in app.config");
            }

            _logger.Debug("Appsetting retrieved");
            _scanner = new Scanner(source, destination);

            _logger.Debug("Timer Started");
            _timer = new Timer(Process, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        private void Process(object state)
        {
            _logger.Debug("Processing");
            //_scanner.Process();
        }


        protected override void OnStop()
        {
            _logger.Debug("Killing timer");
            _timer.Dispose();
        }

    }
}
