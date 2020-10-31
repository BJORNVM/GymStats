using GymStats.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GymStats
{
    class Program
    {
        static void Main(string[] args)
        {
            // *** INFO ***
            // Person > Log > Block > Cycle > Day > Workout > Exercise > Set


            // *** SETUP ***
            // Load data from .xlsx (Excel) file
            DataContext ctx = new DataContext(filePath: Path.Combine(Environment.CurrentDirectory, @"Data\DATA.xlsx"));



            // *** SANDBOX ***

            // Only use data from the first Person
            Person person = ctx.Persons.First();


            // The goal is to find all related Sets to the Person (no direct relation)


            // Find all Logs related to the Person
            List<Log> logs = ctx.Logs.Where(log => log.PersonID == person.ID).ToList();

            // Find all Blocks related to the Logs
            List<Block> blocks = ctx.Blocks.Where(block => logs.Select(log => log.ID).Contains(block.LogID)).ToList();

            // Find all Cycles related to the Blocks
            List<Cycle> cycles = ctx.Cycles.Where(cycle => blocks.Select(block => block.ID).Contains(cycle.BlockID)).ToList();

            // Find all Days related to the Cycles
            List<Day> days = ctx.Days.Where(day => cycles.Select(cycle => cycle.ID).Contains(day.CycleID)).ToList();

            // Find all Workouts related to the Days
            List<Workout> workouts = ctx.Workouts.Where(workout => days.Select(day => day.ID).Contains(workout.DayID)).ToList();

            // Find all Exercises related to the Workouts
            List<Exercise> exercises = ctx.Exercises.Where(exercise => workouts.Select(workout => workout.ID).Contains(exercise.WorkoutID)).ToList();

            // Find all Sets related to the Exercises
            List<Set> sets = ctx.Sets.Where(set => exercises.Select(exercise => exercise.ID).Contains(set.ExerciseID)).ToList();


            // Example: calculate the total amount of Repetitions the Person has performed
            Console.WriteLine($"From Person '{ person.Description }': Sum of Repetitions of all Sets: { sets.Sum(s => s.Repetitions) }");
            // Example: calculate the lowest amount of Repetitions the Person has performed in a Set
            Console.WriteLine($"From Person '{ person.Description }': Min of Repetitions of all Sets: { sets.Min(s => s.Repetitions) }");
            // Example: calculate the highest amount of Volume the Person has performed in a Set
            Console.WriteLine($"From Person '{ person.Description }': Max of Volume of all Sets: { sets.Max(s => s.Volume) }");
        }
    }
}