using System;
using Microsoft.Data.Sqlite;

namespace sqliteApp
{
    class Program
    {
        static void Main(string[] args)
        {
        
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = "./Students.db";

            using(var connection= new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                var delTableCmd = connection.CreateCommand();

                 delTableCmd.CommandText = "DROP TABLE IF EXISTS STUDENTS";
                delTableCmd.ExecuteNonQuery();

                delTableCmd.CommandText = "DROP TABLE IF EXISTS DEPARTMENT";
                delTableCmd.ExecuteNonQuery();
             

                var createTableCmd = connection.CreateCommand();
                createTableCmd.CommandText = "CREATE TABLE DEPARTMENT(ID int PRIMARY KEY NOT NULL, Name VARCHAR(50))";
                int createTableResult = createTableCmd.ExecuteNonQuery();
                Console.WriteLine(createTableResult);
                //Seed some data:
                using (var transaction = connection.BeginTransaction())
                {
                    var insertCmd = connection.CreateCommand();

                    insertCmd.CommandText = "INSERT INTO DEPARTMENT VALUES(1,'COMPUTERS')";
                    insertCmd.ExecuteNonQuery();

                    insertCmd.CommandText = "INSERT INTO DEPARTMENT VALUES(2,'ELECTRONICS')";
                    insertCmd.ExecuteNonQuery();

                    insertCmd.CommandText = "INSERT INTO DEPARTMENT VALUES(3,'MECHANICAL')";
                    insertCmd.ExecuteNonQuery();

                    transaction.Commit();
                }               
                //Read the newly inserted data:
                var select = connection.CreateCommand();
                select.CommandText = "SELECT name FROM DEPARTMENT";

                using (var reader = select.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var message = reader.GetString(0);  
                        Console.WriteLine(message);
                    }
                }

                createTableCmd.CommandText = "CREATE TABLE STUDENTS ("+
                                                "ROLLNO int NOT NULL,"+
                                                "NAME VARCHAR(100) NOT NULL,"+  
                                                "DEPARTMENT int, " +
                                                "PRIMARY KEY (ROLLNO)," +
                                                "CONSTRAINT FK_STUDENTDEPT FOREIGN KEY (DEPARTMENT)"+
                                                "REFERENCES DEPARTMENT(ID));";
    
                int studentTableResult = createTableCmd.ExecuteNonQuery();
                Console.WriteLine(studentTableResult);
                //Seed some data:
                using (var transaction = connection.BeginTransaction())
                {
                    var insertCmd = connection.CreateCommand();

                    insertCmd.CommandText = "INSERT INTO STUDENTS VALUES(1,'SRAVANI',1)";
                    insertCmd.ExecuteNonQuery();

                    insertCmd.CommandText = "INSERT INTO STUDENTS VALUES(2,'SRAVANTI',2)";
                    insertCmd.ExecuteNonQuery();

                    insertCmd.CommandText = "INSERT INTO STUDENTS VALUES(3,'JYOTHI',3)";
                    insertCmd.ExecuteNonQuery();

                    transaction.Commit();
                }


                //Read the newly inserted data:
                var selectCmd = connection.CreateCommand();
                selectCmd.CommandText = "SELECT std.NAME, dept.NAME FROM STUDENTS std "+
                                       "inner join DEPARTMENT dept on std.DEPARTMENT = dept.ID";

                using (var reader = selectCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var student = reader.GetString(0);
                        var department = reader.GetString(1);
                        Console.WriteLine("Student Name : " + student + "   Department : "+ department);
                    }
                } 
                Console.ReadLine();      
            }            
        }
    }
}
