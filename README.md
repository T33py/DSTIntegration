**Welcome to the DSTIntegration library and CLI**

This has been developed as a holliday fun project, and as such documentation may be lacking.
Dokumentation Of Danmarks statistiks API Can be found here: https://www.dst.dk/da/Statistik/brug-statistikken/muligheder-i-statistikbanken/api#tabellerogemner

The CLI is currently under construction and the Library is having its quality of life functionality implemented as is needed for that.
The format in which prints are created is {name}({ID}) eg "Levevilkår(05)" would be the print of the subject with ID 05

**Features currently in the CLI:**
 - To end interaction type "exit" or "-e"
 - Retrieve the table subjects by using "subjects" or "-subs". This command has two optional arguments.
   - "-id {id}" or "-ids {id}" is used to pass a comma-sepparated list of subject IDs that you wish to retrieve sub-subjects for. This does not currently support any whitespace.
   - "-rec" toggles recursive request on, which will result in subjects, sub-subjects, sub-sub-subjects etc. being retrieved. 
 - Retrieve the list of tables related to specific subjects by using "tables" or "-ts". This command has two optional arguments.
   - "-id {id}" or "-ids {id}" is used to pass a comma-sepparated list of subject IDs that you wish to retrieve tables for. This does not currently support any whitespace.
   - "-dslu {int}" or "-ds {int}" is the maximum number of days that has passed sinc the last table update. The plan is to have -1 as the unlimited value.
 - Retrieve metadata of a table using "metadata" or "-md" it takes one optional argument.
   - "-id {id}" is the ID of the table to retrieve.
 - The latest metadata retrieved can be accessed using the following commands
   - "print CurrentMetadata" writes the contents of the metadata to the commandline
   - "print Variable {id}" writes the variable with the given id to the commandline
   - "reset CurrentMetadata" empties the metadata currently stored
 - Retrieve data from the table which you have most recently loaded metadata from by using "GetData". This runs the currently stored DataRequest
   - "-print {location}" if you supply -print the data will be written to the default folder, which is the folder of execution. If you supply a location that will be used in stead(supplied location does not support spaces at the moment). 
 - The currently stored datarequest can be modified/printed using the following commands:
   - "add variable {id}" adds the variable (category of values) to the DataRequest
     - You can add the argument "-all" to add all values with the variable
   - "add value {variableID} {id}" adds both the value and variable to the DataRequest
   - "remove variable {id}" removes the variable and all associated values from the DataRequest
   - "remove value {variableID} {id}" removes the specified value from the DataRequest   
   - "print DataRequest" writes the contents of the datarequest to the commandline
   - "reset datarequest" resets the datarequest according to the currently stored metadata

**Missing functionality of note:**
 - parsing strings in the commandline (arguments surrounded by "")
 - keeping track of data retrieved, so it can be written to files
 - writing data to files

**The library:**

_DSTRequestHandler_ is intended to be the main point of entry. This class implements functions for retrieving deserialized versions of the different kinds of information available. The function for retrieving tabledata will initially just write the retrieved table to the disk, however eventually the intent is for it to also provide a deserialized CSV of tablecontent retrieved, however that is still a bit away.

_DSTConnection_ pings the DanskStatistik API with a request based on the current values of its settings dictionary. 
_DSTSettings.JSON_ contains the default values for any requests. If nothing else is specified in _DSTConnection.Settings_ the dictionary will contain the values read from this file, _SettingConstants_ contains the key values for the settings dictionary.
Of note in the settings is the fact that the DST API only supports 2 languages: "DA" and "EN" which is "Danish" and "English"(I suspect it to be british english).
Furthermore the library only supports parsing JSON, so if that is changed the user will have to interact directly with the _DSTConnection_ class.

_Retrievers_ is a static class that implements the actual calls and deserialization of objects, so if you want custom handling of requests or settings management, you can replace the _DSTRequestHandler_ with your own class that calls the _Retrievers_ once settings of the connection has been handled.