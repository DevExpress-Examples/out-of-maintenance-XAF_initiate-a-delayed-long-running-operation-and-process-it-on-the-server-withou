using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.ExpressApp.Security.ClientServer;
using System.Configuration;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.ExpressApp.Security.ClientServer.Remoting;
using DevExpress.ExpressApp;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.MiddleTier;
using System.Timers;
using LongRunningOperations.Module.BusinessObjects;

namespace ConsoleApplicationServer1
{
    class Program
    {
        private static void serverApplication_DatabaseVersionMismatch(object sender, DatabaseVersionMismatchEventArgs e)
        {
            e.Updater.Update();
            e.Handled = true;
        }
        private static void serverApplication_CreateCustomObjectSpaceProvider(object sender, CreateCustomObjectSpaceProviderEventArgs e)
        {
            e.ObjectSpaceProvider = new XPObjectSpaceProvider(e.ConnectionString, e.Connection);
        }
        static void Main(string[] args)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                ValueManager.ValueManagerType = typeof(MultiThreadValueManager<>).GetGenericTypeDefinition();

                Console.WriteLine("Starting...");

                ServerApplication serverApplication = new ServerApplication();
                // Change the ServerApplication.ApplicationName property value. It should be the same as your client application name. 
                serverApplication.ApplicationName = "LongRunningOperations";

                // Add your client application's modules to the ServerApplication.Modules collection here. 
                serverApplication.Modules.Add(new DevExpress.ExpressApp.SystemModule.SystemModule());
                serverApplication.Modules.Add(new DevExpress.ExpressApp.Security.SecurityModule());
                serverApplication.Modules.Add(new LongRunningOperations.Module.LongRunningOperationsModule());

                serverApplication.DatabaseVersionMismatch += new EventHandler<DatabaseVersionMismatchEventArgs>(serverApplication_DatabaseVersionMismatch);
                serverApplication.CreateCustomObjectSpaceProvider += new EventHandler<CreateCustomObjectSpaceProviderEventArgs>(serverApplication_CreateCustomObjectSpaceProvider);

                serverApplication.ConnectionString = connectionString;

                Console.WriteLine("Setup...");
                serverApplication.Setup();
                Console.WriteLine("CheckCompatibility...");
                serverApplication.CheckCompatibility();

                Console.WriteLine("Starting server...");

                Timer timer = new Timer(10000);
                timer.Elapsed += delegate(object sender, ElapsedEventArgs e) {
                    IObjectSpace objectSpace = serverApplication.CreateObjectSpace();
                    List<ObjectForLongRunningOperations> list = new List<ObjectForLongRunningOperations>(objectSpace.GetObjects<ObjectForLongRunningOperations>());
                    foreach(ObjectForLongRunningOperations obj in list) {
                        Console.WriteLine("Processing object '" + obj.ObjectToProcess.Name+ "'...");
                        if(obj.ObjectToProcess != null) {
                            obj.ObjectToProcess.Description += "Processed on " + DateTime.Now;
                        }
                        obj.Delete();
                        objectSpace.CommitChanges();
                        Console.WriteLine("Done");
                    }
                    list.Clear();
                };
                timer.Enabled = true;
                Console.WriteLine("Server is started. Press Enter to stop.");
                Console.ReadLine();
                Console.WriteLine("Server is stopped.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occurs: " + e.Message);
                Console.WriteLine("Press Enter to close.");
                Console.ReadLine();
            }
        }
    }
}
