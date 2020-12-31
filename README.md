**Welcome to the DSTIntegration library and CLI**

This has been developed as a holliday fun project, and as such documentation may be lacking.
The CLI is currently under construction and as such the Library is having its quality of life functionality implemented as is needed for that.

**Features currently implemented:**
 - To end interaction type "exit" or "-e"
 - Retrieve the table subjects by using "-subjects" or "-subs". This command has two optional arguments.
   - "-id" or "-ids" is used to pass a comma-sepparated list of subject IDs that you wish to retrieve sub-subjects for. This does not currently support any whitespace.
   - "-rec" toggles recursive request on, which will result in subjects, sub-subjects, sub-sub-subjects etc. being retrieved. 
 - Retrieve the list of tables related to specific subjects by using "-tables" or "-ts". This command has two optional arguments.
   - "-id" or "-ids" is used to pass a comma-sepparated list of subject IDs that you wish to retrieve tables for. This does not currently support any whitespace.
   - "-dslu" or "-ds" is the maximum number of days that has passed sinc the last table update.
 - Retrieve metadata of a table using "-metadata" or "-md" it takes one optional argument.
   - "id" is the ID of the table to retrieve.

**Missing functionality of note:**
 - parsing strings in the commandline (arguments surrounded by "")
 - building limited scope metadata for use with getting table data
 - table data
 - keeping track of data retrieved, so it can be written to files
 - writing data to files
