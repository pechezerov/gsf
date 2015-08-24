//*******************************************************************************************************
//  FtpSessionDisconnected.cs
//  Copyright � 2008 - TVA, all rights reserved - Gbtc
//
//  Build Environment: C#, Visual Studio 2008
//  Primary Developer: James R. Carroll
//      Office: PSO TRAN & REL, CHATTANOOGA - MR 2W-C
//       Phone: 423/751-2827
//       Email: jrcarrol@tva.gov
//
//  Code Modification History:
//  -----------------------------------------------------------------------------------------------------
//  05/22/2003 - James R. Carroll
//       Generated original version of source code.
//
//*******************************************************************************************************

using System;

namespace TVA.Net.Ftp
{
    internal class FtpSessionDisconnected : IFtpSessionState
    {
        #region [ Members ]

        // Fields
        private FtpClient m_host;
        private string m_server;
        private int m_port;
        private bool m_caseInsensitive;
        private bool m_disposed;

        #endregion

        #region [ Constructors ]

        internal FtpSessionDisconnected(FtpClient h, bool caseInsensitive)
        {
            m_port = 21;
            m_host = h;
            m_caseInsensitive = caseInsensitive;
        }

        /// <summary>
        /// Releases the unmanaged resources before the <see cref="FtpSessionDisconnected"/> object is reclaimed by <see cref="GC"/>.
        /// </summary>
        ~FtpSessionDisconnected()
        {
            Dispose(false);
        }

        #endregion

        #region [ Properties ]

        public string Server
        {
            get
            {
                return m_server;
            }
            set
            {
                m_server = value;
            }
        }

        public int Port
        {
            get
            {
                return m_port;
            }
            set
            {
                m_port = value;
            }
        }

        public FtpDirectory CurrentDirectory
        {
            get
            {
                throw new InvalidOperationException();
            }
            set
            {
                throw new InvalidOperationException();
            }
        }

        public FtpControlChannel ControlChannel
        {
            get
            {
                throw new InvalidOperationException();
            }
        }

        public bool IsBusy
        {
            get
            {
                throw new InvalidOperationException();
            }
        }

        public FtpDirectory RootDirectory
        {
            get
            {
                throw new InvalidOperationException();
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Releases all the resources used by the <see cref="FtpSessionDisconnected"/> object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="FtpSessionDisconnected"/> object and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                try
                {
                    if (disposing)
                        m_host = null;
                }
                finally
                {
                    m_disposed = true;  // Prevent duplicate dispose.
                }
            }
        }

        public void Connect(string userName, string password)
        {
            FtpControlChannel ctrl = new FtpControlChannel(m_host);

            ctrl.Server = m_server;
            ctrl.Port = m_port;
            ctrl.Connect();

            try
            {
                ctrl.Command("USER " + userName);

                if (ctrl.LastResponse.Code == FtpResponse.UserAcceptedWaitingPass)
                    ctrl.Command("PASS " + password);

                if (ctrl.LastResponse.Code != FtpResponse.UserLoggedIn)
                    throw new FtpAuthenticationException("Failed to login.", ctrl.LastResponse);

                m_host.State = new FtpSessionConnected(m_host, ctrl, m_caseInsensitive);
                ((FtpSessionConnected)m_host.State).InitRootDirectory();
            }
            catch
            {
                ctrl.Close();
                throw;
            }
        }


        public void AbortTransfer()
        {
            throw new InvalidOperationException();
        }

        public void Close()
        {
            // Nothing to do - don't want to throw an error...
        }

        #endregion
    }
}