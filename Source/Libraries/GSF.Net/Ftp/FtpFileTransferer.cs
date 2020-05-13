//******************************************************************************************************
//  FtpFileTransferer.cs - Gbtc
//
//  Copyright � 2012, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  05/22/2003 - J. Ritchie Carroll
//       Generated original version of source code.
//  09/14/2009 - Stephen C. Wills
//       Added new header and license agreement.
//  12/09/2009 - Pinal C. Patel
//       Modified StartTransfer() to open local file in read-only mode for uploads.
//  12/17/2012 - Starlynn Danyelle Gilliam
//       Modified Header.
//
//******************************************************************************************************

#region [ Contributor License Agreements ]

//*******************************************************************************************************
//
//   Code based on the following project:
//        http://www.codeproject.com/KB/IP/net_ftp_upload.aspx
//  
//   Copyright Alex Kwok & Uwe Keim 
//
//   The Code Project Open License (CPOL):
//        http://www.codeproject.com/info/cpol10.aspx
//
//*******************************************************************************************************

#endregion

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using GSF.IO;

namespace GSF.Net.Ftp
{
    #region [ Enumerations ]

    /// <summary>
    /// FTP file transfer direction enumeration.
    /// </summary>
    public enum TransferDirection
    {
        /// <summary>
        /// FTP transfer direction set to upload.
        /// </summary>
        Upload,
        /// <summary>
        /// FTP transfer direction set to download.
        /// </summary>
        Download
    }

    #endregion

    // Internal FTP file transfer class
    internal class FtpFileTransferer
    {
        #region [ Members ]

        // Delegates
        private delegate void FileCommandDelegate(string remoteFileName);
        private delegate void StreamCopyDelegate(Stream remote, Stream local);

        // Fields
        private readonly StreamCopyDelegate m_streamCopyRoutine;
        private readonly FileCommandDelegate m_ftpFileCommandRoutine;
        private readonly FtpSessionConnected m_session;

        #endregion

        #region [ Constructors ]

        internal FtpFileTransferer(FtpDirectory transferStarter, string localFile, string remoteFile, long totalBytes, TransferDirection dir)
        {
            TransferDirection = dir;
            m_session = transferStarter.Session;
            LocalFileName = localFile;
            RemoteFileName = remoteFile;
            TotalBytes = totalBytes;

            if (dir == TransferDirection.Upload)
            {
                m_streamCopyRoutine = LocalToRemote;
                m_ftpFileCommandRoutine = m_session.ControlChannel.STOR;
            }
            else
            {
                m_streamCopyRoutine = RemoteToLocal;
                m_ftpFileCommandRoutine = m_session.ControlChannel.RETR;
            }
        }

        #endregion

        #region [ Properties ]

        public string LocalFileName { get; }

        public string RemoteFileName { get; }

        public long TotalBytes { get; }

        public long TotalBytesTransfered { get; private set; }

        public TransferDirection TransferDirection { get; }

        public FtpAsyncResult TransferResult { get; private set; }

        public int TransferedPercentage { get; private set; }

        #endregion

        #region [ Methods ]

        private void TransferThreadProc()
        {
            try
            {
                StartTransfer();
                TransferResult = new FtpAsyncResult("Success.", FtpAsyncResult.Complete);
                m_session.Host.OnFileTransferNotification(TransferResult);
            }
            catch (FtpExceptionBase e)
            {
                TransferResult = new FtpAsyncResult($"Transfer fail: {e.Message}", FtpAsyncResult.Fail);
                m_session.Host.OnFileTransferNotification(TransferResult);
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        internal void StartTransfer()
        {
            FileStream localStream = null;
            FtpDataStream remoteStream = null;

            try
            {
                // Files just created may still have a file lock, we'll wait a few seconds for read access if needed...
                if (TransferDirection == TransferDirection.Upload)
                    FilePath.WaitForReadLock(LocalFileName, m_session.Host.WaitLockTimeout);

                m_session.Host.OnBeginFileTransfer(LocalFileName, RemoteFileName, TransferDirection);

                remoteStream = m_session.ControlChannel.GetDataStream(TransferDirection);
                m_ftpFileCommandRoutine(RemoteFileName);

                if (TransferDirection == TransferDirection.Download)
                    localStream = new FileStream(LocalFileName, FileMode.Create);
                else
                    localStream = new FileStream(LocalFileName, FileMode.Open, FileAccess.Read);

                m_streamCopyRoutine(remoteStream, localStream);

                // Dispose remote stream before testing file transfer result to ensure
                // we have received the server's response to the file transfer command
                remoteStream.Dispose();
                TestTransferResult();

                m_session.Host.OnEndFileTransfer(LocalFileName, RemoteFileName, TransferDirection);
            }
            catch
            {
                m_session.Host.OnEndFileTransfer(LocalFileName, RemoteFileName, TransferDirection);
                throw;
            }
            finally
            {
                // Need to make sure we end data transfer on the session, which would
                // normally happen automatically when the remote stream is closed
                if (remoteStream != null)
                    remoteStream.Dispose();
                else
                    m_session.EndDataTransfer();

                localStream?.Dispose();
            }
        }

        internal void StartAsyncTransfer()
        {
            Thread thread = new Thread(TransferThreadProc) { Name = $"Transfer file thread: {RemoteFileName}" };
            thread.Start();
        }

        private void TestTransferResult()
        {
            int responseCode = m_session.ControlChannel.LastResponse.Code;

            switch (responseCode)
            {
                case FtpResponse.ClosingDataChannel:
                case FtpResponse.RequestFileActionComplete:
                    return;
                default:
                    throw new FtpDataTransferException("Failed to transfer file.", m_session.ControlChannel.LastResponse);
            }
        }

        private void RemoteToLocal(Stream remote, Stream local)
        {
            StreamCopy(local, remote);
        }

        private void LocalToRemote(Stream remote, Stream local)
        {
            StreamCopy(remote, local);
        }

        private void StreamCopy(Stream dest, Stream source)
        {
            byte[] buffer = new byte[4 * 1024 + 1];
            ProcessProgress<long> progress = new ProcessProgress<long>($"FTP {TransferDirection} File Transfer", $"Transferring file \"{RemoteFileName}\"...", TotalBytes, 0);

            long onePercentage = TotalBytes / 100;
            long bytesReadFromLastProgressEvent = 0;
            int byteRead = source.Read(buffer, 0, 4 * 1024);

            while (byteRead != 0)
            {
                TotalBytesTransfered += byteRead;
                bytesReadFromLastProgressEvent += byteRead;

                if (bytesReadFromLastProgressEvent > onePercentage)
                {
                    TransferedPercentage = (int)(TotalBytesTransfered / (float)TotalBytes * 100);
                    progress.Complete = TotalBytesTransfered;
                    m_session.Host.OnFileTransferProgress(progress, TransferDirection);
                    bytesReadFromLastProgressEvent = 0;
                }

                dest.Write(buffer, 0, byteRead);
                byteRead = source.Read(buffer, 0, 4 * 1024);
            }
        }

        #endregion
    }
}