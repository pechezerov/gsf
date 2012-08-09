﻿//******************************************************************************************************
//  ErrorLogViewModel.cs - Gbtc
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
//  3/26/2012 - prasanthgs
//       Generated original version of source code.
//  04/12/2012 - prasanthgs
//       Reworked as per the comments of codeplex reviewers.
//       Code Optimized.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using TimeSeriesFramework.UI.Commands;
using TimeSeriesFramework.UI.DataModels;
using TimeSeriesFramework.UI.Modal;

namespace TimeSeriesFramework.UI.ViewModels
{
    internal class ErrorLogViewModel : PagedViewModelBase<ErrorLog, int>
    {
        #region [ Members ]

        private ErrorMonitor m_exMonitor;
        private Dispatcher m_dispatcher;
        private RelayCommand m_showCommand;
        private string m_currentSortMemberPath;
        private ListSortDirection m_currentSortDirection;

        #endregion

        #region [ Constructor]
        
        /// <summary>
        /// Creates a new instance of <see cref="ErrorLogViewModel"/> class.
        /// </summary>
        /// <param name="itemsPerPage"> No of Maximum items in each page.</param>
        /// <param name="autoSave">Default value true.</param>
        public ErrorLogViewModel(int itemsPerPage, bool autoSave = true)
            : base(itemsPerPage, autoSave)
        {
            m_dispatcher = Dispatcher.CurrentDispatcher;
        }
        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the <see cref="ErrorMonitor"/> used to receive
        /// updates about recent Errors.
        /// </summary>
        public ErrorMonitor Monitor
        {
            get
            {
                return m_exMonitor;
            }
            set
            {
                if ((object)m_exMonitor != null)
                    m_exMonitor.UpdatedErrors -= Monitor_UpdatedErrors;

                m_exMonitor = value;

                if ((object)m_exMonitor != null)
                {
                    LoadErrors();
                    m_exMonitor.UpdatedErrors += Monitor_UpdatedErrors;
                }
            }
        }

        void Monitor_UpdatedErrors(object sender, EventArgs e)
        {
            m_dispatcher.Invoke(new Action(LoadErrors));
        }

        /// <summary>
        /// Gets Show command.
        /// </summary>
        public ICommand ShowCommand
        {
            get
            {
                if (m_showCommand == null)
                    m_showCommand = new RelayCommand(ShowErrorLog);

                return m_showCommand;
            }
        }

        /// <summary>
        /// Checks for new item in Error list.
        /// Since this is an error viewer, no new records can be added.
        /// Always returns false.
        /// </summary>
        public override bool IsNewRecord
        {
            get { return false; }
        }

        #endregion

        #region [ Methods ]
        /// <summary>
        /// Update the DataGrid with latest error list.
        /// Also sorts the data if sort is applied before refresh.
        /// </summary>
        private void LoadErrors()
        {
            int CurIdx = default(int);

            if ((object)m_exMonitor != null)
            {
                CurIdx = GetCurrentItemKey();
                ItemsSource = m_exMonitor.GetRecentErrors();
                
                //Sort and select current item.
                Sort(CurIdx);
            }
        }

        /// <summary>
        /// Sorts model data.
        /// </summary>
        /// <param name="sortMemberPath">Member path for sorting.</param>
        /// <param name="sortDirection">Ascending or descending.</param>
        public override void SortData(string sortMemberPath, ListSortDirection sortDirection)
        {
            m_currentSortMemberPath = sortMemberPath;
            m_currentSortDirection = sortDirection;
            Sort(GetCurrentItemKey());
        }

        /// <summary>
        /// Initiates parent sortmethod.
        /// Set current selected item after sort based on display index.
        /// </summary>
        /// <param name="currentItemKey">Index of selected item before sort</param>
        private void Sort(int currentItemKey)
        {
            List<ErrorLog> itemsSource;
            ErrorLog newItem = ItemsSource.SingleOrDefault(error => error.ID == currentItemKey);

            if ((object)m_currentSortMemberPath == null)
                return;

            if (m_currentSortDirection == ListSortDirection.Ascending)
            {
                itemsSource = ItemsSource
                    .OrderBy(item => item.GetType().GetProperty(m_currentSortMemberPath).GetValue(item, null))
                    .ToList();
            }
            else
            {
                itemsSource = ItemsSource
                    .OrderByDescending(item => item.GetType().GetProperty(m_currentSortMemberPath).GetValue(item, null))
                    .ToList();
            }

            ItemsSource = new ObservableCollection<ErrorLog>(itemsSource);

            if ((object)newItem != null)
            {
                CurrentItem = newItem;
            }
            else
            {
                Clear();
                CurrentSelectedIndex = -1;
            }
        }

        /// <summary>
        /// Show brief error log details.
        /// </summary>
        private void ShowErrorLog()
        {
            ErrorDetailDisplay logDisplay = new ErrorDetailDisplay(CurrentItem.Detail);
            logDisplay.ShowDialog();
        }

        /// <summary>
        /// Overriden method
        /// </summary>
        public override void Load()
        {
        }

        /// <summary>
        /// Overriden method
        /// Use current item index as key for setting 
        /// error list selected item after refresh.
        /// </summary>
        /// <returns>Returns current item index.</returns>
        public override int GetCurrentItemKey()
        {
            return CurrentItem.ID;
        }

        /// <summary>
        /// Overriden method
        /// Get current item index.
        /// </summary>
        /// <returns>Returns String representation of current index.</returns>
        public override string GetCurrentItemName()
        {
            return CurrentItem.ID.ToString();
        }

        #endregion
    }
}
