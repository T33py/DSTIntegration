using DSTIntegration.CommandHandlers;
using DSTIntegrationLib;
using DSTIntegrationLib.ConnectionHelpers;
using DSTIntegrationLib.SerializationObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSTIntegration
{
    /// <summary>
    /// Abstracts the work of completing operations with the given settings away from the program execution
    /// </summary>
    public class DSTRequestHandler
    {
        DSTConnection connection;

        /// <summary>
        /// Instantiate with default settings
        /// </summary>
        public DSTRequestHandler()
        {
            connection = new DSTConnection();
        }

        /// <summary>
        /// Instantiate using specific connection
        /// </summary>
        /// <param name="connection"></param>
        public DSTRequestHandler(DSTConnection connection)
        {
            this.connection = connection;
        }

        #region GetSubjects

        /// <summary>
        /// Retrieve list of all subjects non-recursively
        /// </summary>
        /// <returns></returns>
        public List<Subject> GetSubjects()
        {
            connection.Settings[SettingConstants.Recursive] = "false";
            return Retrievers.RetrieveSubjects(connection);
        }

        /// <summary>
        /// Retreive all subjects recursively
        /// </summary>
        /// <param name="recursive">Whether subjects should be retrieved recursively</param>
        /// <returns></returns>
        public List<Subject> GetSubjects(bool recursive)
        {
            connection.Settings[SettingConstants.Recursive] = recursive.ToString().ToLower();
            return Retrievers.RetrieveSubjects(connection);
        }

        /// <summary>
        /// Retrieve sub-subjects of the requested subject
        /// </summary>
        /// <param name="specific_subjects">ID of the subject to retrieve</param>
        /// <returns></returns>
        public List<Subject> GetSubjects(string specific_subjects)
        {
            Console.WriteLine("Get subject " + specific_subjects);
            connection.Settings[SettingConstants.SubjectID] = specific_subjects;
            connection.Settings[SettingConstants.Recursive] = "false";
            return Retrievers.RetrieveSubjects(connection);
        }

        /// <summary>
        /// Retrieve the specified subjects recursively
        /// </summary>
        /// <param name="specific_subjects">ID of the subject to retrieve</param>
        /// <param name="recursive">Whether subjects should be retrieved recursively</param>
        /// <returns></returns>
        public List<Subject> GetSubjects(string specific_subjects, bool recursive)
        {
            Console.WriteLine("Get subject " + specific_subjects);
            connection.Settings[SettingConstants.SubjectID] = specific_subjects;
            connection.Settings[SettingConstants.Recursive] = recursive.ToString().ToLower();
            return Retrievers.RetrieveSubjects(connection);
        }

        #endregion

        #region GetTables

        /// <summary>
        /// Retrieve tables according to the current settings
        /// </summary>
        /// <returns></returns>
        public List<Table> GetTables()
        {
            return Retrievers.RetrieveTables(connection);
        }

        /// <summary>
        /// Retrieve tables updated within the specified number of days
        /// </summary>
        /// <param name="updatedWithinDays">The maximum number of days that have passed since the table was updated</param>
        /// <returns></returns>
        public List<Table> GetTables(int updatedWithinDays)
        {
            connection.Settings[SettingConstants.SubjectID] = ""; // we want all the tables here
            connection.Settings[SettingConstants.OnlyTablesUpdatedWithinDays] = updatedWithinDays.ToString();
            return Retrievers.RetrieveTables(connection);
        }

        /// <summary>
        /// Retrieve tables according to subjectIDs
        /// </summary>
        /// <param name="subjectIDs">Comma sepparated list of subject ids to request tables for</param>
        /// <returns></returns>
        public List<Table> GetTables(string subjectIDs)
        {
            connection.Settings[SettingConstants.SubjectID] = subjectIDs;
            return Retrievers.RetrieveTables(connection);
        }

        /// <summary>
        /// Retrieve the tables which both belongs to the subjects specified and was updated within the given number of days
        /// </summary>
        /// <param name="subjectIDs">Comma sepparated list of subject ids to request tables for</param>
        /// <param name="updatedWithinDays">The maximum number of days that have passed since the table was updated</param>
        /// <returns></returns>
        public List<Table> GetTables(string subjectIDs, int updatedWithinDays)
        {
            connection.Settings[SettingConstants.SubjectID] = subjectIDs;
            connection.Settings[SettingConstants.OnlyTablesUpdatedWithinDays] = updatedWithinDays.ToString();
            return Retrievers.RetrieveTables(connection);
        }

        #endregion
    }
}
