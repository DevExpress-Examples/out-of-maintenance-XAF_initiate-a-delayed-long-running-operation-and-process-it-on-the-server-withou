Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Actions
Imports DevExpress.ExpressApp.ConditionalAppearance
Imports DevExpress.Persistent.Base
Imports LongRunningOperations.Module.BusinessObjects

Namespace LongRunningOperations.Module.Controllers
	Public Class StartProcessingController
		Inherits ViewController
		Public Sub New()
			Dim action As New SimpleAction(Me, "StartProcessing", PredefinedCategory.Edit)
			AddHandler action.Execute, AddressOf action_Execute
		End Sub
		Private Sub action_Execute(ByVal sender As Object, ByVal e As SimpleActionExecuteEventArgs)
			StartProcessing()
		End Sub
		Public Sub StartProcessing()
			Dim os As IObjectSpace = Application.CreateObjectSpace()
			Dim obj As ObjectForLongRunningOperations = os.CreateObject(Of ObjectForLongRunningOperations)()
			obj.ObjectToProcess = CType(os.GetObject(View.CurrentObject), Object1)
			If obj.ObjectToProcess Is Nothing Then
				Throw New ArgumentException()
			End If
			os.CommitChanges()
			CType(View.CurrentObject, Object1).SetIsInProcessing(True)
			Frame.GetController(Of AppearanceController)().Refresh()
		End Sub
	End Class
End Namespace
