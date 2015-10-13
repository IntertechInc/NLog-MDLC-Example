using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLogCustomLoggingContext
{
    class Program
    {
        static NLog.Logger logger;
        static void Main(string[] args)
        {
            logger = NLog.LogManager.GetCurrentClassLogger();

            //Set the MDLC context for trackingId (ideally you'd use this in an Async/Await context and set this as early as possible)
            //In reality Global Diagnostics Context (GDC) would be preferable in a serial non-async app such as this example
            NLog.MappedDiagnosticsLogicalContext.Set("trackingId", Guid.NewGuid());

            DoWork("log it");

            DoWork("log it again"); //Should get same trackingId
        }

        static void DoWork(string theWork)
        {
            logger.Debug("Doing work: {0}", theWork);
        }
    }
}
