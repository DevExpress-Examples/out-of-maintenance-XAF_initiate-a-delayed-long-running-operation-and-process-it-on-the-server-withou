Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Data.Filtering
Imports DevExpress.ExpressApp.ConditionalAppearance
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Xpo

Namespace LongRunningOperations.Module.BusinessObjects
	<DefaultClassOptions, Appearance("IsInProcessing", "[IsInProcessing]='True'", Enabled := False, TargetItems:="*")> _
	Public Class Object1
		Inherits BaseObject
		Friend isInProcessing_Renamed? As Boolean
		Private name_Renamed As String
		Private description_Renamed As String
		Public Property Name() As String
			Get
				Return name_Renamed
			End Get
			Set(ByVal value As String)
				SetPropertyValue(Of String)("Name", name_Renamed, value)
			End Set
		End Property
		<Size(SizeAttribute.Unlimited)> _
		Public Property Description() As String
			Get
				Return description_Renamed
			End Get
			Set(ByVal value As String)
				SetPropertyValue(Of String)("Description", description_Renamed, value)
			End Set
		End Property
		Public ReadOnly Property IsInProcessing() As Boolean
			Get
				If (Not isInProcessing_Renamed.HasValue) Then
					isInProcessing_Renamed = (Session.FindObject(Of ObjectForLongRunningOperations)(New BinaryOperator("ObjectToProcess", Me)) IsNot Nothing)
				End If
				Return isInProcessing_Renamed.Value
			End Get
		End Property
		Public Sub New(ByVal session As Session)
			MyBase.New(session)

		End Sub
		Public Sub SetIsInProcessing(ByVal val As Boolean)
			isInProcessing_Renamed = val
			RaisePropertyChangedEvent("IsInProcessing")
		End Sub
	End Class
End Namespace