﻿//******************************************************************************************************
//  ServiceClient.designer.cs - Gbtc
//
//  Copyright © 2010, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  08/20/2010 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

namespace TimeSeriesFramework
{
    public partial class ServiceClientBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_clientHelper.Disconnect();
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.m_clientHelper = new TVA.ServiceProcess.ClientHelper(this.components);
            this.m_remotingClient = new TVA.Communication.TcpClient(this.components);
            this.m_errorLogger = new TVA.ErrorManagement.ErrorLogger(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.m_clientHelper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_remotingClient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_errorLogger)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_errorLogger.ErrorLog)).BeginInit();
            // 
            // m_clientHelper
            // 
            this.m_clientHelper.PersistSettings = true;
            this.m_clientHelper.RemotingClient = this.m_remotingClient;
            // 
            // m_remotingClient
            // 
            this.m_remotingClient.ConnectionString = "Server=localhost:8500";
            this.m_remotingClient.PayloadAware = true;
            this.m_remotingClient.PersistSettings = true;
            this.m_remotingClient.SettingsCategory = "RemotingClient";
            // 
            // m_errorLogger
            // 
            // 
            // 
            // 
            this.m_errorLogger.ErrorLog.FileName = "ServiceClient.ErrorLog.txt";
            this.m_errorLogger.LogToEventLog = false;
            this.m_errorLogger.LogToUI = true;
            this.m_errorLogger.PersistSettings = true;
            ((System.ComponentModel.ISupportInitialize)(this.m_clientHelper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_remotingClient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_errorLogger.ErrorLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_errorLogger)).EndInit();

        }

        #endregion

        private TVA.ServiceProcess.ClientHelper m_clientHelper;
        private TVA.Communication.TcpClient m_remotingClient;
        private TVA.ErrorManagement.ErrorLogger m_errorLogger;


    }
}
