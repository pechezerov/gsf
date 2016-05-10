﻿//******************************************************************************************************
//  RazorView.cs - Gbtc
//
//  Copyright © 2016, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may not use this
//  file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  01/13/2016 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using GSF.Collections;
using GSF.Data;
using RazorEngine.Templating;

// ReSharper disable StaticMemberInGenericType
namespace GSF.Web.Model
{
    /// <summary>
    /// Defines a view class for data context based Razor template implementations.
    /// </summary>
    public class RazorView
    {
        #region [ Members ]

        // Fields
        private readonly IRazorEngine m_razorEngine;

        private readonly DynamicViewBag m_viewBag = new DynamicViewBag();
        private Dictionary<string, string> m_parameters;

        #endregion

        #region [ Constructors ]
        /// <summary>
        /// Creates a new <see cref="RazorView"/>.
        /// </summary>
        /// <param name="razorEngine"><see cref="IRazorEngine"/> instance to use.</param>
        /// <param name="templateName">Name of template file, typically a .cshtml or .vbhtml file.</param>
        /// <param name="exceptionHandler">Delegate to handle exceptions, if any.</param>
        public RazorView(IRazorEngine razorEngine, string templateName, Action<Exception> exceptionHandler = null) : this(razorEngine, templateName, null, null, null, exceptionHandler)
        {
        }

        /// <summary>
        /// Creates a new <see cref="RazorView"/>.
        /// </summary>
        /// <param name="razorEngine"><see cref="IRazorEngine"/> instance to use.</param>
        /// <param name="templateName">Name of template file, typically a .cshtml or .vbhtml file.</param>
        /// <param name="model">Reference to model to use when rendering template.</param>
        /// <param name="modelType">Type of <paramref name="model"/>.</param>
        /// <param name="connection"><see cref="AdoDataConnection"/> to use, if any.</param>
        /// <param name="exceptionHandler">Delegate to handle exceptions, if any.</param>
        public RazorView(IRazorEngine razorEngine, string templateName, object model = null, Type modelType = null, AdoDataConnection connection = null, Action<Exception> exceptionHandler = null)
        {
            m_razorEngine = razorEngine;
            TemplateName = templateName;
            Model = model;
            ModelType = modelType;
            Connection = connection;
            ExceptionHandler = exceptionHandler;
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets name of template file.
        /// </summary>
        public string TemplateName
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets reference to model to use when rendering template.
        /// </summary>
        public object Model
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets type of <see cref="Model"/>.
        /// </summary>
        public Type ModelType
        {
            get; set;
        }

        /// <summary>
        /// Gets reference to view bag used when rendering template.
        /// </summary>
        public dynamic ViewBag => m_viewBag;

        /// <summary>
        /// Gets query string parameter specified by <paramref name="key"/>.
        /// </summary>
        /// <param name="key">Name of query string parameter to retrieve.</param>
        /// <returns>Query string parameter specified by <paramref name="key"/>.</returns>
        public string this[string key] => Parameters[key];

        /// <summary>
        /// Gets a dictionary of query string parameters passed to rendered view.
        /// </summary>
        public Dictionary<string, string> Parameters
        {
            get
            {
                if ((object)m_parameters == null)
                {
                    HttpRequestMessage request = ViewBag.Request;
                    m_parameters = HttpUtility.ParseQueryString(request.RequestUri.Query).ToDictionary();
                }

                return m_parameters;
            }
        }

        /// <summary>
        /// Gets reference to <see cref="IRazorEngine"/> used by this <see cref="RazorView"/>.
        /// </summary>
        public IRazorEngine RazorEngine => m_razorEngine;

        /// <summary>
        /// Gets or sets data connection to provide to <see cref="DataContext"/>, if any.
        /// </summary>
        public AdoDataConnection Connection
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets delegate used to handle exceptions.
        /// </summary>
        public Action<Exception> ExceptionHandler
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets <see cref="IRazorEngine"/> to use for the <see cref="DataContext"/> provided to the view; defaults to <c>null</c>.
        /// </summary>
        /// <remarks>
        /// This should typically not be changed. When value is <c>null</c>, the data context will use the default
        /// <see cref="RazorEngine{TLanguage}"/> instance for <see cref="CSharpEmbeddedResource"/> which is used
        /// to generate template based HTML input fields for a view. The default HTML input templates are defined
        /// as embedded resources in GSF.Web.
        /// </remarks>
        public IRazorEngine DataContextRazorEngine
        {
            get;
            set;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Compiles and executes view template.
        /// </summary>
        /// <returns>Rendered result.</returns>
        public string Execute()
        {
            using (DataContext dataContext = new DataContext(Connection, razorEngine: DataContextRazorEngine, exceptionHandler: ExceptionHandler))
            {
                m_viewBag.AddValue("DataContext", dataContext);
                return m_razorEngine.RunCompile(TemplateName, ModelType, Model, m_viewBag);
            }
        }

        /// <summary>
        /// Compiles and executes view template for specified request message and post data.
        /// </summary>
        /// <returns>Rendered result.</returns>
        public string Execute(HttpRequestMessage requestMessage, dynamic postData)
        {
            using (DataContext dataContext = new DataContext(Connection, razorEngine: DataContextRazorEngine, exceptionHandler: ExceptionHandler))
            {
                m_viewBag.AddValue("DataContext", dataContext);
                m_viewBag.AddValue("Request", requestMessage);

                if ((object)postData == null)
                {
                    m_viewBag.AddValue("IsPost", false);
                }
                else
                {
                    m_viewBag.AddValue("IsPost", true);
                    m_viewBag.AddValue("PostData", postData);
                }

                return m_razorEngine.RunCompile(TemplateName, ModelType, Model, m_viewBag);
            }
        }

        /// <summary>
        /// Asynchronously compiles and executes view template for specified request message and post data.
        /// </summary>
        /// <returns>Task that will provide rendered result.</returns>
        public Task ExecuteAsync(HttpRequestMessage requestMessage, dynamic postData)
        {
            return Task.Run(() => Execute(requestMessage, postData));
        }

        #endregion
    }
}