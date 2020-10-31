using ExcelDataReader;
using GymStats.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace GymStats
{
    class DataContext
    {
        private readonly string FilePath;

        public DataContext(string filePath)
        {
            FilePath = filePath;
            ReadContext();
        }

        public List<Person> Persons = new List<Person>();
        public List<Log> Logs = new List<Log>();
        public List<Block> Blocks = new List<Block>();
        public List<Cycle> Cycles = new List<Cycle>();
        public List<Day> Days = new List<Day>();
        public List<Workout> Workouts = new List<Workout>();
        public List<Exercise> Exercises = new List<Exercise>();
        public List<Set> Sets = new List<Set>();

        private void ReadContext()
        {
            // Fix encoding error
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = File.Open(FilePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    DataSet result = reader.AsDataSet();
                    DataTableCollection tables = result.Tables;

                    foreach (DataTable table in tables)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            // Skip header row
                            if (table.Rows.IndexOf(row) == 0) continue;

                            int ID = Convert.ToInt32(row.ItemArray[0]);

                            switch (table.TableName)
                            {
                                case "PERSONS":
                                    Persons.Add(new Person { ID = ID, Description = Convert.ToString(row.ItemArray[1]) });
                                    break;

                                case "LOGS":
                                    Logs.Add(new Log { ID = ID, PersonID = Convert.ToInt32(row.ItemArray[1]), Description = Convert.ToString(row.ItemArray[2]) });
                                    break;

                                case "BLOCKS":
                                    Blocks.Add(new Block { ID = ID, LogID = Convert.ToInt32(row.ItemArray[1]), Description = Convert.ToString(row.ItemArray[2]) });
                                    break;

                                case "CYCLES":
                                    Cycles.Add(new Cycle { ID = ID, BlockID = Convert.ToInt32(row.ItemArray[1]), Description = Convert.ToString(row.ItemArray[2]) });
                                    break;

                                case "DAYS":
                                    Days.Add(new Day { ID = ID, CycleID = Convert.ToInt32(row.ItemArray[1]), Description = Convert.ToString(row.ItemArray[2]) });
                                    break;

                                case "WORKOUTS":
                                    Workouts.Add(new Workout { ID = ID, DayID = Convert.ToInt32(row.ItemArray[1]), Description = Convert.ToString(row.ItemArray[2]) });
                                    break;

                                case "EXERCISES":
                                    Exercises.Add(new Exercise { ID = ID, WorkoutID = Convert.ToInt32(row.ItemArray[1]), Description = Convert.ToString(row.ItemArray[2]) });
                                    break;

                                case "SETS":
                                    Sets.Add(new Set { 
                                        ID = ID, 
                                        ExerciseID = Convert.ToInt32(row.ItemArray[1]), 
                                        Weight = Convert.ToDouble(row.ItemArray[2]),
                                        Intensity = Convert.ToInt32(row.ItemArray[3]),
                                        Repetitions = Convert.ToInt32(row.ItemArray[4]),
                                    });
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }
}