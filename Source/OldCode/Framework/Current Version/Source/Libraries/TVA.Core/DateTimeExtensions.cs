//*******************************************************************************************************
//  DateTimeExtensions.cs - Gbtc
//
//  Tennessee Valley Authority, 2009
//  No copyright is claimed pursuant to 17 USC § 105.  All Other Rights Reserved.
//
//  This software is made freely available under the TVA Open Source Agreement (see below).
//
//  Code Modification History:
//  -----------------------------------------------------------------------------------------------------
//  02/23/2003 - J. Ritchie Carroll
//       Generated original version of source code.
//  12/21/2005 - J. Ritchie Carroll
//       Migrated 2.0 version of source code from 1.1 source (TVA.Shared.DateTime).
//  08/31/2007 - Darrell Zuercher
//       Edited code comments.
//  09/08/2008 - J. Ritchie Carroll
//       Converted to C# extensions.
//  02/16/2009 - Josh L. Patterson
//       Edited Code Comments.
//  09/14/2009 - Stephen C. Wills
//       Added new header and license agreement.
//  01/06/2010 - Andrew K. Hill
//       Modified the following methods per unit testing:
//       LocalTimeTo(DateTime, string)
//       LocalTimeTo(DateTime, TimeZoneInfo)
//       UniversalTimeTo(DateTime, string)
//       UniversalTimeTo(DateTime, TimeZoneInfo)
//       TimeZoneToTimeZone(DateTime, string, string)
//       TimeZoneToTimeZone(DateTime, TimeZoneInfo, TimeZoneInfo)
//
//*******************************************************************************************************

#region [ TVA Open Source Agreement ]
/*

 THIS OPEN SOURCE AGREEMENT ("AGREEMENT") DEFINES THE RIGHTS OF USE,REPRODUCTION, DISTRIBUTION,
 MODIFICATION AND REDISTRIBUTION OF CERTAIN COMPUTER SOFTWARE ORIGINALLY RELEASED BY THE
 TENNESSEE VALLEY AUTHORITY, A CORPORATE AGENCY AND INSTRUMENTALITY OF THE UNITED STATES GOVERNMENT
 ("GOVERNMENT AGENCY"). GOVERNMENT AGENCY IS AN INTENDED THIRD-PARTY BENEFICIARY OF ALL SUBSEQUENT
 DISTRIBUTIONS OR REDISTRIBUTIONS OF THE SUBJECT SOFTWARE. ANYONE WHO USES, REPRODUCES, DISTRIBUTES,
 MODIFIES OR REDISTRIBUTES THE SUBJECT SOFTWARE, AS DEFINED HEREIN, OR ANY PART THEREOF, IS, BY THAT
 ACTION, ACCEPTING IN FULL THE RESPONSIBILITIES AND OBLIGATIONS CONTAINED IN THIS AGREEMENT.

 Original Software Designation: openPDC
 Original Software Title: The TVA Open Source Phasor Data Concentrator
 User Registration Requested. Please Visit https://naspi.tva.com/Registration/
 Point of Contact for Original Software: J. Ritchie Carroll <mailto:jrcarrol@tva.gov>

 1. DEFINITIONS

 A. "Contributor" means Government Agency, as the developer of the Original Software, and any entity
 that makes a Modification.

 B. "Covered Patents" mean patent claims licensable by a Contributor that are necessarily infringed by
 the use or sale of its Modification alone or when combined with the Subject Software.

 C. "Display" means the showing of a copy of the Subject Software, either directly or by means of an
 image, or any other device.

 D. "Distribution" means conveyance or transfer of the Subject Software, regardless of means, to
 another.

 E. "Larger Work" means computer software that combines Subject Software, or portions thereof, with
 software separate from the Subject Software that is not governed by the terms of this Agreement.

 F. "Modification" means any alteration of, including addition to or deletion from, the substance or
 structure of either the Original Software or Subject Software, and includes derivative works, as that
 term is defined in the Copyright Statute, 17 USC § 101. However, the act of including Subject Software
 as part of a Larger Work does not in and of itself constitute a Modification.

 G. "Original Software" means the computer software first released under this Agreement by Government
 Agency entitled openPDC, including source code, object code and accompanying documentation, if any.

 H. "Recipient" means anyone who acquires the Subject Software under this Agreement, including all
 Contributors.

 I. "Redistribution" means Distribution of the Subject Software after a Modification has been made.

 J. "Reproduction" means the making of a counterpart, image or copy of the Subject Software.

 K. "Sale" means the exchange of the Subject Software for money or equivalent value.

 L. "Subject Software" means the Original Software, Modifications, or any respective parts thereof.

 M. "Use" means the application or employment of the Subject Software for any purpose.

 2. GRANT OF RIGHTS

 A. Under Non-Patent Rights: Subject to the terms and conditions of this Agreement, each Contributor,
 with respect to its own contribution to the Subject Software, hereby grants to each Recipient a
 non-exclusive, world-wide, royalty-free license to engage in the following activities pertaining to
 the Subject Software:

 1. Use

 2. Distribution

 3. Reproduction

 4. Modification

 5. Redistribution

 6. Display

 B. Under Patent Rights: Subject to the terms and conditions of this Agreement, each Contributor, with
 respect to its own contribution to the Subject Software, hereby grants to each Recipient under Covered
 Patents a non-exclusive, world-wide, royalty-free license to engage in the following activities
 pertaining to the Subject Software:

 1. Use

 2. Distribution

 3. Reproduction

 4. Sale

 5. Offer for Sale

 C. The rights granted under Paragraph B. also apply to the combination of a Contributor's Modification
 and the Subject Software if, at the time the Modification is added by the Contributor, the addition of
 such Modification causes the combination to be covered by the Covered Patents. It does not apply to
 any other combinations that include a Modification. 

 D. The rights granted in Paragraphs A. and B. allow the Recipient to sublicense those same rights.
 Such sublicense must be under the same terms and conditions of this Agreement.

 3. OBLIGATIONS OF RECIPIENT

 A. Distribution or Redistribution of the Subject Software must be made under this Agreement except for
 additions covered under paragraph 3H. 

 1. Whenever a Recipient distributes or redistributes the Subject Software, a copy of this Agreement
 must be included with each copy of the Subject Software; and

 2. If Recipient distributes or redistributes the Subject Software in any form other than source code,
 Recipient must also make the source code freely available, and must provide with each copy of the
 Subject Software information on how to obtain the source code in a reasonable manner on or through a
 medium customarily used for software exchange.

 B. Each Recipient must ensure that the following copyright notice appears prominently in the Subject
 Software:

          No copyright is claimed pursuant to 17 USC § 105.  All Other Rights Reserved.

 C. Each Contributor must characterize its alteration of the Subject Software as a Modification and
 must identify itself as the originator of its Modification in a manner that reasonably allows
 subsequent Recipients to identify the originator of the Modification. In fulfillment of these
 requirements, Contributor must include a file (e.g., a change log file) that describes the alterations
 made and the date of the alterations, identifies Contributor as originator of the alterations, and
 consents to characterization of the alterations as a Modification, for example, by including a
 statement that the Modification is derived, directly or indirectly, from Original Software provided by
 Government Agency. Once consent is granted, it may not thereafter be revoked.

 D. A Contributor may add its own copyright notice to the Subject Software. Once a copyright notice has
 been added to the Subject Software, a Recipient may not remove it without the express permission of
 the Contributor who added the notice.

 E. A Recipient may not make any representation in the Subject Software or in any promotional,
 advertising or other material that may be construed as an endorsement by Government Agency or by any
 prior Recipient of any product or service provided by Recipient, or that may seek to obtain commercial
 advantage by the fact of Government Agency's or a prior Recipient's participation in this Agreement.

 F. In an effort to track usage and maintain accurate records of the Subject Software, each Recipient,
 upon receipt of the Subject Software, is requested to register with Government Agency by visiting the
 following website: https://naspi.tva.com/Registration/. Recipient's name and personal information
 shall be used for statistical purposes only. Once a Recipient makes a Modification available, it is
 requested that the Recipient inform Government Agency at the web site provided above how to access the
 Modification.

 G. Each Contributor represents that that its Modification does not violate any existing agreements,
 regulations, statutes or rules, and further that Contributor has sufficient rights to grant the rights
 conveyed by this Agreement.

 H. A Recipient may choose to offer, and to charge a fee for, warranty, support, indemnity and/or
 liability obligations to one or more other Recipients of the Subject Software. A Recipient may do so,
 however, only on its own behalf and not on behalf of Government Agency or any other Recipient. Such a
 Recipient must make it absolutely clear that any such warranty, support, indemnity and/or liability
 obligation is offered by that Recipient alone. Further, such Recipient agrees to indemnify Government
 Agency and every other Recipient for any liability incurred by them as a result of warranty, support,
 indemnity and/or liability offered by such Recipient.

 I. A Recipient may create a Larger Work by combining Subject Software with separate software not
 governed by the terms of this agreement and distribute the Larger Work as a single product. In such
 case, the Recipient must make sure Subject Software, or portions thereof, included in the Larger Work
 is subject to this Agreement.

 J. Notwithstanding any provisions contained herein, Recipient is hereby put on notice that export of
 any goods or technical data from the United States may require some form of export license from the
 U.S. Government. Failure to obtain necessary export licenses may result in criminal liability under
 U.S. laws. Government Agency neither represents that a license shall not be required nor that, if
 required, it shall be issued. Nothing granted herein provides any such export license.

 4. DISCLAIMER OF WARRANTIES AND LIABILITIES; WAIVER AND INDEMNIFICATION

 A. No Warranty: THE SUBJECT SOFTWARE IS PROVIDED "AS IS" WITHOUT ANY WARRANTY OF ANY KIND, EITHER
 EXPRESSED, IMPLIED, OR STATUTORY, INCLUDING, BUT NOT LIMITED TO, ANY WARRANTY THAT THE SUBJECT
 SOFTWARE WILL CONFORM TO SPECIFICATIONS, ANY IMPLIED WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
 PARTICULAR PURPOSE, OR FREEDOM FROM INFRINGEMENT, ANY WARRANTY THAT THE SUBJECT SOFTWARE WILL BE ERROR
 FREE, OR ANY WARRANTY THAT DOCUMENTATION, IF PROVIDED, WILL CONFORM TO THE SUBJECT SOFTWARE. THIS
 AGREEMENT DOES NOT, IN ANY MANNER, CONSTITUTE AN ENDORSEMENT BY GOVERNMENT AGENCY OR ANY PRIOR
 RECIPIENT OF ANY RESULTS, RESULTING DESIGNS, HARDWARE, SOFTWARE PRODUCTS OR ANY OTHER APPLICATIONS
 RESULTING FROM USE OF THE SUBJECT SOFTWARE. FURTHER, GOVERNMENT AGENCY DISCLAIMS ALL WARRANTIES AND
 LIABILITIES REGARDING THIRD-PARTY SOFTWARE, IF PRESENT IN THE ORIGINAL SOFTWARE, AND DISTRIBUTES IT
 "AS IS."

 B. Waiver and Indemnity: RECIPIENT AGREES TO WAIVE ANY AND ALL CLAIMS AGAINST GOVERNMENT AGENCY, ITS
 AGENTS, EMPLOYEES, CONTRACTORS AND SUBCONTRACTORS, AS WELL AS ANY PRIOR RECIPIENT. IF RECIPIENT'S USE
 OF THE SUBJECT SOFTWARE RESULTS IN ANY LIABILITIES, DEMANDS, DAMAGES, EXPENSES OR LOSSES ARISING FROM
 SUCH USE, INCLUDING ANY DAMAGES FROM PRODUCTS BASED ON, OR RESULTING FROM, RECIPIENT'S USE OF THE
 SUBJECT SOFTWARE, RECIPIENT SHALL INDEMNIFY AND HOLD HARMLESS  GOVERNMENT AGENCY, ITS AGENTS,
 EMPLOYEES, CONTRACTORS AND SUBCONTRACTORS, AS WELL AS ANY PRIOR RECIPIENT, TO THE EXTENT PERMITTED BY
 LAW.  THE FOREGOING RELEASE AND INDEMNIFICATION SHALL APPLY EVEN IF THE LIABILITIES, DEMANDS, DAMAGES,
 EXPENSES OR LOSSES ARE CAUSED, OCCASIONED, OR CONTRIBUTED TO BY THE NEGLIGENCE, SOLE OR CONCURRENT, OF
 GOVERNMENT AGENCY OR ANY PRIOR RECIPIENT.  RECIPIENT'S SOLE REMEDY FOR ANY SUCH MATTER SHALL BE THE
 IMMEDIATE, UNILATERAL TERMINATION OF THIS AGREEMENT.

 5. GENERAL TERMS

 A. Termination: This Agreement and the rights granted hereunder will terminate automatically if a
 Recipient fails to comply with these terms and conditions, and fails to cure such noncompliance within
 thirty (30) days of becoming aware of such noncompliance. Upon termination, a Recipient agrees to
 immediately cease use and distribution of the Subject Software. All sublicenses to the Subject
 Software properly granted by the breaching Recipient shall survive any such termination of this
 Agreement.

 B. Severability: If any provision of this Agreement is invalid or unenforceable under applicable law,
 it shall not affect the validity or enforceability of the remainder of the terms of this Agreement.

 C. Applicable Law: This Agreement shall be subject to United States federal law only for all purposes,
 including, but not limited to, determining the validity of this Agreement, the meaning of its
 provisions and the rights, obligations and remedies of the parties.

 D. Entire Understanding: This Agreement constitutes the entire understanding and agreement of the
 parties relating to release of the Subject Software and may not be superseded, modified or amended
 except by further written agreement duly executed by the parties.

 E. Binding Authority: By accepting and using the Subject Software under this Agreement, a Recipient
 affirms its authority to bind the Recipient to all terms and conditions of this Agreement and that
 Recipient hereby agrees to all terms and conditions herein.

 F. Point of Contact: Any Recipient contact with Government Agency is to be directed to the designated
 representative as follows: J. Ritchie Carroll <mailto:jrcarrol@tva.gov>.

*/
#endregion

using System;
using System.Globalization;

namespace TVA
{
    /// <summary>
    /// Defines extension functions related to Date/Time manipulation.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>Determines if the specified UTC time is valid, by comparing it to the system clock.</summary>
        /// <param name="utcTime">UTC time to test for validity.</param>
        /// <param name="lagTime">The allowed lag time, in seconds, before assuming time is too old to be valid.</param>
        /// <param name="leadTime">The allowed lead time, in seconds, before assuming time is too advanced to be
        /// valid.</param>
        /// <returns>True, if time is within the specified range.</returns>
        /// <remarks>
        /// <para>Time is considered valid if it exists within the specified lag time/lead time range of current
        /// time.</para>
        /// <para>Note that lag time and lead time must be greater than zero, but can be set to sub-second
        /// intervals.</para>
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">LagTime and LeadTime must be greater than zero, but can
        /// be less than one.</exception>
        public static bool UtcTimeIsValid(this DateTime utcTime, double lagTime, double leadTime)
        {
            return ((Ticks)utcTime).UtcTimeIsValid(lagTime, leadTime);
        }

        /// <summary>Determines if the specified local time is valid, by comparing it to the system clock.</summary>
        /// <param name="localTime">Time to test for validity.</param>
        /// <param name="lagTime">The allowed lag time, in seconds, before assuming time is too old to be valid.</param>
        /// <param name="leadTime">The allowed lead time, in seconds, before assuming time is too advanced to be
        /// valid.</param>
        /// <returns>True, if time is within the specified range.</returns>
        /// <remarks>
        /// <para>Time is considered valid if it exists within the specified lag time/lead time range of current
        /// time.</para>
        /// <para>Note that lag time and lead time must be greater than zero, but can be set to sub-second
        /// intervals.</para>
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">LagTime and LeadTime must be greater than zero, but can
        /// be less than one.</exception>
        public static bool LocalTimeIsValid(this DateTime localTime, double lagTime, double leadTime)
        {
            return ((Ticks)localTime).LocalTimeIsValid(lagTime, leadTime);
        }

        /// <summary>Determines if time is valid, by comparing it to the specified current time.</summary>
        /// <param name="testTime">Time to test for validity.</param>
        /// <param name="currentTime">Specified current time (e.g., could be Date.Now or Date.UtcNow).</param>
        /// <param name="lagTime">The allowed lag time, in seconds, before assuming time is too old to be valid.</param>
        /// <param name="leadTime">The allowed lead time, in seconds, before assuming time is too advanced to be
        /// valid.</param>
        /// <returns>True, if time is within the specified range.</returns>
        /// <remarks>
        /// <para>Time is considered valid if it exists within the specified lag time/lead time range of current
        /// time.</para>
        /// <para>Note that lag time and lead time must be greater than zero, but can be set to sub-second
        /// intervals.</para>
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">LagTime and LeadTime must be greater than zero, but can
        /// be less than one.</exception>
        public static bool TimeIsValid(this DateTime testTime, DateTime currentTime, double lagTime, double leadTime)
        {
            return ((Ticks)testTime).TimeIsValid(currentTime, lagTime, leadTime);
        }


        /// <summary>Gets the distance, in <see cref="Ticks"/>, beyond the top of the <paramref name="timestamp"/> second.</summary>
        /// <param name="timestamp">Timestamp to evaluate.</param>
        /// <returns>Timestamp's tick distance from the top of the second.</returns>
        public static Ticks DistanceBeyondSecond(this DateTime timestamp)
        {
            return ((Ticks)timestamp).DistanceBeyondSecond();
        }

        /// <summary>Creates a baselined timestamp which begins at the specified time interval.</summary>
        /// <param name="timestamp">Timestamp to baseline.</param>
        /// <param name="interval">
        /// <see cref="BaselineTimeInterval"/> to which <paramref name="timestamp"/> should be baselined.
        /// </param>
        /// <returns>
        /// A new <see cref="DateTime"/> value that represents a baselined timestamp that begins at the
        /// specified <see cref="BaselineTimeInterval"/>.
        /// </returns>
        /// <remarks>
        /// Baselining to the <see cref="BaselineTimeInterval.Second"/> would return the <see cref="DateTime"/>
        /// value starting at zero milliseconds.<br/>
        /// Baselining to the <see cref="BaselineTimeInterval.Minute"/> would return the <see cref="DateTime"/>
        /// value starting at zero seconds and milliseconds.<br/>
        /// Baselining to the <see cref="BaselineTimeInterval.Hour"/> would return the <see cref="DateTime"/>
        /// value starting at zero minutes, seconds and milliseconds.<br/>
        /// Baselining to the <see cref="BaselineTimeInterval.Day"/> would return the <see cref="DateTime"/>
        /// value starting at zero hours, minutes, seconds and milliseconds.<br/>
        /// Baselining to the <see cref="BaselineTimeInterval.Month"/> would return the <see cref="DateTime"/>
        /// value starting at day one, zero hours, minutes, seconds and milliseconds.<br/>
        /// Baselining to the <see cref="BaselineTimeInterval.Year"/> would return the <see cref="DateTime"/>
        /// value starting at month one, day one, zero hours, minutes, seconds and milliseconds.
        /// </remarks>
        public static DateTime BaselinedTimestamp(this DateTime timestamp, BaselineTimeInterval interval)
        {
            return ((Ticks)timestamp).BaselinedTimestamp(interval);
        }

        /// <summary>Converts given local time to Eastern time.</summary>
        /// <param name="timestamp">Timestamp in local time to be converted to Eastern time.</param>
        /// <returns>
        /// <para>Timestamp in Eastern time.</para>
        /// </returns>
        public static DateTime LocalTimeToEasternTime(this DateTime timestamp)
        {
            return TimeZoneInfo.ConvertTime(timestamp, TimeZoneInfo.Local, USTimeZones.Eastern);
        }

        /// <summary>Converts given local time to Central time.</summary>
        /// <param name="timestamp">Timestamp in local time to be converted to Central time.</param>
        /// <returns>
        /// <para>Timestamp in Central time.</para>
        /// </returns>
        public static DateTime LocalTimeToCentralTime(this DateTime timestamp)
        {
            return TimeZoneInfo.ConvertTime(timestamp, TimeZoneInfo.Local, USTimeZones.Central);
        }

        /// <summary>Converts given local time to Mountain time.</summary>
        /// <param name="timestamp">Timestamp in local time to be converted to Mountain time.</param>
        /// <returns>
        /// <para>Timestamp in Mountain time.</para>
        /// </returns>
        public static DateTime LocalTimeToMountainTime(this DateTime timestamp)
        {
            return TimeZoneInfo.ConvertTime(timestamp, TimeZoneInfo.Local, USTimeZones.Mountain);
        }

        /// <summary>Converts given local time to Pacific time.</summary>
        /// <param name="timestamp">Timestamp in local time to be converted to Pacific time.</param>
        /// <returns>
        /// <para>Timestamp in Pacific time.</para>
        /// </returns>
        public static DateTime LocalTimeToPacificTime(this DateTime timestamp)
        {
            return TimeZoneInfo.ConvertTime(timestamp, TimeZoneInfo.Local, USTimeZones.Pacific);
        }

        /// <summary>Converts given local time to Universally Coordinated Time (a.k.a., Greenwich Meridian Time).</summary>
        /// <remarks>This function is only provided for the sake of completeness. All it does is call the
        /// "ToUniversalTime" property on the given timestamp.</remarks>
        /// <param name="timestamp">Timestamp in local time to be converted to Universal time.</param>
        /// <returns>
        /// <para>Timestamp in UniversalTime (a.k.a., GMT).</para>
        /// </returns>
        public static DateTime LocalTimeToUniversalTime(this DateTime timestamp)
        {
            return timestamp.ToUniversalTime();
        }

        /// <summary>Converts given local time to time in specified time zone.</summary>
        /// <param name="timestamp">Timestamp in local time to be converted to time in specified time zone.</param>
        /// <param name="destinationTimeZoneStandardName">Standard name of desired end time zone for given
        /// timestamp.</param>
        /// <returns>
        /// <para>Timestamp in specified time zone.</para>
        /// </returns>
        public static DateTime LocalTimeTo(this DateTime timestamp, string destinationTimeZoneStandardName)
        {
            if (destinationTimeZoneStandardName == null)
                throw new ArgumentNullException("destinationTimeZoneStandardName");

            return TimeZoneInfo.ConvertTime(timestamp, TimeZoneInfo.Local, TimeZoneInfo.FindSystemTimeZoneById(destinationTimeZoneStandardName));
        }

        /// <summary>Converts given local time to time in specified time zone.</summary>
        /// <param name="timestamp">Timestamp in local time to be converted to time in specified time zone.</param>
        /// <param name="destinationTimeZone">Desired end time zone for given timestamp.</param>
        /// <returns>
        /// <para>Timestamp in specified time zone.</para>
        /// </returns>
        public static DateTime LocalTimeTo(this DateTime timestamp, TimeZoneInfo destinationTimeZone)
        {
            if (destinationTimeZone == null)
                throw new ArgumentNullException("destinationTimeZone");

            return TimeZoneInfo.ConvertTime(timestamp, TimeZoneInfo.Local, destinationTimeZone);
        }

        /// <summary>
        /// Converts the specified Universally Coordinated Time timestamp to Eastern time timestamp.
        /// </summary>
        /// <param name="universalTimestamp">The Universally Coordinated Time timestamp that is to be converted.</param>
        /// <returns>The timestamp in Eastern time.</returns>
        public static DateTime UniversalTimeToEasternTime(this DateTime universalTimestamp)
        {
            return TimeZoneInfo.ConvertTime(universalTimestamp, TimeZoneInfo.Utc, USTimeZones.Eastern);
        }

        /// <summary>
        /// Converts the specified Universally Coordinated Time timestamp to Central time timestamp.
        /// </summary>
        /// <param name="universalTimestamp">The Universally Coordinated Time timestamp that is to be converted.</param>
        /// <returns>The timestamp in Central time.</returns>
        public static DateTime UniversalTimeToCentralTime(this DateTime universalTimestamp)
        {
            return TimeZoneInfo.ConvertTime(universalTimestamp, TimeZoneInfo.Utc, USTimeZones.Central);
        }

        /// <summary>
        /// Converts the specified Universally Coordinated Time timestamp to Mountain time timestamp.
        /// </summary>
        /// <param name="universalTimestamp">The Universally Coordinated Time timestamp that is to be converted.</param>
        /// <returns>The timestamp in Mountain time.</returns>
        public static DateTime UniversalTimeToMountainTime(this DateTime universalTimestamp)
        {
            return TimeZoneInfo.ConvertTime(universalTimestamp, TimeZoneInfo.Utc, USTimeZones.Mountain);
        }

        /// <summary>
        /// Converts the specified Universally Coordinated Time timestamp to Pacific time timestamp.
        /// </summary>
        /// <param name="universalTimestamp">The Universally Coordinated Time timestamp that is to be converted.</param>
        /// <returns>The timestamp in Pacific time.</returns>
        public static DateTime UniversalTimeToPacificTime(this DateTime universalTimestamp)
        {
            return TimeZoneInfo.ConvertTime(universalTimestamp, TimeZoneInfo.Utc, USTimeZones.Pacific);
        }

        /// <summary>
        /// Converts the specified Universally Coordinated Time timestamp to timestamp in specified time zone.
        /// </summary>
        /// <param name="universalTimestamp">The Universally Coordinated Time timestamp that is to be converted.</param>
        /// <param name="destinationTimeZoneStandardName">The time zone standard name to which the Universally
        /// Coordinated Time timestamp is to be converted to.</param>
        /// <returns>The timestamp in the specified time zone.</returns>
        public static DateTime UniversalTimeTo(this DateTime universalTimestamp, string destinationTimeZoneStandardName)
        {
            if (destinationTimeZoneStandardName == null)
                throw new ArgumentNullException("destinationTimeZoneStandardName");

            return TimeZoneInfo.ConvertTime(universalTimestamp, TimeZoneInfo.Utc, TimeZoneInfo.FindSystemTimeZoneById(destinationTimeZoneStandardName));
        }

        /// <summary>
        /// Converts the specified Universally Coordinated Time timestamp to timestamp in specified time zone.
        /// </summary>
        /// <param name="universalTimestamp">The Universally Coordinated Time timestamp that is to be converted.</param>
        /// <param name="destinationTimeZone">The time zone to which the Universally Coordinated Time timestamp
        /// is to be converted to.</param>
        /// <returns>The timestamp in the specified time zone.</returns>
        public static DateTime UniversalTimeTo(this DateTime universalTimestamp, TimeZoneInfo destinationTimeZone)
        {
            if (destinationTimeZone == null)
                throw new ArgumentNullException("destinationTimeZone");

            return TimeZoneInfo.ConvertTime(universalTimestamp, TimeZoneInfo.Utc, destinationTimeZone);
        }

        /// <summary>Converts given timestamp from one time zone to another using standard names for time zones.</summary>
        /// <param name="timestamp">Timestamp in source time zone to be converted to time in destination time zone.</param>
        /// <param name="sourceTimeZoneStandardName">Standard name of time zone for given source timestamp.</param>
        /// <param name="destinationTimeZoneStandardName">Standard name of desired end time zone for given source
        /// timestamp.</param>
        /// <returns>
        /// <para>Timestamp in destination time zone.</para>
        /// </returns>
        public static DateTime TimeZoneToTimeZone(this DateTime timestamp, string sourceTimeZoneStandardName, string destinationTimeZoneStandardName)
        {
            if (sourceTimeZoneStandardName == null)
                throw new ArgumentNullException("sourceTimeZoneStandardName");

            if (destinationTimeZoneStandardName == null)
                throw new ArgumentNullException("destinationTimeZoneStandardName");

            return TimeZoneInfo.ConvertTime(timestamp, TimeZoneInfo.FindSystemTimeZoneById(sourceTimeZoneStandardName), TimeZoneInfo.FindSystemTimeZoneById(destinationTimeZoneStandardName));
        }

        /// <summary>Converts given timestamp from one time zone to another.</summary>
        /// <param name="timestamp">Timestamp in source time zone to be converted to time in destination time
        /// zone.</param>
        /// <param name="sourceTimeZone">Time zone for given source timestamp.</param>
        /// <param name="destinationTimeZone">Desired end time zone for given source timestamp.</param>
        /// <returns>
        /// <para>Timestamp in destination time zone.</para>
        /// </returns>
        public static DateTime TimeZoneToTimeZone(this DateTime timestamp, TimeZoneInfo sourceTimeZone, TimeZoneInfo destinationTimeZone)
        {
            if (sourceTimeZone == null)
                throw new ArgumentNullException("sourceTimeZone");

            if (destinationTimeZone == null)
                throw new ArgumentNullException("destinationTimeZone");

            return TimeZoneInfo.ConvertTime(timestamp, sourceTimeZone, destinationTimeZone);
        }

        /// <summary>Gets the abbreviated month name for month of the timestamp.</summary>
        /// <param name="timestamp">Timestamp from which month name is extracted.</param>
        /// <returns>String representation of the month name based on <paramref name="timestamp"/></returns>
        public static string AbbreviatedMonthName(this DateTime timestamp)
        {
            return DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(timestamp.Month);
        }

        /// <summary>Gets the full month name for month of the timestamp.</summary>
        /// <param name="timestamp">Timestamp from which month name is extracted.</param>
        /// <returns>String representation of the month name based on <paramref name="timestamp"/></returns>
        public static string MonthName(this DateTime timestamp)
        {
            return DateTimeFormatInfo.CurrentInfo.GetMonthName(timestamp.Month);
        }

        /// <summary>Gets the abbreviated weekday name for weekday of the timestamp.</summary>
        /// <param name="timestamp">Timestamp from which weekday name is extracted.</param>
        /// <returns>String representation of the weekday name based on <paramref name="timestamp"/></returns>
        public static string AbbreviatedWeekdayName(this DateTime timestamp)
        {
            return DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName(timestamp.DayOfWeek);
        }

        /// <summary>Gets the shortest weekday name for weekday of the timestamp.</summary>
        /// <param name="timestamp">Timestamp from which weekday name is extracted.</param>
        /// <returns>String representation of the short weekday name based on <paramref name="timestamp"/></returns>
        public static string ShortWeekdayName(this DateTime timestamp)
        {
            return DateTimeFormatInfo.CurrentInfo.GetShortestDayName(timestamp.DayOfWeek);
        }

        /// <summary>Gets the full weekday name for weekday of the timestamp.</summary>
        /// <param name="timestamp">Timestamp from which weekday name is extracted.</param>
        /// <returns>String representation of the weekday name based on <paramref name="timestamp"/></returns>
        public static string WeekdayName(this DateTime timestamp)
        {
            return DateTimeFormatInfo.CurrentInfo.GetDayName(timestamp.DayOfWeek);
        }
    }
}