using log4net;
using log4net.Repository.Hierarchy;
using log4net.Appender;
using log4net.Layout;


namespace TLX.Logger {   

	public class TLXLogger
	{

        public static void Setup(string filepath) {

            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
            hierarchy.Root.RemoveAllAppenders();

            FileAppender fileAppender = new FileAppender();
            fileAppender.AppendToFile = true;
            fileAppender.AppendToFile = true;
            fileAppender.LockingModel = new FileAppender.MinimalLock();
            fileAppender.File = filepath;
            PatternLayout pl = new PatternLayout();
            pl.ConversionPattern = "%date %-5level - %message;; %newline";
            pl.ActivateOptions();
            fileAppender.Layout = pl;
            fileAppender.ActivateOptions();

            log4net.Config.BasicConfigurator.Configure(fileAppender);

        }

	}
   
	
}
