Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Persistent.Base
Imports DevExpress.Xpo

Namespace LongRunningOperations.Module.BusinessObjects
    <DefaultClassOptions> _
    Public Class ObjectForLongRunningOperations
        Inherits BaseObject

        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub
        Private obj As Object1
        Public Property ObjectToProcess() As Object1
            Get
                Return obj
            End Get
            Set(ByVal value As Object1)
                SetPropertyValue(Of Object1)("ObejectToProcess", obj, value)
            End Set
        End Property
    End Class
End Namespace
