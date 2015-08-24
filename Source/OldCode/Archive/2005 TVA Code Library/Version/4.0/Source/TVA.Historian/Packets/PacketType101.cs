﻿//*******************************************************************************************************
//  PacketType101.cs
//  Copyright © 2009 - TVA, all rights reserved - Gbtc
//
//  Build Environment: C#, Visual Studio 2008
//  Primary Developer: Pinal C. Patel
//      Office: INFO SVCS APP DEV, CHATTANOOGA - MR BK-C
//       Phone: 423/751-3024
//       Email: pcpatel@tva.gov
//
//  Code Modification History:
//  -----------------------------------------------------------------------------------------------------
//  06/04/2009 - Pinal C. Patel
//       Generated original version of source code.
//
//*******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Text;
using TVA.Historian.Files;
using TVA.Measurements;

namespace TVA.Historian.Packets
{
    /// <summary>
    /// Represents a packet that can be used to send multiple time series data points to a historian for archival.
    /// </summary>
    /// <seealso cref="PacketType101Data"/>
    public class PacketType101 : PacketBase
    {
        // **************************************************************************************************
        // *                                        Binary Structure                                        *
        // **************************************************************************************************
        // * # Of Bytes Byte Index Data Type  Property Name                                                 *
        // * ---------- ---------- ---------- --------------------------------------------------------------*
        // * 2          0-1        Int16      TypeID (packet identifier)                                    *
        // * 4          2-5        Int32      Data.Count                                                    *
        // * 14         6-19       Byte[]     Data[0]                                                       *
        // * 14         n1-n2      Byte[]     Data[Data.Count - 1]                                          *
        // **************************************************************************************************

        #region [ Members ]

        // Fields
        private List<IDataPoint> m_data;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketType101"/> class.
        /// </summary>
        public PacketType101()
            : base(101)
        {
            m_data = new List<IDataPoint>();
            ProcessHandler = Process;
            PreProcessHandler = PreProcess;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketType101"/> class.
        /// </summary>
        /// <param name="dataPoints">A collection of time series data points.</param>
        public PacketType101(IEnumerable<IDataPoint> dataPoints)
            : this()
        {
            if (dataPoints == null)
                throw new ArgumentNullException("value");

            foreach (IDataPoint dataPoint in dataPoints)
            {
                m_data.Add(new PacketType101Data(dataPoint));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketType101"/> class.
        /// </summary>
        /// <param name="measurements">A collection of mesurements.</param>
        public PacketType101(IEnumerable<IMeasurement> measurements)
            : this()
        {
            if (measurements == null)
                throw new ArgumentNullException("value");

            foreach (IMeasurement measurement in measurements)
            {
                m_data.Add(new PacketType101Data((int)measurement.ID,
                                                 new TimeTag((DateTime)measurement.Timestamp),
                                                 (float)measurement.AdjustedValue,
                                                 (measurement.TimestampQualityIsGood && measurement.ValueQualityIsGood ? Quality.Good : Quality.SuspectData)));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketType101"/> class.
        /// </summary>
        /// <param name="binaryImage">Binary image to be used for initializing <see cref="PacketType101"/>.</param>
        /// <param name="startIndex">0-based starting index of initialization data in the <paramref name="binaryImage"/>.</param>
        /// <param name="length">Valid number of bytes in <paramref name="binaryImage"/> from <paramref name="startIndex"/>.</param>
        public PacketType101(byte[] binaryImage, int startIndex, int length)
            : this()
        {
            Initialize(binaryImage, startIndex, length);
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets the time series data in <see cref="PacketType101"/>.
        /// </summary>
        public IList<IDataPoint> Data
        {
            get
            {
                return m_data;
            }
        }

        /// <summary>
        /// Gets the length of the <see cref="BinaryImage"/>.
        /// </summary>
        public override int BinaryLength
        {
            get
            {
                return (2 + 4 + (m_data.Count * PacketType101Data.ByteCount));
            }
        }

        /// <summary>
        /// Gets the binary representation of <see cref="PacketType101"/>.
        /// </summary>
        public override byte[] BinaryImage
        {
            get
            {
                byte[] image = new byte[BinaryLength];

                Array.Copy(EndianOrder.LittleEndian.GetBytes(TypeID), 0, image, 0, 2);
                Array.Copy(EndianOrder.LittleEndian.GetBytes(m_data.Count), 0, image, 2, 4);
                for (int i = 0; i < m_data.Count; i++)
                {
                    Array.Copy(m_data[i].BinaryImage, 0, image, 6 + (i * PacketType101Data.ByteCount), PacketType101Data.ByteCount);
                }

                return image;
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Initializes <see cref="PacketType101"/> from the specified <paramref name="binaryImage"/>.
        /// </summary>
        /// <param name="binaryImage">Binary image to be used for initializing <see cref="PacketType101"/>.</param>
        /// <param name="startIndex">0-based starting index of initialization data in the <paramref name="binaryImage"/>.</param>
        /// <param name="length">Valid number of bytes in <paramref name="binaryImage"/> from <paramref name="startIndex"/>.</param>
        /// <returns>Number of bytes used from the <paramref name="binaryImage"/> for initializing <see cref="PacketType101"/>.</returns>
        public override int Initialize(byte[] binaryImage, int startIndex, int length)
        {
            if (length - startIndex >= 6)
            {
                // Binary image has sufficient data.
                short packetID = EndianOrder.LittleEndian.ToInt16(binaryImage, startIndex);
                if (packetID != TypeID)
                    throw new ArgumentException(string.Format("Unexpected packet id '{0}' (expected '{1}').", packetID, TypeID));

                // Ensure that the binary image is complete
                int dataCount = EndianOrder.LittleEndian.ToInt32(binaryImage, startIndex + 2);
                if (length - startIndex < 6 + dataCount * PacketType101Data.ByteCount)
                    return 0;

                // We have a binary image with the correct packet id.
                m_data.Clear();
                for (int i = 0; i < dataCount; i++)
                {
                    m_data.Add(new PacketType101Data(binaryImage, startIndex + 6 + (i * PacketType101Data.ByteCount), length));
                }

                return BinaryLength;
            }
            else
            {
                // Binary image does not have sufficient data.
                return 0;
            }
        }

        /// <summary>
        /// Extracts time series data from <see cref="PacketType101"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> object of <see cref="ArchiveData"/>.</returns>
        public override IEnumerable<IDataPoint> ExtractTimeSeriesData()
        {
            List<IDataPoint> data = new List<IDataPoint>();
            foreach (IDataPoint dataPoint in m_data)
            {
                data.Add(new ArchiveData(dataPoint));
            }
            return data;
        }

        /// <summary>
        /// Processes <see cref="PacketType101"/>.
        /// </summary>
        /// <returns>A null reference.</returns>
        protected virtual IEnumerable<byte[]> Process()
        {
            if (Archive != null)
            {
                foreach (IDataPoint dataPoint in ExtractTimeSeriesData())
                {
                    Archive.WriteData(dataPoint);
                }
            }

            return null;
        }

        /// <summary>
        /// Pre-processes <see cref="PacketType101"/>.
        /// </summary>
        /// <returns>A <see cref="byte"/> array for "ACK".</returns>
        protected virtual IEnumerable<byte[]> PreProcess()
        {
            return new byte[][] { Encoding.ASCII.GetBytes("ACK") };
        }

        #endregion
    }
}
