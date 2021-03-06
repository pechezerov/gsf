﻿//******************************************************************************************************
//  TsfDataAdapter.cs - Gbtc
//
//  Copyright © 2012, Grid Protection Alliance.  All Rights Reserved.
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
//  10/05/2012 - Adam Crain
//       Generated original version of source code.
//  12/13/2012 - Starlynn Danyelle Gilliam
//       Modified Header.
//  03/2/2014 - Adam Crain
//       Migrated to new ISOEHandler interface
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Automatak.DNP3.Interface;
using GSF.TimeSeries;

namespace DNP3Adapters
{
    /// <summary>
    /// This is the data adapter that converts data from the dnp3 world to the TSF
    /// </summary>
    class TimeSeriesSOEHandler : ISOEHandler
    {
        public delegate void OnNewMeasurements(ICollection<IMeasurement> measurements);
        public event OnNewMeasurements NewMeasurements;

        private readonly MeasurementLookup m_lookup;
        private readonly List<IMeasurement> m_Measurements = new List<IMeasurement>();

        public TimeSeriesSOEHandler(MeasurementLookup lookup)
        {
            TimestampDifferentiation = TimeSpan.FromMilliseconds(1.0D);
            m_lookup = lookup;
        }

        public TimeSpan TimestampDifferentiation { get; set; }

        private void PutTimestamps<T>(IEnumerable<IndexedValue<T>> values) where T : MeasurementBase
        {
            DateTime now = DateTime.UtcNow;

            foreach (IGrouping<ushort, IndexedValue<T>> grouping in values.GroupBy(indexedValue => indexedValue.Index))
            {
                int count = grouping.Count();

                foreach (IndexedValue<T> indexedValue in grouping)
                {
                    TimeSpan offset = TimeSpan.FromTicks(count * TimestampDifferentiation.Ticks);
                    indexedValue.Value.Timestamp = now - offset;
                    count--;
                }
            }
        }

        void ISOEHandler.Start()
        {
            m_Measurements.Clear();
        }

        void ISOEHandler.End()
        {
            if (m_Measurements.Count > 0 && NewMeasurements != null)
            {
                NewMeasurements(m_Measurements);
            }
        }

        void ISOEHandler.Process(HeaderInfo info, IEnumerable<IndexedValue<Binary>> values)
        {
            PutTimestamps(values);

            foreach (var indexedValue in values)
                m_lookup.Lookup(indexedValue.Value, indexedValue.Index, m_Measurements.Add);
        }

        void ISOEHandler.Process(HeaderInfo info, IEnumerable<IndexedValue<DoubleBitBinary>> values)
        {
            PutTimestamps(values);

            foreach (var indexedValue in values)
                m_lookup.Lookup(indexedValue.Value, indexedValue.Index, m_Measurements.Add);
        }

        void ISOEHandler.Process(HeaderInfo info, IEnumerable<IndexedValue<Analog>> values)
        {
            PutTimestamps(values);

            foreach (var indexedValue in values)
                m_lookup.Lookup(indexedValue.Value, indexedValue.Index, m_Measurements.Add);
        }

        void ISOEHandler.Process(HeaderInfo info, IEnumerable<IndexedValue<Counter>> values)
        {
            PutTimestamps(values);

            foreach (var indexedValue in values)
                m_lookup.Lookup(indexedValue.Value, indexedValue.Index, m_Measurements.Add);
        }

        void ISOEHandler.Process(HeaderInfo info, IEnumerable<IndexedValue<FrozenCounter>> values)
        {
            PutTimestamps(values);

            foreach (var indexedValue in values)
                m_lookup.Lookup(indexedValue.Value, indexedValue.Index, m_Measurements.Add);
        }

        void ISOEHandler.Process(HeaderInfo info, IEnumerable<IndexedValue<BinaryOutputStatus>> values)
        {
            PutTimestamps(values);

            foreach (var indexedValue in values)
                m_lookup.Lookup(indexedValue.Value, indexedValue.Index, m_Measurements.Add);
        }

        void ISOEHandler.Process(HeaderInfo info, IEnumerable<IndexedValue<AnalogOutputStatus>> values)
        {
            PutTimestamps(values);

            foreach (var indexedValue in values)
                m_lookup.Lookup(indexedValue.Value, indexedValue.Index, m_Measurements.Add);
        }

        void ISOEHandler.Process(HeaderInfo info, IEnumerable<IndexedValue<OctetString>> values)
        {
        }

        void ISOEHandler.Process(HeaderInfo info, IEnumerable<IndexedValue<TimeAndInterval>> values)
        {
        }

        void ISOEHandler.Process(HeaderInfo info, IEnumerable<IndexedValue<BinaryCommandEvent>> values)
        {
        }

        void ISOEHandler.Process(HeaderInfo info, IEnumerable<IndexedValue<AnalogCommandEvent>> values)
        {
        }

        void ISOEHandler.Process(HeaderInfo info, IEnumerable<IndexedValue<SecurityStat>> values)
        {
        }
    }
}
