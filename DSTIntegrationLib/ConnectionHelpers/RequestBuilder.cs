using DSTIntegrationLib.SerializationObjects;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSTIntegrationLib.ConnectionHelpers
{
    public static class RequestBuilder
    {
        public static RestRequest RetrieveSubjects(Dictionary<string, string> settings)
        {
            bool rec = bool.Parse(settings[SettingConstants.Recursive]);
            bool omit_no_tbl = bool.Parse(settings[SettingConstants.OmitSubjectsWithoutTables]);
            bool include_tables = bool.Parse(settings[SettingConstants.IncludeTables]);
            string lang = settings[SettingConstants.Language].ToLower();
            string subjects_s = settings[SettingConstants.SubjectID];

            Console.WriteLine("Building request: {0}, {1}, {2}, {3}, {4}", rec, omit_no_tbl, include_tables, lang, subjects_s);

            //
            // build request
            //
            RestRequest request = new RestRequest();
            var json = "{";

            json = json + "\"format\":\"" + settings[SettingConstants.Format] + "\",";

            if (lang.Equals("en"))
            {
                json = json + "\"lang\":\"" + lang + "\",";
            }

            if (rec)
            {
                json = json + "\"recursive\":" + rec.ToString().ToLower() + ",";
            }

            if (omit_no_tbl)
            {
                json = json + "\"omitSubjectsWithoutTables\":" +  omit_no_tbl.ToString().ToLower() + ",";
            }

            if (include_tables)
            {
                json = json + "\"includeTables\":" + include_tables.ToString().ToLower() + ",";
            }

            if (subjects_s.Length > 0)
            {
                var subjects = subjects_s.Split(',');
                string subjects_json = "[";

                for (int i = 0; i < subjects.Length; i++)
                {
                    subjects_json = subjects_json + "\"" + subjects[i].Trim() + "\",";
                }
                subjects_json = subjects_json.Substring(0, subjects_json.Length - 1) + "]";

                json = json + "\"subjects\":" + subjects_json + ",";
            }

            json = json.Substring(0, json.Length - 1) + "}";
            Console.WriteLine("REQUEST: " + json);

            request.AddJsonBody(json, "text/json");

            return request;
        }

        public static RestRequest RetrieveTables(Dictionary<string, string> settings)
        {
            int table_age = int.Parse(settings[SettingConstants.OnlyTablesUpdatedWithinDays]);
            string language = settings[SettingConstants.Language].ToLower();
            string subjects = settings[SettingConstants.TableSubject];
            Console.WriteLine("Building request: {0}, {1}, {2}", table_age, language, subjects);

            //
            // build request
            //
            RestRequest request = new RestRequest();

            request.AddParameter("format", settings[SettingConstants.Format]);

            if (language.Equals("en"))
            {
                request.AddParameter("lang", language);
            }

            if (table_age >= 0)
            {
                request.AddParameter("pastDays", table_age);
            }


            if (subjects.Contains(','))
            {
                request = new RestRequest();

                request.AddJsonBody(
                    new
                    {
                        format = settings[SettingConstants.Format],
                        lang = language,
                        pastDays = table_age,
                        subjects = SubjectList()
                    }
                    );
            }
            else if(subjects.Length > 0)
            {
                request.AddParameter("subjects", subjects);
            }


            return request;

            #region util
            string[] SubjectList()
            {
                var sublist = subjects.Split(',');

                for(int i = 0; i < sublist.Length; i++)
                {
                    sublist[i] = sublist[i].Trim();
                }

                return sublist;
            }
            #endregion
        }

        public static RestRequest RetrieveMetadata(Dictionary<string, string> settings)
        {
            string table = settings[SettingConstants.TableID].ToUpper();
            string language = settings[SettingConstants.Language].ToLower();
            //
            // build request
            //
            RestRequest request = new RestRequest();

            request.AddParameter("format", settings[SettingConstants.Format]);
            request.AddParameter("table", table);
            
            if(language.Equals("en")) request.AddParameter("lang", language);

            return request;
        }

        public static RestRequest RetrieveTable(Dictionary<string, string> settings, TableMetadata metadata)
        {
            string table = settings[SettingConstants.TableID].ToUpper();
            string language = settings[SettingConstants.Language].ToLower();
            string json = "{" +
                "\"lang\":\"" + language + "\"," +
                "\"table\":\"" + table + "\"," +
                "\"format\":\"CSV\"," +
                "\"allowVariablesInHead\":true," +
                "\"variables\":" + ReformatMetadata(metadata) + "" +
                "}";


            //
            // build request
            //
            RestRequest request = new RestRequest();

            request.AddJsonBody(json, "text/json");

            return request;
        }

        static string ReformatMetadata(TableMetadata metadata)
        {
            string var_list = "[";

            foreach(var variable in metadata.variables)
            {
                var_list = var_list + "{\"code\":\"" + variable.id + "\",";
                var_list = var_list + "\"values\":" + ReformatValues(variable.values) + "},";
            }

            return var_list.Substring(0, var_list.Length - 1) + "]";
        }

        static string ReformatValues(List<Value> values)
        {
            string val_list = "[";

            foreach (var val in values)
            {
                val_list = val_list + "\"" + val.id + "\",";
            }

            return val_list.Substring(0, val_list.Length - 1) + "]";
        }
    }
}
