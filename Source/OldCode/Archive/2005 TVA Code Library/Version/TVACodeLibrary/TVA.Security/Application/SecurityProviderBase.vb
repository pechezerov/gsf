'*******************************************************************************************************
'  TVA.Security.Application.SecurityProviderBase.vb - Base class for application security provider
'  Copyright � 2006 - TVA, all rights reserved - Gbtc
'
'  Build Environment: VB.NET, Visual Studio 2005
'  Primary Developer: Pinal C. Patel, Operations Data Architecture [TVA]
'      Office: COO - TRNS/PWR ELEC SYS O, CHATTANOOGA, TN - MR 2W-C
'       Phone: 423/751-2250
'       Email: pcpatel@tva.gov
'
'  Code Modification History:
'  -----------------------------------------------------------------------------------------------------
'  09/22/2006 - Pinal C. Patel
'       Original version of source code generated.
'  11/30/2007 - Pinal C. Patel
'       Modified the "design time" check in EndInit() method to use LicenseManager.UsageMode property
'       instead of DesignMode property as the former is more accurate than the latter.
'  12/28/2007 - Pinal C. Patel
'       Modified the ConnectionString property to use the backup SQL Server database in case if any of 
'       the primary databases are unavailable or offline.
'       Renamed the DbConnectionException event to DatabaseException as this event is raised in the
'       event of any SQL Server exception that is encountered.
'  05/06/2008 - Pinal C. Patel
'       Added AuthenticationMode property and modified LoginUser() method to add support for RSA 
'       authentication.
'       Implemented TVA.Configuration.IPersistSettings interface to allow for the property values to 
'       saved and retrieved from the config file.
'       Moved security database connection strings to the User class - better fit.
'  05/13/2008 - Pinal C. Patel
'       Added overload for LoginUser() that allows the user's username and password to be passed in.
'
'*******************************************************************************************************

Imports System.Reflection
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports TVA.Configuration

Namespace Application

    ''' <summary>
    ''' Base class of the security provider control.
    ''' </summary>
    <ProvideProperty("ValidRoles", GetType(Object)), ProvideProperty("ValidRoleAction", GetType(Object))> _
    Public MustInherit Class SecurityProviderBase
        Implements IExtenderProvider, ISupportInitialize, IPersistSettings

#Region " Member Declaration "

        Private m_user As User
        Private m_server As SecurityServer
        Private m_applicationName As String
        Private m_authenticationMode As AuthenticationMode
        Private m_persistSettings As Boolean
        Private m_settingsCategoryName As String
        Private m_extendeeControls As Hashtable

#End Region

#Region " Event Declaration "

        ''' <summary>
        ''' Occurs before the login process is started.
        ''' </summary>
        <Description("Occurs before the login process is started.")> _
        Public Event BeforeLogin As EventHandler(Of CancelEventArgs)

        ''' <summary>
        ''' Occurs after the login process is complete.
        ''' </summary>
        <Description("Occurs after the login process is complete.")> _
        Public Event AfterLogin As EventHandler

        ''' <summary>
        ''' Occurs before the login prompt is shown.
        ''' </summary>
        <Description("Occurs before the login prompt is shown.")> _
        Public Event BeforeLoginPrompt As EventHandler(Of CancelEventArgs)

        ''' <summary>
        ''' Occurs after the login prompt has been shown.
        ''' </summary>
        <Description("Occurs after the login prompt has been shown.")> _
        Public Event AfterLoginPrompt As EventHandler

        ''' <summary>
        ''' Occurs before user data is initialized.
        ''' </summary>
        <Description("Occurs before user data is initialized.")> _
        Public Event BeforeInitializeData As EventHandler

        ''' <summary>
        ''' Occurs after user data has been initialized.
        ''' </summary>
        <Description("Occurs after user data has been initialized.")> _
        Public Event AfterInitializeData As EventHandler

        ''' <summary>
        ''' Occurs before user is authenticated for application access.
        ''' </summary>
        <Description("Occurs before user is authenticated for application access.")> _
        Public Event BeforeAuthenticate As EventHandler(Of CancelEventArgs)

        ''' <summary>
        ''' Occurs after user has been authenticated for application access.
        ''' </summary>
        <Description("Occurs after user has been authenticated for application access.")> _
        Public Event AfterAuthenticate As EventHandler

        ''' <summary>
        ''' Occurs when user has access to the application.
        ''' </summary>
        <Description("Occurs when user has access to the application.")> _
        Public Event AccessGranted As EventHandler(Of CancelEventArgs)

        ''' <summary>
        ''' Occurs when user does not have access to the application.
        ''' </summary>
        <Description("Occurs when user does not have access to the application.")> _
        Public Event AccessDenied As EventHandler(Of CancelEventArgs)

        ''' <summary>
        ''' Occurs when a database exception is encountered during the login process.
        ''' </summary>
        <Description("Occurs when a database exception is encountered during the login process.")> _
        Public Event DatabaseException As EventHandler(Of GenericEventArgs(Of Exception))

#End Region

#Region " Code Scope: Public Code "

        ''' <summary>
        ''' Gets or sets the security database server against which users are authenticated.
        ''' </summary>
        ''' <value></value>
        ''' <returns>One of the TVA.Security.Application.SecurityServer values.</returns>
        <Category("Configuration")> _
        Public Property Server() As SecurityServer
            Get
                Return m_server
            End Get
            Set(ByVal value As SecurityServer)
                m_server = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the application name as defined in the security database that is being secured.
        ''' </summary>
        ''' <value></value>
        ''' <returns>Name of the application as defined in the security database that is being secured.</returns>
        <Category("Configuration")> _
        Public Property ApplicationName() As String
            Get
                Return m_applicationName
            End Get
            Set(ByVal value As String)
                m_applicationName = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the mode of authentication to be used for authenticating users of the application.
        ''' </summary>
        ''' <value></value>
        ''' <returns>One of the TVA.Security.Application.AuthenticationMode values.</returns>
        <Category("Configuration")> _
        Public Property AuthenticationMode() As AuthenticationMode
            Get
                Return m_authenticationMode
            End Get
            Set(ByVal value As AuthenticationMode)
                m_authenticationMode = value
            End Set
        End Property

        ''' <summary>
        ''' Gets data about the current user.
        ''' </summary>
        ''' <value></value>
        ''' <returns>Instance of TVA.Security.Application.User class.</returns>
        <Browsable(False)> _
        Public Property User() As User
            Get
                Return m_user
            End Get
            Protected Set(ByVal value As User)
                ' We'll only allow derived class to update this data.
                m_user = value
            End Set
        End Property

        ''' <summary>
        ''' Determines whether or not the current user has access to the application.
        ''' </summary>
        ''' <returns>True if the current user has access to the application; otherwise False.</returns>
        Public Function UserHasApplicationAccess() As Boolean

            Return (m_user IsNot Nothing AndAlso m_user.IsDefined AndAlso Not m_user.IsLockedOut AndAlso _
                        m_user.IsAuthenticated AndAlso m_user.FindApplication(m_applicationName) IsNot Nothing)

        End Function

        ''' <summary>
        ''' Logs in the current user.
        ''' </summary>
        Public Sub LoginUser()

            LoginUser(String.Empty, String.Empty)

        End Sub

        ''' <summary>
        ''' Logs in the specified user.
        ''' </summary>
        ''' <param name="username">Username of the user to login.</param>
        ''' <param name="password">Password of the user to login.</param>
        Public Sub LoginUser(ByVal username As String, ByVal password As String)

            If Not String.IsNullOrEmpty(m_applicationName) Then
                Dim beforeLoginEventData As New CancelEventArgs()
                RaiseEvent BeforeLogin(Me, beforeLoginEventData)
                If beforeLoginEventData.Cancel Then Exit Sub

                If m_user Is Nothing Then
                    ' We don't have data about the user, so we must get it.
                    If String.IsNullOrEmpty(username) Then username = GetUsername() ' Get username from inheriting class if it has.
                    If String.IsNullOrEmpty(password) Then password = GetPassword() ' Get password from inheriting class if it has.

                    Select Case m_authenticationMode
                        Case Security.Application.AuthenticationMode.AD
                            ' This will get us the login ID of the current user. This will be null in case of web 
                            ' application if:
                            ' 1) Secured web page is being accessed from outside.
                            ' 2) Secured web page is being accessed from inside, but 
                            '    "Integrated Windows Authentication" is turned off for the web site.
                            ' Note: In case of a windows app, we'll always get the login ID of the current user.
                            Dim userLoginID As String = System.Threading.Thread.CurrentPrincipal.Identity.Name

                            ' The order of conditional execution is important for the following scenarios to work:
                            ' o Internal user wants to access a secure page for which he/she does not have access, 
                            '   but has the credentials of a user who has access to this page and want to use the 
                            '   credentials in order to access the secure web page.
                            ' o Developer of an externally facing web site ("Anonymous access" is on) wants to test 
                            '   the security without turning-off "Integrated Windows Authentication" for the web 
                            '   site, as doing so will disable the debugging capabilities from the Visual Studio IDE.
                            ' Note: Both of the scenarios above require that the person trying do access the secured 
                            '       web page with someone else's credentials does not have access to the web page. 
                            If Not String.IsNullOrEmpty(username) AndAlso Not String.IsNullOrEmpty(password) Then
                                ' We have the username and password provided to us by the derived class. Since the
                                ' username and password have been captured and verified by the derived class, we 
                                ' will not authenticate these credentials again.
                                InitializeUser(username, password, False)
                            ElseIf Not String.IsNullOrEmpty(userLoginID) Then
                                ' We don't have the username and password from the derived class, but we have the
                                ' login ID of the current user. Since no authentication has been performed yet, we
                                ' will authenticate the login ID just to comfirm.
                                InitializeUser(userLoginID.Split("\"c)(1), String.Empty, True)
                            Else
                                ' We don't have any option other than prompting for credentials.
                                If Not CaptureCredentials() Then Exit Sub ' Implementer wants to abort.
                            End If
                        Case Security.Application.AuthenticationMode.RSA
                            ' In the case of RSA authentication mode, we must always prompt the user for the
                            ' credentials when the user accesses a secure application for the first time.
                            If Not String.IsNullOrEmpty(username) AndAlso Not String.IsNullOrEmpty(password) Then
                                ' Derived class has captured user credentials and authenticated them successfully.
                                InitializeUser(username, password, False)
                            Else
                                ' User is accessing the secure application for the first time, so the derived class 
                                ' must capture user credentials by prompting them for it and authenticate them.
                                If Not CaptureCredentials() Then Exit Sub ' Implementer wants to abort.
                            End If
                    End Select
                End If

                If m_user IsNot Nothing Then
                    Dim beforeAuthenticateEventData As New CancelEventArgs()
                    RaiseEvent BeforeAuthenticate(Me, beforeAuthenticateEventData)
                    If beforeAuthenticateEventData.Cancel Then Exit Sub

                    If UserHasApplicationAccess() Then
                        ' User has been authenticated successfully and has access to the specified application.
                        Dim accessGrantedEventData As New CancelEventArgs()
                        RaiseEvent AccessGranted(Me, accessGrantedEventData)
                        If accessGrantedEventData.Cancel Then Exit Sub

                        ProcessControls()
                        HandleAccessGranted()
                    Else
                        ' User could not be autheticated or doesn't have access to the specified application.
                        ' Most likely user authentication will never fail because if the user is external, the
                        ' login page will verify the user's password before this process kicks in.
                        Dim accessDeniedEventData As New CancelEventArgs()
                        RaiseEvent AccessDenied(Me, accessDeniedEventData)
                        If accessDeniedEventData.Cancel Then Exit Sub

                        HandleAccessDenied()
                    End If

                    RaiseEvent AfterAuthenticate(Me, EventArgs.Empty)
                End If

                RaiseEvent AfterLogin(Me, EventArgs.Empty)
            Else
                Throw New InvalidOperationException("ApplicationName must be set in order to login the user.")
            End If

        End Sub

        ''' <summary>
        ''' Logs out the logged in user.
        ''' </summary>
        Public MustOverride Sub LogoutUser()

#Region " Interface Implementation "

#Region " IExtenderProvider "

        Public Function CanExtend(ByVal extendee As Object) As Boolean Implements System.ComponentModel.IExtenderProvider.CanExtend

            Return (TypeOf extendee Is System.Web.UI.Control OrElse TypeOf extendee Is System.Windows.Forms.Control)

        End Function

        Public Function GetValidRoles(ByVal extendee As Object) As String

            Return GetProperties(extendee).ValidRoles

        End Function

        Public Sub SetValidRoles(ByVal extendee As Object, ByVal value As String)

            Dim extendedProperties As ControlProperties = GetProperties(extendee)
            extendedProperties.ValidRoles = value

            ProcessControl(extendee, extendedProperties)

        End Sub

        Public Function GetValidRoleAction(ByVal extendee As Object) As ValidRoleAction

            Return GetProperties(extendee).ValidRoleAction

        End Function

        Public Sub SetValidRoleAction(ByVal extendee As Object, ByVal value As ValidRoleAction)

            Dim extendedProperties As ControlProperties = GetProperties(extendee)
            extendedProperties.ValidRoleAction = value

            ProcessControl(extendee, extendedProperties)

        End Sub

        Private Function GetProperties(ByVal extendee As Object) As ControlProperties

            Dim properties As ControlProperties = DirectCast(m_extendeeControls(extendee), ControlProperties)
            If properties Is Nothing Then
                properties = New ControlProperties()
                m_extendeeControls.Add(extendee, properties)
            End If

            Return properties

        End Function

        Private Class ControlProperties

            Public ValidRoles As String
            Public ValidRoleAction As ValidRoleAction
            Public ActionTaken As Boolean

        End Class

#End Region

#Region " ISupportInitialize "

        ''' <summary>
        ''' To be called before the control is initialized.
        ''' </summary>
        Public Sub BeginInit() Implements System.ComponentModel.ISupportInitialize.BeginInit

            ' Nothing needs to be done when the component begins initializing.

        End Sub

        ''' <summary>
        ''' To be called after the control is initialized.
        ''' </summary>
        ''' <remarks>Loads property values from the config file and performs the login operation.</remarks>
        Public Sub EndInit() Implements System.ComponentModel.ISupportInitialize.EndInit

            If LicenseManager.UsageMode = LicenseUsageMode.Runtime Then
                LoadSettings()
                LoginUser()
            End If

        End Sub

#End Region

#Region " IPersistSettings "

        ''' <summary>
        ''' Gets or sets a boolean value indicating whether or not property values are to be saved in the config file.
        ''' </summary>
        ''' <value></value>
        ''' <returns>True if property values are to be saved in the config file; otherwise False.</returns>
        <Category("Persistance")> _
        Public Property PersistSettings() As Boolean Implements IPersistSettings.PersistSettings
            Get
                Return m_persistSettings
            End Get
            Set(ByVal value As Boolean)
                m_persistSettings = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the category name under which the property values are to be saved in the config file if 
        ''' they are to be saved in the config file.
        ''' </summary>
        ''' <value></value>
        ''' <returns>Category name under which the property values are to be saved in the config file.</returns>
        <Category("Persistance")> _
        Public Property SettingsCategoryName() As String Implements IPersistSettings.SettingsCategoryName
            Get
                Return m_settingsCategoryName
            End Get
            Set(ByVal value As String)
                If Not String.IsNullOrEmpty(value) Then
                    m_settingsCategoryName = value
                Else
                    Throw New ArgumentNullException("SettingsCategoryName")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Loads property values from the config file.
        ''' </summary>
        Public Overridable Sub LoadSettings() Implements IPersistSettings.LoadSettings

            If m_persistSettings Then
                Try
                    With TVA.Configuration.Common.CategorizedSettings(m_settingsCategoryName)
                        Server = .Item("Server", True).GetTypedValue(m_server)
                        ApplicationName = .Item("ApplicationName", True).GetTypedValue(m_applicationName)
                        AuthenticationMode = .Item("AuthenticationMode", True).GetTypedValue(m_authenticationMode)
                    End With
                Catch ex As Exception
                    ' Absorb any encountered exception.
                End Try
            End If

        End Sub

        ''' <summary>
        ''' Saves property values to the config file.
        ''' </summary>
        Public Overridable Sub SaveSettings() Implements IPersistSettings.SaveSettings

            If m_persistSettings Then
                Try
                    With TVA.Configuration.Common.CategorizedSettings(m_settingsCategoryName)
                        .Clear()
                        With .Item("Server", True)
                            .Value = m_server.ToString()
                            .Description = "Security database server (Development; Acceptance; Production) against which users are authenticated."
                        End With
                        With .Item("ApplicationName", True)
                            .Value = m_applicationName
                            .Description = "Name of the application as defined in the security database that is being secured."
                        End With
                        With .Item("AuthenticationMode", True)
                            .Value = m_authenticationMode.ToString()
                            .Description = "Mode of authentication (AD; RSA) to be used for authenticating users of the application."
                        End With
                    End With
                    TVA.Configuration.Common.SaveSettings()
                Catch ex As Exception
                    ' Absorb any encountered exception.
                End Try
            End If

        End Sub

#End Region

#End Region

#End Region

#Region " Code Scope: Protected Code "

        ''' <summary>
        ''' Shows a login prompt where user can enter his/her credentials.
        ''' </summary>
        ''' <remarks></remarks>
        Protected MustOverride Sub ShowLoginPrompt()

        ''' <summary>
        ''' Performs any necessary actions that must be performed upon unsuccessful login.
        ''' </summary>
        Protected MustOverride Sub HandleAccessDenied()

        ''' <summary>
        ''' Performs any necessary actions that must be performed upon successful login.
        ''' </summary>
        Protected MustOverride Sub HandleAccessGranted()

        ''' <summary>
        ''' Gets the name that the user provided on the login screen.
        ''' </summary>
        ''' <returns></returns>
        Protected MustOverride Function GetUsername() As String

        ''' <summary>
        ''' Gets the password that the user provided on the login screen.
        ''' </summary>
        ''' <returns></returns>
        Protected MustOverride Function GetPassword() As String

#End Region

#Region " Code Scope: Private Code "

        Private Sub ProcessControls()

            For Each extendee As Object In m_extendeeControls.Keys
                ProcessControl(extendee, DirectCast(m_extendeeControls(extendee), ControlProperties))
            Next

        End Sub

        Private Sub ProcessControl(ByVal extendee As Object, ByVal extendedProperties As ControlProperties)

            If Not extendedProperties.ActionTaken AndAlso _
                    extendedProperties.ValidRoleAction <> ValidRoleAction.None AndAlso _
                    extendedProperties.ValidRoles IsNot Nothing Then
                Dim controlProperty As PropertyInfo = _
                    extendee.GetType().GetProperty(extendedProperties.ValidRoleAction.ToString())

                If m_user IsNot Nothing AndAlso controlProperty IsNot Nothing Then
                    ' User has been logged in and the control property exists.
                    controlProperty.SetValue(extendee, False, Nothing)   ' By default we'll set the property to False.

                    For Each role As String In extendedProperties.ValidRoles.Replace(" ", "").Replace(",", ";").Split(";"c)
                        If m_user.FindRole(role, m_applicationName) IsNot Nothing Then
                            ' We'll set the property to True if the current user belongs either one of the valid roles.
                            controlProperty.SetValue(extendee, True, Nothing)
                            Exit For
                        End If
                    Next
                End If
            End If

        End Sub

        Private Sub InitializeUser(ByVal username As String, ByVal password As String, ByVal authenticate As Boolean)

            Try
                RaiseEvent BeforeInitializeData(Me, EventArgs.Empty)

                m_user = New User(username, password, m_applicationName, _
                                  m_server, m_authenticationMode, authenticate)

                RaiseEvent AfterInitializeData(Me, EventArgs.Empty)

                m_user.LogAccess(Not UserHasApplicationAccess())    ' Log access attempt to security database.
            Catch ex As SqlException
                ' We'll notifying about the excountered SQL exception by rasing an event.
                RaiseEvent DatabaseException(Me, New GenericEventArgs(Of Exception)(ex))
            Catch ex As Exception
                ' We'll bubble-up all other encountered exceptions.
                Throw
            End Try

        End Sub

        Private Function CaptureCredentials() As Boolean

            Dim beforeLoginPromptEventData As New CancelEventArgs()
            RaiseEvent BeforeLoginPrompt(Me, beforeLoginPromptEventData)
            If beforeLoginPromptEventData.Cancel Then Return False

            ShowLoginPrompt()   ' Prompt user for credentials.

            RaiseEvent AfterLoginPrompt(Me, EventArgs.Empty)

            Return True         ' Indicate that credentials are/will be captured.

        End Function

#End Region

    End Class

End Namespace