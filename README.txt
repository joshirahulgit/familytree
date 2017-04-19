This is just a sample application.

Instruction to run:
- Clone the branch to Visual Studio.
- Open the solution 'sample.sln'
- Right click the solution and select 'Manage NuGet Packages' from the context menu.
- Create a MS-SQL database and create all the required schemas(Please, refer MemberDatabase project which contains all the required queries to be executed to support the application). SQLBackup.sql file contains all the quries including some data.
- Open file 'App.config(under project ClientApp)', find connection string named 'CS' and set connectionString attribute value pointing to your local database, you created in last step. Save this file.
- Right click the solution 'sample' and hit build.
- Once build is success. Start 'ClientApp' project to see the output.

In case of any trouble, please email to joshirahul21@gmail.com.
