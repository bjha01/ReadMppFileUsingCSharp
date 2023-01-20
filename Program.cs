// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
using System;
using net.sf.mpxj;
using net.sf.mpxj.reader;
using net.sf.mpxj.writer;
using net.sf.mpxj.mpp;
using System.Collections.ObjectModel;
using java.util;
using Task = net.sf.mpxj.Task;
using static net.sf.mpxj.mspdi.schema.Project.Assignments;
using Assignment = net.sf.mpxj.ResourceAssignment;
using Resource = net.sf.mpxj.Resource;
using Npgsql;
using System.Data;
using jdk.nashorn.@internal.ir;
using com.sun.org.apache.xml.@internal.resolver.helpers;
using static net.sf.mpxj.phoenix.schema.phoenix4.Project.Storepoints.Storepoint.Resources.Resource;
using com.sun.xml.@internal.bind.v2.runtime.unmarshaller;
using System.Security.Cryptography;
using com.sun.tools.javadoc;
using static com.sun.xml.@internal.xsom.impl.WildcardImpl;
using System.Net.Mail;

namespace ConsoleApp3
{


    class Program
    {
        public static string resourceName;
        public static string resourceEmail;
        public static string taskName;
        //public static DateTime assignmentStart=new DateTime();
        //public static DateTime assignmentFinish=new DateTime();
        public static Date assignmentStart;
        public static Date assignmentFinish;
        public static string name;
        public static string email;
        public static string task_name;
        public static Date start;
        public static Date finish;
        public static bool taskfound;
        static void Main(string[] args)
        {
            // TestConnection();
            InsertRecord(resourceName, resourceEmail, taskName, assignmentStart, assignmentFinish);
            Console.ReadKey();
        }


        public static void InsertRecord(string resourceName, string resourceEmail, string taskName, Date assignmentStart, Date assignmentFinish)
        {
            MPPReader reader = new MPPReader();
            ProjectFile projectObj = reader.read("C:\\temp project\\PG Resources 20221205.mpp");

            NpgsqlConnection conn = new NpgsqlConnection(@"Server=localhost;Port=5432;User Id=postgres;Password=748096;Database=postgres;");
            using (var cmdd = new NpgsqlCommand("delete from employee", conn))
            {
                // cmd.Parameters.AddWithValue("p", "some_value");
                conn.Open();

                cmdd.ExecuteNonQuery();
                conn.Close();
            }




            name = "";
            email = "";
            task_name = "";
            start = null;
            finish = null;



            // Console.WriteLine("Resource" + " " + "Email" + " " + "Task" + " " + "Start" + " " + "Finish");
            foreach (Resource resource in ToEnumerable(projectObj.getResources()))
            {
                taskfound = false;
                name = resource.getName();
                email = resource.getEmailAddress();

                if (string.IsNullOrEmpty(resource.getName()) != true)
                {


                    foreach (Task task in ToEnumerable(projectObj.getTasks()))

                    {
                        task_name = task.getName();
                        foreach (Assignment assignment in ToEnumerable(task.getResourceAssignments()))
                        {

                            if (object.Equals(assignment.getResource(), null) != true)
                            {
                                if (string.IsNullOrEmpty(assignment.getResource().getName()) != true && string.IsNullOrEmpty(resource.getName()) != true && assignment.getResource().getName().ToLower().Trim() == resource.getName().ToLower().Trim())
                                {

                                    start = assignment.getStart();
                                    finish = assignment.getFinish();

                                    resourceName = resource.getName();
                                    resourceEmail = resource.getEmailAddress();
                                    taskName = task.getName();
                                    assignmentStart = assignment.getStart();
                                    assignmentFinish = assignment.getFinish();

                                    

                                    NpgsqlConnection con = new NpgsqlConnection(@"Server=localhost;Port=5432;User Id=postgres;Password=748096;Database=postgres;");

                                    using (var cmd = new NpgsqlCommand("insert into employee(name, email_address, task_name, start, finish)Values(" + (resourceName != " " ? ("'" + resourceName + "'") : "null") + ", " + (resourceEmail != "" ? ("'" + resourceEmail + "'") : "null") + ", " + (taskName.Length > 0 ? ("'" + taskName + "'") : "null") + ", " + (assignmentStart != null ? ("'" + assignmentStart + "'") : "null") + ", " + (assignmentFinish != null ? ("'" + assignmentFinish + "'") : "null") + " ) ", con))
                                    {
                                        // cmd.Parameters.AddWithValue("p", "some_value");
                                        con.Open();

                                        cmd.ExecuteNonQuery();
                                        con.Close();

                                        name = "";
                                        email = "";
                                        task_name = "";
                                        start = null;
                                        finish = null;
                                        taskfound = true;
                                    }

                                }

                            }
                        }
                    }
                    if (taskfound = false)
                    {
                        NpgsqlConnection connn = new NpgsqlConnection(@"Server=localhost;Port=5432;User Id=postgres;Password=748096;Database=postgres;");

                        using (var cmds = new NpgsqlCommand("insert into employee(name, email_address, task_name, start, finish)Values(" + (resourceName != "" ? ("'" + resourceName + "'") : "null") + ", " + (resourceEmail != "" ? ("'" + resourceEmail + "'") : "null") + " ) ", connn))
                        {
                            // cmd.Parameters.AddWithValue("p", "some_value");
                            connn.Open();

                            cmds.ExecuteNonQuery();
                            connn.Close();
                            resourceName = "";
                            resourceEmail = "";
                        }


                    }
                }




            }

        }
        private static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(@"Server=localhost;Port=5432;User Id=postgres;Password=748096;Database=postgres;");
        }
        private static IEnumerable<Assignment> ToEnumerable(object value)
        {
            throw new NotImplementedException();
        }

        private static EnumerableCollection ToEnumerable(Collection javaCollection)
        {
            return new EnumerableCollection(javaCollection);
        }

    }
}
