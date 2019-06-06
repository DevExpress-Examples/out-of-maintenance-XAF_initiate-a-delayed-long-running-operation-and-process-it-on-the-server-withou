Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports DevExpress.ExpressApp.Security.ClientServer
Imports System.Configuration
Imports DevExpress.Persistent.Base
Imports DevExpress.ExpressApp.Security
Imports DevExpress.ExpressApp.Security.Strategy
Imports DevExpress.ExpressApp.Security.ClientServer.Remoting
Imports DevExpress.ExpressApp
Imports System.Collections
Imports System.Runtime.Remoting
Imports System.Runtime.Remoting.Channels
Imports System.Runtime.Remoting.Channels.Tcp
Imports DevExpress.ExpressApp.Xpo
Imports DevExpress.ExpressApp.MiddleTier
Imports System.Timers
Imports LongRunningOperations.Module.BusinessObjects

Namespace ConsoleApplicationServer1
	Friend Class Program
		Private Shared Sub serverApplication_DatabaseVersionMismatch(ByVal sender As Object, ByVal e As DatabaseVersionMismatchEventArgs)
			e.Updater.Update()
			e.Handled = True
		End Sub
		Private Shared Sub serverApplication_CreateCustomObjectSpaceProvider(ByVal sender As Object, ByVal e As CreateCustomObjectSpaceProviderEventArgs)
			e.ObjectSpaceProvider = New XPObjectSpaceProvider(e.ConnectionString, e.Connection)
		End Sub
		Shared Sub Main(ByVal args() As String)
			Try
				Dim connectionString As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

				ValueManager.ValueManagerType = GetType(MultiThreadValueManager(Of )).GetGenericTypeDefinition()

				Console.WriteLine("Starting...")

				Dim serverApplication As New ServerApplication()
				' Change the ServerApplication.ApplicationName property value. It should be the same as your client application name. 
				serverApplication.ApplicationName = "LongRunningOperations"

				' Add your client application's modules to the ServerApplication.Modules collection here. 
				serverApplication.Modules.Add(New DevExpress.ExpressApp.SystemModule.SystemModule())
				serverApplication.Modules.Add(New DevExpress.ExpressApp.Security.SecurityModule())
				serverApplication.Modules.Add(New LongRunningOperations.Module.LongRunningOperationsModule())

				AddHandler serverApplication.DatabaseVersionMismatch, AddressOf serverApplication_DatabaseVersionMismatch
				AddHandler serverApplication.CreateCustomObjectSpaceProvider, AddressOf serverApplication_CreateCustomObjectSpaceProvider

				serverApplication.ConnectionString = connectionString

				Console.WriteLine("Setup...")
				serverApplication.Setup()
				Console.WriteLine("CheckCompatibility...")
				serverApplication.CheckCompatibility()

				Console.WriteLine("Starting server...")

				Dim timer As New Timer(10000)
				AddHandler timer.Elapsed, Sub(sender As Object, e As ElapsedEventArgs)
					Dim objectSpace As IObjectSpace = serverApplication.CreateObjectSpace(GetType(ObjectForLongRunningOperations))
					Dim list As New List(Of ObjectForLongRunningOperations)(objectSpace.GetObjects(Of ObjectForLongRunningOperations)())
					For Each obj As ObjectForLongRunningOperations In list
						Console.WriteLine("Processing object '" & obj.ObjectToProcess.Name & "'...")
						If obj.ObjectToProcess IsNot Nothing Then
							obj.ObjectToProcess.Description &= "Processed on " & DateTime.Now
						End If
						obj.Delete()
						objectSpace.CommitChanges()
						Console.WriteLine("Done")
					Next obj
					list.Clear()
				End Sub
				timer.Enabled = True
				Console.WriteLine("Server is started. Press Enter to stop.")
				Console.ReadLine()
				Console.WriteLine("Server is stopped.")
			Catch e As Exception
				Console.WriteLine("Exception occurs: " & e.Message)
				Console.WriteLine("Press Enter to close.")
				Console.ReadLine()
			End Try
		End Sub
	End Class
End Namespace
