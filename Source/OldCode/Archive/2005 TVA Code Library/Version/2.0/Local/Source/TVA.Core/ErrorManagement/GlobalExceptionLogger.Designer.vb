Namespace ErrorManagement

    Partial Class GlobalExceptionLogger
        Inherits System.ComponentModel.Component

        <System.Diagnostics.DebuggerNonUserCode()> _
        Public Sub New(ByVal container As System.ComponentModel.IContainer)
            MyClass.New()

            'Required for Windows.Forms Class Composition Designer support
            If (container IsNot Nothing) Then
                container.Add(Me)
            End If

        End Sub

        <System.Diagnostics.DebuggerNonUserCode()> _
        Public Sub New()
            MyBase.New()

            'This call is required by the Component Designer.
            InitializeComponent()

            m_autoRegister = True
            m_smtpServer = TVA.Net.Smtp.SimpleMailMessage.DefaultMailServer
            m_errorTextMethod = AddressOf GetErrorText
            m_scopeTextMethod = AddressOf GetScopeText
            m_actionTextMethod = AddressOf GetActionText
            m_moreInfoTextMethod = AddressOf GetMoreInfoText
            m_loggers = New List(Of LoggerMethodSignature)()
            m_settingsCategoryName = Me.GetType().Name

        End Sub

        'Component overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()> _
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                Unregister()
                SaveSettings()
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        'Required by the Component Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Component Designer
        'It can be modified using the Component Designer.
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Me._LogFile = New TVA.IO.LogFile(Me.components)
            CType(Me._LogFile, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            '_LogFile
            '
            Me._LogFile.Name = "LogFile.txt"
            CType(Me._LogFile, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents _LogFile As TVA.IO.LogFile

    End Class

End Namespace