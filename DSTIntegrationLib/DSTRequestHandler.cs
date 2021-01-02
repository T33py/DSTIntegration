using DSTIntegration.CommandHandlers;
using DSTIntegrationLib;
using DSTIntegrationLib.CommandHandlers.CommandParameterObjects;
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
            return Retrievers.SubjectsRetriever(connection);
        }

        /// <summary>
        /// Retrieve subjects with the given settings
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<Subject> GetSubjects(GetSubjectsParameters parameters)
        {
            bool recursive = parameters.Recursive;
            string subjects_requested = parameters.SubjectIDs;
            List<Subject> subjects;

            if (subjects_requested.Length > 0 && recursive)
            {
                subjects = GetSubjects(subjects_requested, recursive);
            }
            else if (subjects_requested.Length > 0)
            {
                subjects = GetSubjects(subjects_requested);
            }
            else if (recursive)
            {
                subjects = GetSubjects(recursive);
            }
            else
            {
                subjects = GetSubjects();
            }

            return subjects;
        }

        /// <summary>
        /// Retreive all subjects recursively
        /// </summary>
        /// <param name="recursive">Whether subjects should be retrieved recursively</param>
        /// <returns></returns>
        public List<Subject> GetSubjects(bool recursive)
        {
            connection.Settings[SettingConstants.Recursive] = recursive.ToString().ToLower();
            return Retrievers.SubjectsRetriever(connection);
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
            return Retrievers.SubjectsRetriever(connection);
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
            return Retrievers.SubjectsRetriever(connection);
        }

        #endregion

        #region GetTables

        /// <summary>
        /// Retrieve tables according to the current settings
        /// </summary>
        /// <returns></returns>
        public List<Table> GetTables()
        {
            return Retrievers.TablesRetriever(connection);
        }

        /// <summary>
        /// Get a list of tables based on the provided parameters
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<Table> GetTables(GetTablesParameters parameters)
        {
            List<Table> tables;

            Console.WriteLine(parameters.SubjectIDs + ", " + parameters.UpatedWithinDays);

            if (parameters.SubjectIDs.Length > 0 && parameters.UpatedWithinDays > -1)
            {
                tables = GetTables(parameters.SubjectIDs, parameters.UpatedWithinDays);
            }
            else if (parameters.SubjectIDs.Length > 0)
            {
                tables = GetTables(parameters.SubjectIDs);
            }
            else if (parameters.UpatedWithinDays > -1)
            {
                tables = GetTables(parameters.UpatedWithinDays);
            }
            else
            {
                tables = GetTables();
            }

            return tables;
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
            return Retrievers.TablesRetriever(connection);
        }

        /// <summary>
        /// Retrieve tables according to subjectIDs
        /// </summary>
        /// <param name="subjectIDs">Comma sepparated list of subject ids to request tables for</param>
        /// <returns></returns>
        public List<Table> GetTables(string subjectIDs)
        {
            connection.Settings[SettingConstants.TableSubject] = subjectIDs;
            return Retrievers.TablesRetriever(connection);
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
            return Retrievers.TablesRetriever(connection);
        }

        #endregion

        #region GetMetadata

        /// <summary>
        /// Retrieve metadata of the currently selected table
        /// </summary>
        /// <returns></returns>
        public TableMetadata GetTableMetadata()
        {
            return Retrievers.TableMetadataRetriever(connection);
        }

        /// <summary>
        /// Retrieve metadata based on the parameters provided
        /// </summary>
        /// <param name="parameters">Information for the request</param>
        /// <returns></returns>
        public TableMetadata GetTableMetadata(GetTableMetadataParameters parameters)
        {
            TableMetadata metadata;

            if(parameters.TableID.Length > 0)
            {
                Console.WriteLine("Of table " + parameters.TableID);
                metadata = GetTableMetadata(parameters.TableID);
            }
            else
            {
                metadata = GetTableMetadata();
            }

            return metadata;
        }

        /// <summary>
        /// Retrieve metadata of this table
        /// </summary>
        /// <param name="tableID"></param>
        /// <returns></returns>
        public TableMetadata GetTableMetadata(string tableID)
        {
            connection.Settings[SettingConstants.TableID] = tableID;
            return Retrievers.TableMetadataRetriever(connection);
        }

        #endregion

        #region GetData

        /// <summary>
        /// Since the data requested is tied to the table metadata, a datarequest is performed for the variables contained in the provided metadata object.
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public string GetTableData(TableMetadata requestData)
        {
            return Retrievers.TableDataRetriever(connection, requestData);
        }

        #endregion
    }
}
