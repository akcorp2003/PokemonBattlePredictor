Public Class Battle_Prediction_Thread
    Dim m_id As Integer

    ''' <summary>
    ''' Although the thread class has the ManagedThreadID, we are going to use our own numbering system
    ''' </summary>
    ''' <value>An Integer starting at 0</value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Thread_ID As Integer
        Get
            Return m_id
        End Get
        Set(value As Integer)
            m_id = value
        End Set
    End Property

    Public Event Finished_Projecting(ByVal result As String, ByVal result_array As String())

    Public Sub Start_Threads(ByVal num_threads As Integer)

    End Sub
End Class
