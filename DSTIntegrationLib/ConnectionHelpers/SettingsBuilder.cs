using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSTIntegrationLib.ConnectionHelpers
{
    public class SettingsBuilder
    {

        public Dictionary<string, string> Variables;

        /// <summary>
        /// Instantiate the setting builder to get all the variables from settings files
        /// </summary>
        /// <param name="file"></param>
        public SettingsBuilder(string file, bool verbose)
        {
            Variables = new Dictionary<string, string>();
            JObject json = JObject.Parse(File.ReadAllText(file));

            Variables[SettingConstants.SubjectHTTP] = json[SettingConstants.SubjectHTTP].ToString();
            Variables[SettingConstants.TableHTTP] = json[SettingConstants.TableHTTP].ToString();
            Variables[SettingConstants.MetadataHTTP] = json[SettingConstants.MetadataHTTP].ToString();
            Variables[SettingConstants.DataHTTP] = json[SettingConstants.DataHTTP].ToString();
            
            Variables[SettingConstants.Language] = json[SettingConstants.Language].ToString();
            Variables[SettingConstants.Format] = json[SettingConstants.Format].ToString();
            
            Variables[SettingConstants.Recursive] = json[SettingConstants.Recursive].ToString();
            Variables[SettingConstants.OmitSubjectsWithoutTables] = json[SettingConstants.OmitSubjectsWithoutTables].ToString();
            Variables[SettingConstants.IncludeTables] = json[SettingConstants.IncludeTables].ToString();
            Variables[SettingConstants.SubjectID] = json[SettingConstants.SubjectID].ToString();

            Variables[SettingConstants.OnlyTablesUpdatedWithinDays] = json[SettingConstants.OnlyTablesUpdatedWithinDays].ToString();
            Variables[SettingConstants.TableSubject] = json[SettingConstants.TableSubject].ToString();

            Variables[SettingConstants.TableID] = json[SettingConstants.TableID].ToString();


            if (verbose)
            {
                foreach (var v in Variables.Keys)
                {
                    Console.WriteLine(Variables[v]);
                }
            }
        }


    }

    /// <summary>
    /// Keys for the settings used by the connections
    /// </summary>
    public static class SettingConstants
    {
        public static string SubjectHTTP = "SubjectHTTP";
        public static string TableHTTP = "TableHTTP";
        public static string MetadataHTTP = "MetadataHTTP";
        public static string DataHTTP = "DataHTTP";

        public static string Language = "Language";
        public static string Format = "Format";

        public static string Recursive = "Recursive";
        public static string OmitSubjectsWithoutTables = "OmitSubjectsWithoutTables";
        public static string IncludeTables = "IncludeTables";
        public static string SubjectID = "SubjectID";

        public static string OnlyTablesUpdatedWithinDays = "OnlyTablesUpdatedWithinDays";
        public static string TableSubject = "TableSubject";

        public static string TableID = "TableID";
    }
}
